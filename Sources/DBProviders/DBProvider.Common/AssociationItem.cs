using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [Serializable]
    public class AssociationItem
    {
        #region fields

        private ForeignKeyColumn foreignKeyColumn = null;
        private ForeignKeyColumn auxiliaryForeignKeyColumn = null;

        #endregion fields

        #region properties

        [XmlAttribute("id")]
        public Guid Id { get; set; }

        [XmlAttribute("type")]
        public AssocationType AssocationType { get; set; }

        //[XmlElement("ForeignKeyColumn")]
        [XmlIgnore]
        public ForeignKeyColumn ForeignKeyColumn
        {
            get
            {
                if (foreignKeyColumn == null && !string.IsNullOrEmpty(this.ForeignKeyColumnUrn))
                {
                    DBObject o = this.OwnerSetting.Table.ResolveChildObject(this.ForeignKeyColumnUrn);
                    foreignKeyColumn = o as ForeignKeyColumn;
                }

                return this.foreignKeyColumn;
            }
            private set
            {
                this.foreignKeyColumn = value;
                ForeignKeyColumnUrn = value != null ? value.Urn : string.Empty;
            }
        }

        [XmlAttribute("foreignKeyColumn")]
        public string ForeignKeyColumnUrn { get; set; }

        [XmlIgnore]
        public ForeignKeyColumn AuxiliaryForeignKeyColumn
        {
            get
            {
                if (auxiliaryForeignKeyColumn == null && !string.IsNullOrEmpty(this.AuxiliaryForeignKeyColumnUrn))
                {
                    DBObject o = this.TargetSetting.Table.ResolveChildObject(this.AuxiliaryForeignKeyColumnUrn);
                    auxiliaryForeignKeyColumn = o as ForeignKeyColumn;
                }

                return this.auxiliaryForeignKeyColumn;
            }
            private set
            {
                this.auxiliaryForeignKeyColumn = value;
                this.AuxiliaryForeignKeyColumnUrn = value != null ? value.Urn : string.Empty;
            }
        }

        [XmlAttribute("auxiliaryForeignKeyColumn")]
        public string AuxiliaryForeignKeyColumnUrn { get; set; }

        public bool IsAuxiliary
        {
            get { return ForeignKeyColumn != null && AuxiliaryForeignKeyColumn != null; }
        }

        [XmlElement("Owner")]
        public AssociationItemSetting OwnerSetting { get; set; }

        [XmlElement("Target")]
        public AssociationItemSetting TargetSetting { get; set; }

        #endregion properties

        #region constructors

        public AssociationItem() {}

        public AssociationItem(ForeignKeyColumn foreignKeyColumn, AssocationType assocationType)
            : this(foreignKeyColumn, null, assocationType)
        {}

        public AssociationItem(ForeignKeyColumn foreignKeyColumn, ForeignKeyColumn auxiliaryForeignKeyColumn, AssocationType assocationType)
        {
            this.Id = Guid.NewGuid();

            this.OwnerSetting = new AssociationItemSetting();
            this.TargetSetting = new AssociationItemSetting();

            this.AssocationType = assocationType;

            this.ForeignKeyColumn = foreignKeyColumn;
            this.AuxiliaryForeignKeyColumn = auxiliaryForeignKeyColumn;

            bool isAux = auxiliaryForeignKeyColumn != null;

            Table ownerTable = isAux ? (Table)foreignKeyColumn.Owner.ForeignTable :(Table)foreignKeyColumn.Owner.Owner;
            OwnerSetting.Table = ownerTable;
            OwnerSetting.Column = isAux ? foreignKeyColumn.ReferencedColumn : foreignKeyColumn.Name;
            OwnerSetting.PropertyType = isAux ? AssociationPropertyType.EntitySet : AssociationPropertyType.EntityReference;

            Table foreignTable = isAux ? auxiliaryForeignKeyColumn.Owner.ForeignTable : foreignKeyColumn.Owner.ForeignTable;
            TargetSetting.Table = foreignTable;
            TargetSetting.Column = isAux ? auxiliaryForeignKeyColumn.ReferencedColumn : foreignKeyColumn.ReferencedColumn;
            TargetSetting.PropertyType = isAux ? (assocationType == AssocationType.ManyToMany ? AssociationPropertyType.EntitySet : AssociationPropertyType.EntityReference) : AssociationPropertyType.EntityReference;
        }

        #endregion constructors

        #region methods

        public AssociationItem Clone()
        {
            AssociationItem cloned = new AssociationItem(this.ForeignKeyColumn, this.AuxiliaryForeignKeyColumn, this.AssocationType);
            cloned.Id = this.Id;
            cloned.OwnerSetting = this.OwnerSetting.Clone();
            cloned.TargetSetting = this.TargetSetting.Clone();

            return cloned;
        }

        public void AssignFrom(AssociationItem other)
        {
            this.Id = other.Id;
            this.ForeignKeyColumn = other.ForeignKeyColumn;
            this.AuxiliaryForeignKeyColumn = other.AuxiliaryForeignKeyColumn;
            this.AssocationType = other.AssocationType;
            this.OwnerSetting.AssignFrom(other.OwnerSetting);
            this.TargetSetting.AssignFrom(other.TargetSetting);
        }

        public bool EqualsToSpecial(AssociationItem other)
        {
            var ownerData = GetInfo(true);
            var targetData = GetInfo(false);

            var otherOwnerData = other.GetInfo(true);
            var otherTargetData = other.GetInfo(false);

            return IsEqual(otherOwnerData, ownerData) || IsEqual(otherOwnerData, targetData) ||
                   IsEqual(otherTargetData, ownerData) || IsEqual(otherTargetData, targetData);
        }

        private Tuple<string, string, AssociationPropertyType> GetInfo(bool isOwner)
        {
            string tableUrn = isOwner ? this.OwnerSetting.Table.Urn : this.TargetSetting.Table.Urn;
            string propertyName = isOwner ? this.OwnerSetting.PropertyName : this.TargetSetting.PropertyName;
            AssociationPropertyType propertyType = isOwner ? this.OwnerSetting.PropertyType : this.TargetSetting.PropertyType;
            return new Tuple<string, string, AssociationPropertyType>(tableUrn, propertyName, propertyType);
        }

        private bool IsEqual(Tuple<string, string, AssociationPropertyType> data1, Tuple<string, string, AssociationPropertyType> data2)
        {
            return Util.StringEqual(data1.Item1, data2.Item1, true) && Util.StringEqual(data1.Item2, data2.Item2, true) && data1.Item3 == data2.Item3;
        }

        public bool EqualsTo(ForeignKeyColumn foreignKeyColumn)
        {
            return EqualsTo(foreignKeyColumn, null);
        }

        public bool EqualsTo(ForeignKeyColumn foreignKeyColumn, ForeignKeyColumn auxiliaryForeignKeyColumn)
        {
            bool isAux = auxiliaryForeignKeyColumn != null;

            Table ownerTable = isAux ? foreignKeyColumn.Owner.ForeignTable : (Table)foreignKeyColumn.Owner.Owner;
            string ownerColumn = isAux ? foreignKeyColumn.ReferencedColumn : foreignKeyColumn.Name;
            Table foreignTable = isAux ? auxiliaryForeignKeyColumn.Owner.ForeignTable : foreignKeyColumn.Owner.ForeignTable;
            string targetColumn = isAux ? auxiliaryForeignKeyColumn.ReferencedColumn : foreignKeyColumn.ReferencedColumn;

            bool equals = Util.StringEqual(this.OwnerSetting.Table.Urn, ownerTable.Urn, true);
            equals &= Util.StringEqual(this.OwnerSetting.Column, ownerColumn, true);
            equals &= Util.StringEqual(this.TargetSetting.Table.Urn, foreignTable.Urn, true);
            equals &= Util.StringEqual(this.TargetSetting.Column, targetColumn, true);

            return equals;
        }

        public bool UsedIn(Index dbIndex)
        {
            // get only main columns of index (just IncludeOnly = false)
            Table indexTable = (Table) dbIndex.Owner;
            List<string> mainColumns = dbIndex.Columns.Where(idxCol => !idxCol.IncludeOnly).ToList().ConvertAll(input => input.Name);

            string indexTableUrn = indexTable.Urn;
            
            bool partOk1 = Util.StringEqual(indexTableUrn, OwnerSetting.Table.Urn, true);
            partOk1 &= mainColumns.Any(s => Util.StringEqual(s, OwnerSetting.Column, true));

            bool partOk2 = Util.StringEqual(indexTableUrn, TargetSetting.Table.Urn, true);
            partOk2 &= mainColumns.Any(s => Util.StringEqual(s, TargetSetting.Column, true));

            return partOk1 || partOk2;
        }

        #endregion methods

        #region generate entity item methods
/*

        public void UpdateEntityItems(ref EntityItem ownerEntity, ref EntityItem targetEntity)
        {
            PropertyItem ownerEntityPropertyItem = UpdateEntityItem(ownerEntity, true);
            PropertyItem targetEntityPropertyItem = UpdateEntityItem(targetEntity, false);
            UpdateEntityItemAssociations(ownerEntity, ownerEntityPropertyItem, targetEntity, targetEntityPropertyItem);
        }

        private PropertyItem UpdateEntityItem(EntityItem entityItem, bool isOwner)
        {
            PropertyItem propertyItem = null;

            string columnName = isOwner ? this.OwnerSetting.Column : this.TargetSetting.Column;
            string propertyName = isOwner ? this.OwnerSetting.PropertyName : this.TargetSetting.PropertyName;
            var selectedPropertyType = isOwner ? this.OwnerSetting.PropertyType : this.TargetSetting.PropertyType;

            if (selectedPropertyType != AssociationPropertyType.None && !string.IsNullOrEmpty(propertyName))
            {
                propertyItem = new PropertyItem(propertyName, null);

                if (selectedPropertyType == AssociationPropertyType.EntityReference)
                {
                    entityItem.Properties.RemoveAll(item => Util.StrEqual(item.DBName, columnName) && !IsColumnInIndexOrKey(entityItem, columnName)); //item.DBName));
                }

                entityItem.Properties.RemoveAll(item => item.Name == propertyName);

                string pairTo = isOwner ? this.TargetSetting.PropertyName : this.OwnerSetting.PropertyName;
                var enabledAssociation = !string.IsNullOrEmpty(pairTo) && selectedPropertyType != AssociationPropertyType.None;
                if (enabledAssociation)
                {
                    propertyItem.Association.Enable(pairTo);
                }

                entityItem.Properties.Add(propertyItem);
            }

            return propertyItem;
        }

        private bool IsColumnInIndexOrKey(EntityItem entityItem, string columnName)
        {
            bool any = entityItem.Indexes.Any(index => index.KeyFields.Any(keyField => Util.StringEqual(keyField.DBName, columnName)));
            if (!any)
            {
                any = entityItem.Properties.Any(
                    property => property.Key.Enabled && Util.StringEqual(property.DBName, columnName));
            }

            return any;
        }

        private void UpdateEntityItemAssociations(EntityItem ownerEntityItem, PropertyItem ownerPropertyItem,
            EntityItem targetEntityItem, PropertyItem targetPropertyItem)
        {
            var ownerPropertyType = this.OwnerSetting.PropertyType;

            if (ownerPropertyItem != null)
            {
                if (ownerPropertyType == AssociationPropertyType.EntitySet)
                {
                    //ownerPropertyItem.EntitySet.Enable(targetEntityItem);
                    ownerPropertyItem.PropertyType.SetAsEntitySet(targetEntityItem);
                }
                else if (ownerPropertyType == AssociationPropertyType.EntityReference)
                {
                    ownerPropertyItem.PropertyType.SetAsEntityReference(targetEntityItem);
                }
            }

            var targetPropertyType = this.TargetSetting.PropertyType;

            if (targetPropertyItem != null)
            {
                if (targetPropertyType == AssociationPropertyType.EntitySet)
                {
                    //targetPropertyItem.EntitySet.Enable(ownerEntityItem);
                    targetPropertyItem.PropertyType.SetAsEntitySet(ownerEntityItem);
                }
                else if (targetPropertyType == AssociationPropertyType.EntityReference)
                {
                    targetPropertyItem.PropertyType.SetAsEntityReference(ownerEntityItem);
                }
            }
        }
*/

        #endregion generate entity item methods
    }

    #region class AssociationItemSetting

    [Serializable]
    public class AssociationItemSetting
    {
        private Table table;

        [XmlAttribute("propertyName")]
        public string PropertyName { get; set; }

        [XmlAttribute("onOwnerRemove")]
        public AssociationOnRemoveAction OnOwnerRemove { get; set; }

        [XmlAttribute("onTargetRemove")]
        public AssociationOnRemoveAction OnTargetRemove { get; set; }

        [XmlIgnore]
        public Table Table
        {
            get { return this.table; }
            set
            {
                this.table = value;
                this.TableUrn = value != null ? value.Urn : string.Empty;
            }
        }

        [XmlAttribute("table")]
        public string TableUrn { get; set; }

        [XmlAttribute("column")]
        public string Column { get; set; }

        [XmlAttribute("propertyType")]
        public AssociationPropertyType PropertyType { get; set; }

        public AssociationItemSetting()
        {
            this.OnOwnerRemove = AssociationOnRemoveAction.Default;
            this.OnTargetRemove = AssociationOnRemoveAction.Default;
        }

        public AssociationItemSetting Clone()
        {
            AssociationItemSetting cloned = new AssociationItemSetting();
            cloned.PropertyName = this.PropertyName;
            cloned.OnOwnerRemove = this.OnOwnerRemove;
            cloned.OnTargetRemove = this.OnTargetRemove;
            cloned.table = this.table;
            cloned.TableUrn = this.TableUrn;
            cloned.Column = this.Column;
            cloned.PropertyType = this.PropertyType;

            return cloned;
        }

        public void AssignFrom(AssociationItemSetting other)
        {
            this.PropertyName = other.PropertyName;
            this.OnOwnerRemove = other.OnOwnerRemove;
            this.OnTargetRemove = other.OnTargetRemove;
            this.table = other.table;
            this.TableUrn = other.TableUrn;
            this.Column = other.Column;
            this.PropertyType = other.PropertyType;            
        }
    }

    #endregion class AssociationItemSetting

    #region enum AssocationType

    [Serializable]
    public enum AssocationType
    {
        [XmlEnum("OneToOne")]
        OneToOne,

        [XmlEnum("OneToMany")]
        OneToMany,

        [XmlEnum("ManyToMany")]
        ManyToMany
    }

    #endregion enum AssocationType

    #region enum AssociationPropertyType

    [Serializable]
    public enum AssociationPropertyType
    {
        [XmlEnum("EntitySet")]
        EntitySet,

        [XmlEnum("EntityReference")]
        EntityReference,

        [XmlEnum("None")]
        None
    }

    #endregion enum AssociationPropertyType
}