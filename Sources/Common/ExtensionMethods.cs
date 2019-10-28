using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Xml;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    public static class ExtensionMethods
    {
        public static bool Contains(this List<string> list, string value, bool ignoreCase)
        {
            return list.SingleOrDefault(s => string.Compare(s, value, ignoreCase) == 0) != null;
        }

        public static bool In<T>(this T item, params T[] collection)
        {
            IEnumerable<T> enumerable = collection;
            return In(item, enumerable);
        }

        public static bool In<T>(this T item, IEnumerable<T> collection)
        {
            foreach (T i in collection)
            {
                if (i.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static PropertyGrid ResolvePropertyGrid(this ITypeDescriptorContext descriptorContext)
        {
            return InternalResolvePropertyGrid(descriptorContext);
        }

        public static PropertyGrid ResolvePropertyGrid(this IWindowsFormsEditorService service)
        {
            return InternalResolvePropertyGrid(service);
        }

        private static PropertyGrid InternalResolvePropertyGrid(object instance)
        {
            PropertyGrid grid = null;

            if (instance != null)
            {
                PropertyInfo propertyInfo = instance.GetType().GetProperties().Single(info => info.Name == "OwnerGrid");
                grid = propertyInfo.GetValue(instance, null) as PropertyGrid;
            }
            return grid;
        }

        public static string SerializeToString<T>(this T obj) where T : ISerializableObject
        {
            string result;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriter writer = new XmlTextWriter(ms, Encoding.UTF8);

                obj.SerializeToXml(writer);

                writer.Flush();

                ms.Position = 0;
                result = new StreamReader(ms).ReadToEnd();
                writer.Close();
                ms.Close();
            }

            return result;
        }

        public static void DeserializeFromString<T>(this T obj, string xml) where T : ISerializableObject
        {
            using (XmlTextReader reader = new XmlTextReader(xml, XmlNodeType.Element, null))
            {
                reader.WhitespaceHandling = WhitespaceHandling.None;
                obj.DeserializeFromXml(reader);
                reader.Close();
            }
        }

        public static T CloneBySerialization<T>(this T obj) where T : ISerializableObject//, new()
        {
            return Clone(obj);
        }

        public static T Clone<T>(this T obj) where T : ISerializableObject//, new()
        {
            //T cloned = new T();
            if (obj == null)
            {
                return default(T);
            }

            T cloned = (T) Activator.CreateInstance(obj.GetType());

            var xml = obj.SerializeToString();
            if (!string.IsNullOrEmpty(xml))
            {
                cloned.DeserializeFromString(xml);
            }

            return cloned;
        }

        public static void SkipToNextElementFix(this XmlReader reader)
        {
            if (!reader.EOF && reader.NodeType != XmlNodeType.Element)
            {
                if (reader.Read())
                {
                    reader.MoveToContent();
                }
            }
        }

        public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ReadOnlyCollection<T>(enumerable.ToList());
        }
    }
}