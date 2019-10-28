namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    public class DataSourceInfo
    {
        public string Host { get; set; }
        public string Instance { get; set; }
        public bool IsClustered { get; set; }
        public string Version { get; set; }
    }
}