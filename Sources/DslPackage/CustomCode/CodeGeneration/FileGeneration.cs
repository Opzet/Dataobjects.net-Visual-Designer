using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using Microsoft.Win32;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
//    [ProvideCodeGenerator(typeof(DONetFileGenerator), Const.FILEGENERATOR_IDENT,
//    "Generates implementations of the entity model describled in .dom files", true)]
//    internal sealed partial class DONetEntityModelDesignerPackage
//    {
//
//    }

/*
    [CLSCompliant(false)]
    [Guid("93C1D978-B162-49F7-A658-32830315944C")]
    public class DONetFileGenerator : VsMultipleFileGenerator<string>
    {
        internal static string ToolVersion
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                return assembly.GetName().Version.ToString();
            }
        }

        #region class TemplateCodeGenerator

        /// <summary>
        /// An adaptor class on the TemplatedCodeGenerator in order to
        /// access the otherwise protected method GenerateCode, which we
        /// need to access from outside the class.
        /// 
        /// This class is used to run text transformations as simply as
        /// possible.
        /// </summary>
        private class TemplateCodeGenerator : TemplatedCodeGenerator
        {
            public new byte[] GenerateCode(string inputFileName, string inputFileContent)
            {
                return base.GenerateCode(inputFileName, inputFileContent);
            }
        }

        #endregion class TemplateCodeGenerator

        private TemplateCodeGenerator _templateCodeGenerator;
        private TemplateCodeGenerator TemplateGenerator
        {
            get
            {
                if (_templateCodeGenerator == null)
                {
                    _templateCodeGenerator = new TemplateCodeGenerator();
                    _templateCodeGenerator.SetSite(Site);
                }
                return _templateCodeGenerator;
            }
        }

//        private CodeFileGenerator CodeFileGenerator
//        {
//            get
//            {
//                return new CodeFileGenerator(CodeProvider, RootNamespace);
//            }
//        }

//        private string CodeFileExtension
//        {
//            get
//            {
//                string codeFileExtension = CodeProvider.FileExtension;
//                if (codeFileExtension.StartsWith("."))
//                    codeFileExtension = codeFileExtension.Substring(1);
//                return codeFileExtension;
//            }
//        }

        /// <summary>
        /// Returns a list of file extensions to generate or preserve
        /// </summary>
        /// <returns>A list of file extensions to generate or preserve</returns>
        public override IEnumerator<string> GetEnumerator()
        {
            // "Borrow" a List<>'s Enumerator to do our job for us
            List<string> fileExtensionList = new List<string>();

            //fileExtensionList.Add(CodeFileExtension);
            fileExtensionList.Add("tt");
            fileExtensionList.Add("diagram");
//            fileExtensionList.Add("config");
//            fileExtensionList.Add("xsd");
            return fileExtensionList.GetEnumerator();
        }

        /// <summary>
        /// Returns the filename that matches the file extension given.
        /// </summary>
        /// <param name="fileExtension">One of the file extensions from the <see cref="GetEnumerator()"/> method.</param>
        /// <returns>The name of the input file plus the file extension.</returns>
        protected override string GetFileName(string fileExtension)
        {
            FileInfo fi = new FileInfo(InputFilePath);
            return string.Format("{0}.{1}", fi.Name, fileExtension);
        }

        /// <summary>
        /// Generates the contents of the given file extension.
        /// If null is returned, it means to preserve the existing file instead
        /// of generating a new one.
        /// </summary>
        /// <param name="fileExtension">One of the file extensions from the <see cref="GetEnumerator()"/> method.</param>
        /// <returns>The generated content for the given file extension, or null.</returns>
        private byte[] GenerateAllContent(string fileExtension)
        {
            string inputFileContent;

            // For debugging purposes, it is better to be able to edit the .tt files without having to
            // recompile the solution to test changes. During release, use the embedded .tt files
            // instead.
            switch (fileExtension)
            {
                case "tt":
                    //inputFileContent = File.ReadAllText(Path.Combine(TextTemplateFolder, "DONetEntityModelDesigner.tt"));
                    var inputFileContentBytes = File.ReadAllBytes(Path.Combine(TextTemplateFolder, Const.MODEL_TEMPLATE_FILE));
                    return inputFileContentBytes;
                    break;

                // Preserve the diagram and cs files, don't write anything to them
                case "diagram":
                    return null;

//                case "config":
//                    inputFileContent = File.ReadAllText(Path.Combine(TextTemplateFolder, "ConfigurationSectionDesignerSample.tt"));
//                    break;
//
//                case "xsd":
//                    inputFileContent = File.ReadAllText(Path.Combine(TextTemplateFolder, "ConfigurationSectionDesignerSchema.tt"));
//                    break;
//
                default:
                    //if (fileExtension == CodeFileExtension)
                        return null;
//                    else if (fileExtension == string.Format("{0}-gen", CodeFileExtension))
//                    {
//                        return CodeFileGenerator.GenerateCode(InputFilePath);
//                    }
//                    else
                        throw new ApplicationException("Unhandled file content");
            }

            // Replace our input file name placeholder with the real input file name
            // so the text transformer knows which .dom file to work on.
            inputFileContent = inputFileContent.Replace("$inputFileName$", InputFilePath);
            return TemplateGenerator.GenerateCode(InputFilePath, inputFileContent);
        }

        public override byte[] GenerateContent(string element)
        {
            return GenerateAllContent(element);
        }

        public override int DefaultExtension(out string pbstrDefaultExtension)
        {
            try
            {
                //pbstrDefaultExtension = string.Format(".dom.{0}", CodeFileExtension);
                pbstrDefaultExtension = string.Format(".dom.{0}", "tt");
                return S_OK;
            }
            catch (Exception)
            {
                pbstrDefaultExtension = string.Empty;
                return E_FAIL;
            }

        }

        public override byte[] GenerateDefaultContent()
        {
            //return GenerateAllContent(string.Format("{0}-gen", CodeFileExtension));
            //return GenerateAllContent(string.Format("{0}", CodeFileExtension));
            return GenerateAllContent(string.Format("{0}", "tt"));
        }

        private string _textTemplateFolder;
        private string TextTemplateFolder
        {
            get
            {

                if (_textTemplateFolder == null)
                {

                    // Fetch the install location from the registry
                    string key = string.Format("{0}\\ExtensionManager\\EnabledExtensions",
                            this.Project.DTE.RegistryRoot);
                    RegistryKey regKey = Registry.CurrentUser.OpenSubKey(key);
                    if (regKey != null)
                    {
                        string kn = string.Format("{0},{1}",
                                          Constants.DONetEntityModelDesignerPackageId,
                                          Assembly.GetExecutingAssembly().GetName().Version.ToString()
                                      );
                        _textTemplateFolder = System.IO.Path.Combine(regKey.GetValue(kn) as string, "TextTemplates");
                    }
                    else
                        throw new InvalidOperationException("Could not find TextTemplate directory. Try reinstalling the DataObjects.Net Entit Model Designer.");
                    //#endif
                }
                return _textTemplateFolder;
            }
        }

    }
*/
}