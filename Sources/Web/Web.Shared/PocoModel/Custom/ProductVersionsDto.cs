using System.Collections.Generic;
using System.Runtime.Serialization;


namespace TXSoftware.DataObjectsNetEntityModel
{
    [CollectionDataContract(IsReference = true, Namespace = "TXSoftware.DataObjectsNetEntityModel")]
    public class ProductVersionsDto: List<ProductVersionDto>
    {
        public ProductVersionsDto(IEnumerable<ProductVersionDto> collection) : base(collection)
        {}

        public ProductVersionsDto()
        {}
    }
}