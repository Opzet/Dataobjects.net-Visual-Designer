using System.Runtime.Serialization;
using System.Xml;

namespace TXSoftware.DataObjectsNetEntityModel
{
    public static class SerializeUtils
    {
        public static string SerializeDataContract<T>(T obj)
        {
            var serializer = new DataContractSerializer(obj.GetType());

            using (var backing = new System.IO.StringWriter())
            {
                using (var writer = new System.Xml.XmlTextWriter(backing))
                //using (var writer = new NoNamespaceXmlWriter(backing))
                {
                    serializer.WriteObject(writer, obj);
                    return backing.ToString();
                }
            }
        }

        public static T DeserializeDataContract<T>(string xml) where T : class
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var backing = new System.IO.StringReader(xml))
            {
                using (var reader = new System.Xml.XmlTextReader(backing))
                {
                    return serializer.ReadObject(reader) as T;
                }
            }
        }

//        public class NoNamespaceXmlWriter : XmlTextWriter
//        {
//            //Provide as many contructors as you need
//            public NoNamespaceXmlWriter(System.IO.TextWriter output)
//                : base(output) { Formatting = System.Xml.Formatting.Indented; }
//
//            public override void WriteStartDocument() { }
//
//            public override void WriteStartElement(string prefix, string localName, string ns)
//            {
//                base.WriteStartElement("", localName, "");
//            }
//        }

    }
}