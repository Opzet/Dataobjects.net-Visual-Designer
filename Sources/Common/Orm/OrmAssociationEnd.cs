using System;
using System.Collections;
using System.ComponentModel;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using ElementEventArgs = TXSoftware.DataObjectsNetEntityModel.Common.Modeling.ElementEventArgs;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [Serializable]
    [TypeConverter(typeof(OrmAssociationEndTypeConverter))]
    public sealed class OrmAssociationEnd: ISerializableObject
    {
        private ModelElement owner;

        internal Defaultable<string> pairTo;
        internal MultiplicityKind multiplicity;
        private string endId;

        [Description("")]
        public MultiplicityKind Multiplicity
        {
            get { return multiplicity; }
            set
            {
                if (value != multiplicity)
                {
                    multiplicity = value;

                    NotifyValueChangeToOwner("Multiplicity", endId);
                }
            }
        }

        [Description("Action that will be executed in case that owner Entity (the owner of the reference field) is about to be removed.")]
        [DisplayName("On Owner Remove")]
        public AssociationOnRemoveAction OnOwnerRemove { get; set; }

        [Description("Action that will be executed in case that target (referenced) Entity is about to be removed.")]
        [DisplayName("On Target Remove")]
        public AssociationOnRemoveAction OnTargetRemove { get; set; }

        [Description("Tells if property will be decorated with 'AssociationAttribute' or not.")]
        [DisplayName("Use association attribtue")]
        public bool UseAssociationAttribute { get; set; }

        [Description("Indicates that association (persistent collection or persistent field) is inverse end of another another collection or reference field.")]
        [DisplayName("Pair To")]
        public Defaultable<string> PairTo
        {
            get { return pairTo; }
            set
            {
                if (!value.EqualsTo(pairTo))
                {
                    pairTo = value;

                    NotifyValueChangeToOwner("PairTo", endId);
                }
            }
        }

        public OrmAssociationEnd(ModelElement owner, string endId): this()
        {
            this.owner = owner;
            this.endId = endId;
        }

        public OrmAssociationEnd()
        {
            this.OnOwnerRemove = AssociationOnRemoveAction.Default;
            this.OnTargetRemove = AssociationOnRemoveAction.Default;
            this.UseAssociationAttribute = true;
            this.pairTo = new Defaultable<string>();
            this.pairTo.IsDefault();
        }

        internal void NotifyValueChangeToOwner(string changedProperty, string calledFromEndId)
        {
            IElementEventsHandler eventsHandler = owner as IElementEventsHandler;
            if (eventsHandler != null)
            {
                ElementEventArgs arg = new ElementEventArgs(this, changedProperty, calledFromEndId);
                eventsHandler.HandleEvent(arg);
            }
        }

        public override string ToString()
        {
            string pairTo = !this.PairTo.IsDefault() ? this.PairTo.Value : null;
            return string.IsNullOrEmpty(pairTo) ? "Association" : string.Format("Pair To: {0}", PairTo);
        }

        public void AssignInternalsFrom(OrmAssociationEnd other)
        {
            this.owner = other.owner;
            this.endId = other.endId;
        }

        public void DeserializeFromXml(XmlReader reader)
        {
            DeserializeFromXml(reader, "end");
        }

        public void DeserializeFromXml(XmlReader reader, string propertyName)
        {
            XmlProxy xmlRoot = XmlProxySerializer.Instance.Deserialize(reader);
            if (xmlRoot.ElementName == propertyName)
            {
                string strMultiplicity = xmlRoot.GetAttr("multiplicity");
                MultiplicityKind multiplicityKind;
                if (Enum.TryParse(strMultiplicity, true, out multiplicityKind))
                {
                    this.multiplicity = multiplicityKind;
                }

                string strOnOwnerRemove = xmlRoot.GetAttr("onOwnerRemove");
                AssociationOnRemoveAction onOwnerRemove;
                if (Enum.TryParse(strOnOwnerRemove, true, out onOwnerRemove))
                {
                    this.OnOwnerRemove = onOwnerRemove;
                }

                string strOnTargetRemove = xmlRoot.GetAttr("onTargetRemove");
                AssociationOnRemoveAction onTargetRemove;
                if (Enum.TryParse(strOnTargetRemove, true, out onTargetRemove))
                {
                    this.OnTargetRemove = onTargetRemove;
                }

                this.UseAssociationAttribute = xmlRoot.GetAttr("useAssociationAttribute", true);

                XmlProxy xmlPairTo = xmlRoot["pairTo"];
                this.PairTo = new Defaultable<string>();
                this.PairTo.DeserializeFromXml(xmlPairTo);
            }
        }

        public void SerializeToXml(XmlWriter writer)
        {
            SerializeToXml(writer, "end");
        }

        public void SerializeToXml(XmlWriter writer, string propertyName)
        {
            XmlProxy xmlRoot = new XmlProxy(propertyName);
            xmlRoot.AddAttribute("multiplicity", multiplicity.ToString());
            xmlRoot.AddAttribute("onOwnerRemove", OnOwnerRemove.ToString());
            xmlRoot.AddAttribute("onTargetRemove", OnTargetRemove.ToString());
            xmlRoot.AddAttribute("useAssociationAttribute", this.UseAssociationAttribute);

            XmlProxy xmlPairTo = xmlRoot.AddChild("pairTo");
            this.PairTo.SerializeToXml(xmlPairTo);

            XmlProxySerializer.Instance.Serialize(xmlRoot, writer);
        }

        public bool EqualsTo(OrmAssociationEnd other)
        {
            return this.multiplicity == other.multiplicity &&
                   this.OnOwnerRemove == other.OnOwnerRemove &&
                   this.OnTargetRemove == other.OnTargetRemove &&
                   this.UseAssociationAttribute == other.UseAssociationAttribute &&
                   this.pairTo.EqualsTo(other.pairTo);
        }
    }

    #region class OrmAssociationEndTypeConverter

    public class OrmAssociationEndTypeConverter : ExpandableObjectConverter
    {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            ModelElement owner = context.Instance as ModelElement;
            OrmAssociationEnd currentObj = (OrmAssociationEnd) context.PropertyDescriptor.GetValue(context.Instance);

            MultiplicityKind otherMultiplicity = (MultiplicityKind) propertyValues["Multiplicity"];
            AssociationOnRemoveAction otherOnOwnerRemove = (AssociationOnRemoveAction) propertyValues["OnOwnerRemove"];
            AssociationOnRemoveAction otherOnTargetRemove = (AssociationOnRemoveAction) propertyValues["OnTargetRemove"];
            Defaultable<string> otherPairTo = (Defaultable<string>) propertyValues["PairTo"];
            bool useAssociationAttribute = (bool) propertyValues["UseAssociationAttribute"];

            bool changedMultiplicity = currentObj.Multiplicity != otherMultiplicity;
            bool changedPairTo = !currentObj.PairTo.EqualsTo(otherPairTo);

            string endId = context.PropertyDescriptor.Name;

            OrmAssociationEnd result = new OrmAssociationEnd(owner, endId)
                                           {
                                               multiplicity = otherMultiplicity,
                                               OnOwnerRemove = otherOnOwnerRemove,
                                               OnTargetRemove = otherOnTargetRemove,
                                               pairTo = otherPairTo,
                                               UseAssociationAttribute = useAssociationAttribute
                                           };

//            if (changedMultiplicity)
//            {
//                result.NotifyValueChangeToOwner("Multiplicity");
//            }

            //if (changedPairTo)
            {
                result.NotifyValueChangeToOwner(string.Empty, endId);
            }

            return result;
        }
    }

    #endregion class OrmAssociationEndTypeConverter
}