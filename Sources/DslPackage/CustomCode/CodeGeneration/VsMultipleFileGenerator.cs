using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Designer.Interfaces;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    [CLSCompliant(false)]
    public abstract class VsMultipleFileGenerator<IterativeElementType> : IEnumerable<IterativeElementType>, 
        Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator, IObjectWithSite
    {
        #region Visual Studio Specific Fields
        private object site;
        private ServiceProvider serviceProvider = null;
        public const int S_OK = 0;
        public const int E_FAIL = unchecked((int)0x80004005);
        #endregion

        #region Our Fields
        private string bstrInputFileContents;
        private string wszInputFilePath;
        private EnvDTE.Project _project;
        private CodeDomProvider codeDomProvider;

        private List<string> newFileNames;
        #endregion

        [CLSCompliant(false)]
        protected EnvDTE.Project Project
        {
            get
            {
                return _project;
            }
        }

/*
        /// <summary>
        /// Returns a CodeDomProvider object for the language of the project containing
        /// the project item the generator was called on
        /// </summary>
        /// <returns>A CodeDomProvider object</returns>
        protected CodeDomProvider CodeProvider
        {
            get
            {
                if (codeDomProvider == null)
                {
                    //Query for IVSMDCodeDomProvider/SVSMDCodeDomProvider for this project type
                    IVSMDCodeDomProvider provider = SiteServiceProvider.GetService(typeof(SVSMDCodeDomProvider)) as IVSMDCodeDomProvider;
                    if (provider != null)
                    {
                        codeDomProvider = provider.CodeDomProvider as CodeDomProvider;
                    }
                    else
                    {
                        //In the case where no language specific CodeDom is available, fall back to C#
                        codeDomProvider = CodeDomProvider.CreateProvider("C#");
                    }
                }
                return codeDomProvider;
            }
        }
*/

        protected string RootNamespace
        {
            get
            {
                if (_project == null) return null;

                EnvDTE.Property property = _project.Properties.Item("RootNamespace");
                if (property == null) return null;

                return property.Value.ToString();
            }
        }

        protected string InputFileContents
        {
            get
            {
                return bstrInputFileContents;
            }
        }

        protected string InputFilePath
        {
            get
            {
                return wszInputFilePath;
            }
        }

        protected object Site
        {
            get { return site; }
        }

        [CLSCompliant(false)]
        protected ServiceProvider SiteServiceProvider
        {
            get
            {
                if (serviceProvider == null)
                {
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleServiceProvider = site as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
                    serviceProvider = new ServiceProvider(oleServiceProvider);
                }
                return serviceProvider;
            }
        }

        private Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress _generatorProgress;
        protected Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress GeneratorProgress
        {
            get
            {
                return _generatorProgress;
            }
        }

        public VsMultipleFileGenerator()
        {
            newFileNames = new List<string>();
        }
        public abstract IEnumerator<IterativeElementType> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected abstract string GetFileName(IterativeElementType element);
        public abstract byte[] GenerateContent(IterativeElementType element);


        public abstract byte[] GenerateDefaultContent();


        #region IObjectWithSite Members

        public void GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            if (this.site == null)
            {
                throw new Win32Exception(-2147467259);
            }

            IntPtr objectPointer = Marshal.GetIUnknownForObject(this.site);

            try
            {
                Marshal.QueryInterface(objectPointer, ref riid, out ppvSite);
                if (ppvSite == IntPtr.Zero)
                {
                    throw new Win32Exception(-2147467262);
                }
            }
            finally
            {
                if (objectPointer != IntPtr.Zero)
                {
                    Marshal.Release(objectPointer);
                    objectPointer = IntPtr.Zero;
                }
            }
        }

        public void SetSite(object pUnkSite)
        {
            this.site = pUnkSite;
        }

        #endregion


        public abstract int DefaultExtension(out string pbstrDefaultExtension);

        public int Generate(string wszInputFilePath, string bstrInputFileContents, string wszDefaultNamespace, IntPtr[] rgbOutputFileContents, out uint pcbOutput, Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress pGenerateProgress)
        {
            this.bstrInputFileContents = bstrInputFileContents;
            this.wszInputFilePath = wszInputFilePath;
            this._generatorProgress = pGenerateProgress;
            this.newFileNames.Clear();

            // Look through all the projects in the solution
            EnvDTE.DTE dte = (EnvDTE.DTE)Package.GetGlobalService(typeof(EnvDTE.DTE));
            VSDOCUMENTPRIORITY[] pdwPriority = new VSDOCUMENTPRIORITY[1];
            EnvDTE.ProjectItem item = null;
            foreach (EnvDTE.Project project in dte.Solution.Projects)
            {
                try
                {
                    int iFound = 0;
                    uint itemId = 0;

                    if (string.IsNullOrEmpty(project.FileName) || !File.Exists(project.FileName))
                        continue;

                    // obtain a reference to the current project as an IVsProject type
                    IVsProject vsProject = VsHelper.ToVsProject(project);

                    // this locates, and returns a handle to our source file, as a ProjectItem
                    vsProject.IsDocumentInProject(InputFilePath, out iFound, pdwPriority, out itemId);

                    // if this source file is found in this project
                    if (iFound != 0 && itemId != 0)
                    {
                        Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleSp = null;
                        vsProject.GetItemContext(itemId, out oleSp);
                        if (oleSp != null)
                        {
                            ServiceProvider sp = new ServiceProvider(oleSp);
                            // convert our handle to a ProjectItem
                            item = sp.GetService(typeof(EnvDTE.ProjectItem)) as EnvDTE.ProjectItem;

                            if (item != null)
                            {
                                // We now have what we need. Break out of loop.
                                _project = project;
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // This is here for debugging purposes, as setting a breakpoint here can be very helpful
                    throw ex;
                }
            }

            // Do sanity check
            if (_project == null || item == null)
                throw new ApplicationException("Unable to retrieve Visual Studio ProjectItem. Try running the tool again.");

            // now we can start our work, iterate across all the 'elements' in our source file 
            foreach (IterativeElementType element in this)
            {
                try
                {
                    // obtain a name for this target file
                    string fileName = GetFileName(element);
                    // add it to the tracking cache
                    newFileNames.Add(fileName);
                    // fully qualify the file on the filesystem
                    string strFile = Path.Combine(wszInputFilePath.Substring(0, wszInputFilePath.LastIndexOf(Path.DirectorySeparatorChar)), fileName);

                    FileStream fs = null;
                    try
                    {
                        // generate our target file content
                        byte[] data = GenerateContent(element);

                        // if data is null, it means to ignore the contents of the generated file
                        if (data == null) continue;

                        if (File.Exists(strFile))
                        {
                            // If the file already exists, only save the data
                            // if the generated file is different than the
                            // existing file.
                            byte[] oldData = File.ReadAllBytes(strFile);

                            bool equal = true;
                            if (oldData.Length == data.Length)
                            {
                                for (int i = 0; i < oldData.Length; i++)
                                    if (oldData[i] != data[i])
                                    {
                                        equal = false;
                                        break;
                                    }
                            }
                            else
                                equal = false;

                            if (!equal)
                                fs = File.Open(strFile, FileMode.Truncate);
                        }
                        else
                        {
                            // create the file
                            fs = File.Create(strFile);
                        }

                        if (fs != null)
                        {
                            // write it out to the stream
                            fs.Write(data, 0, data.Length);
                        }

                        // add the newly generated file to the solution, as a child of the source file
                        if (item.ProjectItems.Cast<EnvDTE.ProjectItem>()
                            .Where(pi => pi.Name == fileName).Count() == 0)
                        {
                            EnvDTE.ProjectItem itm = item.ProjectItems.AddFromFile(strFile);
                            /*
                             * Here you may wish to perform some addition logic
                             * such as, setting a custom tool for the target file if it
                             * is intented to perform its own generation process.
                             * Or, set the target file as an 'Embedded Resource' so that
                             * it is embedded into the final Assembly.
                         
                            EnvDTE.Property prop = itm.Properties.Item("CustomTool");
                            //// set to embedded resource
                            itm.Properties.Item("BuildAction").Value = 3;
                            if (String.IsNullOrEmpty((string)prop.Value) || !String.Equals((string)prop.Value, typeof(AnotherCustomTool).Name))
                            {
                                prop.Value = typeof(AnotherCustomTool).Name;
                            }
                            */
                        }
                    }
                    catch (Exception e)
                    {

                        //GeneratorProgress.GeneratorError( false, 0,
                        //    string.Format( "{0}\n{1}", e.Message, e.StackTrace ), -1, -1 );
                        GeneratorProgress.GeneratorError(0, 0, string.Format("{0}\n{1}", e.Message, e.StackTrace), 0, 0);
                        if (File.Exists(strFile))
                        {
                            File.WriteAllText(strFile,
                                              string.Format(
                                                  "An exception occured while running the {0} on this file. See the Error List for details.",
                                                  Const.FILEGENERATOR_IDENT));
                        }
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    // This is here for debugging purposes, as setting a breakpoint here can be very helpful
                    throw ex;
                }
            }

            // perform some clean-up, making sure we delete any old (stale) target-files
            foreach (EnvDTE.ProjectItem childItem in item.ProjectItems)
            {
                string next = string.Empty;
                DefaultExtension(out next);

                if (!(childItem.Name.EndsWith(next) || newFileNames.Contains(childItem.Name)))
                    // then delete it
                    childItem.Delete();
            }

            // generate our default content for our 'single' file
            byte[] defaultData = null;
            try
            {
                defaultData = GenerateDefaultContent();
            }
            catch (Exception ex)
            {
                //GeneratorProgress.GeneratorError( false, 0,
                //    string.Format( "{0}\n{1}", ex.Message, ex.StackTrace ), -1, -1 );
                GeneratorProgress.GeneratorError(0, 0,
                    string.Format("{0}\n{1}", ex.Message, ex.StackTrace), 0, 0);
            }

            if (defaultData == null)
                defaultData = new byte[0];

            // return our default data, so that Visual Studio may write it to disk.
            rgbOutputFileContents[0] = Marshal.AllocCoTaskMem(defaultData.Length);
            Marshal.Copy(defaultData, 0, rgbOutputFileContents[0], defaultData.Length);
            pcbOutput = (uint)defaultData.Length;

            return 0;
        }
    }
}