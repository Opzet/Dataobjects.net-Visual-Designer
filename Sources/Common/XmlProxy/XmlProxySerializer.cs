using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
	/// <summary>
	/// Xml serializer to serialize/deserialize <see cref="XmlProxy"/> object
	/// </summary>
	public class XmlProxySerializer
	{
	    private static XmlProxySerializer instance;

	    public static XmlProxySerializer Instance
	    {
	        get
	        {
                if (instance == null)
                {
                    instance = new XmlProxySerializer();
                }

	            return instance;
	        }
	    }

		#region Serialize section

	    public void Serialize(XmlProxy item, XmlWriter writer)
		{
			writer.WriteStartElement(item.ElementName);
			for(int i = 0; i < item.Attributes.Count; i++)
			{
				XmlProxyAttribute attr = item.Attributes[i];
				writer.WriteAttributeString(attr.Key, attr.Value);
			}

			writer.WriteString(item.ElementValue);

			foreach(XmlProxy child in item.Childs)
			{
				Serialize(child, writer);
			}
			writer.WriteEndElement();
		}

	    public string Serialize(XmlProxy item)
	    {
	        return Serialize(item, Encoding.UTF8);
	    }

	    public string Serialize(XmlProxy item, Encoding encoding)
		{
			if (item == null) return string.Empty;

			MemoryStream ms = new MemoryStream();
			XmlWriter writer = new XmlTextWriter(ms, encoding);
			//writer.Formatting = Formatting.Indented;

			Serialize(item, writer);

			/*writer.Close();
			ms.Flush();
			ms.Close();*/
			writer.Flush();

			ms.Position = 0;
			string s = new StreamReader(ms).ReadToEnd();
            writer.Close();
			ms.Close();
			return s;
		}

		#endregion

		#region Deserialize section

        /// <summary>
        /// Deserialize object to <see cref="XmlProxy"/> object.
        /// </summary>
        /// <param name="data">Xml string from which to deserialize.</param>
        /// <returns>Deserialized <see cref="XmlProxy"/> object.</returns>
	    public XmlProxy Deserialize(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }

            using (XmlTextReader reader = new XmlTextReader(xml, XmlNodeType.Element, null))
            {
                reader.WhitespaceHandling = WhitespaceHandling.None;
                XmlProxy xmlProxy = Deserialize(reader);
                reader.Close();
                return xmlProxy;
            }
        }

	    public XmlProxy Deserialize(XmlReader reader)
	    {
	        reader.MoveToContent();
	        int startDepth = reader.Depth;
	        string startElementName = reader.Name;

	        Dictionary<int, List<XmlProxy>> items = new Dictionary<int, List<XmlProxy>>();
	        XmlProxy lastProxy = null;

	        bool readedOk = !reader.EOF;
	        while ((readedOk) && (!reader.EOF))
	        {
	            switch (reader.NodeType)
	            {
	                case XmlNodeType.Element:
	                    XmlProxy pi = new XmlProxy(reader.Name);
	                    List<XmlProxy> al = items.SingleOrDefault(pair => pair.Key == reader.Depth).Value;
	                    if (al == null)
	                    {
	                        al = new List<XmlProxy>();
                            items.Add(reader.Depth, al);
	                        //items[reader.Depth] = al;
	                    }
	                    al.Add(pi);

	                    // parent
                        List<XmlProxy> pal = items.SingleOrDefault(pair => pair.Key == (reader.Depth - 1)).Value;
	                    if ((pal != null) && (pal.Count > 0))
	                    {
	                        XmlProxy parent = pal[pal.Count-1];
	                        if (parent != null)
	                        {
	                            parent.AddChild(pi);
	                        }
	                    }

	                    // read attributes
	                    if (reader.HasAttributes)
	                    {
	                        while (reader.MoveToNextAttribute())
	                        {
	                            pi.AddAttribute(reader.Name, reader.Value);
	                        }
	                    }

	                    lastProxy = pi;

	                    break;

	                case XmlNodeType.Text:
	                    if (lastProxy != null)
	                    {
	                        lastProxy.ElementValue = reader.Value;
	                    }
	                    break;

	                case XmlNodeType.EndElement:
	                    break;
	            }

	            readedOk = reader.Read();
	            reader.MoveToContent();

                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == startElementName && reader.Depth == startDepth)
                {
                    break;
                }
	        }

	        //reader.Close();

	        if (items.Count > 0)
	        {
	            List<XmlProxy> firstProcyList = items.OrderBy(pair => pair.Key).Select(pair => pair.Value).First();

	            //List<XmlProxy> al = items[0] as List<XmlProxy>;
                if ((firstProcyList != null) && (firstProcyList.Count > 0))
	            {
                    return firstProcyList[0];
	            }
	        }

	        return null;
	    }

	    #endregion

	}
}