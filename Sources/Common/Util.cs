using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    public static class Util
    {
        private static VersionNumber currentVersion;

        public static VersionNumber CurrentVersion
        {
            get
            {
                if (currentVersion == null)
                {
                    currentVersion = new VersionNumber(Assembly.GetExecutingAssembly().GetName().Version.ToString());
                }
                return currentVersion;
            }
        }

        /// <summary>
        /// Hexes the string to bytes.
        /// </summary>
        /// <param name="hexString">The hex string.</param>
        /// <returns></returns>
        public static byte[] HexStringToBytes(string hexString)
        {
            byte[] result = new byte[hexString.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = byte.Parse(hexString.Substring(i * 2, 2), NumberStyles.HexNumber);
            }

            return result;
        }

        /// <summary>
        /// Convert array of bytes to hexa string.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        /// <example>input: byte[]{10, 15, 30} output: 0A0F1E</example>
        public static string BytesToHexString(byte[] bytes)
        {
            string result = string.Empty;

            for (int i = 0; i < bytes.Length; i++)
            {
                result += string.Format("{0:x2}", bytes[i]);
            }

            return result.ToUpper();
        }

        public static bool BytesEqual(byte[] data1, byte[] data2)
        {
            if ((data1 == null) || (data2 == null))
            {
                throw new ArgumentNullException();
            }

            if (data1.Length != data2.Length)
            {
                return false;
            }

            for (int i = 0; i < data1.Length; i++)
            {
                if (data1[i] != data2[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compare two string values if they are equal. 
        /// </summary>
        /// <param name="str1">First string value to compare.</param>
        /// <param name="str2">Second string value to compare.</param>
        /// <param name="ignoreCase">True to ignore case.</param>
        /// <returns>Returns true if specified string values are equal, of false if not.</returns>
        public static bool StringEqual(string str1, string str2, bool ignoreCase)
        {
            return (string.Compare(str1, str2, ignoreCase) == 0);
        }

        public static T DeserializeObjectFromXml<T>(string xml) where T : class
        {
            return DeserializeObjectFromXml<T>(new Type[0], xml);
        }

        public static T DeserializeObjectFromXml<T>(Type[] extraTypes, string xml) where T : class
        {
            return DeserializeObjectFromXml(typeof(T), extraTypes, xml) as T;
        }

        /// <summary>
        /// Deserialize object from xml, which was serilized by [Serializable] flag.
        /// </summary>
        /// <param name="resultType">Type of the result.</param>
        /// <param name="extraTypes">The extra types.</param>
        /// <param name="xml">XML data.</param>
        /// <returns>Deserialized object.</returns>
        public static object DeserializeObjectFromXml(Type resultType, Type[] extraTypes, string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return null;
            }

            var serializer = new XmlSerializer(resultType, extraTypes);
            TextReader r = new StringReader(xml);
            return serializer.Deserialize(r);
        }

        /// <summary>
        /// Create XML from specified object by serialization.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="extraTypes">The extra types.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>XML data.</returns>
        public static string SerializeObjectToXml(Type objectType, Type[] extraTypes, object obj)
        {
            return SerializeObjectToXml(objectType, extraTypes, obj, false);
        }

        public static string SerializeObjectToXml<T>(T obj)
        {
            return SerializeObjectToXml<T>(new Type[0], obj);
        }

        public static string SerializeObjectToXml<T>(Type[] extraTypes, T obj)
        {
            return SerializeObjectToXml(typeof(T), extraTypes, obj);
        }

        /// <summary>
        /// Create XML from specified object by serialization.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="extraTypes">The extra types.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="noneNamespaceAttributes">if set to <c>true</c> [none namespace attributes].</param>
        /// <returns>XML data.</returns>
        public static string SerializeObjectToXml(Type objectType, Type[] extraTypes, object obj,
            bool noneNamespaceAttributes)
        {
            var sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(objectType, extraTypes);

            TextWriter w = new StringWriter(sb);
            if (noneNamespaceAttributes)
            {
                var xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(w, obj, xmlSerializerNamespaces);
            }
            else
            {
                serializer.Serialize(w, obj);
            }

            w.Flush();
            w.Close();

            return sb.ToString();
        }

        public static bool IsFlagSet(Enum flagToTest, Enum testSource)
        {
            int iflagToTest = Convert.ToInt32(flagToTest);
            int itestSource = Convert.ToInt32(testSource);

            return ((itestSource & iflagToTest) == iflagToTest);
        }

        public static Enum ClearFlag(Enum flagToRemove, Enum testSource)
        {
            return SetFlagValue(flagToRemove, testSource, false);
        }

        public static Enum SetFlag(Enum flagToSet, Enum target)
        {
            return SetFlagValue(flagToSet, target, true);
        }

        public static Enum SetFlagValue(Enum flag, Enum target, bool set)
        {
            int iFlag = Convert.ToInt32(flag);
            int iTarget = Convert.ToInt32(target);

            int value = set ? iTarget | iFlag : iTarget & (iTarget ^ iFlag);

            return (Enum)Enum.Parse(target.GetType(), value.ToString());
        }

        public static bool IsNumber<T>(T obj)
        {
            Type objType = obj.GetType();

            return IsNumber(objType);
        }

        public static bool IsNumber(this Type objType)
        {
            if (objType.IsPrimitive)
            {
                if (objType == typeof(object) ||
                    objType == typeof(string) ||
                    objType == typeof(bool))
                    return false;

                return true;
            }

            return false;
        }

        public static string JoinCollection(ICollection collection, string separator)
        {
            return JoinCollection(collection, separator, null);
        }

        /// <summary>
        /// Joins the collection to string with separator.
        /// </summary>
        /// <param name="collection">collection</param>
        /// <param name="separator">separator</param>
        /// <param name="decorateItemWith">The decorate item with.</param>
        /// <returns>joined string with separator</returns>
        public static string JoinCollection(ICollection collection, string separator, string decorateItemWith)
        {
            if (collection != null)
            {
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(separator))
                {
                    foreach (object item in collection)
                    {
                        string itemStr = string.Format("{0}{1}{2}", decorateItemWith ?? string.Empty, item,
                                                       decorateItemWith ?? string.Empty);

                        sb.Append(itemStr);
                        sb.Append(separator);
                    }


                    int separatorLength = separator.Length;
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - separatorLength, separatorLength);
                    }
                }
                else
                {
                    foreach (object item in collection)
                    {
                        sb.Append(item.ToString());
                    }
                }

                return sb.ToString();
            }
            else
            {
                return null;
            }
        }

        public static string MakeRelativePath(string path1, string path2)
        {
            Uri uri1 = new Uri(path1);
            Uri uri2 = new Uri(path2);
            string relativePath = uri1.MakeRelativeUri(uri2).ToString();
            return relativePath.Replace("/", "\\");
        }

        public static string GenerateUniqueName(IEnumerable<string> existingNames, string baseName)
        {
            string newPropertyName = baseName + @"[0-9]+";
            int len = baseName.Length;
            var matchingEntities = existingNames.Where(e => Regex.Match(e, newPropertyName).Success);
            var maxIndex = (matchingEntities.Count() > 0)
                ? matchingEntities.Select(e => int.Parse(e.Substring(len))).Max()
                : 0;

            return string.Format("{0}{1}", baseName, maxIndex + 1);
        }
    }
}