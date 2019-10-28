using System;
using System.Collections;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
	/// <summary>
	/// Class that holds attributes(<see cref="XmlProxy"/> objects) for <see cref="XmlProxyAttribute"/> object.
	/// This class is not thread-safe.
	/// </summary>
	[Serializable]
	public class XmlProxyAttributeCollection: CollectionBase
	{
		#region constructors section

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlProxyAttributeCollection"/> class.
		/// </summary>
		public XmlProxyAttributeCollection()
		{}

		#endregion

		#region public indexers section

		/// <summary>
		/// Gets or sets <see cref="XmlProxyAttribute"/> on specified <c>Index</c>.
		/// </summary>
		public XmlProxyAttribute this[int Index]
		{
			get { return List[Index] as XmlProxyAttribute; }
			set { List[Index] = value; }
		}

		/// <summary>
		/// Gets or sets <see cref="XmlProxyAttribute"/> on specified <c>Key</c>.
		/// </summary>
		public XmlProxyAttribute this[string Key]
		{
			get
			{
				int idx = IndexOf(Key);
				if (idx > -1) return List[idx] as XmlProxyAttribute;

				return null;
			}

			set
			{
				int idx = IndexOf(Key);
				if (idx > -1) List[idx] = value;
			}
		}

		#endregion

		#region public methods section

		/// <summary>
		/// Add packet item attribute.
		/// </summary>
		/// <param name="Key">Attribute key name.</param>
		/// <param name="Value">Attribute value.</param>
		/// <returns>Added <see cref="XmlProxyAttribute"/> object.</returns>
		public XmlProxyAttribute Add(string Key, string Value)
		{
			XmlProxyAttribute attr = new XmlProxyAttribute(Key, Value);
			List.Add(attr);
			return attr;
		}

		/// <summary>
		/// Add packet item attribute.
		/// </summary>
		/// <param name="attribute"><see cref="XmlProxyAttribute"/> object to add.</param>
		/// <returns>Index of added object in collection.</returns>
		public int Add(XmlProxyAttribute attribute)
		{
			return List.Add(attribute);
		}

		/// <summary>
		/// Remove packet item attribute from collection.
		/// </summary>
		/// <param name="attribute"><see cref="XmlProxyAttribute"/> object to remove.</param>
		public void Remove(XmlProxyAttribute attribute)
		{
			List.Remove(attribute);
		}

		/// <summary>
		/// Test if specified attribute exists in collection.
		/// </summary>
		/// <param name="Key">Attribute key name.</param>
		/// <returns>Returns true if collection contains attribute specified by <c>Key</c>.</returns>
		public bool ContainsKey(string Key)
		{
			return (IndexOf(Key) > -1);
		}

		/// <summary>
		/// Gets index of attribute.
		/// </summary>
		/// <param name="Key">Attribute key name.</param>
		/// <returns>Returns index of attribute specified by <c>Key</c> if attribute if found, or -1 if not found.</returns>
		public int IndexOf(string Key)
		{
			for(int i = 0; i < List.Count; i++)
			{
				XmlProxyAttribute attr = List[i] as XmlProxyAttribute;
				if (string.Compare(attr.Key, Key, true) == 0) return i;
			}

			return -1;
		}

		/// <summary>
		/// Clones collection.
		/// </summary>
		/// <returns>Cloned <see cref="XmlProxyAttributeCollection"/> collection object.</returns>
		public XmlProxyAttributeCollection Clone()
		{
			XmlProxyAttributeCollection coll = new XmlProxyAttributeCollection();
			foreach (XmlProxyAttribute attribute in List)
			{
				coll.Add(attribute.Key, attribute.Value);
			}

			return coll;
		}

		/// <summary>
		/// Get value of attribute.
		/// </summary>
		/// <param name="Key">Attribute key name.</param>
		/// <returns>Returns value of attribute specified by <c>Key</c>.</returns>
		public string GetValue(string Key)
		{
			XmlProxyAttribute attr = this[Key];
			if ((attr != null) && (attr.Value != null)) return attr.Value;
			return string.Empty;
		}

		/// <summary>
		/// Set attribute value.
		/// </summary>
		/// <param name="Key">Attribute key name.</param>
		/// <param name="Value">Value for attribute.</param>
		public void SetValue(string Key, string Value)
		{			
			XmlProxyAttribute attr = this[Key];
			if (attr != null)
				attr.Value = Value;
		}

		#endregion
	}
}