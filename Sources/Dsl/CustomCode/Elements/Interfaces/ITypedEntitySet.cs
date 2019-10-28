using System.Collections.ObjectModel;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface ITypedEntitySet: IPersistentType
    {
        IInterface ItemType { get; }

        ReadOnlyCollection<INavigationProperty> NavigationPropertiesReferencingThis { get; }
    }
}