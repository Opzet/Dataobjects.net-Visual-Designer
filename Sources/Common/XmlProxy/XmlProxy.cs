using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    /// <summary>
    /// Class that holds all data from parsed xml. PacketItem has tree structure.
    /// </summary>
	[Serializable]
	public class XmlProxy: ISerializable
	{
		#region ConvertHelper class

        /// <summary>
        /// Convert helper class, to convert from/to string.
        /// </summary>
		internal class ConvertHelper
		{
			internal static string ObjectToString(object obj)
			{
				if (obj == null) return string.Empty;
				
				Type t = obj.GetType();
				if (t == typeof(bool)) return ToString((bool)obj);
			    if (t == typeof(int)) return ToString((int)obj);
			    if (t == typeof(long)) return ToString((long)obj);
			    return t.IsEnum ? ToString(obj as Enum) : obj.ToString();
			}

			internal static string ToString(bool Value)
			{
                return Convert.ToInt32(Value).ToString();
			}

			internal static string ToString(int Value)
			{
                return Value.ToString();
			}

            internal static string ToString(decimal Value)
            {
                return Value.ToString(CultureInfo.InvariantCulture);
            }

			internal static string ToString(long Value)
			{
				return Value.ToString();
			}

			internal static string ToString(Enum Value)
			{
				return Convert.ToInt32(Value).ToString();
			}

			internal static string FromString(string Value, string Default)
			{
			    return string.IsNullOrEmpty(Value) ? Default : Value;
			}

		    internal static bool FromString(string Value, bool Default)
		    {
                return string.IsNullOrEmpty(Value) ? Default : Convert.ToBoolean(Convert.ToInt32(Value));
		    }

		    internal static int FromString(string Value, int Default)
		    {
		        return string.IsNullOrEmpty(Value) ? Default : Convert.ToInt32(Value);
		    }

		    internal static byte FromString(string Value, byte Default)
		    {
		        return string.IsNullOrEmpty(Value) ? Default : Convert.ToByte(Value);
		    }

		    internal static decimal FromString(string Value, decimal Default)
		    {
		        return string.IsNullOrEmpty(Value) ? Default : decimal.Parse(Value, CultureInfo.InvariantCulture);
		    }

//		    internal static MessageType FromString(string Value, MessageType Default)
//			{
//                if (string.IsNullOrEmpty(Value))
//				{
//				    return Default;
//				}
//                return Convert.ToInt32(Value);
//			}

			internal static long FromString(string Value, long Default)
			{
			    return string.IsNullOrEmpty(Value) ? Default : long.Parse(Value);
			}
        }

        #endregion ConvertHelper class

        #region private fields section

        private string mElementName;
		private XmlProxyAttributeCollection mAttributes;
		private string mElementValue;
		private XmlProxyCollection mChilds;
		private object mTag;

        #endregion private fields section

        #region protected methods section

        /// <summary>
        /// Deserialize
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
		protected virtual void Deserialize(SerializationInfo info, StreamingContext context)
		{
			try
			{
				// Element name
				mElementName = info.GetString("ElementName");
                
				mElementValue = info.GetString("ElementValue");
				/*
				// Element value
				string elementValueType = info.GetString("ElementValueType");
				if (elementValueType != string.Empty)
				{
					Type t = Type.GetType(elementValueType);
					object o = info.GetValue("ElementValue", t);
					this.mElementValue = o;
				}
				else
				{
					this.mElementValue = null;
				}
				*/

				// Attributes collection
				mAttributes = new XmlProxyAttributeCollection();
				int attrsCount = info.GetInt32("AttributesCount");
				for(int i = 0; i < attrsCount; i++)
				{
					string si = i.ToString();
					string key = info.GetString("_att_k"+si);
					string val = info.GetString("_att_v"+si);
					mAttributes.Add(key, val);

					/*
					string attrvalueType = info.GetString("_att_vt"+si);
					if (attrvalueType != string.Empty)
					{
						Type t = Type.GetType(attrvalueType);
						object o = info.GetValue("_att_v"+si, t);
						this.mAttributes.Add(key, o);
					}
					else
					{
						this.mAttributes.Add(key, null);
					}
					*/
				}
				
				// Childs collection
				mChilds = new XmlProxyCollection(this);
				mChilds.UpdateOwnerItem(this);
				int childCount = info.GetInt32("ChildsCount");
				for(int i = 0; i < childCount; i++)
				{
					XmlProxy child = info.GetValue("_ch"+i, typeof(XmlProxy)) as XmlProxy;
					mChilds.Add(child);
				}
			}
			catch ( SerializationException e )
			{
				Debug.WriteLine("...Deserialization of DataTreeItem failed: " + e);
			}
        }

        #endregion protected methods section

        #region constructors section

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlProxy"/> class.
        /// </summary>
		public XmlProxy()
		{
			mAttributes = new XmlProxyAttributeCollection();
			mElementValue = string.Empty;
			mChilds = new XmlProxyCollection(this);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlProxy"/> class.
        /// </summary>
        /// <param name="ElementName">Element name.</param>
		public XmlProxy(string ElementName): this()
		{
			mElementName = ElementName;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlProxy"/> class.
        /// </summary>
        /// <param name="ElementName">Element name.</param>
        /// <param name="ElementValue">Element value.</param>
		public XmlProxy(string ElementName, string ElementValue): this(ElementName)
		{
			mElementValue = ElementValue;
		}

        /// <summary>
        /// Deserialization of packet item
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected XmlProxy(SerializationInfo info, StreamingContext context)
		{
            Deserialize(info, context);
        }

        #endregion constructors section

        #region public indexers section

        /// <summary>
        /// This is same as <c>Childs[ChildElementName]</c>. This is the shortest way.
        /// </summary>
        /// <value>
        /// instance of <see cref="XmlProxy"/> if childs contains item with specific ElementName, <c>null</c> otherwise.
        /// </value>
		public XmlProxy this[string ChildElementName]
		{
			get
			{
				return Childs[ChildElementName];
			}
		}

        /// <summary>
        /// This is same as <c>Childs[Index]</c>. This is the shortest way.
        /// </summary>
        /// <value>
        /// instance of <see cref="XmlProxy"/> if childs contains item on specific Index, <c>null</c> otherwise.
        /// </value>
		public XmlProxy this[int Index]
		{
			get
			{
				return Childs[Index];
			}
        }

        #endregion public indexers section

        #region public methods section

        /// <summary>
        /// Serialization of DataTreeItem
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">
        /// The caller does not have the required permission.
        /// </exception>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			try
			{
				info.AddValue("ElementName", ElementName);
				if (ElementValue != null)
				{ 
					info.AddValue("ElementValue", ElementValue);
				}
				else
				{
					info.AddValue("ElementValue", string.Empty);
				}

				info.AddValue("AttributesCount", mAttributes.Count);

				//int si=0;
				for(int si = 0; si < mAttributes.Count; si++)
				{
					XmlProxyAttribute attr = mAttributes[si];

					info.AddValue("_att_k"+si, attr.Key);
					info.AddValue("_att_v"+si, attr.Value);
				}
				/*
				foreach(string key in mAttributes.Keys)
				{
					string val = mAttributes.GetValue(Key);
					info.AddValue("_att_k"+si, key);
					if (val != null) info.AddValue("_att_v"+si, val);
					else info.AddValue("_att_v"+si, string.Empty);
					si++;
				}
				*/

				info.AddValue("ChildsCount", Childs.Count);
				for(int i = 0; i < Childs.Count; i++)
				{
					info.AddValue("_ch"+i, Childs[i]);
				}
			}
			catch ( SerializationException e )
			{
				Debug.WriteLine("...Serialization of DataTreeItem failed: " + e);
			}
		}

		private XmlProxy FindChild(Queue elementNames)
		{
			if ((elementNames == null) || (elementNames.Count == 0)) return null;

			string elementName = elementNames.Dequeue() as string;
			foreach (XmlProxy child in mChilds)
			{
				if (child.ElementName.Equals(elementName))
				{
				    return elementNames.Count == 0 ? child : FindChild(elementNames);
				}
			}

			return null;
		}

        /// <summary>
        /// Try to select attribute using XPath. It also returns attribute value if attribute was found.
        /// </summary>
        /// <param name="elementsXPath">Path of element where to find attribute.
        /// Elements and child elements are separated by / .
        /// Example: if <c>ElementsXPath</c> == <c>Message/Data/SearchRoom</c>
        /// and <c>AttributeName</c> == <c>ParentAddress</c> then it will search for this packet construction:
        /// <code><Message><Data><SearchRoom ParentAddress="..."></SearchRoom></Data></Message></code>.</param>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="attributeValue">Attribute value</param>
        /// <returns>True if attribute was found</returns>
		public bool SelectAttribute(string elementsXPath, string attributeName, out string attributeValue)
		{
			attributeValue = null;
			string[] elements = elementsXPath.Split('/');
			if ((elements.Length == 0)) return false;

			if (elements[0] != ElementName) return false;
			if (elements.Length == 1)
			{
				bool res = Attributes.ContainsKey(attributeName);
				if (res)
				{
				    attributeValue = GetAttr(attributeName);
				}
				return res;
			}

			Queue elementList = new Queue();
			for(int i = 1; i < elements.Length; i++)
			{
				elementList.Enqueue(elements[i]);
			}

			XmlProxy founded = FindChild(elementList);
			if (founded != null)
			{
				bool res = founded.Attributes.ContainsKey(attributeName);
				if (res)
				{
				    attributeValue = founded.GetAttr(attributeName);
				}
				return res;                
			}

			return false;
		}

        /// <summary>
        /// Try to find attribute using XPath. Same as <see cref="SelectAttribute"/> but doesnt return attribute value,
        /// only test if attribute exists.
        /// </summary>
        /// <param name="elementsXPath">Path of element where to find attribute.
        /// Elements and child elements are separated by / .
        /// Example: if <c>ElementsXPath</c>== <c>Message/Data/SearchRoom</c>
        /// and <c>AttributeName</c>== <c>ParentAddress</c> then it will search for this packet construction:
        /// <c><Message><Data><SearchRoom ParentAddress="..."></SearchRoom></Data></Message></c>.</param>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>True if attribute was found</returns>
		public bool FindAttribute(string elementsXPath, string attributeName)
		{
			string attrValue;
			bool result = SelectAttribute(elementsXPath, attributeName, out attrValue);
			return result;
		}

        /// <summary>
        /// Find element using XPath
        /// </summary>
        /// <param name="elementsXPath">Elements XPath, like: <c>Message/MessageType</c>.</param>
        /// <param name="elementName">Element name.</param>
        /// <returns>
        /// If found, then return <see cref="XmlProxy"/>, otherwise returns <c>null</c>.
        /// </returns>
		public XmlProxy FindElement(string elementsXPath, string elementName)
		{
			string[] elements = elementsXPath.Split('/');
			if ((elements.Length == 0))
			{
			    return null;
			}

			if (elements[0] != ElementName)
			{
			    return null;
			}

			if (elements.Length == 1)
			{
			    return this;
			}

			Queue elementList = new Queue();
			for(int i = 1; i < elements.Length; i++)
			{
				elementList.Enqueue(elements[i]);
			}

			return FindChild(elementList);
		}

        /// <summary>
        /// Find element using XPath.
        /// </summary>
        /// <param name="elementsXPath">Elements XPath, like: <c>Message/MessageType</c>.</param>
        /// <param name="elementName">Element name.</param>
        /// <returns>
        /// If found, then return <c>true</c>, otherwise returns <c>false</c>.
        /// </returns>
		public bool FindElementExists(string elementsXPath, string elementName)
		{
			return (FindElement(elementsXPath, elementName) != null);
		}

		#region Add attributes section

        /// <summary>
        /// Add attribute.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Value">Attribute value.</param>
		public void AddAttribute(string Key, bool Value)
		{
			Attributes.Add(Key, ConvertHelper.ToString(Value));
		}

		/// <summary>
		/// Add attribute.
		/// </summary>
		/// <param name="Key">Attribute key.</param>
		/// <param name="Value">Attribute value.</param>
		public void AddAttribute(string Key, int Value)
		{
			Attributes.Add(Key, ConvertHelper.ToString(Value));
		}

        /// <summary>
        /// Add attribute.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Value">Attribute value.</param>
        public void AddAttribute(string Key, decimal Value)
        {
            Attributes.Add(Key, ConvertHelper.ToString(Value));
        }

		/// <summary>
		/// Add attribute.
		/// </summary>
		/// <param name="Key">Attribute key.</param>
		/// <param name="Value">Attribute value.</param>
		public void AddAttribute(string Key, long Value)
		{
			Attributes.Add(Key, ConvertHelper.ToString(Value));
		}

		/// <summary>
		/// Add attribute.
		/// </summary>
		/// <param name="Key">Attribute key.</param>
		/// <param name="Value">Attribute value.</param>
		public void AddAttribute(string Key, Enum Value)
		{
			Attributes.Add(Key, ConvertHelper.ToString(Value));
		}

		/// <summary>
		/// Add attribute.
		/// </summary>
		/// <param name="Key">Attribute key.</param>
		/// <param name="Value">Attribute value.</param>
		public void AddAttribute(string Key, string Value)
		{
			Attributes.Add(Key, Value);
		}

		/// <summary>
		/// Add attribute.
		/// </summary>
		/// <param name="Key">Attribute key.</param>
		/// <param name="Value">Attribute value.</param>
		public void AddAttribute(string Key, Guid Value)
		{
			Attributes.Add(Key, Value.ToString());
        }

        #endregion Add attributes section

        #region Get attribute value section

        /// <summary>
        /// Get attribute value by <c>Key</c>
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <returns>Returns value of attribute.</returns>
		public string GetAttr(string Key)
		{
		    return mAttributes.ContainsKey(Key) ? mAttributes.GetValue(Key) : string.Empty;
		}

        /// <summary>
        /// Get attribute value by <c>Key</c>
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <returns>Returns value of attribute.</returns>
		public Guid GetAttrG(string Key)
		{
			if (mAttributes.ContainsKey(Key))
			{
				string val = mAttributes.GetValue(Key);
				return string.IsNullOrEmpty(val) ? Guid.Empty : new Guid(val);
			}
			return Guid.Empty;
		}

//        /// <summary>
//        /// Returns attribute message type value.
//        /// </summary>
//        /// <param name="Key">Attribute key.</param>
//        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
//        /// <returns>
//        /// 	<see cref="MessageType"/> value of attribute.
//        /// </returns>
//		public MessageType GetAttrMessageType(string Key, MessageType Default)
//		{
//			if (mAttributes.ContainsKey(Key))
//			{
//				string val = mAttributes.GetValue(Key);
//				int iVal;
//				try { iVal = Convert.ToInt32(val); } 
//				catch { iVal = 0; }
//				return iVal;
//			}
//			return Default;
//		}

        /// <summary>
        /// Get attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
        /// <returns>
        /// 	<see cref="Boolean"/> value of attribute.
        /// </returns>
		public bool GetAttr(string Key, bool Default)
		{
		    if (mAttributes.ContainsKey(Key))
			{
				string val = mAttributes.GetValue(Key);
				return ConvertHelper.FromString(val, Default);
			}
		    return Default;
		}

        /// <summary>
        /// Get attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
        /// <returns>
        /// 	<see cref="Boolean"/> value of attribute.
        /// </returns>
		public string GetAttr(string Key, string Default)
        {
            if (mAttributes.ContainsKey(Key))
			{
				string val = mAttributes.GetValue(Key);
				return ConvertHelper.FromString(val, Default);
			}
            return Default;
        }

        /// <summary>
        /// Get attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
        /// <returns><see cref="long"/> value of attribute.</returns>
		public long GetAttrL(string Key, long Default)
	    {
	        if (mAttributes.ContainsKey(Key))
			{
				string val = mAttributes.GetValue(Key);
				return ConvertHelper.FromString(val, Default);
			}
	        return Default;
	    }

        /// <summary>
        /// Get attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
        /// <returns>
        /// 	<see cref="System.Int32"/> value of attribute.
        /// </returns>
		public int GetAttr(string Key, int Default)
	    {
	        if (mAttributes.ContainsKey(Key))
			{
				string val = mAttributes.GetValue(Key);
				return ConvertHelper.FromString(val, Default);
			}
	        return Default;
	    }

        /// <summary>
        /// Get attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
        /// <returns>
        /// 	<see cref="System.Decimal"/> value of attribute.
        /// </returns>
		public decimal GetAttr(string Key, decimal Default)
	    {
	        if (mAttributes.ContainsKey(Key))
			{
				string val = mAttributes.GetValue(Key);
				return ConvertHelper.FromString(val, Default);
			}
	        return Default;
        }

        #endregion Get attribute value section

        #region Set attribute value section

        /// <summary>
        /// Set attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Value">string value.</param>
		public void SetAttr(string Key, string Value)
		{
			if (!mAttributes.ContainsKey(Key))
			{
				AddAttribute(Key, Value);
			}
            else
			{
			    mAttributes.SetValue(Key, Value);
			}
		}

        /// <summary>
        /// Set attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Value">string value.</param>
		public void SetAttr(string Key, Guid Value)
		{
			if (!mAttributes.ContainsKey(Key))
			{
                AddAttribute(Key, Value);
			}
            else
			{
                mAttributes.SetValue(Key, Value.ToString());
			}
		}

        /// <summary>
        /// Set attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Value"><see cref="Boolean"/> value.</param>
		public void SetAttr(string Key, bool Value)
		{
			if (!mAttributes.ContainsKey(Key))
			{
			    AddAttribute(Key, Value);
			}
		    else
			{
                mAttributes.SetValue(Key, ConvertHelper.ToString(Value));
			}
		}

        /// <summary>
        /// Set attribute <see cref="long"/> value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Value"><see cref="long"/> value.</param>
		public void SetAttrL(string Key, long Value)
		{
			if (!mAttributes.ContainsKey(Key))
			{
                AddAttribute(Key, Value);
			}
            else
			{
                mAttributes.SetValue(Key, ConvertHelper.ToString(Value));
			}
		}

        /// <summary>
        /// Set attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Value"><see cref="System.Int32"/> value.</param>
		public void SetAttr(string Key, int Value)
		{
			if (!mAttributes.ContainsKey(Key))
			{
                AddAttribute(Key, Value);
			}
            else
			{
                mAttributes.SetValue(Key, ConvertHelper.ToString(Value));
			}
		}

        /// <summary>
        /// Set attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Value"><see cref="System.Enum"/> value.</param>
		public void SetAttr(string Key, Enum Value)
		{
			if (!mAttributes.ContainsKey(Key))
			{
                AddAttribute(Key, Value);
			}
            else
			{
                mAttributes.SetValue(Key, ConvertHelper.ToString(Value));
			}
		}

        /// <summary>
        /// Set attribute value.
        /// </summary>
        /// <param name="Key">Attribute key.</param>
        /// <param name="Value"><see cref="System.Decimal"/> value.</param>
        public void SetAttr(string Key, Decimal Value)
        {
            if (!mAttributes.ContainsKey(Key))
            {
                AddAttribute(Key, Value);
            }
            else
            {
                mAttributes.SetValue(Key, ConvertHelper.ToString(Value));
            }
        }

        #endregion Set attribute value section

        #region Add child section

        /// <summary>
        /// Add child.
        /// </summary>
        /// <param name="elementName">Element name.</param>
        /// <param name="elementValue">Element string value.</param>
        /// <returns>
        /// instance of <see cref="XmlProxy"/> if succesfully, otherwise <c>null</c>.
        /// </returns>
        public XmlProxy AddChild(string elementName, string elementValue)
        {
            return Childs.Add(elementName, elementValue);
        }

        /// <summary>
        /// Add child.
        /// </summary>
        /// <param name="elementName">Element name.</param>
        /// <param name="elementValue">Element <see cref="Guid"/> value.</param>
        /// <returns>
        /// instance of <see cref="XmlProxy"/> if succesfully, otherwise <c>null</c>.
        /// </returns>
        public XmlProxy AddChild(string elementName, Guid elementValue)
        {
            return Childs.Add(elementName, elementValue.ToString());
        }

        /// <summary>
        /// Add child.
        /// </summary>
        /// <param name="elementName">Element name.</param>
        /// <param name="elementValue">Element <see cref="Boolean"/> value.</param>
        /// <returns>
        /// instance of <see cref="XmlProxy"/> if succesfully, otherwise <c>null</c>.
        /// </returns>
		public XmlProxy AddChild(string elementName, bool elementValue)
		{
			return Childs.Add(elementName, ConvertHelper.ToString(elementValue));
		}

        /// <summary>
        /// Add child.
        /// </summary>
        /// <param name="elementName">Element name.</param>
        /// <param name="elementValue">Element <see cref="System.Int32"/> value.</param>
        /// <returns>
        /// instance of <see cref="XmlProxy"/> if succesfully, otherwise <c>null</c>.
        /// </returns>
		public XmlProxy AddChild(string elementName, int elementValue)
		{
			return Childs.Add(elementName, ConvertHelper.ToString(elementValue));
		}

        /// <summary>
        /// Add child.
        /// </summary>
        /// <param name="elementName">Element name.</param>
        /// <param name="elementValue">Element <see cref="long"/> value.</param>
        /// <returns>
        /// instance of <see cref="XmlProxy"/> if succesfully, otherwise <c>null</c>.
        /// </returns>
		public XmlProxy AddChild(string elementName, long elementValue)
		{
			return Childs.Add(elementName, ConvertHelper.ToString(elementValue));
		}

        /// <summary>
        /// Add child.
        /// </summary>
        /// <param name="elementName">Element name.</param>
        /// <param name="elementValue">Element <see cref="System.Enum"/> value.</param>
        /// <returns>
        /// instance of <see cref="XmlProxy"/> if succesfully, otherwise <c>null</c>.
        /// </returns>
		public XmlProxy AddChild(string elementName, Enum elementValue)
		{
			return Childs.Add(elementName, ConvertHelper.ToString(elementValue));
		}

        /// <summary>
        /// Add child.
        /// </summary>
        /// <param name="elementName">Element name.</param>
        /// <returns>
        /// instance of <see cref="XmlProxy"/> if succesfully, otherwise <c>null</c>.
        /// </returns>
		public XmlProxy AddChild(string elementName)
		{
			return Childs.Add(elementName);
		}

        /// <summary>
        /// Add child.
        /// </summary>
        /// <param name="childItem"><see cref="XmlProxy"/> object as a child.</param>
		public void AddChild(XmlProxy childItem)
		{
			Childs.Add(childItem);
        }

        #endregion Add child section

        #region Get child value section

        /// <summary>
        /// Get child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <returns>
        /// Returns child value as <see cref="System.String"/>
        /// </returns>
		public string GetChildValue(string ChildElementName)
		{
			XmlProxy item = mChilds[ChildElementName];
			if ((item != null) && (item.ElementValue != null)) return item.ElementValue;
			return string.Empty;
		}

        /// <summary>
        /// Get child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <returns>
        /// Returns child value as <see cref="System.Guid"/>
        /// </returns>
		public Guid GetChildValueGuid(string ChildElementName)
		{
			XmlProxy item = mChilds[ChildElementName];
			if ((item != null) && (item.ElementValue != null))
			{
				if (item.ElementValue.Length == 0) return Guid.Empty;
				return new Guid(item.ElementValue);
			}
			return Guid.Empty;
		}

        /// <summary>
        /// Get child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
        /// <returns>
        /// Returns child value as <see cref="System.Int32"/>
        /// </returns>
        public int GetChildValueInt(string ChildElementName, int Default)
        {
            string val = GetChildValue(ChildElementName);
            return ConvertHelper.FromString(val, Default);
        }

        /// <summary>
        /// Get child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
        /// <returns>
        /// Returns child value as <see cref="System.Byte"/>
        /// </returns>
        public byte GetChildValueByte(string ChildElementName, byte Default)
        {
            string val = GetChildValue(ChildElementName);
            return ConvertHelper.FromString(val, Default);
        }
//
//        /// <summary>
//        /// Get child value.
//        /// </summary>
//        /// <param name="ChildElementName">Child element name.</param>
//        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
//        /// <returns>
//        /// Returns child value as <see cref="MessageType"/>
//        /// </returns>
//		public MessageType GetChildValueMessageType(string ChildElementName, MessageType Default)
//		{
//			string val = GetChildValue(ChildElementName);
//			return ConvertHelper.FromString(val, Default);
//		}

        /// <summary>
        /// Get child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
        /// <returns>
        /// Returns child value as <see cref="System.Boolean"/>
        /// </returns>
		public bool GetChildValueBool(string ChildElementName, bool Default)
		{
			string val = GetChildValue(ChildElementName);
			return ConvertHelper.FromString(val, Default);
		}

        /// <summary>
        /// Get child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="Default">Default value, which is return, if specified attribute is not found.</param>
        /// <returns>
        /// Returns child value as <see cref="long"/>
        /// </returns>
		public long GetChildValueLong(string ChildElementName, long Default)
		{
			string val = GetChildValue(ChildElementName);
			return ConvertHelper.FromString(val, Default);
        }

        #endregion Get child value section

        #region Set child value section

        /// <summary>
        /// Set child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="ChildElementValue"><see cref="System.String"/> value.</param>
        public void SetChildValue(string ChildElementName, string ChildElementValue)
        {
            XmlProxy item = mChilds[ChildElementName];
            if (item != null) item.ElementValue = ChildElementValue;
        }

        /// <summary>
        /// Set child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="ChildElementValue"><see cref="Guid"/> value.</param>
        public void SetChildValue(string ChildElementName, Guid ChildElementValue)
        {
            XmlProxy item = mChilds[ChildElementName];
            if (item != null) item.ElementValue = ChildElementValue.ToString();
        }

        /// <summary>
        /// Set child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="ChildElementValue"><see cref="System.Int32"/> value.</param>
		public void SetChildValueInt(string ChildElementName, int ChildElementValue)
		{
			SetChildValue(ChildElementName, ConvertHelper.ToString(ChildElementValue));
		}

        /// <summary>
        /// Set child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="ChildElementValue"><see cref="System.Boolean"/> value.</param>
		public void SetChildValueBool(string ChildElementName, bool ChildElementValue)
		{
			SetChildValue(ChildElementName, ConvertHelper.ToString(ChildElementValue));
		}

        /// <summary>
        /// Set child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="ChildElementValue"><see cref="long"/> value.</param>
		public void SetChildValueLong(string ChildElementName, long ChildElementValue)
		{
			SetChildValue(ChildElementName, ConvertHelper.ToString(ChildElementValue));
		}

        /// <summary>
        /// Set child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="ChildElementValue"><see cref="System.Enum"/> value.</param>
		public void SetChildValueEnum(string ChildElementName, Enum ChildElementValue)
		{
			SetChildValue(ChildElementName, ConvertHelper.ToString(ChildElementValue));
		}

        /// <summary>
        /// Set child value.
        /// </summary>
        /// <param name="ChildElementName">Child element name.</param>
        /// <param name="ChildElementValue"><see cref="System.Guid"/> value.</param>
		public void SetChildValueEnum(string ChildElementName, Guid ChildElementValue)
		{
			SetChildValue(ChildElementName, ChildElementValue.ToString());
        }

        #endregion Set child value section


        /// <summary>
        /// Clones <see cref="XmlProxy"/> object
        /// </summary>
        /// <returns>Cloned <see cref="XmlProxy"/> object.</returns>
		public XmlProxy Clone()
		{
			XmlProxy cloned = new XmlProxy(ElementName, ElementValue);
			cloned.Tag = Tag;
			cloned.Attributes = Attributes.Clone();
			foreach(XmlProxy child in Childs)
			{
				cloned.AddChild(child.Clone());
			}

			return cloned;
        }

        #endregion public methods section

        #region public properties section

        /// <summary>
        /// Gets or sets ElementName
        /// </summary>
        /// <value>The name of the element.</value>
		public string ElementName
		{
			get { return mElementName; }
			set { mElementName = value; }
		}

        /// <summary>
        /// Gets or sets attributes collection.
        /// </summary>
        /// <value>The attributes.</value>
		public XmlProxyAttributeCollection Attributes
		{
			get { return mAttributes; }
			set { mAttributes = value; }
		}

        /// <summary>
        /// Gets or sets ElementValue
        /// </summary>
        /// <value>The element value.</value>
		public string ElementValue
		{
			get { return mElementValue; }
			set { mElementValue = value; }
		}

        /// <summary>
        /// Returns reference to childs <see cref="XmlProxyCollection"/>
        /// </summary>
        /// <value>The childs.</value>
        /// <remarks>This value is not serialized.</remarks>
		public XmlProxyCollection Childs
		{
			get { return mChilds; }
		}

        /// <summary>
        /// Gets or sets tag value.
        /// </summary>
        /// <value>The tag.</value>
		public object Tag
		{
			get { return mTag; }
			set { mTag = value; }
        }

        #endregion public properties section
    }
}