using System.Collections.ObjectModel;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IPersistentType : IAccessibleElement
    {
        PersistentTypeKind TypeKind { get; }

        string Namespace { get; set;  }

        IOrmAttribute[] TypeAttributes { get; }

        DataContractDescriptor DataContract { get; }
        
        ReadOnlyCollection<IPersistentType> PersistentTypeAssociations { get; }

        /// <summary>
        /// Gets scalar and structure properties
        /// </summary>
        ReadOnlyCollection<IPropertyBase> Properties { get; }

        ReadOnlyCollection<INavigationProperty> NavigationProperties { get; }

        ReadOnlyCollection<IPropertyBase> AllProperties { get; }

        /// <summary>
        /// Gets all properties.
        /// </summary>
        /// <param name="includeInheritance"></param>
        /// <returns></returns>
        ReadOnlyCollection<IPropertyBase> GetAllProperties(bool includeInheritance);

        ReadOnlyCollection<IScalarProperty> GetScalarProperties();

        ReadOnlyCollection<IStructureProperty> GetStructureProperties();

        //IPropertiesBuilder GetPropertiesBuilder();
    }

    public enum InheritanceMergeMode
    {
        None = 0,
        Merge
    }
}