using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Dsl;

/*~T4_TEMPLATE_HEADER_BEGIN~
#><#@ import namespace="Microsoft.CSharp"
#><#@ import namespace="TXSoftware.DataObjectsNetEntityModel.Common"
#><#@ import namespace="TXSoftware.DataObjectsNetEntityModel.Common.UIEditors"
#><#+
~T4_TEMPLATE_HEADER_END~*/

public class CodeGenerationTools
{
    private const string SYSTEM_NULLABLE_FORMAT = "System.Nullable<{0}>";
    private const string MODIFIER_VIRTUAL = "virtual";
    private const string TYPE_KIND_INTERFACE = "interface";
    private const string TYPE_KIND_CLASS = "class";
    private const string TYPE_KIND_STRUCT = "struct";
    private const string TYPE_FORMAT_GUID = "new System.Guid(\"{0}\")";
    private const string TYPE_FORMAT_DATETIME = "new System.DateTime({0}, System.DateTimeKind.Unspecified)";
    private const string TYPE_FORMAT_BYTE_ARRAY = "new System.Byte[] {{{0}}}";
    private const string TYPE_FORMAT_TIMESPAN = "new System.TimeSpan({0}, {1}, {2})";
    private const string TYPE_FORMAT_DATETIMEOFFSET = "new System.DateTimeOffset({0}, new System.TimeSpan({1}))";
    private const string TYPE_HASHSET = "System.Collections.Generic.HashSet<{0}>";
    private const string ATTRIBUTE_DATACONTRACT = "[System.Runtime.Serialization.DataContract]";
    private const string ATTRIBUTE_DATAMEMBER = "[System.Runtime.Serialization.DataMember]";
    private const string ATTRIBUTE_SERIALIZABLE = "System.SerializableAttribute";
    
    private readonly DynamicTextTransformation _textTransformation;
    private readonly CSharpCodeProvider _code;

    public bool ActiveDTOStage { get; set; }

    #region Accessibility

    private const string VISIBILITY_PRIVATE = "private";
    private const string VISIBILITY_INTERNAL = "internal";
    private const string VISIBILITY_PROTECTED = "protected";
    private const string VISIBILITY_PROTECTED_INTERNAL = "protected internal";

    public string AccessibilityToString(PropertyAccessModifier propertyAccessModifier)
    {
        return propertyAccessModifier == PropertyAccessModifier.ProtectedInternal
                   ? VISIBILITY_PROTECTED_INTERNAL
                   : propertyAccessModifier.ToString().ToLower();
    }

    public string AccessibilityForType(IPropertyBase property)
    {
        PropertyAccessModifier accessModifier = PropertyAccessModifier.Public;
        if (!this.ActiveDTOStage)
        {
            accessModifier = property.PropertyAccess.GetHigherModifier();
        }
        
        string accessibility = accessModifier == PropertyAccessModifier.ProtectedInternal 
            ? VISIBILITY_PROTECTED_INTERNAL 
            : accessModifier.ToString().ToLowerInvariant();

        if ((property as IPropertyBase).Owner.TypeKind == PersistentTypeKind.Interface)
        {
            accessibility = string.Empty;
        }

        return accessibility;
    }

    public string AccessibilityForType(IAccessibleElement element)
    {
        AccessModifier accessModifier = this.ActiveDTOStage ? AccessModifier.Public : element.Access;
        string accessibility = accessModifier.ToString().ToLowerInvariant();
//        if (element is IPropertyBase)
//        {
//            if ((element as IPropertyBase).Owner.TypeKind == PersistentTypeKind.Interface)
//            {
//                accessibility = string.Empty;
//            }
//        }

        return accessibility;
    }

    public bool VisibleSetter(IPropertyBase property, IPropertiesBuilder propertiesBuilder)
    {
        if (this.ActiveDTOStage)
        {
            return true;
        }

        string setter = ForSetter(property, propertiesBuilder);
        if (setter == VISIBILITY_PRIVATE && property.Owner.TypeKind == PersistentTypeKind.Interface)
        {
            return false;
        }

        return true;
    }

    public string ForGetter(IPropertyBase property)
    {
        string result = string.Empty;
        if (this.ActiveDTOStage)
        {
            return result;
        }

        PropertyAccessModifier higherModifier = property.PropertyAccess.GetHigherModifier();
        if (property.PropertyAccess.Getter != PropertyAccessModifier.Public && property.PropertyAccess.Getter != higherModifier)
        {
            result = GetPropertyAccessModifierString(property.PropertyAccess.Getter);
        }

        return result;
    }
    
    public string ForSetter(IPropertyBase property, IPropertiesBuilder propertiesBuilder)
    {
        string result = string.Empty;
        if (this.ActiveDTOStage)
        {
            return result;
        }

        switch (property.PropertyKind)
        {
            case PropertyKind.Scalar:
            {
                IOrmAttribute[] typeAttributes = propertiesBuilder.GetPropertyTypeAttributes(property);
                OrmKeyAttribute keyAttribute = (OrmKeyAttribute)typeAttributes.Single(item => item is OrmKeyAttribute);

                result = keyAttribute.Enabled ? VISIBILITY_PRIVATE : string.Empty;
                break;
            }
            case PropertyKind.Navigation:
            {
                IOrmAttribute[] typeAttributes = propertiesBuilder.GetPropertyTypeAttributes(property);
                OrmKeyAttribute keyAttribute = (OrmKeyAttribute)typeAttributes.Single(item => item is OrmKeyAttribute);

                INavigationProperty navigationProperty = (INavigationProperty)property;
                if (navigationProperty.Multiplicity == MultiplicityKind.Many || keyAttribute.Enabled)
                {
                    result = VISIBILITY_PRIVATE;
                }

                break;
            }
        }

        // only when result is not directly 'private' because of business rules we can adjust it from settings
        if (string.IsNullOrEmpty(result))
        {
            PropertyAccessModifier higherModifier = property.PropertyAccess.GetHigherModifier();

            if (property.PropertyAccess.Setter != PropertyAccessModifier.Public && property.PropertyAccess.Setter != higherModifier)
            {
                result = GetPropertyAccessModifierString(property.PropertyAccess.Setter);
            }
        }

        return result;
    }

    private string GetPropertyAccessModifierString(PropertyAccessModifier modifier)
    {
        switch (modifier)
        {
            case PropertyAccessModifier.Internal:
                return VISIBILITY_INTERNAL;

            case PropertyAccessModifier.Private:
                return VISIBILITY_PRIVATE;

            case PropertyAccessModifier.Protected:
                return VISIBILITY_PROTECTED;

            case PropertyAccessModifier.ProtectedInternal:
                return string.Format("{0} {1}", VISIBILITY_PROTECTED, VISIBILITY_INTERNAL);
        }

        return string.Empty;
    }
    
    #endregion Accessibility

    /// <summary>
    /// Initializes a new CodeGenerationTools object with the TextTransformation (T4 generated class)
    /// that is currently running
    /// </summary>
    public CodeGenerationTools(object textTransformation)
    {
        if (textTransformation == null)
        {
            throw new ArgumentNullException("textTransformation");
        }

        _textTransformation = DynamicTextTransformation.Create(textTransformation);
        _code = new CSharpCodeProvider();
        FullyQualifySystemTypes = false;
        CamelCaseFields = true;
        ActiveDTOStage = false;
    }

    public string TemplateFile
    {
        get { return _textTransformation.Host.TemplateFile; }
    }

    public string GlobalNamespace { get; set; }

    /// <summary>
    /// When true, all types that are not being generated
    /// are fully qualified to keep them from conflicting with
    /// types that are being generated. Useful when you have
    /// something like a type being generated named System.
    ///
    /// Default is false.
    /// </summary>
    public bool FullyQualifySystemTypes { get; set; }

    /// <summary>
    /// When true, the field names are Camel Cased,
    /// otherwise they will preserve the case they
    /// start with.
    ///
    /// Default is true.
    /// </summary>
    public bool CamelCaseFields { get; set; }

    /// <summary>
    /// Returns the NamespaceName suggested by VS if running inside VS.  Otherwise, returns
    /// null.
    /// </summary>
    public string VsNamespaceSuggestion()
    {
        string suggestion = _textTransformation.Host.ResolveParameterValue("directiveId", "namespaceDirectiveProcessor", "namespaceHint");
        if (String.IsNullOrEmpty(suggestion))
        {
            return null;
        }

        return suggestion;
    }

    public IPropertiesBuilder PropertiesBuilderForType(IPersistentType persistentType)
    {
        return PropertiesBuilderContext.Current.Get(persistentType);
    }

    public IPropertiesBuilder PropertiesBuilderForTypeByOwner(IPropertyBase property)
    {
        IPersistentType realOwner = property.GetRealOwner();
        return PropertiesBuilderForType(realOwner);
    }

    /// <summary>
    /// Returns a string that is safe for use as an identifier in C#.
    /// Keywords are escaped.
    /// </summary>
    public string Escape(string name)
    {
        if (name == null)
        {
            return null;
        }

        return _code.CreateEscapedIdentifier(name);
    }

    public string EscapeNamespace(IPersistentType persistentType)
    {
        return string.IsNullOrEmpty(persistentType.Namespace) ? EscapeNamespace(GlobalNamespace) : EscapeNamespace(persistentType.Namespace);
    }

    public string BuildXtensiveType(OrmType type, params string[] args)
    {
        if (ActiveDTOStage)
        {
            return BuildPocoType(type, args);
        }
            
        return OrmUtils.BuildXtensiveType(type, args);
    }

    private string BuildPocoType(OrmType type, params string[] args)
    {
        string result = string.Empty;

        switch (type)
        {
            case OrmType.EntitySet:
            {
                result = string.Format(TYPE_HASHSET, args[0]);
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Returns the name of the Type object formatted for
    /// use in source code.
    ///
    /// This method changes behavior based on the FullyQualifySystemTypes
    /// setting.
    /// </summary>
    public string Escape(Type clrType)
    {
        if (clrType == null)
        {
            return null;
        }

        string typeName;
        if (FullyQualifySystemTypes)
        {
            typeName = "global::" + clrType.FullName;
        }
        else
        {
            typeName = _code.GetTypeOutput(new CodeTypeReference(clrType));
        }
        return typeName;
    }

    public string EscapeNameWithNamespace(IPersistentType persistentType, IPersistentType dependTypeNamespace)
    {
        string namespaceName = EscapeNamespace(persistentType);

        bool useFullForm = dependTypeNamespace == null || !Util.StringEqual(namespaceName, EscapeNamespace(dependTypeNamespace), true);

        if (useFullForm)
        {
            return string.Format("{0}.{1}", namespaceName, persistentType.Name);
        }

        return persistentType.Name;
    }

    public string EscapeType(ITypedEntitySet typedEntitySet)
    {
        return BuildXtensiveType(OrmType.EntitySet, EscapeNameWithNamespace(typedEntitySet.ItemType, typedEntitySet));
    }

    public string EscapeType(IPropertyBase property)
    {
        string result = null;

        if (property is IScalarProperty)
        {
            IScalarProperty scalarProperty = (IScalarProperty) property;
            Type clrType = scalarProperty.Type.TryGetClrType(null);
            string typeName;

            if (clrType != null)
            {
                typeName = Escape(clrType);
                Defaultable<bool> nullable = scalarProperty.FieldAttribute.Nullable;
                bool isNullable = !nullable.IsDefault() && nullable.Value;
                if (clrType.IsValueType && isNullable)
                {
                    typeName = String.Format(CultureInfo.InvariantCulture, SYSTEM_NULLABLE_FORMAT, typeName);
                }
            }
            else
            {
                typeName = scalarProperty.Type.FullName;
            }

            result = typeName;
        }
        else if (property is INavigationProperty)
        {
            INavigationProperty navigationProperty = (INavigationProperty) property;
            result = navigationProperty.GetPropertyType(_code, EscapeNameWithNamespace,
                delegate(OrmType type, string s)
                {
                    return BuildXtensiveType(type, s);
                });
        }
        else if (property is IStructureProperty)
        {
            IStructureProperty structureProperty = (IStructureProperty) property;
            result = EscapeNameWithNamespace(structureProperty.TypeOf, property.Owner);
        }

        return result;
    }

    public string Escape(IElement element)
    {
        if (element == null)
        {
            return null;
        }

        return element.Name;
    }

    /// <summary>
    /// Returns the NamespaceName with each segment safe to
    /// use as an identifier.
    /// </summary>
    public string EscapeNamespace(string namespaceName)
    {
        if (String.IsNullOrEmpty(namespaceName))
        {
            return namespaceName;
        }

        string[] parts = namespaceName.Split('.');
        namespaceName = String.Empty;
        foreach (string part in parts)
        {
            if (namespaceName != String.Empty)
            {
                namespaceName += ".";
            }

            namespaceName += Escape(part);
        }

        return namespaceName;
    }

    public bool CanDecorateWithContract(bool templateFlag, IPersistentType persistentType, out string decoratorAttribute)
    {
        decoratorAttribute = null;
        bool result = CanDecorateWithContract(templateFlag, persistentType.DataContract);
        if (result && persistentType.TypeKind != PersistentTypeKind.Interface)
        {
            string[] typeAttributes = CreateTypeAttributesCommon(persistentType.DataContract);

            decoratorAttribute = typeAttributes == null || typeAttributes.Length == 0
                                     ? ATTRIBUTE_DATACONTRACT
                                     : string.Format("[{0}]", typeAttributes[0]);

            //decoratorAttribute = ATTRIBUTE_DATACONTRACT;
        }
        else
        {
            result = false;
        }

        return result;
    }

    public bool CanDecorateWithContract(bool templateFlag, IPropertyBase property, out string decoratorAttribute)
    {
        decoratorAttribute = null;
        bool result = CanDecorateWithContract(templateFlag, property.DataMember);
        if (result && property.GetRealOwner().TypeKind != PersistentTypeKind.Interface)
        {
            string[] typeAttributes = CreateTypeAttributesCommon(property.DataMember);

            decoratorAttribute = typeAttributes == null || typeAttributes.Length == 0
                                     ? ATTRIBUTE_DATAMEMBER
                                     : string.Format("[{0}]", typeAttributes[0]);
            //decoratorAttribute = ATTRIBUTE_DATAMEMBER;
        }
        return result;
    }

    private bool CanDecorateWithContract(bool templateFlag, ContractDescriptorBase contractDescriptor)
    {
        return contractDescriptor.Mode == ContractDescriptorApplyMode.Enabled ||
               (templateFlag && contractDescriptor.Mode == ContractDescriptorApplyMode.Default);
    }

    public string FieldName(IPropertyBase property)
    {
        if (property == null)
        {
            return null;
        }

        return FieldName(property.Name);
    }

    private string FieldName(string name)
    {
        return CamelCaseFields ? CamelCase(name) : name;
    }

    public string InheritanceModifier(IPropertyBase propertyBase)
    {
        bool isSealed = false;
        if (propertyBase.Owner is IEntityBase)
        {
            isSealed = (propertyBase.Owner as IEntityBase).InheritanceModifier == InheritanceModifiers.Sealed;
        }

        if (ActiveDTOStage && propertyBase.Owner.TypeKind == PersistentTypeKind.Structure)
        {
            isSealed = true; // simulate 'sealed' to return empty string as modifier
        }

        return propertyBase.Owner.TypeKind == PersistentTypeKind.Interface || isSealed ? string.Empty : MODIFIER_VIRTUAL;
    }

    public string InheritanceModifier(IPersistentType entity)
    {
        IEntityBase entityBase = entity as IEntityBase;

        if (entityBase == null || entityBase.InheritanceModifier == InheritanceModifiers.None)
        {
            return string.Empty;
        }
        return entityBase.InheritanceModifier.ToString().ToLowerInvariant();
    }

    public bool HasInheritance(IPersistentType persistentType)
    {
        int inheritanceCount = 0;

        IInterface @interface = persistentType as IInterface;
        IEntityBase entityBase = persistentType as IEntityBase;

        if (@interface != null)
        {
            if (entityBase == null)
            {
                inheritanceCount = @interface.InheritedInterfaces.Count;
            }
        }

        if (entityBase != null)
        {
            inheritanceCount = entityBase.InheritedInterfaces.Count;
            if (entityBase.BaseType != null)
            {
                inheritanceCount++;
            }
        }

        return inheritanceCount > 0;
    }

    public string TypeIdent(IPersistentType persistentType)
    {
        return persistentType.TypeKind == PersistentTypeKind.Interface 
            ? TYPE_KIND_INTERFACE :
            this.ActiveDTOStage && persistentType.TypeKind == PersistentTypeKind.Structure ? TYPE_KIND_STRUCT : TYPE_KIND_CLASS;
    }

    public string[] InheritanceList(IPersistentType persistentType)
    {
        List<string> result = null;

        if (persistentType is ITypedEntitySet)
        {
            ITypedEntitySet typedEntitySet = (ITypedEntitySet) persistentType;
            result = new List<string> { BuildXtensiveType(OrmType.EntitySet, EscapeNameWithNamespace(typedEntitySet.ItemType, persistentType)) };
            return result.ToArray();
        }

        IInterface @interface = persistentType as IInterface;
        string entityBaseBaseTypeStr = null;
        InheritanceTree inheritanceTree = null;

        if (@interface != null)
        {
            inheritanceTree = InheritanceTreeCache.Get(@interface);
            if (!inheritanceTree.TreeRebuilded)
            {
                inheritanceTree.RebuildTree(false);
            }

            IInterface entityBaseBaseType = null;
            if (@interface is IEntityBase)
            {
                IEntityBase entityBase = (IEntityBase)@interface;
                entityBaseBaseType = entityBase.BaseType;
                entityBaseBaseTypeStr = entityBase.BaseType == null
                                            ? null
                                            : EscapeNameWithNamespace(entityBase.BaseType, persistentType);
            }

            ReadOnlyCollection<InheritanceNode> flatList = inheritanceTree.GetFlatList(InheritanceListMode.CurrentLevel);
            IEnumerable<IInterface> allInheritanceList = flatList.Select(node => node.Interface).Where(item => item != entityBaseBaseType);

            result = allInheritanceList.Select(item => EscapeNameWithNamespace(item, persistentType)).ToList();

        }

        string commonBaseType = persistentType.TypeKind == PersistentTypeKind.Structure
                                    ? BuildXtensiveType(OrmType.Structure)
                                    : BuildXtensiveType(OrmType.Entity);

        bool noInheritance = (result == null || result.Count == 0 && persistentType.TypeKind != PersistentTypeKind.Interface) && string.IsNullOrEmpty(entityBaseBaseTypeStr);

        if (noInheritance)
        {
            result = new List<string>();
            if (!this.ActiveDTOStage)
            {
                result.Add(commonBaseType);
            }
        }
        else
        {
            if (persistentType.TypeKind.In(PersistentTypeKind.Entity, PersistentTypeKind.Structure))
            {
                if (@interface.InheritingByInterfaces.Count == 0 && string.IsNullOrEmpty(entityBaseBaseTypeStr) && !this.ActiveDTOStage)
                {
                    result.Insert(0, commonBaseType);
                }
            }
            else if (persistentType.TypeKind == PersistentTypeKind.Interface)
            {
                bool inheritesCommonIEntity = @interface.InheritsIEntity == InheritsIEntityMode.AlwaysInherit;
                if (!inheritesCommonIEntity)
                {
                    inheritesCommonIEntity = @interface.InheritedInterfaces.Count == 0;
                }

                if (inheritesCommonIEntity && !this.ActiveDTOStage)
                {
                    string commonIEntityType = BuildXtensiveType(OrmType.IEntity);
                    result.Insert(0, commonIEntityType);
                }
            }
        }

        if (!string.IsNullOrEmpty(entityBaseBaseTypeStr))
        {
            result.Insert(0, entityBaseBaseTypeStr);
        }

        return result.ToArray();
    }

    public string CamelCase(string identifier)
    {
        if (String.IsNullOrEmpty(identifier))
        {
            return identifier;
        }

        if (identifier.Length == 1)
        {
            return identifier[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
        }

        return identifier[0].ToString(CultureInfo.InvariantCulture).ToLowerInvariant() + identifier.Substring(1);
    }

    /// <summary>
    /// If the value parameter is null or empty an empty string is returned,
    /// otherwise it retuns value with a single space concatenated on the end.
    /// </summary>
    public string SpaceAfter(string value)
    {
        return StringAfter(value, " ");
    }

    /// <summary>
    /// If the value parameter is null or empty an empty string is returned,
    /// otherwise it retuns value with a single space concatenated on the end.
    /// </summary>
    public string SpaceBefore(string value)
    {
        return StringBefore(" ", value);
    }

    /// <summary>
    /// If the value parameter is null or empty an empty string is returned,
    /// otherwise it retuns value with append concatenated on the end.
    /// </summary>
    public string StringAfter(string value, string append)
    {
        if (String.IsNullOrEmpty(value))
        {
            return String.Empty;
        }

        return value + append;
    }

    public string StringOptional(Func<String> optionalString, bool expression)
    {
        return expression ? optionalString() : string.Empty;
    }

    public string StringOptional(string optionalString, bool expression)
    {
        return expression ? optionalString : string.Empty;
    }

    /// <summary>
    /// If the value parameter is null or empty an empty string is returned,
    /// otherwise it retuns value with prepend concatenated on the front.
    /// </summary>
    public string StringBefore(string prepend, string value)
    {
        if (String.IsNullOrEmpty(value))
        {
            return String.Empty;
        }

        return prepend + value;
    }

    /// <summary>
    /// Retuns as full of a name as possible, if a namespace is provided
    /// the namespace and name are combined with a period, otherwise just
    /// the name is returned.
    /// </summary>
    public string CreateFullName(string namespaceName, string name)
    {
        if (String.IsNullOrEmpty(namespaceName))
        {
            return name;
        }

        return namespaceName + "." + name;
    }

    public string CreateLiteral(object value)
    {
        if (value == null)
        {
            return "null";
            //return string.Empty;
        }

        Type type = value.GetType();
        if (type.IsEnum)
        {
            return OrmEnumUtils.BuildXtensiveType(value);
        }
        if (type == typeof(Guid))
        {
            return string.Format(CultureInfo.InvariantCulture, TYPE_FORMAT_GUID,
                ((Guid)value).ToString("D", CultureInfo.InvariantCulture));
        }
        
        if (type == typeof(DateTime))
        {
            return string.Format(CultureInfo.InvariantCulture, TYPE_FORMAT_DATETIME,
                ((DateTime)value).Ticks);
        }
        
        if (type == typeof(byte[]))
        {
            var arrayInit = string.Join(", ", ((byte[])value).Select(b => b.ToString(CultureInfo.InvariantCulture)).ToArray());
            return string.Format(CultureInfo.InvariantCulture, TYPE_FORMAT_BYTE_ARRAY, arrayInit);
        }
        
        if (type == typeof(TimeSpan))
        {
            var ts = (TimeSpan) value;
            return string.Format(CultureInfo.InvariantCulture, TYPE_FORMAT_TIMESPAN,
                ts.Hours, ts.Minutes, ts.Seconds);
        }

        if (type == typeof(DateTimeOffset))
        {
            var dto = (DateTimeOffset)value;
            return string.Format(CultureInfo.InvariantCulture, TYPE_FORMAT_DATETIMEOFFSET,
                dto.Ticks, dto.Offset.Ticks);
        }

        if (type == typeof(string[]))
        {
            var ss = (string[]) value;
            return "new[] {" + Util.JoinCollection(ss, ",", "\"") + "}";
        }
        
        if (type == typeof(ObjectValue))
        {
            ObjectValue objectValue = (ObjectValue) value;
            if (objectValue.UseCustomExpression)
            {
                return objectValue.CustomExpression;
            }
            
            return CreateLiteral(objectValue.Value);
        }

        var expression = new CodePrimitiveExpression(value);
        var writer = new StringWriter();
        CSharpCodeProvider code = new CSharpCodeProvider();
        code.GenerateCodeFromExpression(expression, writer, new CodeGeneratorOptions());
        return writer.ToString();
    }

    public string[] CreateTypeAttributes(IPersistentType persistentType, IPropertiesBuilder propertiesBuilder)
    {
        if (this.ActiveDTOStage)
        {
            return new string[0];
        }

        IEnumerable<IOrmAttribute> typeAttributes = null;
        if (propertiesBuilder != null)
        {
            typeAttributes = propertiesBuilder.MergedTypeAttributes;
        }

        if (typeAttributes == null)
        {
            typeAttributes = persistentType.TypeAttributes;
        }

        ITypeAttributesBuilder attributesBuilder = TypeAttributesBuilder.Create(persistentType, typeAttributes);
        string[] extraAttributes = persistentType.TypeKind == PersistentTypeKind.Interface ? new string[0] : new string[] { ATTRIBUTE_SERIALIZABLE };

        return CreateTypeAttributes(attributesBuilder, typeAttributes.ToArray()).Concat(extraAttributes).ToArray();
    }

    public string[] CreateTypeAttributes(IPropertyBase property, IPropertiesBuilder propertiesBuilder)
    {
        if (this.ActiveDTOStage)
        {
            return new string[0];
        }

        IEnumerable<IOrmAttribute> typeAttributes = null;
        if (propertiesBuilder != null)
        {
            typeAttributes = propertiesBuilder.GetPropertyTypeAttributes(property);

            property = propertiesBuilder.GetProperty(property, InheritanceMember.Inherited);
        }

        if (typeAttributes == null)
        {
            typeAttributes = property.TypeAttributes;
        }

        ITypeAttributesBuilder attributesBuilder = TypeAttributesBuilder.Create(property, typeAttributes);

        return CreateTypeAttributes(attributesBuilder, typeAttributes);
    }

    public string[] CreateTypeAttributesCommon(IOrmAttribute attribute)
    {
        return CreateTypeAttributesCommon(new[] {attribute});
    }

    public string[] CreateTypeAttributesCommon(IEnumerable<IOrmAttribute> attributes)
    {
        ITypeAttributesBuilder attributesBuilder = TypeAttributesBuilder.CreateCommon(attributes);

        return CreateTypeAttributes(attributesBuilder, attributes, true);
    }

    private string[] CreateTypeAttributes(ITypeAttributesBuilder attributesBuilder, IEnumerable<IOrmAttribute> attributes,
        bool allowInPoco = false)
    {
        if (this.ActiveDTOStage && !allowInPoco)
        {
            return new string[0];
        }

        List<string> result = new List<string>();

        if (attributes != null)
        {
            foreach (var attribute in attributes)
            {
                foreach (var attributeGroup in attributesBuilder.GetAttributeGroups(attribute))
                {
                    var attributeGroupItems = attributesBuilder.GetAttributeGroupItems(attribute, attributeGroup);
                    StringBuilder attr = new StringBuilder(attributeGroup.FormatFullName());
                    if (attributeGroupItems.Count > 0)
                    {
                        int beforeItemsLen = attr.Length;

                        bool handledByCustom = attributesBuilder.BuildCustomFormatComplete(attributeGroup,
                            attributeGroupItems, ref attr);

                        if (!handledByCustom)
                        {
                            foreach (KeyValuePair<string, Defaultable> attributeGroupItem in attributesBuilder.SortAttributeGroupItems(
                                attributeGroup, attributeGroupItems))
                            {
                                string attrItemName = attributeGroupItem.Key;
                                Defaultable attrItemValue = attributeGroupItem.Value;
                                if (!attrItemValue.IsDefault())
                                {
                                    object value = attrItemValue.GetValue();
                                    string valueLiteral = CreateLiteral(value);

                                    bool buildedCustomFormat = attributesBuilder.BuildCustomFormat(attributeGroup,
                                        attrItemName, valueLiteral, value, ref attr);

                                    if (!buildedCustomFormat)
                                    {
                                        attr.AppendFormat("{0}={1},", attrItemName, valueLiteral);
                                    }
                                }
                            }
                        }

                        if (attr[attr.Length - 1] == ',')
                        {
                            attr = attr.Remove(attr.Length - 1, 1);
                        }

                        if (attr.Length > beforeItemsLen)
                        {
                            attr.Insert(beforeItemsLen, "(");
                            attr.Append(")");
                        }
                    }

                    result.Add(attr.ToString());
                }
            }
        }
        
        return result.ToArray();
    }
}