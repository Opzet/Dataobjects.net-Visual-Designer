using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [TypeConverter(typeof(OrmIndexFieldsTypeConverter))]
    [Serializable]
    public class OrmIndexFields : ISerializableObject
    {
        internal const string TAG_KEY_FIELDS = "KeyFields";
        internal const string TAG_NAME_KEY_FIELDS = "Key Fields";
        
        internal const string TAG_INCLUDED_FIELDS = "IncludedFields";
        internal const string TAG_NAME_INCLUDED_FIELDS = "Included Fields";

        public const string ATTRIBUTE_GROUP_ITEM_KEY_FIELDS = "KeyFields";
        public const string ATTRIBUTE_GROUP_ITEM_INCLUDED_FIELDS = "IncludedFields";

        [DisplayName("Key Fields")]
        [Description("List of key field names.")]
        [Editor(typeof(ModalDialogEditor<FormItemsEditor>), typeof(UITypeEditor))]
        [ItemsEditorAttribute(ItemsEditorOptions.UniqueDisplayValues | ItemsEditorOptions.SortingIcons, typeof(OrmIndexFieldsValidator), TAG_KEY_FIELDS, 
            EditorCaption = "Key Fields Editor", EditorTitle = "Key Fields:")]
        public TreeNodeDataCollection<StringTreeNode> KeyFields { get; set; }

        [DisplayName("Included Fields")]
        [Description("List of included field names, that must be included into the index.")]
        [Editor(typeof(ModalDialogEditor<FormItemsEditor>), typeof(UITypeEditor))]
        [ItemsEditorAttribute(ItemsEditorOptions.UniqueDisplayValues, typeof(OrmIndexFieldsValidator), TAG_INCLUDED_FIELDS,
            EditorCaption = "Included Fields Editor", EditorTitle = "Included Fields:")]
        public TreeNodeDataCollection<StringTreeNode> IncludedFields { get; set; }

        public OrmIndexFields()
        {
            this.KeyFields = new TreeNodeDataCollection<StringTreeNode>();
            this.IncludedFields = new TreeNodeDataCollection<StringTreeNode>();
        }

        public override string ToString()
        {
            return string.Format("Keys: {0}, Included: {1}", KeyFields.Count, IncludedFields.Count);
        }

        public void DeserializeFromXml(XmlReader reader)
        {
            XmlProxy xmlRoot = XmlProxySerializer.Instance.Deserialize(reader);
            DeserializeFromXml(xmlRoot);
        }

        public void DeserializeFromXml(XmlProxy parent)
        {
            if (parent.ElementName == "fields")
            {
                this.KeyFields.DeserializeFromXml(parent, "keyFields");
                this.IncludedFields.DeserializeFromXml(parent, "includedFields");
            }
        }

        public void SerializeToXml(XmlWriter writer)
        {
            XmlProxy xmlRoot = new XmlProxy("fields");
            SerializeToXml(xmlRoot);

            XmlProxySerializer.Instance.Serialize(xmlRoot, writer);
        }

        public void SerializeToXml(XmlProxy parent)
        {
            this.KeyFields.SerializeToXml(parent, "keyFields");
            this.IncludedFields.SerializeToXml(parent, "includedFields");
        }

        public void FillAttributeGroupItems(/*OrmAttributeGroup @group,*/ Dictionary<string, Defaultable> result)
        {
            string keyFields = string.Join(",",
                KeyFields.Select(
                    node =>
                    string.Format("{0}{1}", node.DisplayValue,
                        node.IconIndex == TreeNodeProxy.ICON_INDEX_SORT_DESC ? ":DESC" : string.Empty)));

            Defaultable<string[]> defKeyFields = new Defaultable<string[]>();
            defKeyFields.SetAsCustom(keyFields.Split(','));
            result.Add(ATTRIBUTE_GROUP_ITEM_KEY_FIELDS, defKeyFields);

            string includedFields = string.Join(",", IncludedFields.Select(node => node.DisplayValue));
            Defaultable<string[]> defIncludedFields = new Defaultable<string[]>();
            if (string.IsNullOrEmpty(includedFields))
            {
                defIncludedFields.SetAsDefault();
            }
            else
            {
                defIncludedFields.SetAsCustom(includedFields.Split(','));
            }
            result.Add(ATTRIBUTE_GROUP_ITEM_INCLUDED_FIELDS, defIncludedFields);
        }

        public OrmIndexFields MergeChanges(OrmIndexFields other, MergeConflictAction mergeConflictAction)
        {
            OrmIndexFields mergedResult = new OrmIndexFields();
            var defaultableFields = GetDefaultableFields();
            var otherDefaultableFields = other.GetDefaultableFields();

            var mergedKeyFields = defaultableFields.Item1.Merge(otherDefaultableFields.Item1, mergeConflictAction);
            var mergedKeyFieldsItems = mergedKeyFields.Value.Split(',');
            mergedResult.KeyFields = new TreeNodeDataCollection<StringTreeNode>();
            foreach (string mergedKeyFieldsItem in mergedKeyFieldsItems)
            {
                mergedResult.KeyFields.Add(new StringTreeNode{DisplayValue = mergedKeyFieldsItem});
            }

            var mergedIncludedFields = defaultableFields.Item2.Merge(otherDefaultableFields.Item2, mergeConflictAction);
            var mergedIncludedFieldsItems = mergedIncludedFields.Value.Split(',');
            mergedResult.IncludedFields = new TreeNodeDataCollection<StringTreeNode>();
            foreach (string mergedIncludedFieldsItem in mergedIncludedFieldsItems)
            {
                mergedResult.IncludedFields.Add(new StringTreeNode { DisplayValue = mergedIncludedFieldsItem });
            }

            return mergedResult;
        }

        private Tuple<Defaultable<string>, Defaultable<string>> GetDefaultableFields() 
        {
            Dictionary<string, Defaultable> fields = new Dictionary<string, Defaultable>();
            FillAttributeGroupItems(/*OrmAttributeGroup.Empty, */fields);
            Defaultable<string> keyFields = (Defaultable<string>) fields["KeyFields"];
            Defaultable<string> includedFields = (Defaultable<string>) fields["IncludedFields"];

            return new Tuple<Defaultable<string>, Defaultable<string>>(keyFields, includedFields);
        }

        public bool EqualsTo(OrmIndexFields other)
        {
            bool isEqual = this.KeyFields.Count == other.KeyFields.Count &&
                           this.IncludedFields.Count == other.IncludedFields.Count;

            if (isEqual)
            {
                isEqual = this.KeyFields.Select(item => string.Format("{0}@{1}", item.DisplayValue, item.IconIndex))
                            .Except(other.KeyFields.Select(item => string.Format("{0}@{1}", item.DisplayValue, item.IconIndex)))
                            .Count() == 0;

                isEqual &= this.IncludedFields.Select(item => string.Format("{0}@{1}", item.DisplayValue, item.IconIndex))
                            .Except(other.IncludedFields.Select(item => string.Format("{0}@{1}", item.DisplayValue, item.IconIndex)))
                            .Count() == 0;
            }

            return isEqual;
        }
    }

    #region class OrmIndexFieldsTypeConverter

    public class OrmIndexFieldsTypeConverter : ExpandableObjectConverter
    {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            TreeNodeDataCollection<StringTreeNode> keyFields =
                (TreeNodeDataCollection<StringTreeNode>) propertyValues["KeyFields"];

            TreeNodeDataCollection<StringTreeNode> includedFields =
                (TreeNodeDataCollection<StringTreeNode>) propertyValues["IncludedFields"];

            OrmIndexFields result = new OrmIndexFields();
            result.KeyFields = keyFields;
            result.IncludedFields = includedFields;
            return result;
        }
    }

    #endregion class OrmIndexFieldsTypeConverter

    #region class OrmIndexFieldsValidator

    public class OrmIndexFieldsValidator : ItemsEditorValidatorBase<OrmIndexFields>
    {
        public override bool Validate(IEnumerable<ITreeNodeData> items, out string errorMessage)
        {
            errorMessage = string.Empty;
            TreeNodeDataCollection<StringTreeNode> oppositeItems;
            string tagName;

            if (this.Owner.Tag == OrmIndexFields.TAG_KEY_FIELDS)
            {
                oppositeItems = this.Container.IncludedFields;
                tagName = OrmIndexFields.TAG_INCLUDED_FIELDS;
            }
            else
            {
                oppositeItems = this.Container.KeyFields;
                tagName = OrmIndexFields.TAG_KEY_FIELDS;
            }

            var duplicateItems = items.Select(sourceItem => sourceItem.DisplayValue).Intersect(
                oppositeItems.Select(oppositeItem => oppositeItem.DisplayValue));

            bool isValid = duplicateItems.Count() == 0;
            if (!isValid)
            {
                string sameItems = string.Join(",", duplicateItems.ToArray());

                errorMessage = string.Format("Property '{0}' already contains fields: {1}", tagName, sameItems);
            }

            return isValid;
        }
    }

    #endregion class OrmIndexFieldsValidator
}