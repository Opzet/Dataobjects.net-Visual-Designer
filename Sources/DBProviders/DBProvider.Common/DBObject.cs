using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class DBObject : INamedObject
    {
        #region fields

        private string name;

        #endregion fields

        #region properties

        [XmlAttribute("name")]
        public string Name
        {
            get { return name; }
            set
            {
                if (!Util.StringEqual(name, value, true))
                {
                    name = value;
                    BuildUrn();
                }
            }
        }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlIgnore]
        public string Urn
        {
            get { return this._internalurn; }
        }

        //[XmlAttribute("urn")]
        [XmlIgnore]
        [Browsable(false)]
        public string _internalurn { get; set; }

        [XmlIgnore]
        public abstract DbItemType ItemType { get; }

        #endregion

        #region constructors

        protected DBObject(string name)
        {
            this.name = name;
        }

        protected DBObject(): this(string.Empty)
        {
        }

        #endregion

        #region methods

        protected virtual void BuildToString(StringBuilder sb)
        {
            sb.AppendFormat("[{0}] Name: '{1}'", this.ItemType, this.Name);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            BuildToString(sb);
            return sb.ToString();
        }

        public string SerializeToXml()
        {
            return Util.SerializeObjectToXml(this.GetType(), new Type[0], this);
        }

        public static T DeserializeFromXml<T>(string xml) where T: DBObject
        {
            return Util.DeserializeObjectFromXml(typeof (T), new Type[0], xml) as T;
        }

        protected void BuildUrn()
        {
            StringBuilder sb = new StringBuilder();
            InternalBuildUrn(sb);
            this._internalurn = sb.ToString();
        }

        protected internal virtual void InternalBuildUrn(StringBuilder sb)
        {
            sb.Insert(0, string.Format("/{0}[@Name='{1}']", this.ItemType, this.Name));
        }

        protected internal virtual object GetOwner()
        {
            return null;
        }

        public DBObject ResolveChildObject(string childObjectUrn)
        {
            DBObjectUrn dbObjectUrn = DBObjectUrn.Parse(childObjectUrn);
            return InternalResolveChildObject(dbObjectUrn);
        }

        protected internal abstract DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn);

        public bool EqualsByUrn(DBObject other)
        {
            return Util.StringEqual(this.Urn, other.Urn, true);
        }

        #endregion
    }

    #region DBObject<TOwner>

    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public abstract class DBObject<TOwner>: DBObject where TOwner: DBObject
    {
        #region properties

        [XmlIgnore]
        [Browsable(false)]
        public TOwner Owner { get; set; }

        #endregion

        #region constructors

        protected DBObject(string name, TOwner owner)
            : base(name)
        {
            this.Owner = owner;
            BuildUrn();
        }

        protected DBObject()
        {
        }

        #endregion

        #region methods
        
        protected internal override void InternalBuildUrn(StringBuilder sb)
        {
            base.InternalBuildUrn(sb);

            if (this.Owner != null)
            {
                this.Owner.InternalBuildUrn(sb);
            }
        }

        protected internal override object GetOwner()
        {
            return this.Owner;
        }

        protected internal override abstract DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn);

        #endregion methods
    }

    #endregion

    #region enum DbItemType

    public enum DbItemType
    {
        Server,
        Database,
        Schema,
        Table,
        Column,
        Index,
        ForeignKey,
        ForeignKeyColumn,
        IndexedColumn,
        Reserved1,
        TemporaryObject,
        View
    }

    #endregion

    #region class DBObjectUrn

    public class DBObjectUrn
    {
        private readonly Dictionary<DbItemType, string> parts = new Dictionary<DbItemType, string>();
        private readonly static Type enumType = typeof(DbItemType);

        public string TableSchema { get; private set; }

        public string this[DbItemType itemType]
        {
            get { return parts[itemType]; }
        }

        public DBObjectUrn()
        {
            TableSchema = string.Empty;

            foreach (DbItemType dbItemType in EnumType<DbItemType>.Values)
            {
                parts.Add(dbItemType, string.Empty);
            }
        }

        public static DBObjectUrn Parse(string urn)
        {
            string[] items = urn.Split('/');
            DBObjectUrn result = new DBObjectUrn();
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                string[] subitems1 = item.Split('[');
                DbItemType itemType = (DbItemType)Enum.Parse(enumType, subitems1[0], true);

                string namePart = subitems1[1];
                int startIdx = namePart.IndexOf("'");
                int endIdx = namePart.IndexOf("'", startIdx + 1);

                string itemName = namePart.Substring(startIdx+1, endIdx - startIdx - 1);

                if (itemType == DbItemType.Table /*|| itemType == View*/)
                {
                    if (namePart.ToLower().Contains("@schema")) ;
                    {
                        startIdx = namePart.IndexOf("'", endIdx + 1);
                        if (startIdx > -1)
                        {
                            endIdx = namePart.IndexOf("'", startIdx + 1);

                            if (endIdx > -1)
                            {
                                result.TableSchema = namePart.Substring(startIdx + 1, endIdx - startIdx - 1);
                            }
                        }
                    }
                }

                result.parts[itemType] = itemName;
            }

            return result;
        }
    }

    #endregion class DBObjectUrn
}