using System;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
	/// <summary>
	/// PacketItemAttribute class
	/// </summary>
	[Serializable]
	public class XmlProxyAttribute
	{
		#region private fields section

		private string mKey;
		private string mValue;

		#endregion

		#region constructors section

		internal XmlProxyAttribute(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		#endregion

		#region public properties section

		/// <summary>
		/// Gets or sets attribute key name
		/// </summary>
		public string Key
		{
			get { return mKey; }
			set { mKey = value; }
		}

		/// <summary>
		/// Gets or sets attribute value
		/// </summary>
		public string Value
		{
			get { return mValue; }
			set { mValue = value; }
		}

		#endregion
	}
}