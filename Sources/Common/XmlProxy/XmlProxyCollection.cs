using System;
using System.Collections.Generic;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
	/// <summary>
	/// PacketItemCollection class
	/// </summary>
	[Serializable]
	public class XmlProxyCollection: List<XmlProxy> //: CollectionBase
	{
		#region private fields section

		private XmlProxy mOwnerItem;

		#endregion

		#region internal methods section

		internal void UpdateOwnerItem(XmlProxy ownerItem)
		{
			this.mOwnerItem = ownerItem;
		}

		#endregion

		#region constructors section

		/// <summary>
		/// Initializes a new instance of the <see cref="XmlProxyCollection"/> class.
		/// </summary>
		/// <param name="ownerItem"></param>
		public XmlProxyCollection(XmlProxy ownerItem)
		{
			UpdateOwnerItem(ownerItem);
		}

		#endregion

		#region public indexers section

		/// <summary>
		/// Gets or sets <see cref="XmlProxy"/> specified by <c>ElementName</c>.
		/// </summary>
		public XmlProxy this[string ElementName]
		{
			get
			{
				int idx = IndexOf(ElementName);
				if (idx > -1)
					return this[idx];

				return null;
			}

			set
			{
				int idx = IndexOf(ElementName);
				if (idx > -1) { this[idx] = value; }
			}
		}

//		/// <summary>
//		/// Gets or sets <see cref="XmlProxy"/> specified by <c>Index</c>.
//		/// </summary>
//		public XmlProxy this[int Index]
//		{
//			get
//			{
//				return List[Index] as XmlProxy;
//			}
//
//			set
//			{
//				List[Index] = value;
//			}
//		}

		#endregion

		#region public methods section

//		/// <summary>
//		/// Add <see cref="XmlProxy"/> object to collection.
//		/// </summary>
//		/// <param name="item"><see cref="XmlProxy"/> object to add.</param>
//		/// <returns>Index of added item in collection.</returns>
//		public int Add(XmlProxy item)
//		{
//			return List.Add(item);
//		}

		/// <summary>
		/// Clear collection.
		/// </summary>
		/// <param name="ClearChilds">True to recursive clear all childs and subchilds.</param>
		public void Clear(bool ClearChilds)
		{
			if (ClearChilds)
			{
				foreach(XmlProxy child in this)
				{
					child.Childs.Clear(true);
				}
			}

			base.Clear();
		}

		/// <summary>
		/// Add packet item to collection.
		/// </summary>
		/// <param name="elementName">Element name.</param>
		/// <param name="elementValue">Element value.</param>
		/// <returns>Added <see cref="XmlProxy"/> object.</returns>
		public XmlProxy Add(string elementName, string elementValue)
		{
			XmlProxy item = new XmlProxy(elementName, elementValue);
			this.Add(item);
			return item;
		}

		/// <summary>
		/// Add packet item to collection.
		/// </summary>
		/// <param name="elementName">Element name.</param>
		/// <returns>Added <see cref="XmlProxy"/> object.</returns>
		public XmlProxy Add(string elementName)
		{
			return Add(elementName, null);
		}

//		/// <summary>
//		/// Remove <see cref="XmlProxy"/> object from collection.
//		/// </summary>
//		/// <param name="item"><see cref="XmlProxy"/> object to remove.</param>
//		public void Remove(XmlProxy item)
//		{
//			List.Remove(item);
//		}

		/// <summary>
		/// Remove packet item specified by <c>ElementName</c>.
		/// </summary>
		/// <param name="elementName">Element name of item to remove from collection.</param>
		public void Remove(string elementName)
		{
			int idx = IndexOf(elementName);
			if (idx > -1)
			{
				RemoveAt(idx);
			}
		}

		/// <summary>
		/// Gets index of packet item specified by <c>ElementName</c>.
		/// </summary>
		/// <param name="elementName">Element name.</param>
		/// <returns>Returns index of item in collection.</returns>
		public int IndexOf(string elementName)
		{
			for(int i = 0; i < this.Count; i++)
			{
                XmlProxy item = this[i] as XmlProxy;
				if (item.ElementName.Equals(elementName))
					return i;
			}

			return -1;
		}

//		/// <summary>
//		/// Copies collection to <see cref="XmlProxy"/> array.
//		/// </summary>
//		/// <returns>Returns <see cref="XmlProxy"/> array.</returns>
//		public XmlProxy[] CopyToArray()
//		{
//			XmlProxy[] res = new XmlProxy[Count];
//            this.CopyTo(res, 0);
//
//			return res;
//		}

		#endregion
	}
}