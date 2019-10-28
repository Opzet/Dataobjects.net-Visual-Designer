<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="99eed35b-b20e-4623-a645-6f62554bb450" Description="DataObjects.Net Entity Model Designer" Name="DONetEntityModelDesigner" DisplayName="DataObjects.Net Entity Model Designer" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" ProductName="DataObjects.Net Entity Model Designer" CompanyName="TX Software" PackageGuid="429f3394-2cd0-4913-8ca0-0177f2b76897" PackageNamespace="TXSoftware.DataObjectsNetEntityModel.Dsl" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="80d70f04-2f78-4427-a240-e99fe6bec68a" Description="The root in which all other elements are embedded. Appears as a diagram." Name="EntityModel" DisplayName="Entity Model" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <Properties>
        <DomainProperty Id="11c032a8-2ba1-407e-9d52-291309789523" Description="" Name="Namespace" DisplayName="Namespace">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="24d997f7-79ff-4c51-ac38-5077863cfdbc" Description="" Name="ModelRoot" DisplayName="Model Root" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="IModelRoot" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1dae6de8-9c44-4185-898e-60d4c5d71ff6" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.EntityModel.Version" Name="Version" DisplayName="Version" Kind="Calculated" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Notes>Creates an embedding link when an element is dropped onto a model. </Notes>
          <Index>
            <DomainClassMoniker Name="PersistentType" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityModelHasPersistentTypes.PersistentTypes</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="e3924dcc-7d94-47e0-b067-379b4ee51266" Description="" Name="PersistentType" DisplayName="Persistent Type" InheritanceModifier="Abstract" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" HasCustomConstructor="true">
      <Properties>
        <DomainProperty Id="ba091093-9248-4c23-b584-3bb96e316b96" Description="" Name="Name" DisplayName="Name" DefaultValue="" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d707c727-1db8-4b1e-b860-e94d29ab78e2" Description="" Name="Namespace" DisplayName="Namespace">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c58584b6-3201-4b11-9d42-031418a55790" Description="" Name="TypeName" DisplayName="Type Name" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f1991af7-0111-4195-b322-15f6eedc2642" Description="" Name="Access" DisplayName="Access" DefaultValue="Public">
          <Type>
            <DomainEnumerationMoniker Name="AccessModifier" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8115e9cf-c284-46e5-b581-7b41d4b50bfd" Description="" Name="InheritsFrom" DisplayName="Inherits From" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c99d2699-9e28-489b-84f2-89c677dbb608" Description="" Name="InheritsFromName" DisplayName="Inherits From Name" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="3cc68dd2-3eea-4019-90a6-bf459e986395" Description="" Name="TypeKind" DisplayName="Type Kind" Kind="Calculated" IsBrowsable="false">
          <Type>
            <DomainEnumerationMoniker Name="PersistentTypeKind" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5087c50d-1cb3-4677-a94c-e3643d08e9ea" Description="" Name="TypeDescription" DisplayName="Type Description" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f967f0d5-52e7-4000-9126-c330385ffcba" Description="" Name="Documentation" DisplayName="Documentation">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="28da08c8-e890-4c0a-b675-7d7e37cf24f8" Description="" Name="DataContract" DisplayName="Data Contract">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/DataContractDescriptor" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="a08b1b7a-9be5-4f93-b9a7-cc36dcdad1e8" Description="" Name="Entity" DisplayName="Entity" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" HasCustomConstructor="true">
      <BaseClass>
        <DomainClassMoniker Name="EntityBase" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="802a1868-3e11-41a0-a451-aae3cdbadfa6" Description="" Name="HierarchyRootAttribute" DisplayName="Hierarchy Root">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/OrmHierarchyRootAttribute" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b739264e-a361-45c5-838b-74c45f2a5d71" Description="" Name="IsHierarchyRoot" DisplayName="Is Hierarchy Root" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="bb52c836-8ffe-44ca-a101-15267f1f78d1" Description="" Name="KeyGenerator" DisplayName="Key Generator">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/OrmKeyGeneratorAttribute" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b8e69e25-1847-4793-b4c6-260d4c4e9e9a" Description="" Name="TypeDiscriminatorValue" DisplayName="Type Discriminator Value">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/OrmTypeDiscriminatorValueAttribute" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="e7347eae-9f7f-4f9e-a625-4b1b60873992" Description="" Name="Structure" DisplayName="Structure" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <BaseClass>
        <DomainClassMoniker Name="EntityBase" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="8a1292cb-aa81-4162-8136-58fe40187d83" Description="" Name="Interface" DisplayName="Interface" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <BaseClass>
        <DomainClassMoniker Name="PersistentType" />
      </BaseClass>
      <CustomTypeDescriptor>
        <DomainTypeDescriptor CustomCoded="true" />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="e62fe112-a5f4-493a-b52b-3f040a5c6946" Description="" Name="InheritInterfaces" DisplayName="Inherit Interfaces" Kind="Calculated" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="153d4790-a381-463f-b5bd-117bb5be6071" Description="" Name="InheritsIEntity" DisplayName="Inherits IEntity">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/InheritsIEntityMode" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="a444bbf1-d845-4765-9704-2973712543d2" Description="" Name="EntityBase" DisplayName="Entity Base" InheritanceModifier="Abstract" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <BaseClass>
        <DomainClassMoniker Name="Interface" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="03a89120-aa19-41c5-8486-d2da6b6a668a" Description="" Name="InheritanceModifier" DisplayName="Inheritance Modifier" DefaultValue="None">
          <Type>
            <DomainEnumerationMoniker Name="InheritanceModifiers" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="09ba427a-0c25-4966-90ba-2e8e0df7e15e" Description="" Name="PropertyBase" DisplayName="Property Base" InheritanceModifier="Abstract" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" HasCustomConstructor="true">
      <CustomTypeDescriptor>
        <DomainTypeDescriptor CustomCoded="true" />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="c9bf73e8-8d94-4114-b0af-ea0172fa601c" Description="" Name="Name" DisplayName="Name" Category="Common" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e220b206-6c2e-457e-bc60-00ebe53e1ad1" Description="" Name="TypeName" DisplayName="Type Name" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="deaf6e10-5a17-4a9e-8da9-218a0f76f218" Description="" Name="PropertyAccess" DisplayName="Property Access" DefaultValue="" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/PropertyAccessModifiers" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="52b48dbc-1ee4-4b43-ac1a-4ef319019bd3" Description="" Name="IsBrowsable" DisplayName="Is Browsable" DefaultValue="true" Category="Designer">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0b4666da-fb2b-45c2-b7e3-6efdf8b79db1" Description="" Name="Documentation" DisplayName="Documentation" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="dbbd4741-7088-420c-8001-00b1e3958388" Description="" Name="PropertyKind" DisplayName="Property Kind" Kind="Calculated" IsBrowsable="false">
          <Type>
            <DomainEnumerationMoniker Name="PropertyKind" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="466dc6ae-ffae-4abc-baf0-83143f149dd1" Description="" Name="FieldAttribute" DisplayName="Field" Category="Attributes">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/OrmFieldAttribute" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="34608f2d-3b78-46a6-8974-fd591b2d1c74" Description="" Name="IsInherited" DisplayName="Is Inherited Property" DefaultValue="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="960f3791-72e6-4f37-90a4-c0e9d342657c" Description="" Name="Constraints" DisplayName="Constraints">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/OrmPropertyConstraints" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d5dc9119-09c0-4875-8678-f9239dcc5095" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PropertyBase.Data Member" Name="DataMember" DisplayName="Data Member">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/DataMemberDescriptor" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="2217b5e9-a3f5-419c-a1a3-f45af68ea089" Description="" Name="ScalarProperty" DisplayName="Scalar Property" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" HasCustomConstructor="true">
      <BaseClass>
        <DomainClassMoniker Name="PropertyBase" />
      </BaseClass>
      <Properties>
        <DomainProperty Id="a89b5e56-f2b5-49d5-afc1-83d2fcecc82c" Description="" Name="KeyAttribute" DisplayName="Key" Category="Attributes">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/OrmKeyAttribute" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2c6fba2a-f452-47f3-8b44-6cb9ab0ec79b" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.ScalarProperty.Type" Name="Type" DisplayName="Type">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverterAttribute">
              <Parameters>
                <AttributeParameter Value="typeof(TXSoftware.DataObjectsNetEntityModel.Dsl.DomainTypeTypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="IDomainType" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="7ebd18f3-60ee-4f87-9b1d-cb116c597900" Description="" Name="NavigationProperty" DisplayName="Navigation Property" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" HasCustomConstructor="true">
      <BaseClass>
        <DomainClassMoniker Name="PropertyBase" />
      </BaseClass>
      <CustomTypeDescriptor>
        <DomainTypeDescriptor CustomCoded="true" />
      </CustomTypeDescriptor>
      <Properties>
        <DomainProperty Id="ff74466b-f4dd-4a83-a12a-df382eeb2004" Description="" Name="Multiplicity" DisplayName="Multiplicity">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/MultiplicityKind" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9817dfd0-1c36-45a8-a778-715b84899a14" Description="" Name="ReturnType" DisplayName="Return Type" Kind="Calculated">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2cc176d1-6041-4391-8c61-73f15d4ee5d9" Description="" Name="PairFrom" DisplayName="Paired From" Kind="Calculated" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="610b4518-58f0-4b7d-97b3-b6950f30695d" Description="" Name="PairTo" DisplayName="Paired To" Kind="Calculated" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f1aa2dcb-37cc-433f-8914-81970d18432c" Description="" Name="KeyAttribute" DisplayName="Key" Category="Attributes">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/OrmKeyAttribute" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="c2fb7628-9396-41a6-8dbc-306c80bed563" Description="" Name="StructureProperty" DisplayName="Structure Property" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <BaseClass>
        <DomainClassMoniker Name="PropertyBase" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="3cff2aa3-0e4e-44fe-9883-78dbca2bf7ca" Description="" Name="TypedEntitySet" DisplayName="Typed Entity Set" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <BaseClass>
        <DomainClassMoniker Name="PersistentType" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="56a29e4f-71e1-4715-baca-603b8c1b860a" Description="" Name="EntityIndex" DisplayName="Entity Index" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" HasCustomConstructor="true">
      <Properties>
        <DomainProperty Id="bbd1e345-99a5-464c-b078-cb82dc687081" Description="Indicating whether the index is unique." Name="Unique" DisplayName="Unique">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/Defaultable&lt;System.Boolean&gt;" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1b362c34-9814-4c72-8abf-0b6bc9b75a55" Description="Fill factor for this index, must be a real number between 0 and 1." Name="FillFactor" DisplayName="Fill Factor">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/Defaultable&lt;System.Double&gt;" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="db5dfe67-64f3-40d3-a592-ab6e6aa49fba" Description="" Name="Name" DisplayName="Name" IsElementName="true" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d3e7c651-8a89-46fc-9902-01ba156a22b6" Description="" Name="IndexName" DisplayName="Name">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/Defaultable&lt;System.String&gt;" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="3c17157d-8fa7-451a-b3f9-fd8b55debce1" Description="" Name="CalculatedName" DisplayName="Calculated Name" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9fccb352-5294-401b-93e1-56649ac01aae" Description="" Name="Fields" DisplayName="Fields">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/OrmIndexFields" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="6881cfb0-e300-4d8b-8343-e1824c44c35a" Description="" Name="DomainType" DisplayName="Domain Type" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <Properties>
        <DomainProperty Id="4f3a46e3-4f34-4bf3-a512-9d3f8965a1ba" Description="" Name="Name" DisplayName="Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="dc4d745e-fafb-4b75-8d9d-7c0b187761fe" Description="" Name="Namespace" DisplayName="Namespace">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4bfb499b-1766-4d3f-8f3f-05eee5fa82b4" Description="" Name="FullName" DisplayName="Full Name" Kind="Calculated" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="724d24b1-d884-41ea-a16d-97341ad52528" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.DomainType.Is BuildIn Type" Name="IsBuildIn" DisplayName="Is BuildIn Type" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7e66c2d5-fb70-4f1b-b843-0850cd10dda2" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.DomainType.Build In ID" Name="BuildInID" DisplayName="Build In ID" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="1716852c-97ed-482e-8e74-ce75c4d63d78" Description="Embedding relationship between the Model and Elements" Name="EntityModelHasPersistentTypes" DisplayName="Entity Model Has Persistent Types" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="f74bf639-22de-415f-9596-f8f406327b8e" Description="" Name="EntityModel" DisplayName="Entity Model" PropertyName="PersistentTypes" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyGetterAccessModifier="Assembly" PropertySetterAccessModifier="Assembly" PropertyDisplayName="Persistent Types">
          <RolePlayer>
            <DomainClassMoniker Name="EntityModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="d1978db9-853e-4ed8-8bf3-50b9c952db6a" Description="" Name="Element" DisplayName="Element" PropertyName="EntityModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Entity Model">
          <RolePlayer>
            <DomainClassMoniker Name="PersistentType" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="ecd47b63-a3c8-49b5-bb75-aa648140f1b1" Description="" Name="InterfaceInheritInterfaces" DisplayName="Inheritance" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <Properties>
        <DomainProperty Id="a2519999-1d25-4a7e-94d9-4152b6a92a88" Description="" Name="BaseType" DisplayName="Base Type" Kind="Calculated" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e178c942-2456-4b65-8788-c18fd493da2c" Description="" Name="DerivedType" DisplayName="Derived Type" Kind="Calculated" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="41182262-f64d-4d0b-944e-ceb25add522d" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.InterfaceInheritInterfaces.SourceInheritInterface" Name="SourceInheritInterface" DisplayName="Source Inherit Interfaces X" PropertyName="InheritedInterfaces" PropertyDisplayName="Inherited Interfaces">
          <RolePlayer>
            <DomainClassMoniker Name="Interface" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="55b0592f-56dc-46af-b8f3-fbc3f690ea70" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.InterfaceInheritInterfaces.TargetInheritByInterface" Name="TargetInheritByInterface" DisplayName="Target Inherit By Interfaces Y" PropertyName="InheritingByInterfaces" IsPropertyBrowsable="false" PropertyDisplayName="Inheriting By Interfaces">
          <RolePlayer>
            <DomainClassMoniker Name="Interface" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="21818c26-e5f9-4413-b823-c29652ff6168" Description="" Name="EntityBaseHasBaseType" DisplayName="Inheritance" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <Properties>
        <DomainProperty Id="c49c6603-a1b4-4e71-ac68-39cd8d86708d" Description="" Name="BaseType" DisplayName="Base Type" Kind="Calculated" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ac29e4f8-1a3b-4aa8-9a62-17d433fc9a0c" Description="" Name="DerivedType" DisplayName="Derived Type" Kind="Calculated" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="a5ff3f41-c3fb-41af-b606-33a635b0c428" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.EntityBaseHasBaseType.SourceEntityBase" Name="SourceEntityBase" DisplayName="Source Entity Base" PropertyName="BaseType" Multiplicity="ZeroOne" PropertyDisplayName="Base Type">
          <RolePlayer>
            <DomainClassMoniker Name="EntityBase" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="0d7a5358-e2b9-41f3-b60f-b8214333385a" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.EntityBaseHasBaseType.TargetEntityBase" Name="TargetEntityBase" DisplayName="Target Entity Base" PropertyName="BaseTypeOf" PropertyDisplayName="Base Type Of">
          <RolePlayer>
            <DomainClassMoniker Name="EntityBase" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="d62e5c30-b20a-466b-a483-7c9c6776b76f" Description="" Name="StructurePropertyHasType" DisplayName="Structure Property Has Type" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <Source>
        <DomainRole Id="6dff1a67-fed1-49dc-a684-1520cf1122a4" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.StructurePropertyHasType.StructureProperty" Name="StructureProperty" DisplayName="Structure Property" PropertyName="Type" Multiplicity="ZeroOne" PropertyDisplayName="Type">
          <RolePlayer>
            <DomainClassMoniker Name="StructureProperty" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="98f95eca-6331-4c10-a5ec-4f233807f1e2" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.StructurePropertyHasType.Structure" Name="Structure" DisplayName="Structure" PropertyName="TypeOf" PropertyDisplayName="Type Of">
          <RolePlayer>
            <DomainClassMoniker Name="Structure" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="4be128ba-604f-4d7f-b225-31b01ca9a0b4" Description="" Name="PersistentTypeHasProperties" DisplayName="Persistent Type Has Properties" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="9f7d7b2c-25a8-4865-806f-3ae1e81634a9" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeHasProperties.Properties" Name="Properties" DisplayName="Properties" PropertyName="Properties" PropertyDisplayName="Properties">
          <RolePlayer>
            <DomainClassMoniker Name="PersistentType" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="9590cf04-e691-4350-8aa8-16d3d9ad784a" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeHasProperties.PersistentType" Name="PersistentType" DisplayName="Persistent Type" PropertyName="PersistentType" Multiplicity="ZeroOne" PropagatesDelete="true" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Persistent Type">
          <RolePlayer>
            <DomainClassMoniker Name="PropertyBase" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="e6dcee2b-147d-4cac-ad5e-c71b1ebbe249" Description="" Name="PersistentTypeHasNavigationProperties" DisplayName="Persistent Type Has Navigation Properties" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="37528d18-ad13-411e-a420-bdb4bff8e225" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeHasNavigationProperties.NavigationProperties" Name="NavigationProperties" DisplayName="NavigationProperties" PropertyName="NavigationProperties" PropertyDisplayName="Navigation Properties">
          <RolePlayer>
            <DomainClassMoniker Name="PersistentType" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="2afbefd1-7669-4a7a-adb5-442257a79899" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeHasNavigationProperties.PersistentTypeOfNavigationProperty" Name="PersistentTypeOfNavigationProperty" DisplayName="Persistent Type Of Navigation Property" PropertyName="PersistentTypeOfNavigationProperty" Multiplicity="ZeroOne" PropagatesDelete="true" PropertyDisplayName="Persistent Type Of Navigation Property">
          <RolePlayer>
            <DomainClassMoniker Name="NavigationProperty" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="1442d501-f954-42e3-bd77-742963ce8958" Description="" Name="PersistentTypeHasAssociations" DisplayName="Association" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" HasCustomConstructor="true" AllowsDuplicates="true">
      <Properties>
        <DomainProperty Id="c7465fe2-9709-4fcb-a7a6-8f6501c52919" Description="" Name="Name" DisplayName="Association Name" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0f88fe6b-46f6-4f1e-a598-e070691bf35d" Description="" Name="End1" DisplayName="End1">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/OrmAssociationEnd" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="388a7a97-ac4f-4143-afbd-f780118fa6e9" Description="" Name="End2" DisplayName="End2">
          <Type>
            <ExternalTypeMoniker Name="/TXSoftware.DataObjectsNetEntityModel.Common/OrmAssociationEnd" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="106e1bc1-d06e-4fd5-a90e-d418c74cea4b" Description="" Name="SourceMultiplicity" DisplayName="Source Multiplicity" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e748f66e-d248-44ac-b392-24bc14056865" Description="" Name="TargetMultiplicity" DisplayName="Target Multiplicity" Kind="Calculated" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="5361f935-3433-4b48-9579-c49a9835c6a5" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeHasAssociations.SourcePersistentType" Name="SourcePersistentType" DisplayName="Source Persistent Type" PropertyName="PersistentTypeAssociations" PropertyDisplayName="Persistent Type Associations">
          <RolePlayer>
            <DomainClassMoniker Name="PersistentType" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="455b6392-fe71-4e09-a8f6-c1c02dea5ed7" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeHasAssociations.TargetPersistentType" Name="TargetPersistentType" DisplayName="Target Persistent Type" PropertyName="SourcePersistentTypes" IsPropertyGenerator="false" IsPropertyBrowsable="false" PropertyDisplayName="Source Persistent Types">
          <RolePlayer>
            <DomainClassMoniker Name="PersistentType" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="38623460-9242-4582-9e83-8caf46cd98e5" Description="" Name="NavigationPropertyHasAssociation" DisplayName="Navigation Property Has Association" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <Source>
        <DomainRole Id="8da87d9f-cd64-4a50-b1a3-f7a5e86c74e5" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.NavigationPropertyHasAssociation.NavigationProperty" Name="NavigationProperty" DisplayName="Association" PropertyName="PersistentTypeHasAssociations" Multiplicity="ZeroOne" PropagatesDelete="true" PropertyDisplayName="Association">
          <RolePlayer>
            <DomainClassMoniker Name="NavigationProperty" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="5a2023e8-ede4-4594-beda-9c51bab222b4" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.NavigationPropertyHasAssociation.PersistentTypeHasAssociations" Name="PersistentTypeHasAssociations" DisplayName="Persistent Type Has Associations" PropertyName="NavigationProperties" IsPropertyBrowsable="false" PropertyDisplayName="Navigation Properties">
          <RolePlayer>
            <DomainRelationshipMoniker Name="PersistentTypeHasAssociations" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="7e98b08f-b820-44ca-8199-4cb9784fa116" Description="" Name="TypedEntitySetHasItemType" DisplayName="Item Type of EntitySet" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <Properties>
        <DomainProperty Id="beb44acf-1005-4e35-952f-409b3fc6de22" Description="" Name="ResultTypeName" DisplayName="Typed EntitySet" Kind="Calculated">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="2b6a7e22-c6c0-41b3-932c-0b4259ee67ab" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.TypedEntitySetHasItemType.TypedEntitySet" Name="TypedEntitySet" DisplayName="Typed Entity Set" PropertyName="ItemType" Multiplicity="One" PropertyDisplayName="Item Type">
          <RolePlayer>
            <DomainClassMoniker Name="TypedEntitySet" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="c2275136-c032-4115-9b31-bf2012dade70" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.TypedEntitySetHasItemType.TypeOfItem" Name="TypeOfItem" DisplayName="Type Of Item" PropertyName="TypedEntitySets" PropertyDisplayName="Typed Entity Sets">
          <RolePlayer>
            <DomainClassMoniker Name="Interface" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="c162aa7a-06c8-46d4-9892-bda1cf940641" Description="" Name="InterfaceHasIndexes" DisplayName="Interface Has Indexes" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="8511a387-1d33-4646-8ddd-7e9ef7a114ad" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.InterfaceHasIndexes.Indexes" Name="Indexes" DisplayName="Indexes" PropertyName="Indexes" PropertyDisplayName="Indexes">
          <RolePlayer>
            <DomainClassMoniker Name="Interface" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="18f5599f-e545-4af0-bea5-e7b4d8f91cc3" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.InterfaceHasIndexes.InterfaceOfIndex" Name="InterfaceOfIndex" DisplayName="Interface Of Index" PropertyName="InterfaceOfIndex" Multiplicity="One" PropagatesDelete="true" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Interface Of Index">
          <RolePlayer>
            <DomainClassMoniker Name="EntityIndex" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="7f89ec29-c4a9-4cd4-8aec-da24bc65ffb2" Description="" Name="NavigationPropertyHasTypedEntitySet" DisplayName="Navigation Property Has Typed Entity Set" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
      <Source>
        <DomainRole Id="eed2b8ef-b63a-4d03-be29-b49b39754066" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.NavigationPropertyHasTypedEntitySet.OwnerNavigationProperty" Name="OwnerNavigationProperty" DisplayName="Owner Navigation Property" PropertyName="TypedEntitySet" Multiplicity="ZeroOne" PropertyDisplayName="Typed Entity Set">
          <RolePlayer>
            <DomainClassMoniker Name="NavigationProperty" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="3b202d34-7d0f-49a5-bfee-84736d12c908" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.NavigationPropertyHasTypedEntitySet.TypedEntitySet" Name="TypedEntitySet" DisplayName="Typed Entity Set" PropertyName="TypedEntitySetNavigationProperties" PropertyDisplayName="Typed Entity Set Navigation Properties">
          <RolePlayer>
            <DomainClassMoniker Name="TypedEntitySet" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="77cdd6f4-e2db-4117-9830-42c37e33e0b7" Description="" Name="EntityModelHasDomainTypes" DisplayName="Entity Model Has Domain Types" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="2a9db156-c9e2-45c0-a1b8-d284212438e4" Description="" Name="EntityModel" DisplayName="Entity Model" PropertyName="DomainTypes" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyGetterAccessModifier="Assembly" PropertySetterAccessModifier="Assembly" PropertyDisplayName="Domain Types">
          <RolePlayer>
            <DomainClassMoniker Name="EntityModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="988ef68c-14a5-403c-98c4-83147144ca6f" Description="" Name="DomainType" DisplayName="Domain Type" PropertyName="EntityModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Entity Model">
          <RolePlayer>
            <DomainClassMoniker Name="DomainType" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
  </Relationships>
  <Types>
    <ExternalType Name="DateTime" Namespace="System" />
    <ExternalType Name="String" Namespace="System" />
    <ExternalType Name="Int16" Namespace="System" />
    <ExternalType Name="Int32" Namespace="System" />
    <ExternalType Name="Int64" Namespace="System" />
    <ExternalType Name="UInt16" Namespace="System" />
    <ExternalType Name="UInt32" Namespace="System" />
    <ExternalType Name="UInt64" Namespace="System" />
    <ExternalType Name="SByte" Namespace="System" />
    <ExternalType Name="Byte" Namespace="System" />
    <ExternalType Name="Double" Namespace="System" />
    <ExternalType Name="Single" Namespace="System" />
    <ExternalType Name="Guid" Namespace="System" />
    <ExternalType Name="Boolean" Namespace="System" />
    <ExternalType Name="Char" Namespace="System" />
    <DomainEnumeration Name="AccessModifier" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.AccessModifier.Internal" Name="Internal" Value="0" />
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.AccessModifier.Public" Name="Public" Value="1" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="DashStyle" Namespace="System.Drawing.Drawing2D" />
    <DomainEnumeration Name="RequiredValue" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" Description="" />
    <ExternalType Name="OrmKeyAttribute" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="OrmFieldAttribute" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="Object" Namespace="System" />
    <DomainEnumeration Name="PropertyKind" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PropertyKind.Scalar" Name="Scalar" Value="0" />
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PropertyKind.Structure" Name="Structure" Value="1" />
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PropertyKind.Navigation" Name="Navigation" Value="2" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="PersistentTypeKind" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeKind.Interface" Name="Interface" Value="0" />
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeKind.Entity" Name="Entity" Value="1" />
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeKind.Structure" Name="Structure" Value="2" />
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeKind.ExternalType" Name="ExternalType" Value="3" />
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentTypeKind.TypedEntitySet" Name="TypedEntitySet" Value="4" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="MultiplicityKind" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="OrmAssociationEnd" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="Color" Namespace="System.Drawing" />
    <ExternalType Name="OrmHierarchyRootAttribute" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="Defaultable&lt;System.Double&gt;" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="Defaultable&lt;System.Boolean&gt;" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="Defaultable&lt;System.String&gt;" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="OrmIndexFields" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <DomainEnumeration Name="InheritanceModifiers" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="None" Value="0" />
        <EnumerationLiteral Description="" Name="Abstract" Value="1" />
        <EnumerationLiteral Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.InheritanceModifiers.Sealed" Name="Sealed" Value="2" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="IModelRoot" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" />
    <ExternalType Name="OrmKeyGeneratorAttribute" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="OrmTypeDiscriminatorValueAttribute" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="OrmPropertyConstraints" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="InheritsIEntityMode" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="DataContractDescriptor" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="DataMemberDescriptor" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
    <ExternalType Name="IDomainType" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" />
    <ExternalType Name="PropertyAccessModifiers" Namespace="TXSoftware.DataObjectsNetEntityModel.Common" />
  </Types>
  <Shapes>
    <CompartmentShape Id="8470ebf0-d5c6-4ee4-a2f0-06ebe67dec2d" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentShape" Name="PersistentShape" DisplayName="Persistent Shape" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" GeneratesDoubleDerived="true" FixedTooltipText="Persistent Shape" OutlineColor="Silver" InitialHeight="0.4" OutlineThickness="0.015625" ExposesFillColorAsProperty="true" ExposesOutlineDashStyleAsProperty="true" ExposesOutlineThicknessAsProperty="true" Geometry="RoundedRectangle">
      <Properties>
        <DomainProperty Id="26cdb409-f654-4317-9c70-95ecd3f8348c" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentShape.Outline Dash Style" Name="OutlineDashStyle" DisplayName="Outline Dash Style" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing.Drawing2D/DashStyle" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6adb581c-d12a-4dd1-adbe-5a4f066eae20" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentShape.Outline Thickness" Name="OutlineThickness" DisplayName="Outline Thickness" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Single" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a5795d1c-8df1-42a1-8cf3-cc5b9e5dd09a" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.PersistentShape.Fill Color" Name="FillColor" DisplayName="Fill Color" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
      </Properties>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0">
        <TextDecorator Name="TextName" DisplayName="Text Name" DefaultText="TextName" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IconEntity" DisplayName="Icon Entity" DefaultIcon="Resources\Entity.bmp" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ExpandCollapse" DisplayName="Expand Collapse" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IconStructure" DisplayName="Icon Structure" DefaultIcon="Resources\Structure.bmp" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IconInterface" DisplayName="Icon Interface" DefaultIcon="Resources\Interface.bmp" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0.15">
        <IconDecorator Name="IconInherit" DisplayName="Icon Inherit" DefaultIcon="Resources\InheritArrow.bmp" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.2" VerticalOffset="0.15">
        <TextDecorator Name="TextDescription" DisplayName="Text Description" DefaultText="TextDescription" FontSize="7" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0" VerticalOffset="0">
        <IconDecorator Name="IconTypedEntitySet" DisplayName="Icon Typed Entity Set" DefaultIcon="Resources\EntitySet.bmp" />
      </ShapeHasDecorators>
      <Compartment TitleFillColor="WhiteSmoke" Name="Properties" Title="Properties" />
      <Compartment TitleFillColor="AliceBlue" Name="NavigationProperties" Title="Navigation Properties" />
    </CompartmentShape>
    <CompartmentShape Id="6ac89853-30c6-42a5-923b-1cf3bea448de" Description="" Name="TypedEntitySetShape" DisplayName="Typed Entity Set Shape" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" GeneratesDoubleDerived="true" FixedTooltipText="Typed Entity Set Shape" OutlineColor="Silver" InitialHeight="0.5" OutlineThickness="0.015625" Geometry="Rectangle">
      <BaseCompartmentShape>
        <CompartmentShapeMoniker Name="PersistentShape" />
      </BaseCompartmentShape>
    </CompartmentShape>
    <CompartmentShape Id="8f29a06a-5642-4cba-873f-a777fb76b6ed" Description="" Name="EntityShape" DisplayName="Entity Shape" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" GeneratesDoubleDerived="true" FixedTooltipText="Entity Shape" OutlineColor="Silver" InitialHeight="0.5" OutlineThickness="0.015625" Geometry="Rectangle">
      <BaseCompartmentShape>
        <CompartmentShapeMoniker Name="EntityBaseShape" />
      </BaseCompartmentShape>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0.18">
        <IconDecorator Name="IconHierarchyRoot" DisplayName="Icon Hierarchy Root" DefaultIcon="Resources\Hierarchy.png" />
      </ShapeHasDecorators>
    </CompartmentShape>
    <CompartmentShape Id="5fc3a0b8-0a42-4366-baf0-a86e5cb34e05" Description="" Name="InterfaceShape" DisplayName="Interface Shape" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" GeneratesDoubleDerived="true" FixedTooltipText="Interface Shape" OutlineColor="Silver" InitialHeight="0.5" OutlineThickness="0.015625" Geometry="Rectangle">
      <BaseCompartmentShape>
        <CompartmentShapeMoniker Name="PersistentShape" />
      </BaseCompartmentShape>
      <Compartment TitleFillColor="SeaShell" Name="Indexes" Title="Indexes" />
    </CompartmentShape>
    <CompartmentShape Id="9e9faa75-d840-4491-8d22-94ad92bd7427" Description="" Name="EntityBaseShape" DisplayName="Entity Base Shape" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" GeneratesDoubleDerived="true" FixedTooltipText="Entity Base Shape" OutlineColor="Silver" InitialHeight="0.5" OutlineThickness="0.015625" Geometry="Rectangle">
      <BaseCompartmentShape>
        <CompartmentShapeMoniker Name="InterfaceShape" />
      </BaseCompartmentShape>
    </CompartmentShape>
  </Shapes>
  <Connectors>
    <Connector Id="21df4358-051f-405d-9699-2400856e8196" Description="Inheritance connector between the types" Name="EntityBaseInheritanceConnector" DisplayName="Entity Base Inheritance Connector" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" FixedTooltipText="Entity Base Inheritance Connector" Color="Silver" TargetEndStyle="HollowArrow" Thickness="0.015625" />
    <Connector Id="3154960d-c3ce-4608-ac08-11c3dda2c7c6" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.InterfaceInheritanceConnector" Name="InterfaceInheritanceConnector" DisplayName="Interface Inheritance Connector" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" FixedTooltipText="Interface Inheritance Connector" Color="Silver" TargetEndStyle="HollowArrow" Thickness="0.015625" />
    <Connector Id="cbd05b08-77d1-4f8c-a667-eda3f90c1275" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.StructurePropertyTypeOfConnector" Name="StructurePropertyTypeOfConnector" DisplayName="Structure Property Type Of Connector" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" FixedTooltipText="Structure Property Type Of Connector" DashStyle="Dot" TargetEndStyle="EmptyDiamond" Thickness="0.015625" />
    <Connector Id="15137587-8515-474a-809b-d4da8c2bf307" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.AssociationConnector" Name="AssociationConnector" DisplayName="Association Connector" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" TooltipType="Variable" FixedTooltipText="Association Connector" Color="Gray" DashStyle="Dash" SourceEndStyle="EmptyDiamond" TargetEndStyle="EmptyDiamond" Thickness="0.015625">
      <ConnectorHasDecorators Position="SourceBottom" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="TextSourceMultiplicity" DisplayName="Text Source Multiplicity" DefaultText="TextSourceMultiplicity" />
      </ConnectorHasDecorators>
      <ConnectorHasDecorators Position="TargetBottom" OffsetFromShape="0" OffsetFromLine="0">
        <TextDecorator Name="TextTargetMultiplicity" DisplayName="Text Target Multiplicity" DefaultText="TextTargetMultiplicity" />
      </ConnectorHasDecorators>
    </Connector>
    <Connector Id="beafe105-af27-4d71-aec9-5b17d259cd3b" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.TypedEntitySetHasItemTypeConnector" Name="TypedEntitySetHasItemTypeConnector" DisplayName="Typed Entity Set Has Item Type Connector" AccessModifier="Assembly" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl" FixedTooltipText="Typed Entity Set Has Item Type Connector" TextColor="Silver" Color="LightGray" DashStyle="Dot" Thickness="0.015625" />
  </Connectors>
  <XmlSerializationBehavior Name="EntityModelDesignerSerializationBehavior" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
    <ClassData>
      <XmlClassData TypeName="EntityModel" MonikerAttributeName="" IsCustom="true" SerializeId="true" MonikerElementName="entityModelMoniker" ElementName="entityModel" MonikerTypeName="EntityModelMoniker">
        <DomainClassMoniker Name="EntityModel" />
        <ElementData>
          <XmlRelationshipData RoleElementName="persistentTypes">
            <DomainRelationshipMoniker Name="EntityModelHasPersistentTypes" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="EntityModel/Namespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="modelRoot" Representation="Ignore">
            <DomainPropertyMoniker Name="EntityModel/ModelRoot" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="version" Representation="Ignore">
            <DomainPropertyMoniker Name="EntityModel/Version" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="domainTypes">
            <DomainRelationshipMoniker Name="EntityModelHasDomainTypes" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="PersistentType" MonikerAttributeName="name" IsCustom="true" MonikerElementName="persistentTypeMoniker" ElementName="persistentType" MonikerTypeName="PersistentTypeMoniker">
        <DomainClassMoniker Name="PersistentType" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="PersistentType/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="PersistentType/Namespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="typeName" Representation="Ignore">
            <DomainPropertyMoniker Name="PersistentType/TypeName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="access">
            <DomainPropertyMoniker Name="PersistentType/Access" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="properties">
            <DomainRelationshipMoniker Name="PersistentTypeHasProperties" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="navigationProperties">
            <DomainRelationshipMoniker Name="PersistentTypeHasNavigationProperties" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="persistentTypeAssociations">
            <DomainRelationshipMoniker Name="PersistentTypeHasAssociations" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="inheritsFrom" Representation="Ignore">
            <DomainPropertyMoniker Name="PersistentType/InheritsFrom" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="inheritsFromName" Representation="Ignore">
            <DomainPropertyMoniker Name="PersistentType/InheritsFromName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="typeKind" Representation="Ignore">
            <DomainPropertyMoniker Name="PersistentType/TypeKind" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="typeDescription" Representation="Ignore">
            <DomainPropertyMoniker Name="PersistentType/TypeDescription" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="documentation">
            <DomainPropertyMoniker Name="PersistentType/Documentation" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataContract" Representation="Element">
            <DomainPropertyMoniker Name="PersistentType/DataContract" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityModelHasPersistentTypes" MonikerAttributeName="" MonikerElementName="entityModelHasPersistentTypesMoniker" ElementName="entityModelHasPersistentTypes" MonikerTypeName="EntityModelHasPersistentTypesMoniker">
        <DomainRelationshipMoniker Name="EntityModelHasPersistentTypes" />
      </XmlClassData>
      <XmlClassData TypeName="EntityBaseInheritanceConnector" MonikerAttributeName="" MonikerElementName="entityBaseInheritanceConnectorMoniker" ElementName="entityBaseInheritanceConnector" MonikerTypeName="EntityBaseInheritanceConnectorMoniker">
        <ConnectorMoniker Name="EntityBaseInheritanceConnector" />
      </XmlClassData>
      <XmlClassData TypeName="EntityDiagram" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityDiagramMoniker" ElementName="entityDiagram" MonikerTypeName="EntityDiagramMoniker">
        <DiagramMoniker Name="EntityDiagram" />
      </XmlClassData>
      <XmlClassData TypeName="PersistentShape" MonikerAttributeName="" MonikerElementName="persistentShapeMoniker" ElementName="persistentShape" MonikerTypeName="PersistentShapeMoniker">
        <CompartmentShapeMoniker Name="PersistentShape" />
        <ElementData>
          <XmlPropertyData XmlName="outlineDashStyle">
            <DomainPropertyMoniker Name="PersistentShape/OutlineDashStyle" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineThickness">
            <DomainPropertyMoniker Name="PersistentShape/OutlineThickness" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="fillColor">
            <DomainPropertyMoniker Name="PersistentShape/FillColor" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Entity" MonikerAttributeName="" IsCustom="true" MonikerElementName="entityMoniker" ElementName="entity" MonikerTypeName="EntityMoniker">
        <DomainClassMoniker Name="Entity" />
        <ElementData>
          <XmlPropertyData XmlName="hierarchyRootAttribute" Representation="Element">
            <DomainPropertyMoniker Name="Entity/HierarchyRootAttribute" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isHierarchyRoot" Representation="Ignore">
            <DomainPropertyMoniker Name="Entity/IsHierarchyRoot" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="keyGenerator" Representation="Element">
            <DomainPropertyMoniker Name="Entity/KeyGenerator" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="typeDiscriminatorValue" Representation="Element">
            <DomainPropertyMoniker Name="Entity/TypeDiscriminatorValue" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Structure" MonikerAttributeName="" MonikerElementName="structureMoniker" ElementName="structure" MonikerTypeName="StructureMoniker">
        <DomainClassMoniker Name="Structure" />
      </XmlClassData>
      <XmlClassData TypeName="Interface" MonikerAttributeName="" IsCustom="true" MonikerElementName="interfaceMoniker" ElementName="interface" MonikerTypeName="InterfaceMoniker">
        <DomainClassMoniker Name="Interface" />
        <ElementData>
          <XmlRelationshipData OmitElement="true" UseFullForm="true" RoleElementName="inheritedInterfaces">
            <DomainRelationshipMoniker Name="InterfaceInheritInterfaces" />
          </XmlRelationshipData>
          <XmlRelationshipData RoleElementName="indexes">
            <DomainRelationshipMoniker Name="InterfaceHasIndexes" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="inheritInterfaces" Representation="Ignore">
            <DomainPropertyMoniker Name="Interface/InheritInterfaces" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="inheritsIEntity" Representation="Element">
            <DomainPropertyMoniker Name="Interface/InheritsIEntity" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityBase" MonikerAttributeName="" MonikerElementName="entityBaseMoniker" ElementName="entityBase" MonikerTypeName="EntityBaseMoniker">
        <DomainClassMoniker Name="EntityBase" />
        <ElementData>
          <XmlPropertyData XmlName="inheritanceModifier">
            <DomainPropertyMoniker Name="EntityBase/InheritanceModifier" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="baseType">
            <DomainRelationshipMoniker Name="EntityBaseHasBaseType" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="InterfaceInheritInterfaces" MonikerAttributeName="" SerializeId="true" MonikerElementName="interfaceInheritInterfacesMoniker" ElementName="inheritedInterfaces" MonikerTypeName="InterfaceInheritInterfacesMoniker">
        <DomainRelationshipMoniker Name="InterfaceInheritInterfaces" />
        <ElementData>
          <XmlPropertyData XmlName="baseType" Representation="Ignore">
            <DomainPropertyMoniker Name="InterfaceInheritInterfaces/BaseType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="derivedType" Representation="Ignore">
            <DomainPropertyMoniker Name="InterfaceInheritInterfaces/DerivedType" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="InterfaceInheritanceConnector" MonikerAttributeName="" MonikerElementName="interfaceInheritanceConnectorMoniker" ElementName="interfaceInheritanceConnector" MonikerTypeName="InterfaceInheritanceConnectorMoniker">
        <ConnectorMoniker Name="InterfaceInheritanceConnector" />
      </XmlClassData>
      <XmlClassData TypeName="EntityBaseHasBaseType" MonikerAttributeName="" MonikerElementName="entityBaseHasBaseTypeMoniker" ElementName="entityBaseHasBaseType" MonikerTypeName="EntityBaseHasBaseTypeMoniker">
        <DomainRelationshipMoniker Name="EntityBaseHasBaseType" />
        <ElementData>
          <XmlPropertyData XmlName="baseType" Representation="Ignore">
            <DomainPropertyMoniker Name="EntityBaseHasBaseType/BaseType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="derivedType" Representation="Ignore">
            <DomainPropertyMoniker Name="EntityBaseHasBaseType/DerivedType" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="PropertyBase" MonikerAttributeName="name" IsCustom="true" MonikerElementName="propertyBaseMoniker" ElementName="propertyBase" MonikerTypeName="PropertyBaseMoniker">
        <DomainClassMoniker Name="PropertyBase" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="PropertyBase/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="typeName" Representation="Ignore">
            <DomainPropertyMoniker Name="PropertyBase/TypeName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="propertyAccess" Representation="Element">
            <DomainPropertyMoniker Name="PropertyBase/PropertyAccess" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isBrowsable">
            <DomainPropertyMoniker Name="PropertyBase/IsBrowsable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="documentation">
            <DomainPropertyMoniker Name="PropertyBase/Documentation" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="propertyKind" Representation="Ignore">
            <DomainPropertyMoniker Name="PropertyBase/PropertyKind" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="fieldAttribute" Representation="Element">
            <DomainPropertyMoniker Name="PropertyBase/FieldAttribute" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isInherited">
            <DomainPropertyMoniker Name="PropertyBase/IsInherited" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="constraints" Representation="Element">
            <DomainPropertyMoniker Name="PropertyBase/Constraints" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataMember" Representation="Element">
            <DomainPropertyMoniker Name="PropertyBase/DataMember" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ScalarProperty" MonikerAttributeName="" IsCustom="true" MonikerElementName="scalarPropertyMoniker" ElementName="scalarProperty" MonikerTypeName="ScalarPropertyMoniker">
        <DomainClassMoniker Name="ScalarProperty" />
        <ElementData>
          <XmlPropertyData XmlName="keyAttribute" Representation="Element">
            <DomainPropertyMoniker Name="ScalarProperty/KeyAttribute" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="ScalarProperty/Type" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="NavigationProperty" MonikerAttributeName="" IsCustom="true" MonikerElementName="navigationPropertyMoniker" ElementName="navigationProperty" MonikerTypeName="NavigationPropertyMoniker">
        <DomainClassMoniker Name="NavigationProperty" />
        <ElementData>
          <XmlPropertyData XmlName="multiplicity" Representation="Element">
            <DomainPropertyMoniker Name="NavigationProperty/Multiplicity" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="returnType" Representation="Ignore">
            <DomainPropertyMoniker Name="NavigationProperty/ReturnType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="pairFrom" Representation="Ignore">
            <DomainPropertyMoniker Name="NavigationProperty/PairFrom" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="pairTo" Representation="Ignore">
            <DomainPropertyMoniker Name="NavigationProperty/PairTo" />
          </XmlPropertyData>
          <XmlRelationshipData RoleElementName="persistentTypeHasAssociations">
            <DomainRelationshipMoniker Name="NavigationPropertyHasAssociation" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="typedEntitySet">
            <DomainRelationshipMoniker Name="NavigationPropertyHasTypedEntitySet" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="keyAttribute" Representation="Element">
            <DomainPropertyMoniker Name="NavigationProperty/KeyAttribute" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="StructureProperty" MonikerAttributeName="" MonikerElementName="structurePropertyMoniker" ElementName="structureProperty" MonikerTypeName="StructurePropertyMoniker">
        <DomainClassMoniker Name="StructureProperty" />
        <ElementData>
          <XmlRelationshipData RoleElementName="type">
            <DomainRelationshipMoniker Name="StructurePropertyHasType" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="StructurePropertyHasType" MonikerAttributeName="" MonikerElementName="structurePropertyHasTypeMoniker" ElementName="structurePropertyHasType" MonikerTypeName="StructurePropertyHasTypeMoniker">
        <DomainRelationshipMoniker Name="StructurePropertyHasType" />
      </XmlClassData>
      <XmlClassData TypeName="PersistentTypeHasProperties" MonikerAttributeName="" MonikerElementName="persistentTypeHasPropertiesMoniker" ElementName="persistentTypeHasProperties" MonikerTypeName="PersistentTypeHasPropertiesMoniker">
        <DomainRelationshipMoniker Name="PersistentTypeHasProperties" />
      </XmlClassData>
      <XmlClassData TypeName="StructurePropertyTypeOfConnector" MonikerAttributeName="" MonikerElementName="structurePropertyTypeOfConnectorMoniker" ElementName="structurePropertyTypeOfConnector" MonikerTypeName="StructurePropertyTypeOfConnectorMoniker">
        <ConnectorMoniker Name="StructurePropertyTypeOfConnector" />
      </XmlClassData>
      <XmlClassData TypeName="PersistentTypeHasNavigationProperties" MonikerAttributeName="" MonikerElementName="persistentTypeHasNavigationPropertiesMoniker" ElementName="persistentTypeHasNavigationProperties" MonikerTypeName="PersistentTypeHasNavigationPropertiesMoniker">
        <DomainRelationshipMoniker Name="PersistentTypeHasNavigationProperties" />
      </XmlClassData>
      <XmlClassData TypeName="AssociationConnector" MonikerAttributeName="" MonikerElementName="associationConnectorMoniker" ElementName="associationConnector" MonikerTypeName="AssociationConnectorMoniker">
        <ConnectorMoniker Name="AssociationConnector" />
      </XmlClassData>
      <XmlClassData TypeName="PersistentTypeHasAssociations" MonikerAttributeName="name" IsCustom="true" MonikerElementName="persistentTypeHasAssociationsMoniker" ElementName="persistentTypeHasAssociations" MonikerTypeName="PersistentTypeHasAssociationsMoniker">
        <DomainRelationshipMoniker Name="PersistentTypeHasAssociations" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="PersistentTypeHasAssociations/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="end1" Representation="Element">
            <DomainPropertyMoniker Name="PersistentTypeHasAssociations/End1" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="end2" Representation="Element">
            <DomainPropertyMoniker Name="PersistentTypeHasAssociations/End2" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sourceMultiplicity" Representation="Ignore">
            <DomainPropertyMoniker Name="PersistentTypeHasAssociations/SourceMultiplicity" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetMultiplicity" Representation="Ignore">
            <DomainPropertyMoniker Name="PersistentTypeHasAssociations/TargetMultiplicity" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="NavigationPropertyHasAssociation" MonikerAttributeName="" MonikerElementName="navigationPropertyHasAssociationMoniker" ElementName="navigationPropertyHasAssociation" MonikerTypeName="NavigationPropertyHasAssociationMoniker">
        <DomainRelationshipMoniker Name="NavigationPropertyHasAssociation" />
      </XmlClassData>
      <XmlClassData TypeName="TypedEntitySet" MonikerAttributeName="" MonikerElementName="typedEntitySetMoniker" ElementName="typedEntitySet" MonikerTypeName="TypedEntitySetMoniker">
        <DomainClassMoniker Name="TypedEntitySet" />
        <ElementData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="itemType">
            <DomainRelationshipMoniker Name="TypedEntitySetHasItemType" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="TypedEntitySetHasItemType" MonikerAttributeName="" MonikerElementName="typedEntitySetHasItemTypeMoniker" ElementName="typedEntitySetHasItemType" MonikerTypeName="TypedEntitySetHasItemTypeMoniker">
        <DomainRelationshipMoniker Name="TypedEntitySetHasItemType" />
        <ElementData>
          <XmlPropertyData XmlName="resultTypeName" Representation="Ignore">
            <DomainPropertyMoniker Name="TypedEntitySetHasItemType/ResultTypeName" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="TypedEntitySetHasItemTypeConnector" MonikerAttributeName="" MonikerElementName="typedEntitySetHasItemTypeConnectorMoniker" ElementName="typedEntitySetHasItemTypeConnector" MonikerTypeName="TypedEntitySetHasItemTypeConnectorMoniker">
        <ConnectorMoniker Name="TypedEntitySetHasItemTypeConnector" />
      </XmlClassData>
      <XmlClassData TypeName="TypedEntitySetShape" MonikerAttributeName="" MonikerElementName="typedEntitySetShapeMoniker" ElementName="typedEntitySetShape" MonikerTypeName="TypedEntitySetShapeMoniker">
        <CompartmentShapeMoniker Name="TypedEntitySetShape" />
      </XmlClassData>
      <XmlClassData TypeName="EntityShape" MonikerAttributeName="" MonikerElementName="entityShapeMoniker" ElementName="entityShape" MonikerTypeName="EntityShapeMoniker">
        <CompartmentShapeMoniker Name="EntityShape" />
      </XmlClassData>
      <XmlClassData TypeName="EntityIndex" MonikerAttributeName="name" IsCustom="true" MonikerElementName="entityIndexMoniker" ElementName="entityIndex" MonikerTypeName="EntityIndexMoniker">
        <DomainClassMoniker Name="EntityIndex" />
        <ElementData>
          <XmlPropertyData XmlName="unique" Representation="Element">
            <DomainPropertyMoniker Name="EntityIndex/Unique" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="fillFactor" Representation="Element">
            <DomainPropertyMoniker Name="EntityIndex/FillFactor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="EntityIndex/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="indexName" Representation="Element">
            <DomainPropertyMoniker Name="EntityIndex/IndexName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="calculatedName" Representation="Ignore">
            <DomainPropertyMoniker Name="EntityIndex/CalculatedName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="fields" Representation="Element">
            <DomainPropertyMoniker Name="EntityIndex/Fields" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="InterfaceHasIndexes" MonikerAttributeName="" MonikerElementName="interfaceHasIndexesMoniker" ElementName="interfaceHasIndexes" MonikerTypeName="InterfaceHasIndexesMoniker">
        <DomainRelationshipMoniker Name="InterfaceHasIndexes" />
      </XmlClassData>
      <XmlClassData TypeName="InterfaceShape" MonikerAttributeName="" MonikerElementName="interfaceShapeMoniker" ElementName="interfaceShape" MonikerTypeName="InterfaceShapeMoniker">
        <CompartmentShapeMoniker Name="InterfaceShape" />
      </XmlClassData>
      <XmlClassData TypeName="EntityBaseShape" MonikerAttributeName="" MonikerElementName="entityBaseShapeMoniker" ElementName="entityBaseShape" MonikerTypeName="EntityBaseShapeMoniker">
        <CompartmentShapeMoniker Name="EntityBaseShape" />
      </XmlClassData>
      <XmlClassData TypeName="NavigationPropertyHasTypedEntitySet" MonikerAttributeName="" SerializeId="true" MonikerElementName="navigationPropertyHasTypedEntitySetMoniker" ElementName="navigationPropertyHasTypedEntitySet" MonikerTypeName="NavigationPropertyHasTypedEntitySetMoniker">
        <DomainRelationshipMoniker Name="NavigationPropertyHasTypedEntitySet" />
      </XmlClassData>
      <XmlClassData TypeName="DomainType" MonikerAttributeName="" IsCustom="true" SerializeId="true" MonikerElementName="domainTypeMoniker" ElementName="domainType" MonikerTypeName="DomainTypeMoniker">
        <DomainClassMoniker Name="DomainType" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="DomainType/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="namespace">
            <DomainPropertyMoniker Name="DomainType/Namespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="fullName" Representation="Ignore">
            <DomainPropertyMoniker Name="DomainType/FullName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isBuildIn" Representation="Ignore">
            <DomainPropertyMoniker Name="DomainType/IsBuildIn" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="buildInID" Representation="Ignore">
            <DomainPropertyMoniker Name="DomainType/BuildInID" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityModelHasDomainTypes" MonikerAttributeName="" MonikerElementName="entityModelHasDomainTypesMoniker" ElementName="entityModelHasDomainTypes" MonikerTypeName="EntityModelHasDomainTypesMoniker">
        <DomainRelationshipMoniker Name="EntityModelHasDomainTypes" />
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="DONetEntityModelDesignerExplorer">
    <CustomNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\DO-Model.png">
        <Class>
          <DomainClassMoniker Name="EntityModel" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\BuildInDomainType.png">
        <Class>
          <DomainClassMoniker Name="DomainType" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <DomainPropertyMoniker Name="DomainType/FullName" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
    </CustomNodeSettings>
  </ExplorerBehavior>
  <ConnectionBuilders>
    <ConnectionBuilder Name="InterfaceInheritInterfacesBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="InterfaceInheritInterfaces" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Interface" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true">
            <AcceptingClass>
              <DomainClassMoniker Name="Interface" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EntityBaseHasBaseType" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="EntityBase" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true">
            <AcceptingClass>
              <DomainClassMoniker Name="EntityBase" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="AssociationConnectorBuilder" IsCustom="true">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="PersistentTypeHasAssociations" />
        <SourceDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true">
            <AcceptingClass>
              <DomainClassMoniker Name="PersistentType" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true">
            <AcceptingClass>
              <DomainClassMoniker Name="PersistentType" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="TypedEntitySetHasItemType" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="TypedEntitySet" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective UsesRoleSpecificCustomAccept="true">
            <AcceptingClass>
              <DomainClassMoniker Name="Interface" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="e9b83bcb-ed9e-4841-a53a-203725382ffc" Description="Description for TXSoftware.DataObjectsNetEntityModel.Dsl.EntityDiagram" Name="EntityDiagram" DisplayName="DataObjects.Net Entity Model Diagram" Namespace="TXSoftware.DataObjectsNetEntityModel.Dsl">
    <Class>
      <DomainClassMoniker Name="EntityModel" />
    </Class>
    <ShapeMaps>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="PersistentType" />
        <ParentElementPath>
          <DomainPath>EntityModelHasPersistentTypes.EntityModel/!EntityModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="PersistentShape/TextName" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="PersistentType/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="PersistentShape/IconEntity" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="PersistentType/TypeName" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="Entity" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="PersistentShape/IconStructure" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="PersistentType/TypeName" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="Structure" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="PersistentShape/IconInterface" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="PersistentType/TypeName" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="Interface" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="PersistentShape/IconInherit" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="PersistentType/InheritsFrom" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="True" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="PersistentShape/TextDescription" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="PersistentType/TypeDescription" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <IconDecoratorMoniker Name="PersistentShape/IconTypedEntitySet" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="PersistentType/TypeKind" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="TypedEntitySet" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="PersistentShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="PersistentShape/Properties" />
          <ElementsDisplayed>
            <DomainPath>PersistentTypeHasProperties.Properties/!PersistentType</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="PropertyBase/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
        <CompartmentMap>
          <CompartmentMoniker Name="PersistentShape/NavigationProperties" />
          <ElementsDisplayed>
            <DomainPath>PersistentTypeHasNavigationProperties.NavigationProperties/!PersistentTypeOfNavigationProperty</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="PropertyBase/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="TypedEntitySet" />
        <ParentElementPath>
          <DomainPath>EntityModelHasPersistentTypes.EntityModel/!EntityModel</DomainPath>
        </ParentElementPath>
        <CompartmentShapeMoniker Name="TypedEntitySetShape" />
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Entity" />
        <ParentElementPath>
          <DomainPath>EntityModelHasPersistentTypes.EntityModel/!EntityModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <IconDecoratorMoniker Name="EntityShape/IconHierarchyRoot" />
          <VisibilityPropertyPath>
            <DomainPropertyMoniker Name="Entity/IsHierarchyRoot" />
            <PropertyFilters>
              <PropertyFilter FilteringValue="True" />
            </PropertyFilters>
          </VisibilityPropertyPath>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="EntityShape" />
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Interface" />
        <ParentElementPath>
          <DomainPath>EntityModelHasPersistentTypes.EntityModel/!EntityModel</DomainPath>
        </ParentElementPath>
        <CompartmentShapeMoniker Name="InterfaceShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="InterfaceShape/Indexes" />
          <ElementsDisplayed>
            <DomainPath>InterfaceHasIndexes.Indexes/!InterfaceOfIndex</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="EntityIndex/CalculatedName" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="EntityBase" />
        <ParentElementPath>
          <DomainPath>EntityModelHasPersistentTypes.EntityModel/!EntityModel</DomainPath>
        </ParentElementPath>
        <CompartmentShapeMoniker Name="EntityBaseShape" />
      </CompartmentShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="InterfaceInheritanceConnector" />
        <DomainRelationshipMoniker Name="InterfaceInheritInterfaces" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="EntityBaseInheritanceConnector" />
        <DomainRelationshipMoniker Name="EntityBaseHasBaseType" />
      </ConnectorMap>
      <ConnectorMap ConnectsCustomSource="true">
        <ConnectorMoniker Name="StructurePropertyTypeOfConnector" />
        <DomainRelationshipMoniker Name="StructurePropertyHasType" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="AssociationConnector" />
        <DomainRelationshipMoniker Name="PersistentTypeHasAssociations" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationConnector/TextSourceMultiplicity" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="PersistentTypeHasAssociations/SourceMultiplicity" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="AssociationConnector/TextTargetMultiplicity" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="PersistentTypeHasAssociations/TargetMultiplicity" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="TypedEntitySetHasItemTypeConnector" />
        <DomainRelationshipMoniker Name="TypedEntitySetHasItemType" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer FileExtension="dom" EditorGuid="7187e39a-cd41-48f4-bffd-1a7d310581e4">
    <RootClass>
      <DomainClassMoniker Name="EntityModel" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="false">
      <XmlSerializationBehaviorMoniker Name="EntityModelDesignerSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="DataObjects.Net Entity Model Designer">
      <ElementTool Name="Entity" ToolboxIcon="Resources\Entity.bmp" Caption="Entity" Tooltip="Entity" HelpKeyword="Entity">
        <DomainClassMoniker Name="Entity" />
      </ElementTool>
      <ElementTool Name="Structure" ToolboxIcon="Resources\Structure.bmp" Caption="Structure" Tooltip="Structure" HelpKeyword="Structure">
        <DomainClassMoniker Name="Structure" />
      </ElementTool>
      <ConnectionTool Name="Inheritance" ToolboxIcon="Resources\Inheritance.bmp" Caption="Inheritance" Tooltip="Inheritance" HelpKeyword="">
        <ConnectionBuilderMoniker Name="DONetEntityModelDesigner/InterfaceInheritInterfacesBuilder" />
      </ConnectionTool>
      <ElementTool Name="Interface" ToolboxIcon="Resources\Interface.bmp" Caption="Interface" Tooltip="Interface" HelpKeyword="Interface">
        <DomainClassMoniker Name="Interface" />
      </ElementTool>
      <ConnectionTool Name="Association" ToolboxIcon="Resources\Association.bmp" Caption="Association" Tooltip="Create an Association between two Entities" HelpKeyword="Association">
        <ConnectionBuilderMoniker Name="DONetEntityModelDesigner/AssociationConnectorBuilder" />
      </ConnectionTool>
      <ElementTool Name="TypedEntitySet" ToolboxIcon="Resources\EntitySet.bmp" Caption="Typed EntitySet" Tooltip="Typed Entity Set" HelpKeyword="">
        <DomainClassMoniker Name="TypedEntitySet" />
      </ElementTool>
    </ToolboxTab>
    <Validation UsesMenu="true" UsesOpen="true" UsesSave="true" UsesCustom="true" UsesLoad="false" />
    <DiagramMoniker Name="EntityDiagram" />
  </Designer>
  <Explorer ExplorerGuid="4d9b3d9a-b0e6-4646-a612-1b2d3415ab2f" Title="DataObjects.Net Entity Model Explorer">
    <ExplorerBehaviorMoniker Name="DONetEntityModelDesigner/DONetEntityModelDesignerExplorer" />
  </Explorer>
</Dsl>