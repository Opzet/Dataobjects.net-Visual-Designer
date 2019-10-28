using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class EntityIndex : IEntityIndex
    {
        //private readonly string[] ATTRIBUTE_GROUPS = new[] {"Index"};
        public static readonly OrmAttributeGroup ATTRIBUTE_GROUP_INDEX = new OrmAttributeGroup("Index", OrmUtils.GetOrmNamespace(OrmNamespace.Orm));

        private const string ERROR_EMPTY_NAME_IN_INDEX_NAME =
"Name could not be empty when is set as 'Custom'.";

        private const string CODE_EMPTY_NAME_IN_INDEX_NAME = "EmptyNameInName";

        public const string ATTRIBUTE_GROUP_ITEM_UNIQUE = "Unique";
        public const string ATTRIBUTE_GROUP_ITEM_FILLFACTOR = "FillFactor";
        public const string ATTRIBUTE_GROUP_ITEM_NAME = "Name";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public EntityIndex(Store store, params PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public EntityIndex(Partition partition, params PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.Unique = new Defaultable<bool>();
            this.FillFactor = new Defaultable<double>();
            this.IndexName = new Defaultable<string> {Value = string.Empty};
            this.IndexName.SetAsDefault();
            this.Fields = new OrmIndexFields();
        }

        private string GetCalculatedNameValue()
        {
            bool isDefault = this.IndexName.IsDefault();
            return isDefault || string.IsNullOrEmpty(this.IndexName.Value) ? this.Name : this.IndexName.Value;
        }

        protected override void OnCopy(ModelElement sourceElement)
        {
            base.OnCopy(sourceElement);

            EntityIndex sourceIndex = (EntityIndex) sourceElement;

            this.Fields = sourceIndex.Fields.Clone();
            this.FillFactor = Common.ExtensionMethods.Clone(sourceIndex.FillFactor);
            this.IndexName = Common.ExtensionMethods.Clone(sourceIndex.IndexName);
            this.Unique = Common.ExtensionMethods.Clone(sourceIndex.Unique);
        }

        #region Implementation of IElement

        string IElement.Name
        {
            get { return this.namePropertyStorage; }
            set { namePropertyStorage = value; }
        }

        string IElement.Documentation
        {
            get { return string.Empty; }
        }

        #endregion

        #region Implementation of IEntityIndex

        Defaultable<bool> IEntityIndex.Unique
        {
            get { return this.Unique; }
        }

        Defaultable<double> IEntityIndex.FillFactor
        {
            get { return this.FillFactor; }
        }

        Defaultable<string> IEntityIndex.IndexName
        {
            get { return this.IndexName; }
        }

        OrmIndexFields IEntityIndex.Fields
        {
            get { return this.Fields; }
        }

        IInterface IEntityIndex.OwnerInterface
        {
            get { return this.InterfaceOfIndex; }
        }

        #endregion

        #region Implementation of IOrmAttribute

        public OrmAttributeGroup[] GetAttributeGroups(AttributeGroupsListMode listMode)
        {
            List<OrmAttributeGroup> result = new List<OrmAttributeGroup>
                                             {
                                                 ATTRIBUTE_GROUP_INDEX
                                             };

            return result.ToArray();
        }

        public List<Defaultable> GetAttributeGroupItems(AttributeGroupsListMode listMode)
        {
            var query = from attrGroup in GetAttributeGroups(listMode)
                        select GetAttributeGroupItems(attrGroup).Values;

            List<Defaultable> items = query.SelectMany(list => list).ToList();
            return items;
        }


        [Browsable(false)]
        OrmAttributeKind IOrmAttribute.AttributeKind
        {
            get
            {
                return OrmAttributeKind.Type;
            }
        }

        public Dictionary<string, Defaultable> GetAttributeGroupItems(OrmAttributeGroup @group)
        {
            var result = new Dictionary<string, Defaultable>();

            if (group == ATTRIBUTE_GROUP_INDEX)
            {
                result.Add(ATTRIBUTE_GROUP_ITEM_UNIQUE, Unique);
                result.Add(ATTRIBUTE_GROUP_ITEM_FILLFACTOR, FillFactor);
                result.Add(ATTRIBUTE_GROUP_ITEM_NAME, IndexName);
                this.Fields.FillAttributeGroupItems(/*group, */result);
            }

            return result;
        }

        public IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction)
        {
            EntityIndex other = (EntityIndex) otherAttribute;

            EntityIndex mergedResult = new EntityIndex((Store)null);
            mergedResult.Unique = this.Unique.Merge(other.Unique, mergeConflictAction);
            mergedResult.FillFactor = this.FillFactor.Merge(other.FillFactor, mergeConflictAction);
            mergedResult.IndexName = this.IndexName.Merge(other.IndexName, mergeConflictAction);

            mergedResult.Fields = this.Fields.MergeChanges(other.Fields, mergeConflictAction);

            return mergedResult;
        }

        public void Validate(ValidationContext context, ModelElement ownerElement)
        {
            if (this.IndexName.IsCustom() && string.IsNullOrEmpty(this.IndexName.Value))
            {
                context.LogError(ERROR_EMPTY_NAME_IN_INDEX_NAME, CODE_EMPTY_NAME_IN_INDEX_NAME,
                    new[]
                    {
                        ownerElement
                    });
            }
        }

        public void DeserializeFromXml(XmlReader reader)
        {
            XmlProxy xmlRoot = XmlProxySerializer.Instance.Deserialize(reader);
            if (xmlRoot.ElementName == "index")
            {
                XmlProxy xmlUnique = xmlRoot["unique"];
                this.Unique.DeserializeFromXml(xmlUnique);

                XmlProxy xmlFillFactor = xmlRoot["fillFactor"];
                this.FillFactor.DeserializeFromXml(xmlFillFactor);

                XmlProxy xmlIndexName = xmlRoot["name"];
                this.IndexName.DeserializeFromXml(xmlIndexName);

                XmlProxy xmlFields = xmlRoot["fields"];
                this.Fields.DeserializeFromXml(xmlFields);
            }
        }

        public void SerializeToXml(XmlWriter writer)
        {
            XmlProxy xmlRoot = new XmlProxy("index");
            
            XmlProxy xmlUnique = xmlRoot.AddChild("unique");
            this.Unique.SerializeToXml(xmlUnique);

            XmlProxy xmlFillFactor = xmlRoot.AddChild("fillFactor");
            this.FillFactor.SerializeToXml(xmlFillFactor);

            XmlProxy xmlIndexName = xmlRoot.AddChild("name");
            this.IndexName.SerializeToXml(xmlIndexName);

            XmlProxy xmlFields = xmlRoot.AddChild("fields");
            this.Fields.SerializeToXml(xmlFields);

            XmlProxySerializer.Instance.Serialize(xmlRoot, writer);
        }

        #endregion
    }
}