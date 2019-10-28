using System;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public sealed partial class DONetEntityModelDesignerSerializationHelper
    {
/*
        protected override void CheckVersion(SerializationContext serializationContext, XmlReader reader)
        {
            #region Check Parameters
			global::System.Diagnostics.Debug.Assert(serializationContext != null);
			if (serializationContext == null)
				throw new global::System.ArgumentNullException("serializationContext");
			global::System.Diagnostics.Debug.Assert(reader != null);
			if (reader == null)
				throw new global::System.ArgumentNullException("reader");
			#endregion
	
			string oldVersion = reader.GetAttribute("dslVersion");
            if (oldVersion != null)
            {
                Version actualVersion = new Version(oldVersion);
                try
                {
                    if (!UpgradeVersions.CanUpgrade(actualVersion, Util.CurrentVersion))
                    {
                        DONetEntityModelDesignerSerializationBehaviorSerializationMessages.VersionMismatch(serializationContext, reader, Util.CurrentVersion, actualVersion);
                    }
                }
                catch (global::System.ArgumentException)
                {
                    DONetEntityModelDesignerSerializationBehaviorSerializationMessages.InvalidPropertyValue(serializationContext, reader, "dslVersion", typeof(global::System.Version), oldVersion);
                }
                catch (global::System.FormatException)
                {
                    DONetEntityModelDesignerSerializationBehaviorSerializationMessages.InvalidPropertyValue(serializationContext, reader, "dslVersion", typeof(global::System.Version), oldVersion);
                }
                catch (global::System.OverflowException)
                {
                    DONetEntityModelDesignerSerializationBehaviorSerializationMessages.InvalidPropertyValue(serializationContext, reader, "dslVersion", typeof(global::System.Version), oldVersion);
                }
            }
        }
*/
    }
}