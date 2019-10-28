using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Dsl;

namespace TXSoftware.DataObjectsNetEntityModel.Tests.Modeling
{
    /// <summary>
    /// Summary description for InheritanceTreeTest
    /// </summary>
    [TestClass]
    public class InheritanceTreeTest
    {
        public InheritanceTreeTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void PropertiesBuilder_Test()
        {
            ModelRoot modelRoot = new ModelRoot();
            Func<Guid, IDomainType> getDomainType = modelRoot.GetDomainType;

            Interface IEntity = new Interface(modelRoot, "IEntity");
            IEntity.AddProperty(new ScalarProperty("Id", getDomainType(SystemPrimitiveTypesConverter.TYPE_ID_INT32)));
            IEntity.AddProperty(new ScalarProperty("OId", getDomainType(SystemPrimitiveTypesConverter.TYPE_ID_BYTE)));

            Entity EntityBase = new Entity(modelRoot, "EntityBase");
            EntityBase.SetInheritance(IEntity);
            ScalarProperty propertyId = new ScalarProperty("Id", getDomainType(SystemPrimitiveTypesConverter.TYPE_ID_INT32));
            propertyId.IsInherited = true;
            EntityBase.AddProperty(propertyId);
            ScalarProperty propertyOId = new ScalarProperty("OId", getDomainType(SystemPrimitiveTypesConverter.TYPE_ID_BYTE));
            propertyOId.IsInherited = true;
            EntityBase.AddProperty(propertyOId);
            EntityBase.AddProperty(new ScalarProperty("Age", getDomainType(SystemPrimitiveTypesConverter.TYPE_ID_STRING)));
            EntityBase.AddProperty(new ScalarProperty("ScalarProperty4", getDomainType(SystemPrimitiveTypesConverter.TYPE_ID_STRING)));

            Entity Car = new Entity(modelRoot, "Car");
            Car.SetInheritance(EntityBase);
            Car.AddProperty(new ScalarProperty("SuperKey", getDomainType(SystemPrimitiveTypesConverter.TYPE_ID_INT32)));
            Car.AddProperty(new NavigationProperty("SimilarCars"));

            modelRoot.UpdateTopHierarchyTypes(Car);

            PropertiesBuilderContext.Initialize(modelRoot);
            var carPropertiesBuilder = PropertiesBuilderContext.Current.Get(Car);
            var inheritedProperties = carPropertiesBuilder.GetInheritedProperties();
        }

        [TestMethod]
        public void InheritanceTree_Test()
        {
            ModelRoot modelRoot = new ModelRoot();

            Interface I00 = new Interface(modelRoot, "I00");
            Interface I01 = new Interface(modelRoot, "I01");
            Interface I02 = new Interface(modelRoot, "I02");
            Interface I03 = new Interface(modelRoot, "I03");
            Interface I04 = new Interface(modelRoot, "I04");
            Interface I05 = new Interface(modelRoot, "I05");
            Entity C01 = new Entity(modelRoot, "C01");
            Entity C02 = new Entity(modelRoot, "C02");
            Entity C03 = new Entity(modelRoot, "C03");

            // assign inheritance
            I02.SetInheritance(I01);
            
            I04.SetInheritance(I01);
            
            I03.SetInheritance(I02, I04);
            
            I05.SetInheritance(I04, I02);
            
            C01.SetInheritance(I03, I05, I00);

            C02.SetInheritance(C01);

            //C03.SetInheritance(I00);

            // test case #1
            var inheritanceTrees = InheritanceTree.Create(C01, C02, C03);
            InheritanceTree.RebuildTree(true, inheritanceTrees.ToArray());
            InheritanceTree C01_inheritanceTree = inheritanceTrees[0];
            //C01_inheritanceTree.RebuildTree(true);

            ReadOnlyCollection<InheritanceNode> flatList = C01_inheritanceTree.GetFlatList(InheritanceListMode.CurrentLevel);
            IEnumerable<IInterface> flatInterfaces = flatList.Select(node => node.Interface);
            
            Assert.IsTrue(flatInterfaces.Contains(I03));
            Assert.IsTrue(flatInterfaces.Contains(I05));
            Assert.IsTrue(flatInterfaces.Contains(I00));

            // test case #2
            //inheritanceTrees[1].RebuildTree(true);
            var mergedPaths = InheritanceTree.MergePaths(inheritanceTrees).ToArray();
        }

        #region classes

        #region class DomainType

        public class DomainType : IDomainType
        {
            public string Name { get; set; }
            public string Documentation { get; private set; }
            public string Namespace { get; set; }
            public string FullName { get; private set; }
            public bool IsBuildIn { get; private set; }
            public IModelRoot EntityModel { get; private set; }
            public Guid Id { get; private set; }
            public Guid BuildInID { get; set; }

            public DomainType(string name, string ns, Guid buildiId)
            {
                Name = name;
                Namespace = ns;
                IsBuildIn = buildiId != Guid.Empty;
                BuildInID = buildiId;
                if (!IsBuildIn)
                {
                    this.Id = Guid.NewGuid();
                }
            }

            public Guid GetTypeId()
            {
                return IsBuildIn ? BuildInID : this.Id;
            }

            public Type TryGetClrType(Type defaultType)
            {
                Type result = null;

                string displayName = SystemPrimitiveTypesConverter.GetDisplayName(this.FullName);
                if (!string.IsNullOrEmpty(displayName))
                {
                    result = SystemPrimitiveTypesConverter.GetClrType(displayName);
                }

                return result ?? defaultType;
            }

            public bool EqualsTo(IDomainType other)
            {
                return this.IsBuildIn == other.IsBuildIn &&
                   this.BuildInID == other.BuildInID &&
                   this.FullName == other.FullName;
            }
        }

        #endregion class DomainType

        #region class PersistentType

        public abstract class PersistentType : IPersistentType
        {
            #region fields 

            protected readonly List<IPropertyBase> allProperties = new List<IPropertyBase>();

            #endregion fields

            #region properties 

            public string Name { get; set; }
            public string Documentation { get; private set; }
            public AccessModifier Access { get; private set; }
            public abstract PersistentTypeKind TypeKind { get; }
            public string Namespace { get; set; }
            public IOrmAttribute[] TypeAttributes { get; private set; }

            public DataContractDescriptor DataContract
            {
                get { throw new NotImplementedException(); }
            }

            public ReadOnlyCollection<IPersistentType> PersistentTypeAssociations { get; private set; }

            public ReadOnlyCollection<IPropertyBase> Properties
            {
                get
                {
                    var props =
                        allProperties.Where(prop => prop.PropertyKind != PropertyKind.Navigation).Cast<IPropertyBase>().
                            ToList();
                    return new ReadOnlyCollection<IPropertyBase>(props);
                }
            }

            public ReadOnlyCollection<INavigationProperty> NavigationProperties
            {
                get
                {
                    var navProps =
                        allProperties.Where(prop => prop.PropertyKind == PropertyKind.Navigation).Cast
                            <INavigationProperty>().ToList();
                    return new ReadOnlyCollection<INavigationProperty>(navProps);
                }
            }

            public ReadOnlyCollection<IPropertyBase> AllProperties
            {
                get { return new ReadOnlyCollection<IPropertyBase>(allProperties); }
            }

            #endregion properties

            #region methods 

            public ReadOnlyCollection<IPropertyBase> GetAllProperties(bool includeInheritance)
            {
                throw new NotImplementedException();
            }

            public ReadOnlyCollection<IScalarProperty> GetScalarProperties()
            {
                return
                    AllProperties.Where(prop => prop.PropertyKind == PropertyKind.Scalar).Cast<IScalarProperty>().
                        ToReadOnlyCollection();
            }

            public ReadOnlyCollection<IStructureProperty> GetStructureProperties()
            {
                return
                    AllProperties.Where(prop => prop.PropertyKind == PropertyKind.Structure).Cast<IStructureProperty>().
                        ToReadOnlyCollection();
            }

            public IPropertiesBuilder GetPropertiesBuilder()
            {
                return PropertiesBuilderContext.Current.Get(this);
            }

            public override string ToString()
            {
                return string.Format("{0} [{1}]", this.Name, this.TypeKind);
            }

            public void AddProperty<T>(T property) where T : IPropertyBase
            {
                this.allProperties.Add(property);
                (property as PropertyBase).UpdateOwner(this);
            }

            #endregion methods

            protected PersistentType(ModelRoot modelRoot, string name)
            {
                modelRoot.AddPersistentType(this);
                Name = name;
                TypeAttributes = new IOrmAttribute[0];
            }
        }

        #endregion class PersistentType

        #region class Interface

        public class Interface : PersistentType, IInterface
        {
            #region fields 

            private readonly List<IInterface> inheritedInterfaces = new List<IInterface>();

            #endregion fields

            #region properties

            public override PersistentTypeKind TypeKind
            {
                get { return PersistentTypeKind.Interface; }
            }

            public ReadOnlyCollection<IInterface> InheritedInterfaces
            {
                get { return new ReadOnlyCollection<IInterface>(inheritedInterfaces); }
            }

            public ReadOnlyCollection<ITypedEntitySet> ReferencedInTypedEntitySets { get; private set; }

            public ReadOnlyCollection<IEntityIndex> Indexes { get; private set; }

            public ReadOnlyCollection<IInterface> InheritingByInterfaces { get; private set; }

            public InheritsIEntityMode InheritsIEntity
            {
                get { throw new NotImplementedException(); }
            }

            #endregion properties

            #region constructors 

            public Interface(ModelRoot modelRoot, string name)
                : base(modelRoot, name)
            {
            }

            #endregion constructors

            #region methods

            public bool InheritInterface(IInterface @interface)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IInterface> GetCurrentLevelInheritedInterfaces()
            {
                return this.InheritedInterfaces;
            }

            public InheritanceTree GetInheritanceTree()
            {
                return InheritanceTree.Create(this);
            }

            public void ImplementToType(IEntityBase targetType, ImplementTypeOptions options)
            {
                throw new NotImplementedException();
            }

            public IEntityBase ImplementToNewType(PersistentTypeKind newTypeKind, string newTypeName,
                ImplementTypeOptions options)
            {
                throw new NotImplementedException();
            }

            public bool ContainsProperty(PropertyKind propertyKind, string propertyName)
            {
                throw new NotImplementedException();
            }

            public bool ContainsIndex(IEntityIndex other)
            {
                throw new NotImplementedException();
            }

            public ReadOnlyCollection<IPropertyBase> GetAllProperties(bool includeInheritance)
            {
                if (!includeInheritance)
                {
                    return base.GetAllProperties(false);
                }

                var inheritanceTree = InheritanceTreeCache.Get(this);
                var distinctByName = new IdentityProjectionEqualityComparer
                    <IPropertyBase, string>(property => property.Name);
                var inheritanceList =
                    inheritanceTree.GetFlatList(InheritanceListMode.WholeTree).Select(node => node.Interface);
                var result = (from item in inheritanceList
                              select item.AllProperties).SelectMany(list => list).Distinct(distinctByName);

                return new ReadOnlyCollection<IPropertyBase>(result.ToList());
            }

            #endregion methods

            public void SetInheritance(params Interface[] interfaces)
            {
                this.inheritedInterfaces.Clear();
                this.inheritedInterfaces.AddRange(interfaces);
            }
        }

        #endregion class Interface

        #region class EntityBase

        public class EntityBase : Interface, IEntityBase
        {
            public EntityBase(ModelRoot modelRoot, string name)
                : base(modelRoot, name)
            {
            }

            public InheritanceModifiers InheritanceModifier { get; private set; }
            public IEntityBase BaseType { get; private set; }
            public ReadOnlyCollection<IEntityBase> ReferencesAsBaseType { get; private set; }

            public ReadOnlyCollection<IEntityBase> GetBaseTypesGraph(InheritanceGraphDirection graphDirection)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IInterface> GetCurrentLevelInheritedInterfaces()
            {
                List<IInterface> list = new List<IInterface>(base.InheritedInterfaces);
                if (this.BaseType != null)
                {
                    list.Add(BaseType);
                }

                return list;
            }
        }

        #endregion class EntityBase

        #region class Entity

        public class Entity : EntityBase, IEntity
        {
            public Entity(ModelRoot modelRoot, string name)
                : base(modelRoot, name)
            {
            }

            public OrmHierarchyRootAttribute HierarchyRoot { get; private set; }
            public OrmKeyGeneratorAttribute KeyGenerator { get; private set; }
            public OrmTypeDiscriminatorValueAttribute TypeDiscriminatorValue { get; private set; }

            public override PersistentTypeKind TypeKind
            {
                get { return PersistentTypeKind.Entity; }
            }
        }

        #endregion class Entity

        #region class Structure

        public class Structure : EntityBase, IStructure
        {
            #region properties 

            public Structure(ModelRoot modelRoot, string name)
                : base(modelRoot, name)
            {
            }

            public ReadOnlyCollection<IStructureProperty> ReferencedInProperties
            {
                get { throw new NotImplementedException(); }
            }

            public override PersistentTypeKind TypeKind
            {
                get { return PersistentTypeKind.Structure; }
            }

            #endregion properties
        }

        #endregion class Structure

        #region class PropertyBase

        public abstract class PropertyBase : IPropertyBase
        {
            public string Name { get; set; }
            public string Documentation { get; private set; }
            public AccessModifier Access { get; private set; }
            public IPersistentType Owner { get; private set; }
            public OrmFieldAttribute FieldAttribute { get; private set; }
            public OrmPropertyConstraints Constraints { get; private set; }
            public IOrmAttribute[] TypeAttributes { get; private set; }
            public DataMemberDescriptor DataMember { get; private set; }

            public PropertyAccessModifiers PropertyAccess
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsInherited { get; set; }

            public abstract PropertyKind PropertyKind { get; }


            public override string ToString()
            {
                return string.Format("{0} [{1}]", this.Name, this.PropertyKind);
            }

            public bool IsImplementedBy(IInterface @interface)
            {
                throw new NotImplementedException();
            }

            public IPersistentType GetRealOwner()
            {
                throw new NotImplementedException();
            }

            protected PropertyBase(string name)
            {
                Name = name;
                TypeAttributes = new IOrmAttribute[0];
            }

            public void UpdateOwner(PersistentType owner)
            {
                this.Owner = owner;
            }
        }

        #endregion class PropertyBase

        #region ScalarProperty

        public class ScalarProperty : PropertyBase, IScalarProperty
        {
            public ScalarProperty(string name, IDomainType domainType)
                : base(name)
            {
                this.Type = domainType;
            }

            public override PropertyKind PropertyKind
            {
                get { return PropertyKind.Scalar; }
            }

            public override string ToString()
            {
                return string.Format("{0}:{1} [{2}]", this.Name, Type, this.PropertyKind);
            }

            public IDomainType Type { get; private set; }

//            public Type ClrType
//            {
//                get { return this.Type.TryGetClrType(typeof (string)); }
//            }

            public OrmKeyAttribute KeyAttribute { get; private set; }
        }

        #endregion ScalarProperty

        #region StructureProperty

        public class StructureProperty : PropertyBase, IStructureProperty
        {
            private IStructure _typeof;

            public StructureProperty(string name, IStructure structure) : base(name)
            {
                this._typeof = structure;
            }

            public override PropertyKind PropertyKind
            {
                get { return PropertyKind.Structure; }
            }

            public IStructure TypeOf
            {
                get { return _typeof; }
            }
        }

        #endregion StructureProperty

        #region NavigationProperty

        public class NavigationProperty : PropertyBase, INavigationProperty
        {
            public NavigationProperty(string name) : base(name)
            {
            }

            public override PropertyKind PropertyKind
            {
                get { return PropertyKind.Navigation; }
            }

            public MultiplicityKind Multiplicity { get; private set; }
            public string PairFrom { get; private set; }
            public string PairTo { get; private set; }
            public IPersistentType OwnerPersistentType { get; private set; }
            public IPersistentTypeHasAssociations PersistentTypeHasAssociations { get; private set; }
            public ITypedEntitySet TypedEntitySet { get; private set; }

            public OrmKeyAttribute KeyAttribute
            {
                get { throw new NotImplementedException(); }
            }
        }

        #endregion NavigationProperty

        #region ModelRoot

        public class ModelRoot : IModelRoot
        {
            private readonly List<IDomainType> domainTypes = new List<IDomainType>();
            private readonly List<IPersistentType> persistentTypes = new List<IPersistentType>();

            public string Namespace { get; private set; }
            public ReadOnlyCollection<IPersistentType> PersistentTypes
            {
                get
                {
                    return new ReadOnlyCollection<IPersistentType>(persistentTypes);
                }
            }
            public ReadOnlyCollection<IInterface> TopHierarchyTypes { get; private set; }

            public ReadOnlyCollection<IDomainType> DomainTypes
            {
                get { return new ReadOnlyCollection<IDomainType>(domainTypes); }
            }

            public ReadOnlyCollection<IDomainType> BuildInDomainTypes
            {
                get { return new ReadOnlyCollection<IDomainType>(domainTypes.Where(item => item.IsBuildIn).ToArray()); }
            }

            public ModelRoot()
            {
                //TopHierarchyTypes = topHierarchyTypes.ToList().ToReadOnlyCollection();
                GenerateBuildInTypes();
            }

            internal void AddPersistentType(IPersistentType persistentType)
            {
                persistentTypes.Add(persistentType);
            }

            public void UpdateTopHierarchyTypes(params IInterface[] topHierarchyTypes)
            {
                TopHierarchyTypes = topHierarchyTypes.ToList().ToReadOnlyCollection();
            }

            public void Validate(string templateVersion)
            {
                throw new NotImplementedException();
            }

            public IDomainType GetDomainType(Guid typeId)
            {
                return DomainTypes.SingleOrDefault(type => type.GetTypeId() == typeId);
            }

            public void AddDomainType(IDomainType domainType)
            {
                this.domainTypes.Add(domainType);
            }

            internal void GenerateBuildInTypes()
            {
                foreach (var typePair in SystemPrimitiveTypesConverter.TypeIds)
                {
                    Guid typeId = typePair.Key;
                    Type type = typePair.Value;

                    if (!this.DomainTypes.Any(domainType => Util.StringEqual(domainType.Name, type.Name, true) &&
                                                            Util.StringEqual(domainType.Namespace, type.Namespace, true)))
                    {
                        var buildInDomainType = new DomainType(type.Name, type.Namespace, typeId);
                        AddDomainType(buildInDomainType);
                    }
                }
            }
        }

        #endregion ModelRoot

        #endregion classes
    }
}
