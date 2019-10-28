using System;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public interface IDomainType: IElement
    {
        string Namespace { get; set; }

        string FullName { get; }

        bool IsBuildIn { get; }

        IModelRoot EntityModel { get; }

        Guid BuildInID { get; set; }
        
        Guid GetTypeId();
        
        Type TryGetClrType(Type defaultType);

        bool EqualsTo(IDomainType other);
    }
}