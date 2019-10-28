using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    public interface IOrmAttribute: ISerializableObject
    {
        [Browsable(false)]
        OrmAttributeKind AttributeKind { get; }

        OrmAttributeGroup[] GetAttributeGroups(AttributeGroupsListMode listMode);

        List<Defaultable> GetAttributeGroupItems(AttributeGroupsListMode listMode);

        Dictionary<string, Defaultable> GetAttributeGroupItems(OrmAttributeGroup @group);

        IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction);

        void Validate(ValidationContext context, ModelElement ownerElement);
    }

    #region class OrmAttributeGroup

    [Serializable]
    public class OrmAttributeGroup
    {
        public string Name { get; set; }
        public string Namespace { get; set; }

        public static readonly OrmAttributeGroup Empty = new OrmAttributeGroup(string.Empty, string.Empty);

        public OrmAttributeGroup(string name, string @namespace)
        {
            Name = name;
            Namespace = @namespace;
        }

        public override string ToString()
        {
            return FormatFullName();
        }

        public string FormatFullName()
        {
            return string.Format("{0}.{1}", Namespace, Name);
        }

        public bool EqualsTo(OrmAttributeGroup @group)
        {
            return Util.StringEqual(this.Name, @group.Name, false) &&
                   Util.StringEqual(this.Namespace, @group.Namespace, false);
        }

        public static bool operator ==(OrmAttributeGroup group1, OrmAttributeGroup group2)
        {
            if (Object.Equals(group1, null) || Object.Equals(group2, null))
            {
                if (Object.Equals(group1, null) && Object.Equals(group2, null))
                {
                    return true;
                }
                return false;
            }

            return group1.EqualsTo(group2);
        }

        public static bool operator !=(OrmAttributeGroup group1, OrmAttributeGroup group2)
        {
            if (Object.Equals(group1, null) || Object.Equals(group2, null))
            {
                if (Object.Equals(group1, null) && Object.Equals(group2, null))
                {
                    return false;
                }
                return true;
            }

            return !group1.EqualsTo(group2);
        }
    }

    #endregion class OrmAttributeGroup

    #region class OrmAttributeBase

    public abstract class OrmAttributeBase : IOrmAttribute
    {
        [Browsable(false)]
        public OrmAttributeKind AttributeKind
        {
            get { return GetAttributeKind(); }
        }

        public abstract Dictionary<string, Defaultable> GetAttributeGroupItems(OrmAttributeGroup @group);

        public abstract IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction);
        public abstract void Validate(ValidationContext context, ModelElement ownerElement);

        protected abstract OrmAttributeGroup[] GetAllAttributeGroups();

        public virtual List<Defaultable> GetAttributeGroupItems(AttributeGroupsListMode listMode)
        {
            var query = from attrGroup in GetAttributeGroups(listMode)
                        select GetAttributeGroupItems(attrGroup).Values;

            List<Defaultable> items = query.SelectMany(list => list).ToList();
            return items;
        }

        public OrmAttributeGroup[] GetAttributeGroups(AttributeGroupsListMode listMode)
        {
            var allAttributeGroups = GetAllAttributeGroups();
            if (listMode == AttributeGroupsListMode.Filtered)
            {
                List<OrmAttributeGroup> attributesToFilter = new List<OrmAttributeGroup>(allAttributeGroups);
                FilterAttributeGroups(attributesToFilter);
                allAttributeGroups = attributesToFilter.ToArray();
            }

            return allAttributeGroups;
        }

        protected abstract void FilterAttributeGroups(List<OrmAttributeGroup> attributeGroupsToFilter);

        protected abstract OrmAttributeKind GetAttributeKind();

        public abstract void DeserializeFromXml(XmlReader reader);
        public abstract void SerializeToXml(XmlWriter writer);
    }

    #endregion class OrmAttributeBase

    #region enum AttributeGroupsListMode

    public enum AttributeGroupsListMode
    {
        All,
        Filtered
    }

    #endregion enum AttributeGroupsListMode

    #region enum MergeConflictAction

    public enum MergeConflictAction
    {
        NoAction = 0,
        TakeCurrent,
        TakeOther,
    }

    #endregion enum MergeConflictAction

    #region enum OrmAttributeKind

    public enum OrmAttributeKind
    {
        Property,
        Type
    }

    #endregion enum OrmAttributeKind
}