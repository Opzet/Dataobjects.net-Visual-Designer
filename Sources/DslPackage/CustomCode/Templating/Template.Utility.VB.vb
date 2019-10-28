Imports System
Imports System.CodeDom
Imports System.Globalization
Imports System.IO
Imports System.CodeDom.Compiler
Imports System.Text
Imports System.Linq
Imports System.Collections.Generic
Imports Microsoft.VisualBasic
Imports System.Collections.ObjectModel
Imports TXSoftware.DataObjectsNetEntityModel.Dsl
Imports TXSoftware.DataObjectsNetEntityModel.Common

'~T4_TEMPLATE_HEADER_BEGIN~
'#><#@ import namespace="Microsoft.VisualBasic"
'#><#@ import namespace="TXSoftware.DataObjectsNetEntityModel.Common"
'#><#@ import namespace="TXSoftware.DataObjectsNetEntityModel.Common.UIEditors"
'#><#+
'~T4_TEMPLATE_HEADER_END~

Public NotInheritable Class Accessibility
    Private Const VISIBILITY_PRIVATE As String = "Private"

    Private Sub New()
    End Sub
    Public Shared Function ForType(element As IAccessibleElement) As String
        Dim accessibility As String = element.Access.ToString() '.ToLowerInvariant()
        If TypeOf element Is IPropertyBase Then
            If TryCast(element, IPropertyBase).Owner.TypeKind = PersistentTypeKind.[Interface] Then
                accessibility = String.Empty
            End If
        End If

        Return accessibility
    End Function

    Public Shared Function VisibleSetter([property] As IPropertyBase, propertiesBuilder As IPropertiesBuilder) As Boolean
        Dim setter As String = ForSetter([property], propertiesBuilder)
        If setter = VISIBILITY_PRIVATE AndAlso [property].Owner.TypeKind = PersistentTypeKind.[Interface] Then
            Return False
        End If

        Return True
    End Function

    Public Shared Function ForSetter([property] As IPropertyBase, propertiesBuilder As IPropertiesBuilder) As String
        Dim result As String = String.Empty

        Select Case [property].PropertyKind
            Case PropertyKind.Scalar
                If True Then
                    Dim typeAttributes As IOrmAttribute() = propertiesBuilder.GetPropertyTypeAttributes([property])
                    Dim keyAttribute As OrmKeyAttribute = DirectCast(typeAttributes.[Single](Function(item) TypeOf item Is OrmKeyAttribute), OrmKeyAttribute)

                    result = If(keyAttribute.Enabled, VISIBILITY_PRIVATE, String.Empty)
                    Exit Select
                End If
            Case PropertyKind.Navigation
                If True Then
                    Dim navigationProperty As INavigationProperty = DirectCast([property], INavigationProperty)
                    If navigationProperty.Multiplicity = MultiplicityKind.Many Then
                        result = VISIBILITY_PRIVATE
                    End If

                    Exit Select
                End If
        End Select

        Return result
    End Function
End Class

Public Class CodeGenerationTools
    Private Const SYSTEM_NULLABLE_FORMAT As String = "System.Nullable(Of {0})"
    Private Const MODIFIER_VIRTUAL As String = "Overridable"
    Private Const TYPE_KIND_INTERFACE As String = "Interface"
    Private Const TYPE_KIND_CLASS As String = "Class"
    Private Const TYPE_FORMAT_GUID As String = "New System.Guid(""{0}"")"
    Private Const TYPE_FORMAT_DATETIME As String = "New System.DateTime({0}, System.DateTimeKind.Unspecified)"
    Private Const TYPE_FORMAT_BYTE_ARRAY As String = "New System.Byte[] {{{0}}}"
    Private Const TYPE_FORMAT_TIMESPAN As String = "New System.TimeSpan({0}, {1}, {2})"
    Private Const TYPE_FORMAT_DATETIMEOFFSET As String = "New System.DateTimeOffset({0}, new System.TimeSpan({1}))"

    Private ReadOnly _textTransformation As DynamicTextTransformation
    Private ReadOnly _code As VBCodeProvider

    ''' <summary>
    ''' Initializes a new CodeGenerationTools object with the TextTransformation (T4 generated class)
    ''' that is currently running
    ''' </summary>
    Public Sub New(textTransformation As Object)
        If textTransformation Is Nothing Then
            Throw New ArgumentNullException("textTransformation")
        End If

        _textTransformation = DynamicTextTransformation.Create(textTransformation)
        _code = New VBCodeProvider()
        FullyQualifySystemTypes = False
        CamelCaseFields = True
    End Sub

    Public ReadOnly Property TemplateFile() As String
        Get
            Return _textTransformation.Host.TemplateFile
        End Get
    End Property

    Public Property GlobalNamespace() As String
        Get
            Return m_GlobalNamespace
        End Get
        Set(value As String)
            m_GlobalNamespace = Value
        End Set
    End Property
    Private m_GlobalNamespace As String

    ''' <summary>
    ''' When true, all types that are not being generated
    ''' are fully qualified to keep them from conflicting with
    ''' types that are being generated. Useful when you have
    ''' something like a type being generated named System.
    '''
    ''' Default is false.
    ''' </summary>
    Public Property FullyQualifySystemTypes() As Boolean
        Get
            Return m_FullyQualifySystemTypes
        End Get
        Set(value As Boolean)
            m_FullyQualifySystemTypes = Value
        End Set
    End Property
    Private m_FullyQualifySystemTypes As Boolean

    ''' <summary>
    ''' When true, the field names are Camel Cased,
    ''' otherwise they will preserve the case they
    ''' start with.
    '''
    ''' Default is true.
    ''' </summary>
    Public Property CamelCaseFields() As Boolean
        Get
            Return m_CamelCaseFields
        End Get
        Set(value As Boolean)
            m_CamelCaseFields = Value
        End Set
    End Property
    Private m_CamelCaseFields As Boolean

    ''' <summary>
    ''' Returns the NamespaceName suggested by VS if running inside VS.  Otherwise, returns
    ''' null.
    ''' </summary>
    Public Function VsNamespaceSuggestion() As String
        Dim suggestion As String = _textTransformation.Host.ResolveParameterValue("directiveId", "namespaceDirectiveProcessor", "namespaceHint")
        If [String].IsNullOrEmpty(suggestion) Then
            Return Nothing
        End If

        Return suggestion
    End Function

    ''' <summary>
    ''' Returns a string that is safe for use as an identifier in C#.
    ''' Keywords are escaped.
    ''' </summary>
    Public Function Escape(name As String) As String
        If name Is Nothing Then
            Return Nothing
        End If

        Return _code.CreateEscapedIdentifier(name)
    End Function

    Public Function EscapeNamespace(persistentType As IPersistentType) As String
        Return If(String.IsNullOrEmpty(persistentType.[Namespace]), EscapeNamespace(GlobalNamespace), EscapeNamespace(persistentType.[Namespace]))
    End Function

    Public Function BuildXtensiveType(type As OrmType, ParamArray args As String()) As String
        If type = OrmType.EntitySet Then
            Dim entitySet As String = OrmUtils.BuildXtensiveType(OrmType.EntitySet)
            Return String.Format("{0}(Of {1})", entitySet, args(0))
        End If

        Return OrmUtils.BuildXtensiveType(type, args)
    End Function

    ''' <summary>
    ''' Returns the name of the Type object formatted for
    ''' use in source code.
    '''
    ''' This method changes behavior based on the FullyQualifySystemTypes
    ''' setting.
    ''' </summary>
    Public Function Escape(clrType As Type) As String
        If clrType Is Nothing Then
            Return Nothing
        End If

        Dim typeName As String
        If FullyQualifySystemTypes Then
            typeName = "global::" & Convert.ToString(clrType.FullName)
        Else
            typeName = _code.GetTypeOutput(New CodeTypeReference(clrType))
        End If
        Return typeName
    End Function

    Public Function EscapeNameWithNamespace(persistentType As IPersistentType, dependTypeNamespace As IPersistentType) As String
        Dim namespaceName As String = EscapeNamespace(persistentType)

        Dim useFullForm As Boolean = dependTypeNamespace Is Nothing OrElse Not Util.StringEqual(namespaceName, EscapeNamespace(dependTypeNamespace), True)

        If useFullForm Then
            Return String.Format("{0}.{1}", namespaceName, persistentType.Name)
        End If

        Return persistentType.Name
    End Function

    Public Function EscapeType([property] As IPropertyBase) As String
        Dim result As String = Nothing

        If TypeOf [property] Is IScalarProperty Then
            Dim scalarProperty As IScalarProperty = DirectCast([property], IScalarProperty)
            Dim clrType As Type = scalarProperty.ClrType
            Dim typeName As String = Escape(clrType)
            Dim nullable As Defaultable(Of Boolean) = scalarProperty.FieldAttribute.Nullable
            Dim isNullable As Boolean = Not nullable.IsDefault() AndAlso nullable.Value
            If clrType.IsValueType AndAlso isNullable Then
                Return [String].Format(CultureInfo.InvariantCulture, SYSTEM_NULLABLE_FORMAT, typeName)
            End If

            result = typeName
        ElseIf TypeOf [property] Is INavigationProperty Then
            Dim navigationProperty As INavigationProperty = DirectCast([property], INavigationProperty)
            result = navigationProperty.GetPropertyType(_code, AddressOf EscapeNameWithNamespace)
        ElseIf TypeOf [property] Is IStructureProperty Then
            Dim structureProperty As IStructureProperty = DirectCast([property], IStructureProperty)
            result = EscapeNameWithNamespace(structureProperty.[TypeOf], [property].Owner)
        End If

        Return result
    End Function

    Public Function Escape(element As IElement) As String
        If element Is Nothing Then
            Return Nothing
        End If

        Return element.Name
    End Function

    ''' <summary>
    ''' Returns the NamespaceName with each segment safe to
    ''' use as an identifier.
    ''' </summary>
    Public Function EscapeNamespace(namespaceName As String) As String
        If [String].IsNullOrEmpty(namespaceName) Then
            Return namespaceName
        End If

        Dim parts As String() = namespaceName.Split("."c)
        namespaceName = [String].Empty
        For Each part As String In parts
            If namespaceName <> [String].Empty Then
                namespaceName += "."
            End If

            namespaceName += Escape(part)
        Next

        Return namespaceName
    End Function

    Public Function FieldName([property] As IPropertyBase) As String
        If [property] Is Nothing Then
            Return Nothing
        End If

        Return FieldName([property].Name)
    End Function

    Private Function FieldName(name As String) As String
        Return If(CamelCaseFields, CamelCase(name), name)
    End Function

    Public Function InheritanceModifier(propertyBase As IPropertyBase) As String
        Dim isSealed As Boolean = False
        If TypeOf propertyBase.Owner Is IEntityBase Then
            isSealed = TryCast(propertyBase.Owner, IEntityBase).InheritanceModifier = InheritanceModifiers.Sealed
        End If

        Return If(propertyBase.Owner.TypeKind = PersistentTypeKind.[Interface] OrElse isSealed, String.Empty, MODIFIER_VIRTUAL)
    End Function

    Public Function InheritanceModifier(entity As IPersistentType) As String
        Dim entityBase As IEntityBase = TryCast(entity, IEntityBase)

        If entityBase Is Nothing OrElse entityBase.InheritanceModifier = InheritanceModifiers.None Then
            Return String.Empty
        End If
        Return If(entityBase.InheritanceModifier = InheritanceModifiers.Abstract, "MustInherit", "NotInheritable")
    End Function

    Public Function HasInheritance(persistentType As IPersistentType) As Boolean
        Dim inheritanceCount As Integer = 0

        Dim [interface] As IInterface = TryCast(persistentType, IInterface)
        Dim entityBase As IEntityBase = TryCast(persistentType, IEntityBase)

        If [interface] IsNot Nothing Then
            If entityBase Is Nothing Then
                inheritanceCount = [interface].InheritedInterfaces.Count
            End If
        End If

        If entityBase IsNot Nothing Then
            inheritanceCount = entityBase.InheritedInterfaces.Count
            If entityBase.BaseType IsNot Nothing Then
                inheritanceCount += 1
            End If
        End If

        Return inheritanceCount > 0
    End Function

    Public Function TypeIdent(persistentType As IPersistentType) As String
        Return If(persistentType.TypeKind = PersistentTypeKind.[Interface], TYPE_KIND_INTERFACE, TYPE_KIND_CLASS)
    End Function

    Public Function InheritanceList(persistentType As IPersistentType) As Tuple(Of String(), String()) 'As String()
        'Dim result As List(Of String) = Nothing
        'Dim result As Tuple(Of String(), String()) = Nothing
        ' in result - first array of strings is inheritance list of entities, seconds is of interfaces

        If TypeOf persistentType Is ITypedEntitySet Then
            Dim typedEntitySet As ITypedEntitySet = DirectCast(persistentType, ITypedEntitySet)
            Dim _result = New List(Of String)() From { _
             BuildXtensiveType(OrmType.EntitySet, EscapeNameWithNamespace(typedEntitySet.ItemType, persistentType)) _
            }
            'Return result.ToArray()
            Return New Tuple(Of String(), String())(_result.ToArray(), Nothing)
        End If

        Dim [interface] As IInterface = TryCast(persistentType, IInterface)
        Dim entityBaseBaseTypeStr As String = Nothing
        Dim entitiesList As List(Of String) = New List(Of String)()
        Dim interfacesList As List(Of String) = New List(Of String)()

        Dim entityBaseBaseType As IInterface = Nothing

        If [interface] IsNot Nothing Then
            Dim inheritanceTree = InheritanceTreeCache.[Get]([interface])
            If Not inheritanceTree.TreeRebuilded Then
                inheritanceTree.RebuildTree(False)
            End If

            If TypeOf [interface] Is IEntityBase Then
                Dim entityBase As IEntityBase = DirectCast([interface], IEntityBase)
                entityBaseBaseType = entityBase.BaseType
                entityBaseBaseTypeStr = If(entityBase.BaseType Is Nothing, Nothing, EscapeNameWithNamespace(entityBase.BaseType, persistentType))
            End If

            Dim flatList As ReadOnlyCollection(Of InheritanceNode) = inheritanceTree.GetFlatList(InheritanceListMode.CurrentLevel)
            Dim allInheritanceList As IEnumerable(Of IInterface) = flatList.[Select](Function(node) node.[Interface]).[Where](Function(item) item IsNot entityBaseBaseType)

            If (allInheritanceList.Count() = 0) Then
                'result = New List(Of String)()
                entitiesList = New List(Of String)()
                interfacesList = New List(Of String)()
            Else
                'result = allInheritanceList.[Select](Function(item) EscapeNameWithNamespace(item, persistentType)).ToList()
                entitiesList = allInheritanceList.Where(Function(item) item.TypeKind <> PersistentTypeKind.[Interface]).[Select](Function(item) EscapeNameWithNamespace(item, persistentType)).ToList()
                interfacesList = allInheritanceList.Where(Function(item) item.TypeKind = PersistentTypeKind.[Interface]).[Select](Function(item) EscapeNameWithNamespace(item, persistentType)).ToList()
            End If

        End If

        Dim commonBaseType As String = If(persistentType.TypeKind = PersistentTypeKind.[Structure], BuildXtensiveType(OrmType.[Structure]), OrmUtils.BuildXtensiveType(OrmType.Entity))

        Dim noInheritance As Boolean = (entitiesList Is Nothing OrElse entitiesList.Count = 0 AndAlso persistentType.TypeKind <> PersistentTypeKind.[Interface]) _
                                       AndAlso entityBaseBaseType Is Nothing

        If noInheritance Then
            entitiesList = New List(Of String)() From { _
             commonBaseType _
            }
        Else
            If persistentType.TypeKind.[In](PersistentTypeKind.Entity, PersistentTypeKind.[Structure]) Then
                If [interface].InheritingByInterfaces.Count = 0 AndAlso String.IsNullOrEmpty(entityBaseBaseTypeStr) Then
                    entitiesList.Insert(0, commonBaseType)
                End If
            ElseIf persistentType.TypeKind = PersistentTypeKind.[Interface] Then
                Dim inheritesCommonIEntity As Boolean = [interface].InheritsIEntity = InheritsIEntityMode.AlwaysInherit
                If Not inheritesCommonIEntity Then
                    inheritesCommonIEntity = [interface].InheritedInterfaces.Count = 0
                End If

                If inheritesCommonIEntity Then
                    Dim commonIEntityType As String = BuildXtensiveType(OrmType.IEntity)

                    If interfacesList Is Nothing Then
                        interfacesList = New List(Of String)()
                    End If

                    interfacesList.Insert(0, commonIEntityType)
                End If
            End If
        End If

        If Not String.IsNullOrEmpty(entityBaseBaseTypeStr) Then
            entitiesList.Insert(0, entityBaseBaseTypeStr)
        End If

        'Return result.ToArray()
        Return New Tuple(Of String(), String())(entitiesList.ToArray(), interfacesList.ToArray())
    End Function

    Public Function CamelCase(identifier As String) As String
        If [String].IsNullOrEmpty(identifier) Then
            Return identifier
        End If

        If identifier.Length = 1 Then
            Return identifier(0).ToString(CultureInfo.InvariantCulture).ToLowerInvariant()
        End If

        Return identifier(0).ToString(CultureInfo.InvariantCulture).ToLowerInvariant() & identifier.Substring(1)
    End Function

    ''' <summary>
    ''' If the value parameter is null or empty an empty string is returned,
    ''' otherwise it retuns value with a single space concatenated on the end.
    ''' </summary>
    Public Function SpaceAfter(value As String) As String
        Return StringAfter(value, " ")
    End Function

    ''' <summary>
    ''' If the value parameter is null or empty an empty string is returned,
    ''' otherwise it retuns value with a single space concatenated on the end.
    ''' </summary>
    Public Function SpaceBefore(value As String) As String
        Return StringBefore(" ", value)
    End Function

    ''' <summary>
    ''' If the value parameter is null or empty an empty string is returned,
    ''' otherwise it retuns value with append concatenated on the end.
    ''' </summary>
    Public Function StringAfter(value As String, append As String) As String
        If [String].IsNullOrEmpty(value) Then
            Return [String].Empty
        End If

        Return value & append
    End Function

    Public Function StringOptional([optional] As String, expression As Boolean) As String
        Return If(expression, [optional], String.Empty)
    End Function

    ''' <summary>
    ''' If the value parameter is null or empty an empty string is returned,
    ''' otherwise it retuns value with prepend concatenated on the front.
    ''' </summary>
    Public Function StringBefore(prepend As String, value As String) As String
        If [String].IsNullOrEmpty(value) Then
            Return [String].Empty
        End If

        Return prepend & value
    End Function

    ''' <summary>
    ''' Retuns as full of a name as possible, if a namespace is provided
    ''' the namespace and name are combined with a period, otherwise just
    ''' the name is returned.
    ''' </summary>
    Public Function CreateFullName(namespaceName As String, name As String) As String
        If [String].IsNullOrEmpty(namespaceName) Then
            Return name
        End If

        Return namespaceName & "." & name
    End Function

    Public Function CreateLiteral(value As Object) As String
        If value Is Nothing Then
            'Return String.Empty
            Return "Nothing"
        End If

        Dim type As Type = value.[GetType]()
        If type.IsEnum Then
            Return OrmEnumUtils.BuildXtensiveType(value)
        End If
        If type = GetType(Guid) Then
            Return String.Format(CultureInfo.InvariantCulture, TYPE_FORMAT_GUID, DirectCast(value, Guid).ToString("D", CultureInfo.InvariantCulture))
        End If

        If type = GetType(DateTime) Then
            Return String.Format(CultureInfo.InvariantCulture, TYPE_FORMAT_DATETIME, DirectCast(value, DateTime).Ticks)
        End If

        If type = GetType(Byte()) Then
            Dim arrayInit = String.Join(", ", DirectCast(value, Byte()).[Select](Function(b) b.ToString(CultureInfo.InvariantCulture)).ToArray())
            Return String.Format(CultureInfo.InvariantCulture, TYPE_FORMAT_BYTE_ARRAY, arrayInit)
        End If

        If type = GetType(TimeSpan) Then
            Dim ts = DirectCast(value, TimeSpan)
            Return String.Format(CultureInfo.InvariantCulture, TYPE_FORMAT_TIMESPAN, ts.Hours, ts.Minutes, ts.Seconds)
        End If

        If type = GetType(DateTimeOffset) Then
            Dim dto = DirectCast(value, DateTimeOffset)
            Return String.Format(CultureInfo.InvariantCulture, TYPE_FORMAT_DATETIMEOFFSET, dto.Ticks, dto.Offset.Ticks)
        End If

        If type = GetType(String()) Then
            Dim ss = DirectCast(value, String())
            Return "New String() {" & Util.JoinCollection(ss, ",", """") & "}"
        End If

        If type = GetType(ObjectValue) Then
            Dim objectValue As ObjectValue = DirectCast(value, ObjectValue)
            If objectValue.UseCustomExpression Then
                Return objectValue.CustomExpression
            End If

            Return CreateLiteral(objectValue.Value)
        End If

        Dim expression = New CodePrimitiveExpression(value)
        Dim writer = New StringWriter()
        Dim code As New VBCodeProvider()
        code.GenerateCodeFromExpression(expression, writer, New CodeGeneratorOptions())
        Return writer.ToString()
    End Function

    Public Function CreateTypeAttributes(persistentType As IPersistentType, propertiesBuilder As IPropertiesBuilder) As String()
        Dim typeAttributes As IEnumerable(Of IOrmAttribute) = Nothing
        If propertiesBuilder IsNot Nothing Then
            typeAttributes = propertiesBuilder.MergedTypeAttributes
        End If

        If typeAttributes Is Nothing Then
            typeAttributes = persistentType.TypeAttributes
        End If

        Dim attributesBuilder As ITypeAttributesBuilder = TypeAttributesBuilder.Create(persistentType, typeAttributes)

        Return CreateTypeAttributes(attributesBuilder, typeAttributes.ToArray())
    End Function

    Public Function CreateTypeAttributes([property] As IPropertyBase, propertiesBuilder As IPropertiesBuilder) As String()
        Dim typeAttributes As IEnumerable(Of IOrmAttribute) = Nothing
        If propertiesBuilder IsNot Nothing Then
            typeAttributes = propertiesBuilder.GetPropertyTypeAttributes([property])

            [property] = propertiesBuilder.GetProperty([property], InheritanceMember.Inherited)
        End If

        If typeAttributes Is Nothing Then
            typeAttributes = [property].TypeAttributes
        End If

        Dim attributesBuilder As ITypeAttributesBuilder = TypeAttributesBuilder.Create([property], typeAttributes)

        Return CreateTypeAttributes(attributesBuilder, typeAttributes)
    End Function

    Private Function CreateTypeAttributes(attributesBuilder As ITypeAttributesBuilder, attributes As IEnumerable(Of IOrmAttribute)) As String()
        Dim result As New List(Of String)()

        If attributes IsNot Nothing Then
            For Each attribute As IOrmAttribute In attributes
                For Each attributeGroup As OrmAttributeGroup In attributesBuilder.GetAttributeGroups(attribute)
                    Dim attributeGroupItems = attributesBuilder.GetAttributeGroupItems(attribute, attributeGroup)
                    Dim attr As New StringBuilder(attributeGroup.FormatFullName())
                    If attributeGroupItems.Count > 0 Then
                        Dim beforeItemsLen As Integer = attr.Length

                        Dim handledByCustom As Boolean = attributesBuilder.BuildCustomFormatComplete(attributeGroup, attributeGroupItems, attr)

                        If Not handledByCustom Then
                            For Each attributeGroupItem As KeyValuePair(Of String, Defaultable) In attributesBuilder.SortAttributeGroupItems(attributeGroup, attributeGroupItems)
                                Dim attrItemName As String = attributeGroupItem.Key
                                Dim attrItemValue As Defaultable = attributeGroupItem.Value
                                If Not attrItemValue.IsDefault() Then
                                    Dim value As Object = attrItemValue.GetValue()
                                    Dim valueLiteral As String = CreateLiteral(value)

                                    Dim buildedCustomFormat As Boolean = attributesBuilder.BuildCustomFormat(attributeGroup, attrItemName, valueLiteral, value, attr)

                                    If Not buildedCustomFormat Then
                                        attr.AppendFormat("{0}:={1},", attrItemName, valueLiteral)
                                    End If
                                End If
                            Next
                        End If

                        If attr(attr.Length - 1) = ","c Then
                            attr = attr.Remove(attr.Length - 1, 1)
                        End If

                        If attr.Length > beforeItemsLen Then
                            attr.Insert(beforeItemsLen, "(")
                            attr.Append(")")
                        End If
                    End If

                    result.Add(attr.ToString())
                Next
            Next
        End If

        Return result.ToArray()
    End Function
End Class