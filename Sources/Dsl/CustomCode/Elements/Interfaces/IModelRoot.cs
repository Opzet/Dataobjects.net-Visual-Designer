using System;
using System.Collections.ObjectModel;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IModelRoot
    {
        string Namespace { get; }

        ReadOnlyCollection<IPersistentType> PersistentTypes { get; }

        ReadOnlyCollection<IInterface> TopHierarchyTypes { get; }

        ReadOnlyCollection<IDomainType> DomainTypes { get; }

        ReadOnlyCollection<IDomainType> BuildInDomainTypes { get; }

        void Validate(string templateVersion);

        IDomainType GetDomainType(Guid typeId);
    }
}