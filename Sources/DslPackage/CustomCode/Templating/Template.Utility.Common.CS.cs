using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EnvDTE;

/*~T4_TEMPLATE_HEADER_BEGIN~
<#@ assembly name="System.Core"
#><#@ assembly name="System.Data.Linq"
#><#@ assembly name="EnvDTE"
#><#@ assembly name="System.Xml"
#><#@ assembly name="System.Xml.Linq"
#><#@ import namespace="System"
#><#@ import namespace="System.CodeDom"
#><#@ import namespace="System.CodeDom.Compiler"
#><#@ import namespace="System.Collections.Generic"
#><#@ import namespace="System.Data.Linq"
#><#@ import namespace="System.Data.Linq.Mapping"
#><#@ import namespace="System.IO"
#><#@ import namespace="EnvDTE"
#><#@ import namespace="System.Linq"
#><#@ import namespace="System.Reflection"
#><#@ import namespace="System.Text"
#><#@ import namespace="System.Text.RegularExpressions"
#><#@ import namespace="System.Xml.Linq"
#><#@ import namespace="System.Globalization"
#><#@ import namespace="Microsoft.VisualStudio.TextTemplating"
~T4_TEMPLATE_HEADER_END~*/


public class FileManager
{
    #region class OutputTarget

    public class OutputTarget
    {
        public string ProjectName { get; set; }

        /// <summary>
        /// Project relative path, supports subfolders syntax like "Dir1\SubDir1\SubSubDir1"
        /// </summary>
        public string ProjectPath { get; set; }

        //internal ProjectItem ProjectItem { get; set; }
        internal ProjectItems ProjectItemsHolder { get; set; }

        internal string OutputDirectory
        {
            get
            {
                //string fileName = this.ProjectItem.FileNames[0];
                string fileName = GetProjectItemFileName(); //this.GetProjectItem().FileNames[0];
                return Path.GetDirectoryName(fileName);
            }
        }

        public OutputTarget(string projectName, string projectPath)
        {
            this.ProjectName = projectName;
            this.ProjectPath = projectPath;
        }

        internal OutputTarget(ProjectItem projectItem)
        {
            Update(projectItem.ProjectItems);
        }

        public override string ToString()
        {
            return GetProjectItemFileName();
        }

        private string GetProjectItemFileName()
        {
            var parent = ProjectItemsHolder.Parent;
            ProjectItem projectItem = parent as ProjectItem;
            Project project = parent as Project;

            return project != null ? project.FileName : projectItem.FileNames[0];
        }

        private ProjectItem GetProjectItem()
        {
            return (ProjectItem)ProjectItemsHolder.Parent;
        }

        internal void Update(ProjectItems holder)
        {
            this.ProjectItemsHolder = holder;
            var projectItem = GetProjectItem();

            this.ProjectName = projectItem.ContainingProject.Name;

            object parent = projectItem.Kind == Constants.vsProjectItemKindPhysicalFolder
                            ? (object)projectItem
                            : projectItem.Collection.Parent;
            List<string> pathItems = new List<string>();
            int loop = 0;
            while (parent != null)
            {
                ProjectItem parentItem = parent as ProjectItem;
                if (parentItem != null)
                {
                    if (parentItem.Kind == Constants.vsProjectItemKindPhysicalFolder)
                    {
                        pathItems.Add(parentItem.Name);

                        parent = parentItem.Collection.Parent as ProjectItem;
                    }
                }
                else
                {
                    parent = null;
                }

                loop++;
                if (loop == 20)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }

            if (pathItems.Count > 0)
            {
                pathItems.Reverse();
                this.ProjectPath = string.Join("\\", pathItems);
            }
            else
            {
                this.ProjectPath = string.Empty;
            }
        }
    }

    #endregion class OutputTarget

    #region class FileBlock

    private class FileBlock : BlockData
    {
        public String Name;
        public BlockData Header;
        public BlockData Footer;
        public OutputTarget Target;

        internal string TargetFileName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, TargetFileName);
        }

        public override string GetData(StringBuilder sb)
        {
            int st = this.Start;
            int len = this.Length;
            if (this.Header != null && this.Header.Length > 0)
            {
                st += this.Header.Length;
                len -= this.Header.Length;
            }

            if (this.Footer != null && this.Footer.Length > 0)
            {
                len -= this.Footer.Length;
            }

            return sb.ToString(st, len);
        }
    }

    private class BlockData
    {
        public int Start;
        public int Length;

        public virtual string GetData(StringBuilder sb)
        {
            return sb.ToString(Start, Length);
        }
    }

    #endregion class Block


    private readonly List<FileBlock> files = new List<FileBlock>();
//    private Block footer = new Block();
//    private Block header = new Block();
    protected DynamicTextTransformation _textTransformation;
    private FileBlock currentFileBlock;
    //protected List<String> generatedFileNames = new List<String>();
    //protected string outputPath;
    private string templateDirectory;
    protected string templateFile;

    private FileManager(DynamicTextTransformation textTransformation)
    {
        this._textTransformation = textTransformation;
        this.templateFile = textTransformation.Host.TemplateFile;
        this.templateDirectory = Path.GetDirectoryName(templateFile);
    }

    public string TemplateFile
    {
        get { return templateFile; }
    }

    //public string OutputProjectName { get; set; }

    public OutputTarget TemplateFileAsOutputTarget { get; protected set; }

//    public String OutputPath
//    {
//        get { return outputPath; }
//        set
//        {
//            if (value != outputPath)
//            {
//                outputPath = value;
//                if (outputPath != null && outputPath[outputPath.Length - 1] != Path.DirectorySeparatorChar)
//                {
//                    outputPath += Path.DirectorySeparatorChar;
//                }
//            }
//        }
//    }

//    protected virtual string ResolveOutputPath()
//    {
//        string result = this.OutputPath;
//
//        if (!string.IsNullOrEmpty(this.OutputPath))
//        {
//            if (!Path.IsPathRooted(this.OutputPath))
//            {
//                result = Path.Combine(templateDirectory, this.OutputPath);
//            }
//        }
//        else
//        {
//            result = templateDirectory;
//        }
//
//        return result;
//    }

    public virtual String DefaultProjectNamespace
    {
        get { return null; }
    }

    public static FileManager Create(object textTransformation)
    {
        FileManager result = null;
        bool inVSNET = false;

        DynamicTextTransformation transformation = DynamicTextTransformation.Create(textTransformation);
        IDynamicHost host = transformation.Host;

        try
        {

#if !PREPROCESSED_TEMPLATE
            if (host.AsIServiceProvider() != null)
            {
                inVSNET = true;
            }
#endif

            result = inVSNET ? new VSManager(transformation) : new FileManager(transformation);
        }
        catch (Exception e)
        {
            //host.LogError(e);
        }
        return result;
    }

    public void StartNewFile(String name)
    {
        StartNewFile(name, TemplateFileAsOutputTarget);
    }

    public void StartNewFile(String name, string projectName, string projectPath)
    {
        StartNewFile(name, new OutputTarget(projectName, projectPath));
    }

    public virtual void StartNewFile(String name, OutputTarget outputTarget)
    {
        if (name == null)
        {
            throw new ArgumentNullException("name");
        }

        CurrentFileBlock = new FileBlock
                       {
                           Name = name,
                           Target = outputTarget
                       };
    }

    public void EndNewFile()
    {
        //EndFileBlock();
    }

    public void StartFooter()
    {
        if (CurrentFileBlock == null)
        {
            throw new InvalidOperationException(
                "CurrentFileBlock is not initialized, call 'StartNewFile' prior using 'StartFooter' !");
        }

        CurrentFileBlock.Footer = new BlockData();
        StartBlockData(CurrentFileBlock.Footer);
    }

    public void EndFooter()
    {
        if (CurrentFileBlock == null)
        {
            throw new InvalidOperationException(
                "CurrentFileBlock is not initialized, call 'StartNewFile' prior using 'EndFooter' !");
        }

        EndBlockData(CurrentFileBlock.Footer);
    }

    public void StartHeader()
    {
        if (CurrentFileBlock == null)
        {
            throw new InvalidOperationException(
                "CurrentFileBlock is not initialized, call 'StartNewFile' prior using 'StartHeader' !");
        }

        CurrentFileBlock.Header = new BlockData();
        StartBlockData(CurrentFileBlock.Header);
    }

    public void EndHeader()
    {
        if (CurrentFileBlock == null)
        {
            throw new InvalidOperationException(
                "CurrentFileBlock is not initialized, call 'StartNewFile' prior using 'EndHeader' !");
        }

        EndBlockData(CurrentFileBlock.Header);
    }

    private FileBlock CurrentFileBlock
    {
        get { return currentFileBlock; }
        set
        {
            if (CurrentFileBlock != null)
            {
                EndFileBlock();
            }

            if (value != null)
            {
                //value.Start = this._textTransformation.GenerationEnvironment.Length;
                StartBlockData(value);
            }

            currentFileBlock = value;
        }
    }

    private void StartBlockData(BlockData blockData)
    {
        if (blockData != null)
        {
            blockData.Start = Template.Length;
        }
    }

    private void EndBlockData(BlockData blockData)
    {
        if (blockData != null)
        {
            blockData.Length = this.Template.Length - blockData.Start;
        }
    }

    private void EndFileBlock()
    {
        if (CurrentFileBlock == null)
            return;

        EndBlockData(CurrentFileBlock); //.Length = this.Template.Length - CurrentBlock.Start;
        //if (CurrentBlock != header && CurrentBlock != footer)
        files.Add(CurrentFileBlock);
        currentFileBlock = null;
    }

    public void Flush()
    {
        this.Process();
    }

    protected StringBuilder Template
    {
        get { return this._textTransformation.GenerationEnvironment; }
    }

    public virtual void CleanUp()
    {
        //this.footer = new Block();
        //this.header = new Block();
        this.files.Clear();
        //this.generatedFileNames.Clear();
        this.CurrentFileBlock = null;
        this.Template.Clear();
    }

    protected internal virtual void Process()
    {
        EndFileBlock();

//        String headerText = this.Template.ToString(header.Start, header.Length);
//        String footerText = this.Template.ToString(footer.Start, footer.Length);

        //String outputPath = ResolveOutputPath();

        files.Reverse();
        foreach (var fileBlock in files)
        {
            string outputDir = fileBlock.Target.OutputDirectory;
            String fileName = Path.Combine(outputDir, fileBlock.Name);

            string headerText = fileBlock.Header == null ? string.Empty : fileBlock.Header.GetData(this.Template);
            string footerText = fileBlock.Footer == null ? string.Empty : fileBlock.Footer.GetData(this.Template);

            string content = headerText + fileBlock.GetData(this.Template) + footerText;
            
            //generatedFileNames.Add(fileName);

            CreateFile(fileName, content);

            fileBlock.TargetFileName = fileName;

            //this.Template.Remove(fileBlock.Start, fileBlock.Length);
        }

        this.Template.Clear();
    }

    protected virtual void CreateFile(String fileName, String content)
    {
        if (IsFileContentDifferent(fileName, content))
        {
            File.WriteAllText(fileName, content);
        }
    }

    public virtual String GetCustomToolNamespace(String fileName)
    {
        return null;
    }

    public virtual string GetTemplateItemCustomToolNamespace()
    {
        return string.Empty;
    }

    public string ResolveNamespace()
    {
        if (!string.IsNullOrEmpty(this.DefaultProjectNamespace))
        {
            return this.DefaultProjectNamespace;
        }

        return GetTemplateItemCustomToolNamespace();
    }

    protected bool IsFileContentDifferent(String fileName, String newContent)
    {
        return !(File.Exists(fileName) && File.ReadAllText(fileName) == newContent);
    }

    #region Nested type: VSManager

    private class VSManager : FileManager
    {
        private readonly Action<String> checkOutAction;
        private readonly DTE dte;
        //private readonly Action<IEnumerable<String>> projectSyncAction;
        private readonly Action<IEnumerable<FileBlock>> projectSyncAction;
        private ProjectItem templateProjectItem;
        //private ProjectItem outputPathProjectItem;
        private List<ProjectItem> oldProjectItemsToDelete = new List<ProjectItem>();

        internal VSManager(DynamicTextTransformation templatingHost)
            : base(templatingHost)
        {
            var hostServiceProvider = _textTransformation.Host.AsIServiceProvider();
            if (hostServiceProvider == null)
            {
                throw new ArgumentNullException("Could not obtain hostServiceProvider");
            }

            dte = (EnvDTE.DTE) hostServiceProvider.GetService(typeof (EnvDTE.DTE));
            if (dte == null)
                throw new ArgumentNullException("Could not obtain DTE from host");
            templateProjectItem = dte.Solution.FindProjectItem(templateFile);
            TemplateFileAsOutputTarget = new OutputTarget(templateProjectItem);

            //BuildOldTemplateProjectFilesToDelete(templateProjectItem);

            checkOutAction = (String fileName) => dte.SourceControl.CheckOutItem(fileName);
            
            projectSyncAction =
                (IEnumerable<FileBlock> generatedFiles) =>
                ProjectSync(templateProjectItem, generatedFiles, oldProjectItemsToDelete);
        }

        public override void StartNewFile(String name, OutputTarget outputTarget)
        {
            base.StartNewFile(name, outputTarget);

            if (this.CurrentFileBlock.Target.ProjectItemsHolder == null)
            {
                ResolveOutputTargetProjectItem(this.CurrentFileBlock);
            }
        }

        private void ResolveOutputTargetProjectItem(FileBlock fileBlock)
        {
            OutputTarget outputTarget = fileBlock.Target;

            Project project = string.IsNullOrEmpty(outputTarget.ProjectName) 
                ? null
                : FindProjectByName(dte, outputTarget.ProjectName);

            ProjectItems targetProjectItems = null;

            if (project != null)
            {
                if (!string.IsNullOrEmpty(outputTarget.ProjectPath))
                {
                    var pathItems = outputTarget.ProjectPath.Split('\\');
                    var projectItems = project.ProjectItems;
                    foreach (var pathItem in pathItems)
                    {
                        var projItem = FindProjectItem(dte, projectItems, pathItem, Constants.vsProjectItemKindPhysicalFolder);
                        if (projItem != null)
                        {
                            if (projItem.ProjectItems.Count > 0)
                            {
                                projectItems = projItem.ProjectItems;
                            }

                            if (pathItem == pathItems.Last())
                            {
                                targetProjectItems = projItem.ProjectItems;
                            }
                        }
                    }
                }
                else
                {
                    var dir = Path.GetDirectoryName(project.FileName);
                    var templateFileDir = TemplateFileAsOutputTarget.OutputDirectory;

                    if (dir != templateFileDir)
                    {
                        targetProjectItems = project.ProjectItems;
                    }
                }
            }

            //outputTarget.ProjectItem = targetProjectItem ?? TemplateFileAsOutputTarget;
            if (targetProjectItems != null)
            {
                outputTarget.Update(targetProjectItems);
            }
            else
            {
                fileBlock.Target = TemplateFileAsOutputTarget;
            }
        }

        public override void CleanUp()
        {
            base.CleanUp();
            this.oldProjectItemsToDelete.Clear();
        }

        private void BuildListOfProjectItemsToDelete()
        {
            oldProjectItemsToDelete.Clear();

            foreach (var fileBlock in files)
            {
                var items = fileBlock.Target.ProjectItemsHolder.OfType<ProjectItem>()
                    .Where(projItem => !oldProjectItemsToDelete.Contains(projItem))
                    .ToList();

                oldProjectItemsToDelete.AddRange(items);
            }
        }

        public override String DefaultProjectNamespace
        {
            get { return templateProjectItem.ContainingProject.Properties.Item("DefaultNamespace").Value.ToString(); }
        }

        public override String GetCustomToolNamespace(string fileName)
        {
            return dte.Solution.FindProjectItem(fileName).Properties.Item("CustomToolNamespace").Value.ToString();
        }

        public virtual string GetTemplateItemCustomToolNamespace()
        {
            return templateProjectItem != null
                       ? templateProjectItem.Properties.Item("CustomToolNamespace").Value.ToString()
                       : string.Empty;
        }

        protected override void CreateFile(String fileName, String content)
        {
            if (IsFileContentDifferent(fileName, content))
            {
                CheckoutFileIfRequired(fileName);
                File.WriteAllText(fileName, content);
            }
        }

        private static Project FindProjectByName(DTE vsObj, string name)
        {
            Project result = null;
            if (!string.IsNullOrEmpty(name))
            {
                foreach (Project project in vsObj.Solution.Projects)
                {
                    string projName = project.Name;
                    if (string.Compare(projName, name, true) == 0)
                    {
                        result = project;
                        break;
                    }
                }
            }

            return result;
        }

        private static ProjectItem FindProjectItem(DTE vsObj, ProjectItems inProjectItems,
            string ProjectItemName, string projectItemKind, bool byName = true)
        {
            foreach (ProjectItem projItem in inProjectItems)
            {
                string _name = projItem.Name;
                for (short i = 0; i < projItem.FileCount; i++)
                {
                    bool kindOk = (projectItemKind == null || projItem.Kind == projectItemKind);
                    string prjName = byName ? projItem.Name : projItem.FileNames[i];
                    bool nameOk = prjName == ProjectItemName;
                    if (nameOk && kindOk)
                    {
                        return projItem;
                    }
                    else if (projItem.ProjectItems.Count > 0)
                    {
                        ProjectItem found = FindProjectItem(vsObj, projItem.ProjectItems, ProjectItemName, projectItemKind);
                        if (found != null)
                        {
                            return found;
                        }
                    }
                }
            }

            return null;
        }

        protected internal override void Process()
        {
            if (templateProjectItem.ProjectItems == null)
            {
                return;
            }

            base.Process();
            BuildListOfProjectItemsToDelete();
            //projectSyncAction.EndInvoke(projectSyncAction.BeginInvoke(generatedFileNames, null, null));
            projectSyncAction.EndInvoke(projectSyncAction.BeginInvoke(files, null, null));
        }

        private static void ProjectSync(ProjectItem templateProjectItem, IEnumerable<FileBlock> generatedFileBlocks, List<ProjectItem> projectItemsToDelete)
        {
            var generatedFiles = generatedFileBlocks.Select(item => item.Target);
            string templateFileName = templateProjectItem.FileNames[0];
            var keepFileNames = generatedFileBlocks.Select(item => item.TargetFileName);

            Dictionary<ProjectItems, List<string>> projectItemExistingFiles = new Dictionary<ProjectItems, List<string>>();
            foreach (var genProjItems in generatedFiles.Select(item => item.ProjectItemsHolder))
            {
                if (!projectItemExistingFiles.ContainsKey(genProjItems))
                {
                    var projectItems = genProjItems.OfType<ProjectItem>();
                    projectItemExistingFiles.Add(genProjItems, projectItems.Select(item => item.FileNames[0]).ToList());
                }
            }

            if (projectItemsToDelete != null)
            {
                foreach (var oldProjectItem in projectItemsToDelete)
                {
                    string oldFileName = oldProjectItem.FileNames[0];
                    if (oldFileName != templateFileName && !keepFileNames.Contains(oldFileName))
                    {
                        oldProjectItem.Delete();
                    }
                }
            }

            // Add missing files to the project
            foreach (var generatedFile in generatedFileBlocks)
            {
                var target = generatedFile.Target;
                string ts = target.ToString();
                var existingFiles = projectItemExistingFiles[target.ProjectItemsHolder];
                string targetFileName = generatedFile.TargetFileName;
                if (!existingFiles.Contains(targetFileName))
                {
                    target.ProjectItemsHolder.AddFromFile(targetFileName);
                }
            }
        }

        private void CheckoutFileIfRequired(String fileName)
        {
            SourceControl sc = dte.SourceControl;
            if (sc != null && sc.IsItemUnderSCC(fileName) && !sc.IsItemCheckedOut(fileName))
                checkOutAction.EndInvoke(checkOutAction.BeginInvoke(fileName, null, null));
        }

        //        private void BuildOldTemplateProjectFilesToDelete(ProjectItem sourceProjectItem)
        //        {
        //            oldProjectItemsToDelete = sourceProjectItem.ProjectItems.OfType<ProjectItem>().ToList();
        //        }

        /*protected override string ResolveOutputPath()
        {
            Project outputProject = FindOutputProject(dte);
            if (outputProject != null)
            {
                templateDirectory = Path.GetDirectoryName(outputProject.FileName);
            }
            else
            {
                outputProject = templateProjectItem.ContainingProject;
            }

            string result = base.ResolveOutputPath();

            outputPathProjectItem = FindProjectItem(dte, outputProject.ProjectItems, result);

            if (!string.IsNullOrEmpty(result))
            {
                string dir = File.Exists(result) ? Path.GetDirectoryName(result) : result;
                if (Directory.Exists(dir))
                {
                    string templateFileName = templateProjectItem.FileNames[0];
                    string templateDir = Path.GetDirectoryName(templateFileName);

                    if (string.Compare(dir, templateDir, true) == 0)
                    {
                        outputPathProjectItem = templateProjectItem;
                    }
                }
            }

            if (outputPathProjectItem == null)
            {
                outputPathProjectItem = templateProjectItem;
            }
            else
            {
                string templateFileName = templateProjectItem.FileNames[0];
                string outputPathProjectItemFileName = outputPathProjectItem.FileNames[0];
                if (string.Compare(templateFileName, outputPathProjectItemFileName, true) != 0)
                {
                    //templateProjectItem = outputPathProjectItem;
                    BuildOldTemplateProjectFilesToDelete(outputPathProjectItem);
                }
            }

            return result;
        }*/

        /*private Project FindOutputProject(DTE vsObj)
        {
            Project result = null;
            if (!string.IsNullOrEmpty(OutputProjectName))
            {
                foreach (Project project in vsObj.Solution.Projects)
                {
                    string projName = project.Name;
                    if (string.Compare(projName, OutputProjectName, true) == 0)
                    {
                        result = project;
                        break;
                    }
                }
            }

            return result;
        }*/
    }

    #endregion
}

/// <summary>
/// Reponsible for abstracting the use of Host between times
/// when it is available and not
/// </summary>
public interface IDynamicHost
{
    /// <summary>
    /// An abstracted call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
    /// </summary>
    string ResolveParameterValue(string id, string name, string otherName);

    /// <summary>
    /// An abstracted call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
    /// </summary>
    string ResolvePath(string path);

    /// <summary>
    /// An abstracted call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
    /// </summary>
    string TemplateFile { get; }

    /// <summary>
    /// Returns the Host instance cast as an IServiceProvider
    /// </summary>
    IServiceProvider AsIServiceProvider();
}

/// <summary>
/// Reponsible for implementing the IDynamicHost as a dynamic
/// shape wrapper over the Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost interface
/// rather than type dependent wrapper.  We don't use the
/// interface type so that the code can be run in preprocessed mode
/// on a .net framework only installed machine.
/// </summary>
public class DynamicHost : IDynamicHost
{
    private readonly object _instance;
    private readonly MethodInfo _resolveParameterValue;
    private readonly MethodInfo _resolvePath;
    private readonly PropertyInfo _templateFile;

    /// <summary>
    /// Creates an instance of the DynamicHost class around the passed in
    /// Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost shapped instance passed in.
    /// </summary>
    public DynamicHost(object instance)
    {
        _instance = instance;
        Type type = _instance.GetType();
        _resolveParameterValue = type.GetMethod("ResolveParameterValue", new Type[]
                                                                         {
                                                                             typeof (string), typeof (string),
                                                                             typeof (string)
                                                                         });
        _resolvePath = type.GetMethod("ResolvePath", new Type[]
                                                     {
                                                         typeof (string)
                                                     });
        _templateFile = type.GetProperty("TemplateFile");

    }

    /// <summary>
    /// A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
    /// </summary>
    public string ResolveParameterValue(string id, string name, string otherName)
    {
        return (string) _resolveParameterValue.Invoke(_instance, new object[]
                                                                 {
                                                                     id, name, otherName
                                                                 });
    }

    /// <summary>
    /// A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
    /// </summary>
    public string ResolvePath(string path)
    {
        return (string) _resolvePath.Invoke(_instance, new object[]
                                                       {
                                                           path
                                                       });
    }

    /// <summary>
    /// A call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
    /// </summary>
    public string TemplateFile
    {
        get { return (string) _templateFile.GetValue(_instance, null); }
    }

    /// <summary>
    /// Returns the Host instance cast as an IServiceProvider
    /// </summary>
    public IServiceProvider AsIServiceProvider()
    {
        return _instance as IServiceProvider;
    }
}

/// <summary>
/// Reponsible for implementing the IDynamicHost when the
/// Host property is not available on the TextTemplating type. The Host
/// property only exists when the hostspecific attribute of the template
/// directive is set to true.
/// </summary>
public class NullHost : IDynamicHost
{
    /// <summary>
    /// An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolveParameterValue
    /// that simply retuns null.
    /// </summary>
    public string ResolveParameterValue(string id, string name, string otherName)
    {
        return null;
    }

    /// <summary>
    /// An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost ResolvePath
    /// that simply retuns the path passed in.
    /// </summary>
    public string ResolvePath(string path)
    {
        return path;
    }

    /// <summary>
    /// An abstraction of the call to Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost TemplateFile
    /// that returns null.
    /// </summary>
    public string TemplateFile
    {
        get { return null; }
    }

    /// <summary>
    /// Returns null.
    /// </summary>
    public IServiceProvider AsIServiceProvider()
    {
        return null;
    }
}

/// <summary>
/// Responsible creating an instance that can be passed
/// to helper classes that need to access the TextTransformation
/// members.  It accesses member by name and signature rather than
/// by type.  This is necessary when the
/// template is being used in Preprocessed mode
/// and there is no common known type that can be
/// passed instead
/// </summary>
public class DynamicTextTransformation
{
    private object _instance;
    private IDynamicHost _dynamicHost;

    private readonly MethodInfo _write;
    private readonly MethodInfo _writeLine;
    private readonly PropertyInfo _generationEnvironment;
    private readonly PropertyInfo _errors;
    private readonly PropertyInfo _host;

    /// <summary>
    /// Creates an instance of the DynamicTextTransformation class around the passed in
    /// TextTransformation shapped instance passed in, or if the passed in instance
    /// already is a DynamicTextTransformation, it casts it and sends it back.
    /// </summary>
    public static DynamicTextTransformation Create(object instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException("instance");
        }

        DynamicTextTransformation textTransformation = instance as DynamicTextTransformation;
        if (textTransformation != null)
        {
            return textTransformation;
        }

        return new DynamicTextTransformation(instance);
    }

    private DynamicTextTransformation(object instance)
    {
        _instance = instance;
        Type type = _instance.GetType();
        _write = type.GetMethod("Write", new Type[]
                                         {
                                             typeof (string)
                                         });
        _writeLine = type.GetMethod("WriteLine", new Type[]
                                                 {
                                                     typeof (string)
                                                 });
        _generationEnvironment = type.GetProperty("GenerationEnvironment",
            BindingFlags.Instance | BindingFlags.NonPublic);
        _host = type.GetProperty("Host");
        _errors = type.GetProperty("Errors");
    }

    /// <summary>
    /// Gets the value of the wrapped TextTranformation instance's GenerationEnvironment property
    /// </summary>
    public StringBuilder GenerationEnvironment
    {
        get { return (StringBuilder) _generationEnvironment.GetValue(_instance, null); }
    }

    /// <summary>
    /// Gets the value of the wrapped TextTranformation instance's Errors property
    /// </summary>
    public System.CodeDom.Compiler.CompilerErrorCollection Errors
    {
        get { return (System.CodeDom.Compiler.CompilerErrorCollection) _errors.GetValue(_instance, null); }
    }

    /// <summary>
    /// Calls the wrapped TextTranformation instance's Write method.
    /// </summary>
    public void Write(string text)
    {
        _write.Invoke(_instance, new object[]
                                 {
                                     text
                                 });
    }

    /// <summary>
    /// Calls the wrapped TextTranformation instance's WriteLine method.
    /// </summary>
    public void WriteLine(string text)
    {
        _writeLine.Invoke(_instance, new object[]
                                     {
                                         text
                                     });
    }

    /// <summary>
    /// Gets the value of the wrapped TextTranformation instance's Host property
    /// if available (shows up when hostspecific is set to true in the template directive) and returns
    /// the appropriate implementation of IDynamicHost
    /// </summary>
    public IDynamicHost Host
    {
        get
        {
            if (_dynamicHost == null)
            {
                if (_host == null)
                {
                    _dynamicHost = new NullHost();
                }
                else
                {
                    _dynamicHost = new DynamicHost(_host.GetValue(_instance, null));
                }
            }
            return _dynamicHost;
        }
    }
}