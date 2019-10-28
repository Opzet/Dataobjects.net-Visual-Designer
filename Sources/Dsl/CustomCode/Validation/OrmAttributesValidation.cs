//using System;
//using System.Text.RegularExpressions;
//using Microsoft.VisualStudio.Modeling;
//using Microsoft.VisualStudio.Modeling.Validation;
//using TXSoftware.DataObjectsNetEntityModel.Common;
//
//namespace TXSoftware.DataObjectsNetEntityModel.Dsl
//{
//    internal static class OrmAttributesValidation
//    {
//        #region fields 
//
//        private const string regexTableMappingNamePattern = "[_A-Za-z0-9-.]";
//        private static readonly Regex regexTableMappingName = new Regex(regexTableMappingNamePattern);
//
//        #endregion fields
//
//        #region errors and codes 
//
//        private const string ERROR_INVALID_CHARACTERS_IN_TABLE_MAPPING_NAME =
//            "Table mapping name '{0}' contains invalid characters, support characters are '{1}'.";
//
//        private const string ERROR_EMPTY_NAME_IN_TABLE_MAPPING_NAME = "Table mapping name could not be empty when is set as 'Custom'.";
//
//        private const string CODE_INVALID_CHARACTERS_IN_TABLE_MAPPING_NAME = "InvalidCharactersInTableMappingName";
//        private const string CODE_EMPTY_NAME_IN_TABLE_MAPPING_NAME = "EmptyNameInTableMappingName";
//
//        #endregion errors and codes
//
//        internal static void Validate(PersistentType owner, IOrmAttribute attribute, ValidationContext context)
//        {
//            OrmHierarchyRootAttribute hierarchyRootAttribute = attribute as OrmHierarchyRootAttribute;
//            if (hierarchyRootAttribute != null)
//            {
//                DoValidate(owner, hierarchyRootAttribute, context);
//            }
//
//            orm
//        }
//
//        #region validate 'OrmHierarchyRootAttribute'
//
//        private static void DoValidate(PersistentType owner, OrmHierarchyRootAttribute hierarchyRootAttribute,
//            ValidationContext context)
//        {
//            if (!hierarchyRootAttribute.Enabled)
//            {
//                return;
//            }
//
//            if (hierarchyRootAttribute.MappingName.IsCustom())
//            {
//                string mappingName = hierarchyRootAttribute.MappingName.Value;
//
//                if (!string.IsNullOrEmpty(mappingName))
//                {
//                    Match match = regexTableMappingName.Match(mappingName);
//                    if (!Util.StringEqual(match.Value, mappingName, false))
//                    {
//                        context.LogError(
//                            string.Format(ERROR_INVALID_CHARACTERS_IN_TABLE_MAPPING_NAME,
//                                mappingName, regexTableMappingNamePattern),
//                            CODE_INVALID_CHARACTERS_IN_TABLE_MAPPING_NAME,
//                            new ModelElement[]
//                            {
//                                owner
//                            });
//                    }
//                }
//                else
//                {
//                    context.LogError(ERROR_EMPTY_NAME_IN_TABLE_MAPPING_NAME, CODE_EMPTY_NAME_IN_TABLE_MAPPING_NAME,
//                        new ModelElement[]
//                        {
//                            owner
//                        });
//
//                }
//            }
//        }
//
//        #endregion validate 'OrmHierarchyRootAttribute'
//    }
//}