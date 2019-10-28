using System.Collections.ObjectModel;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IPersistentTypeHasAssociations: IElement
    {
        IAssociationInfo SourceAssociation { get; }
        
        IAssociationInfo TargetAssociation { get; }

        ReadOnlyCollection<INavigationProperty> NavigationProperties { get; }

        IPersistentType SourcePersistentType { get; }

        IPersistentType TargetPersistentType { get; }

        bool EqualAssociationLinkTo(IPersistentTypeHasAssociations other);
    }
}