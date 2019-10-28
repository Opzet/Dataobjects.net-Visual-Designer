using System;
using System.IO;
using Microsoft.VisualStudio.Shell;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    // Attribute used to register our custom tool with Visual Studio

    class FileGenerationRegistrationAttribute : RegistrationAttribute
    {
        private string _packageGuid;
        private string _generatorClsid;
        private string _editorFactoryGuid;
        private Type _generatorType;

        private const string CSharpGeneratorsGuid = "{fae04ec1-301f-11d3-bf4b-00c04f79efbc}";
        private const string CSharpProjectGuid = "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}";

        private const string VBGeneratorsGuid = "{164B10B9-B200-11D0-8C61-00A0C91E29D5}";
        private const string VBProjectGuid = "{F184B08F-C81C-45f6-A57F-5ABD9991F28F}";

        public FileGenerationRegistrationAttribute(string packageGuid, string generatorClsid, string editorFactoryGuid, Type generatorType)
        {
            if (packageGuid == null)
                throw new ArgumentNullException("packageGuid");
            if (generatorClsid == null)
                throw new ArgumentNullException("generatorClsid");
            if (editorFactoryGuid == null)
                throw new ArgumentNullException("editorFactoryGuid");
            if (generatorType == null)
                throw new ArgumentNullException("generatorType");

            _packageGuid = packageGuid;
            _generatorClsid = generatorClsid;
            _generatorType = generatorType;
            _editorFactoryGuid = editorFactoryGuid;
        }

        public override void Register(RegistrationAttribute.RegistrationContext context)
        {
            try
            {
                context.Log.Write(string.Format("Registering {0} ... ", Const.FILEGENERATOR_NAME));

                // register class
                Key key = context.CreateKey(@"CLSID");
                Key subKey = key.CreateSubkey(_generatorClsid);
                subKey.SetValue("ThreadingModel", "Both");
                subKey.SetValue("InprocServer32", Path.Combine(Environment.SystemDirectory, "mscoree.dll"));
                subKey.SetValue("Class", _generatorType.FullName);
                subKey.SetValue("Assembly", _generatorType.Assembly.FullName);
                subKey.Close();
                key.Close();

                // register custom generator
                key = context.CreateKey(@"Generators\" + CSharpGeneratorsGuid);
                subKey = key.CreateSubkey(Const.FILEGENERATOR_IDENT);
                subKey.SetValue(string.Empty, Const.FILEGENERATOR_NAME);
                subKey.SetValue("CLSID", _generatorClsid);
                subKey.SetValue("GeneratesDesignTimeSource", 1);
                subKey.Close();
                key.Close();

                key = context.CreateKey(@"Generators\" + VBGeneratorsGuid);
                subKey = key.CreateSubkey(Const.FILEGENERATOR_IDENT);
                subKey.SetValue(string.Empty, Const.FILEGENERATOR_NAME);
                subKey.SetValue("CLSID", _generatorClsid);
                subKey.SetValue("GeneratesDesignTimeSource", 1);
                subKey.Close();
                key.Close();

                // register .dom editor notification
                key = context.CreateKey(@"Projects\" + CSharpProjectGuid + @"\FileExtensions");
                subKey = key.CreateSubkey(Const.DESIGNER_FILE_EXT);
                subKey.SetValue("EditorFactoryNotify", _editorFactoryGuid);
                subKey.Close();
                key.Close();
                context.Log.WriteLine("Success.");

                key = context.CreateKey(@"Projects\" + VBProjectGuid + @"\FileExtensions");
                subKey = key.CreateSubkey(Const.DESIGNER_FILE_EXT);
                subKey.SetValue("EditorFactoryNotify", _editorFactoryGuid);
                subKey.Close();
                key.Close();
                context.Log.WriteLine("Success.");
            }
            catch (Exception e)
            {
                context.Log.WriteLine("Failure: " + e);
            }
        }

        public override void Unregister(RegistrationAttribute.RegistrationContext context)
        {
            try
            {
                context.Log.Write(string.Format("Unregistering {0} ... ", Const.FILEGENERATOR_NAME));

                context.RemoveKey(@"CLSID\" + _generatorClsid);
                context.RemoveKey(@"Generators\" + CSharpGeneratorsGuid + @"\" + Const.FILEGENERATOR_IDENT);
                context.RemoveKey(@"Generators\" + VBGeneratorsGuid + @"\" + Const.FILEGENERATOR_IDENT);
                context.RemoveKey(@"Projects\" + CSharpProjectGuid + @"\FileExtensions\" + Const.DESIGNER_FILE_EXT);
                context.RemoveKey(@"Projects\" + VBProjectGuid + @"\FileExtensions\" + Const.DESIGNER_FILE_EXT);
                context.Log.WriteLine("Success.");
            }
            catch (Exception e)
            {
                context.Log.WriteLine("Failure: " + e);
            }
        }
    }
}