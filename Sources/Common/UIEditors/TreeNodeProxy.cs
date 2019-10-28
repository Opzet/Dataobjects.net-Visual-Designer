using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    #region interface ITreeNodeData

    public interface ITreeNodeData
    {
        int IconIndex { get; set; }
        string DisplayValue { get; set; }
        void SerializeToXml(XmlProxy parent, object tag);
        void DeserializeFromXml(XmlProxy parent);
    }

    #endregion interface ITreeNodeData

    #region class StringTreeNode

    public class StringTreeNode : ITreeNodeData
    {
        public int IconIndex { get; set; }

        public string DisplayValue { get; set; }

        public StringTreeNode()
        {
            this.IconIndex = TreeNodeProxy.ICON_INDEX_DEFAULT;
        }

        public void SerializeToXml(XmlProxy parent, object tag)
        {
            parent.AddAttribute("value", DisplayValue);
            parent.AddAttribute("image", IconIndex);
            int order = (int) tag;
            parent.AddAttribute("order", order);
        }

        public void DeserializeFromXml(XmlProxy parent)
        {
            this.DisplayValue = parent.GetAttr("value");
            this.IconIndex = parent.GetAttr("image", 0);
        }

        public override string ToString()
        {
            return DisplayValue;
        }
    }

    #endregion class StringTreeNode

    #region TreeNodeDataCollection<T>

    [TypeConverter(typeof (TreeNodeDataConverter))]
    public class TreeNodeDataCollection<T> : List<T> where T : ITreeNodeData, new()
    {
        public void SerializeToXml(XmlProxy parent, string itemName)
        {
            XmlProxy xmlChild = parent.AddChild(itemName);
            int order = 0;
            foreach (T item in this)
            {
                var xmlItem = xmlChild.AddChild("item");

                item.SerializeToXml(xmlItem, order);
                order++;
            }
        }

        public void DeserializeFromXml(XmlProxy parent, string itemName)
        {
            var xmlChild = parent[itemName];
            if (xmlChild.ElementName == itemName)
            {
                this.Clear();
                var xmlChilds = xmlChild.Childs.OrderBy(child => child.GetAttr("order", 0));

                foreach (XmlProxy xmlItem in xmlChilds)
                {
                    T newItem = new T();
                    newItem.DeserializeFromXml(xmlItem);
                    this.Add(newItem);
                }
            }
        }
    }

    #endregion TreeNodeDataCollection<T>

    #region class TreeNodeDataConverter

    public class TreeNodeDataConverter : ArrayConverter
    {
        private static readonly Type baseCollectionType = typeof (TreeNodeDataCollection<>);
        private const int MAX_LIMIT_ITEM_COUNT = 10;

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            //TEST
            //return true;
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            //TEST
            //return true;
            return false;
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            object result = base.ConvertFrom(context, culture, value);
            return result;
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            //bool supported = true;
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            object instance = base.CreateInstance(context, propertyValues);
            return instance;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            IEnumerable<ITreeNodeData> sourceCollection = value as IEnumerable<ITreeNodeData>;

            if (destinationType == typeof(string))
            {
                if (sourceCollection == null)
                {
                    return base.ConvertTo(context, culture, value, destinationType);
                }

                bool limitItemCountReached = sourceCollection.Count() == MAX_LIMIT_ITEM_COUNT;
                StringBuilder itemList = sourceCollection.Take(MAX_LIMIT_ITEM_COUNT).Aggregate(new StringBuilder(),
                    (builder, data) =>
                    builder.AppendFormat("{0}{1},", data.DisplayValue,
                        data.IconIndex == TreeNodeProxy.ICON_INDEX_SORT_DESC
                            ? ":DESC"
                            : string.Empty),
                    stringBuilder =>
                    {
                        if (stringBuilder.Length > 0)
                        {
                            StringBuilder sb = stringBuilder.Remove(stringBuilder.Length - 1, 1);
                            if (limitItemCountReached)
                            {
                                sb.Append("...");
                            }

                            return sb;
                        }

                        stringBuilder.Append("(Empty)");
                        return stringBuilder;
                    });

                return itemList.ToString();
            }


            Type itemType = destinationType.GetGenericArguments().Single();
            Type genericCollectionType = baseCollectionType.MakeGenericType(itemType);
            object targetInstance = Activator.CreateInstance(genericCollectionType);
            IList targetCollection = (IList) targetInstance;

            foreach (ITreeNodeData sourceItem in sourceCollection)
            {
                ITreeNodeData targetItem = sourceItem;
                if (sourceItem is ICloneable)
                {
                    targetItem = (ITreeNodeData) (sourceItem as ICloneable).Clone();
                }

                targetCollection.Add(targetItem);
            }

            return targetCollection;
        }
    }

    #endregion class TreeNodeDataConverter

    #region class TreeNodeProxy

    internal abstract class TreeNodeProxy
    {
        public const string NEW_ITEM_NODE_TEXT = "-click to add new item-";

        public const int ICON_INDEX_DEFAULT = 0;
        public const int ICON_INDEX_SORT_ASC = 1;
        public const int ICON_INDEX_SORT_DESC = 2;

        public bool IsNewItemNode { get; set; }

        internal abstract ITreeNodeData GetNodeData();
    }

    internal class TreeNodeProxy<T> : TreeNodeProxy where T : ITreeNodeData
    {
        public T Item { get; set; }

        public TreeNodeProxy(T item, bool isNewItemNode)
        {
            Item = item;
            IsNewItemNode = isNewItemNode;
        }

        internal override ITreeNodeData GetNodeData()
        {
            return Item;
        }
    }

    #endregion class TreeNodeData

    #region class TreeNodeExtensions

    internal static class TreeNodeExtensions
    {
        internal static void SetNodeProxy(this TreeNode @this, TreeNodeProxy nodeProxy)
        {
            @this.Tag = nodeProxy;
        }

        internal static TreeNodeProxy GetNodeProxy(this TreeNode @this)
        {
            return @this == null ? null : @this.Tag as TreeNodeProxy;
        }

        //public static T GetNodeData<T>(this TreeNode @this) where T : ITreeNodeData
        public static ITreeNodeData GetNodeData(this TreeNode @this)
        {
            if (@this == null)
            {
                return null;
            }

            TreeNodeProxy nodeProxy = @this.GetNodeProxy();
            return nodeProxy.GetNodeData();
        }

        public static bool IsNewItemNode(this TreeNode @this)
        {
            var nodeProxy = @this.GetNodeProxy();
            return nodeProxy.IsNewItemNode;
        }

        public static IEnumerable<TreeNodeProxy> AllNodeProxy(this LabelEditEnhancedTreeView @this)
        {
            return @this.Nodes.OfType<TreeNode>().Select(node => node.GetNodeProxy());
        }

        public static IEnumerable<ITreeNodeData> AllNodeData(this LabelEditEnhancedTreeView @this)
        {
            return @this.Nodes.OfType<TreeNode>().Select(node => node.GetNodeData());
        }

        public static void ForEachNodeData(this LabelEditEnhancedTreeView @this, Predicate<ITreeNodeData> predicate)
        {
            foreach (TreeNode treeNode in @this.Nodes)
            {
                predicate(treeNode.GetNodeData());
            }
        }

        public static bool HasNewItemNode(this LabelEditEnhancedTreeView @this)
        {
            foreach (TreeNode treeNode in @this.Nodes)
            {
                if (treeNode.GetNodeProxy().IsNewItemNode)
                {
                    return true;
                }
            }

            return false;
        }

        public static TreeNode CreateNewItemNode(this LabelEditEnhancedTreeView @this)
        {
            ITreeNodeData treeNodeData = @this.CreateNewNodeData(TreeNodeProxy.NEW_ITEM_NODE_TEXT);
            TreeNode treeNode = CreateNode(@this, treeNodeData);
            treeNode.ForeColor = Color.Gray;
            treeNode.ImageIndex = 0;
            treeNode.StateImageIndex = 0;
            treeNode.SelectedImageIndex = 0;
            TreeNodeProxy treeNodeProxy = treeNode.GetNodeProxy();
            treeNodeProxy.IsNewItemNode = true;
            return treeNode;
        }

        public static TreeNode CreateNode<T>(this LabelEditEnhancedTreeView @this, T nodeData)
            where T : ITreeNodeData
        {
            string displayValue = nodeData == null ? string.Empty : nodeData.DisplayValue;
            TreeNode treeNode = @this.Nodes.Add(displayValue);
            bool isNewItemNode = nodeData == null;
            TreeNodeProxy nodeProxy = new TreeNodeProxy<T>(nodeData, isNewItemNode);
            int iconIndex = isNewItemNode ? 0 : nodeData.IconIndex;
            treeNode.ImageIndex = iconIndex;
            treeNode.StateImageIndex = iconIndex;
            treeNode.SelectedImageIndex = iconIndex;

            treeNode.SetNodeProxy(nodeProxy);
            return treeNode;
        }

        public static ITreeNodeData UpdateNodeDataDisplayValue(this TreeNode @this, string displayValue)
        {
            //TreeNodeProxy treeNodeProxy = @this.GetNodeProxy();
            ITreeNodeData treeNodeData = @this.GetNodeData();
            if (treeNodeData == null)
            {
                treeNodeData = CreateNewNodeData(@this.TreeView as LabelEditEnhancedTreeView, displayValue);
            }
            else
            {
                treeNodeData.DisplayValue = displayValue;
            }

//            if (treeNodeProxy.IsNewItemNode)
//            {
//                treeNodeProxy.IsNewItemNode = false;
//            }

            return treeNodeData;
        }

        public static void RegisterNodeDataType(this LabelEditEnhancedTreeView @this, Type type)
        {
            @this.Tag = type;
        }

//        public static void RegisterNodeDataType<T>(this LabelEditEnhancedTreeView @this) where T : ITreeNodeData, new()
//        {
//            @this.Tag = new Func<string, T>(s => (T) CreateNodeDataInstance<T>(s));
//        }

        public static ITreeNodeData CreateNewNodeData(this LabelEditEnhancedTreeView @this, string displayValue)
        {
            //Func<string, ITreeNodeData> createFunc = (Func<string, ITreeNodeData>)@this.Tag;
            //return createFunc(displayValue);

            Type type = (Type) @this.Tag;

            ITreeNodeData newData = (ITreeNodeData) System.Activator.CreateInstance(type);
            newData.DisplayValue = displayValue;
            return newData;
        }

//        private static ITreeNodeData CreateNodeDataInstance<T>(string displayValue) where T : ITreeNodeData, new()
//        {
//            T newData = new T();
//            newData.DisplayValue = displayValue;
//            return newData;
//        }
    }

    #endregion class TreeNodeExtensions

    #region interface IItemsEditorValidator

    public interface IItemsEditorValidator
    {
        ItemsEditorAttribute Owner { get; set; }
        bool Validate(IEnumerable<ITreeNodeData> items, out string errorMessage);
        void SetContainer(object container);
    }

    public interface IItemsEditorValidator<T> : IItemsEditorValidator
    {
        T Container { get; set; }
    }

    #endregion interface IItemsEditorValidator

    #region class ItemsEditorValidatorBase

    public abstract class ItemsEditorValidatorBase<T> : IItemsEditorValidator<T>
    {
        public ItemsEditorAttribute Owner { get; set; }
        public T Container { get; set; }
        public abstract bool Validate(IEnumerable<ITreeNodeData> items, out string errorMessage);

        public void SetContainer(object container)
        {
            this.Container = (T) container;
        }
    }

    #endregion class ItemsEditorValidatorBase

    #region enum ItemsEditorOptions

    [Flags]
    public enum ItemsEditorOptions
    {
        Default = 0,
        UniqueDisplayValues = 1,
        SortingIcons = 2
    }

    #endregion enum ItemsEditorOptions

    #region class ItemsEditorAttribute

    [AttributeUsage(AttributeTargets.Property)]
    public class ItemsEditorAttribute : Attribute
    {
        public ItemsEditorOptions EditorOptions { get; private set; }
        public Type ValidationType { get; private set; }
        public string Tag { get; private set; }
        public string EditorCaption { get; set; }
        public string EditorTitle { get; set; }

        public ItemsEditorAttribute(ItemsEditorOptions editorOptions)
            : this(editorOptions, null)
        {
        }

        public ItemsEditorAttribute(ItemsEditorOptions editorOptions, Type validationType)
            : this(editorOptions, validationType, null)
        {
        }


        public ItemsEditorAttribute(ItemsEditorOptions editorOptions, Type validationType,
                                    string tag)
        {
            this.EditorOptions = editorOptions;
            this.ValidationType = validationType;
            this.Tag = tag;
        }

        public IItemsEditorValidator CreateValidator(object container)
        {
            IItemsEditorValidator validator = null;

            if (ValidationType != null)
            {
                validator = (IItemsEditorValidator) Activator.CreateInstance(ValidationType);
                validator.Owner = this;
                validator.SetContainer(container);
            }

            return validator;
        }
    }

    #endregion class ItemsEditorAttribute
}