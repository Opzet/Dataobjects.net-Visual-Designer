using System;
using System.Xml;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    public interface ISerializableObject
    {
        void DeserializeFromXml(XmlReader reader);
        void SerializeToXml(XmlWriter writer);
    }
}