namespace TXSoftware.DataObjectsNetEntityModel
{
    public partial class ProductVersionDto
    {
        public string ToXml()
        {
            return SerializeUtils.SerializeDataContract(this);
        }

        public static ProductVersionDto FromXml(string xml)
        {
            return SerializeUtils.DeserializeDataContract<ProductVersionDto>(xml);
        }
    }
}