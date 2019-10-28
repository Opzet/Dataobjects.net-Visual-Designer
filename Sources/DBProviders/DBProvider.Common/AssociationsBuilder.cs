using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    public class AssociationsBuilder
    {
        #region fields 

        private readonly List<AssociationItem> associations = new List<AssociationItem>();
        private readonly Dictionary<Table, bool?> auxiliaryTables = new Dictionary<Table, bool?>();

        private const string REGEX_NORMALIZE_PATTERN = @"[^A-Za-z0-9_]";
        private static readonly Regex regexNormalize = new Regex(REGEX_NORMALIZE_PATTERN, RegexOptions.IgnoreCase);

        #endregion fields

        #region Naming helper methods

        public static string NormalizeDBName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            name = regexNormalize.Replace(name, "");
            return name;
        }

        private static string BuildEntitySetPropertyName(string name)
        {
            return string.Format("{0}Items", name);
        }

        private Tuple<string, AssociationPropertyType, string, AssociationPropertyType>
            GenerateAssociationPropertyNames(AssocationType assocationType, AssociationItem associationItem)
        {
            string ownerPropertyName;
            AssociationPropertyType ownerPropertyType;
            string targetPropertyName;
            AssociationPropertyType targetPropertyType;
            if (assocationType == AssocationType.OneToOne)
            {
                //TODO: Generating property name - is OK ??
                //ownerPropertyName = NormalizeDBName(associationItem.ForeignKeyColumn.Owner.ForeignTable.Name);
                ownerPropertyName = NormalizeDBName(associationItem.ForeignKeyColumn.Name ?? associationItem.ForeignKeyColumn.Owner.ForeignTable.Name);
                ownerPropertyType = AssociationPropertyType.EntityReference;
                targetPropertyName = string.Empty;
                targetPropertyType = AssociationPropertyType.None;
            }
            else
            {
                if (assocationType == AssocationType.ManyToMany)
                {
                    ownerPropertyName = BuildEntitySetPropertyName(NormalizeDBName(associationItem.TargetSetting.Table.Name));
                    targetPropertyName = BuildEntitySetPropertyName(NormalizeDBName(associationItem.OwnerSetting.Table.Name));
                    ownerPropertyType = AssociationPropertyType.EntitySet;
                    targetPropertyType = AssociationPropertyType.EntitySet;
                }
                else
                {
                    ownerPropertyName = BuildEntitySetPropertyName(NormalizeDBName(associationItem.TargetSetting.Table.Name));
                    ownerPropertyType = AssociationPropertyType.EntitySet;
                    targetPropertyName = NormalizeDBName(associationItem.OwnerSetting.Table.Name);
                    targetPropertyType = AssociationPropertyType.EntityReference;
                }
            }

            return new Tuple<string, AssociationPropertyType, string, AssociationPropertyType>(ownerPropertyName,
                                                                                               ownerPropertyType, targetPropertyName, targetPropertyType);
        }

        #endregion Naming helper methods

        public static bool DetectTableIsAuxiliary(Table table)
        {
            bool isAux = table.Columns.All(
                column =>
                table.ForeignKeys.Any(
                    foreignKey => foreignKey.Columns.Any(foreignKeyColumn => foreignKeyColumn.Name == column.Name)));

            isAux &= (table.Columns.Count == 2 && table.ForeignKeys.Count == 2);

            return isAux;
        }

        private void ScanTables(IEnumerable<Table> tables)
        {
            foreach (var table in tables)
            {
                List<AssociationItem> items = new List<AssociationItem>();
                var isAuxiliary = DetectTableIsAuxiliary(table);

                if (!auxiliaryTables.ContainsKey(table))
                {
                    auxiliaryTables.Add(table, isAuxiliary);
                }
                else if (!auxiliaryTables[table].HasValue)
                {
                    auxiliaryTables[table] = isAuxiliary;
                }

                if (isAuxiliary)
                {
                    ForeignKeyColumn foreignKeyColumn = table.ForeignKeys[0].Columns[0];
                    ForeignKeyColumn auxForeignKeyColumn = table.ForeignKeys[1].Columns[0];

                    var associationItem = new AssociationItem(foreignKeyColumn, auxForeignKeyColumn, AssocationType.OneToMany);

                    var genResult = GenerateAssociationPropertyNames(AssocationType.OneToMany, associationItem);
                    associationItem.OwnerSetting.PropertyName = genResult.Item1;
                    associationItem.OwnerSetting.PropertyType = genResult.Item2;
                    associationItem.TargetSetting.PropertyName = genResult.Item3;
                    associationItem.TargetSetting.PropertyType = genResult.Item4;

                    //addItemFunc(allAssociationItems, items, associationItem);
                }
                else
                {
                    foreach (var foreignKey in table.ForeignKeys)
                    {
                        var associationItem = new AssociationItem(foreignKey.Columns[0], AssocationType.OneToOne);
                        var genResult = GenerateAssociationPropertyNames(AssocationType.OneToOne, associationItem);
                        associationItem.OwnerSetting.PropertyName = genResult.Item1;
                        associationItem.OwnerSetting.PropertyType = genResult.Item2;
                        associationItem.TargetSetting.PropertyName = genResult.Item3;
                        associationItem.TargetSetting.PropertyType = genResult.Item4;

                        //addItemFunc(allAssociationItems, items, associationItem);
                    }
                }

                if (items.Count > 0)
                {
                    //allAssociationItems.AddRange(items);
                    this.associations.AddRange(items);
                }
            }
        }

//        public IEnumerable<AssociationItem> BuildAssociationsFrom(IEnumerable<Table> tables)
//        {
//            
//        }
    }
}