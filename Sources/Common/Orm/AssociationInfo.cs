using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    public interface IAssociationInfo: IOrmAttribute
    {
        MultiplicityKind Multiplicity { get; }

        AssociationOnRemoveAction OnOwnerRemove { get; }

        AssociationOnRemoveAction OnTargetRemove { get; }

        Defaultable<string> PairTo { get; }

        bool UseAssociationAttribute { get; set; }
    }

    public class AssociationInfo : OrmAttributeBase, IAssociationInfo
    {
        public static readonly OrmAttributeGroup ATTRIBUTE_GROUP_ASSOCIATION = 
            new OrmAttributeGroup("Association", OrmUtils.GetOrmNamespace(OrmNamespace.Orm));

        private const string ERROR_EMPTY_NAME_IN_PAIRTO = "PairTo could not be empty when is set as 'Custom'.";
        private const string CODE_EMPTY_NAME_IN_PAIRTO = "EmptyNameInPairTo";


        public MultiplicityKind Multiplicity { get; set; }
        public AssociationOnRemoveAction OnOwnerRemove { get; set; }
        public AssociationOnRemoveAction OnTargetRemove { get; set; }
        public Defaultable<string> PairTo { get; set; }

        public bool UseAssociationAttribute { get; set; }

        public AssociationInfo()
        {
            this.OnOwnerRemove = AssociationOnRemoveAction.Default;
            this.OnTargetRemove  = AssociationOnRemoveAction.Default;
            this.PairTo = new Defaultable<string>();
            this.UseAssociationAttribute = true;
        }

        public AssociationInfo(OrmAssociationEnd source)
        {
            AssignFrom(source);
        }

        private void AssignFrom(OrmAssociationEnd source)
        {
            this.Multiplicity = source.Multiplicity;
            this.OnOwnerRemove = source.OnOwnerRemove;
            this.OnTargetRemove = source.OnTargetRemove;
            this.PairTo = (Defaultable<string>)source.PairTo.Clone();
            this.UseAssociationAttribute = source.UseAssociationAttribute;
        }

        public OrmAssociationEnd ToOrmAssociationEnd()
        {
            OrmAssociationEnd associationEnd = new OrmAssociationEnd();
            associationEnd.multiplicity = this.Multiplicity;
            associationEnd.OnOwnerRemove = this.OnOwnerRemove;
            associationEnd.OnTargetRemove = this.OnTargetRemove;
            associationEnd.UseAssociationAttribute = this.UseAssociationAttribute;
            associationEnd.PairTo = (Defaultable<string>) this.PairTo.Clone();
            return associationEnd;
        }

        #region Implementation of IOrmAttribute

        public override Dictionary<string, Defaultable> GetAttributeGroupItems(OrmAttributeGroup @group)
        {
            Dictionary<string, Defaultable> result = new Dictionary<string, Defaultable>();

            if (group == ATTRIBUTE_GROUP_ASSOCIATION)
            {
                result.Add("PairTo", PairTo);

                var onOwnerRemove = GetDefaultableOnRemoveAction(true);
                result.Add("OnOwnerRemove", onOwnerRemove);

                var onTargetRemove = GetDefaultableOnRemoveAction(false);
                result.Add("OnTargetRemove", onTargetRemove);
            }

            return result;
        }

        public override void Validate(ValidationContext context, ModelElement ownerElement)
        {
            if (this.PairTo.IsCustom() && string.IsNullOrEmpty(this.PairTo.Value))
            {
                context.LogError(ERROR_EMPTY_NAME_IN_PAIRTO, CODE_EMPTY_NAME_IN_PAIRTO,
                    new []
                    {
                        ownerElement
                    });
            }
        }

        protected override OrmAttributeGroup[] GetAllAttributeGroups()
        {
            return new [] { ATTRIBUTE_GROUP_ASSOCIATION };
        }

        protected override void FilterAttributeGroups(List<OrmAttributeGroup> attributeGroupsToFilter)
        {
            if (!this.UseAssociationAttribute)
            {
                attributeGroupsToFilter.RemoveAll(item => item.EqualsTo(ATTRIBUTE_GROUP_ASSOCIATION));
            }
        }

        protected override OrmAttributeKind GetAttributeKind()
        {
            return OrmAttributeKind.Property;
        }

        private Defaultable<AssociationOnRemoveAction> GetDefaultableOnRemoveAction(bool onOwnerRemove)
        {
            var defaultable = new Defaultable<AssociationOnRemoveAction>();
            AssociationOnRemoveAction associationOnRemoveAction = onOwnerRemove ? this.OnOwnerRemove : this.OnTargetRemove;
            if (associationOnRemoveAction != AssociationOnRemoveAction.Default)
            {
                defaultable.SetAsCustom(associationOnRemoveAction);
            }
            else
            {
                defaultable.SetAsDefault();
            }
            return defaultable;
        }

        private Defaultable<bool> GetDefaultableUseAssociationAttribute()
        {
            Defaultable<bool> defaultable = new Defaultable<bool>();
            defaultable.SetAsCustom(this.UseAssociationAttribute);
            return defaultable;
        }

        public override IOrmAttribute MergeChanges(IOrmAttribute otherAttribute, MergeConflictAction mergeConflictAction)
        {
            AssociationInfo other = (AssociationInfo) otherAttribute;
            
            AssociationInfo mergedResult = new AssociationInfo();
            mergedResult.PairTo = this.PairTo.Merge(other.PairTo, mergeConflictAction);

            var onOwnerRemove = GetDefaultableOnRemoveAction(true);
            var mergedOnOwnerRemove = onOwnerRemove.Merge(other.GetDefaultableOnRemoveAction(true), mergeConflictAction);
            mergedResult.OnOwnerRemove = mergedOnOwnerRemove.IsDefault()
                                             ? AssociationOnRemoveAction.Default
                                             : mergedOnOwnerRemove.Value;

            var onTargetRemove = GetDefaultableOnRemoveAction(false);
            var mergedOnTargetRemove = onTargetRemove.Merge(other.GetDefaultableOnRemoveAction(false), mergeConflictAction);
            mergedResult.OnTargetRemove = mergedOnTargetRemove.IsDefault()
                                             ? AssociationOnRemoveAction.Default
                                             : mergedOnTargetRemove.Value;

            var mergedUseAssociationAttribute =
                this.GetDefaultableUseAssociationAttribute().Merge(other.GetDefaultableUseAssociationAttribute(),
                    mergeConflictAction);

            mergedResult.UseAssociationAttribute = mergedUseAssociationAttribute.Value;

            return mergedResult;
        }

        #endregion

        public override void DeserializeFromXml(XmlReader reader)
        {
            OrmAssociationEnd associationEnd = new OrmAssociationEnd();
            associationEnd.DeserializeFromXml(reader);
            this.AssignFrom(associationEnd);
        }

        public override void SerializeToXml(XmlWriter writer)
        {
            this.ToOrmAssociationEnd().SerializeToXml(writer);
        }
    }
}