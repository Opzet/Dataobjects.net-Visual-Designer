using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [Serializable]
    [XmlType("Column")]
    public sealed class Column : DBObject<TableBase>
    {
        #region fields

        private object underlyingSqlDataType;
        private Type crlDataType;

        #endregion

        #region properties

        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("isIdentity")]
        public bool IsIdentity { get; set; }

        [XmlAttribute("identityIncrement")]
        public long IdentityIncrement { get; set; }

        [XmlAttribute("identitySeed")]
        public long IdentitySeed { get; set; }

        [XmlAttribute("notForReplication")]
        public bool NotForReplication { get; set; }

        [XmlAttribute("isNullable")]
        public bool IsNullable { get; set; }

        [XmlAttribute("rowGuidCol")]
        public bool RowGuidCol { get; set; }

        [XmlElement]
        public object DefaultValue { get; set; }

        [XmlAttribute("maxLength")]
        public int MaxLength { get; set; }

        [XmlAttribute("precision")]
        public int Precision { get; set; }

        [XmlAttribute("scale")]
        public int Scale { get; set; }

//        [XmlIgnore]
//        [Browsable(false)]
//        public object UnderlyingSqlDataType
//        {
//            get
//            {
//                if (underlyingSqlDataType == null && !string.IsNullOrEmpty(UnderlyingSqlDataTypeFullName))
//                {
//                    Type type = Type.GetType(UnderlyingSqlDataTypeFullName, false);
//                    if (type != null)
//                    {
//                        underlyingSqlDataType = Activator.CreateInstance(type);
//                    } 
//                }
//
//                return underlyingSqlDataType;
//            }
//            set
//            {
//                underlyingSqlDataType = value;
//                if (value != null)
//                {
//                    UnderlyingSqlDataTypeFullName = value.GetType().AssemblyQualifiedName;
//                }
//            }
//        }

        [XmlAttribute("sqlDataType")]
        public string SqlDataType { get; set; }

        [XmlIgnore]
        [Browsable(false)]
        public Type ClrDataType
        {
            get
            {
                if (crlDataType == null && !string.IsNullOrEmpty(CrlDataTypeFullName))
                {
                    crlDataType = Type.GetType(CrlDataTypeFullName, false);
                }
                return crlDataType;
            }
            set
            {
                crlDataType = value;
                CrlDataTypeFullName = value.AssemblyQualifiedName;
            }
        }

        [XmlAttribute("crlDataType")]
        public string CrlDataTypeFullName { get; set; }

        [XmlIgnore]
        public override DbItemType ItemType
        {
            get { return DbItemType.Column; }
        }

        [XmlAttribute("isMaxLength")]
        public bool IsMaxLength { get; set; }

        #endregion

        #region constructors

        public Column(string name, TableBase owner) : base(name, owner) { }

        public Column()
        {
        }

        #endregion

        #region methods

        protected override void BuildToString(StringBuilder sb)
        {
            base.BuildToString(sb);

            if (IsIdentity)
            {
                sb.Append(", IsIdentity");
            }

            if (IsNullable)
            {
                sb.Append(", IsNullable");
            }

            if (RowGuidCol)
            {
                sb.Append(", IsNullawRowGuidColble");
            }

            if (SqlDataType != null)
            {
                sb.AppendFormat(", SqlDataType: '{0}'", this.SqlDataType);
            }

            sb.AppendFormat(", CrlDataType: '{0}'", ClrDataType.Name);
        }

        protected internal override DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn)
        {
            DBObject result = null;

            if (Util.StringEqual(childObjectUrn[ItemType], this.Name, true))
            {
                result = this;
            }

            return result;
        }

        #endregion
    }

    #region class ColumnCollection

    public sealed class ColumnCollection : ObjectCollection<Column>
    {
        public ColumnCollection(IList<Column> list) : base(list) { }

        public ColumnCollection()
        {}
    }

    #endregion
}