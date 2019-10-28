using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Linq;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.DBProvider
{
    [Serializable]
    public class Server: DBObject
    {
        #region properties

        [XmlArrayItem("Database")]
        public DatabaseCollection Databases { get; set; }

//        [XmlAttribute("isLocal")]
//        public bool IsLocal { get; set; }

        [XmlAttribute("isClustered")]
        public bool IsClustered { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlIgnore]
        public override DbItemType ItemType
        {
            get { return DbItemType.Server; }
        }

        #endregion

        #region constructors

        public Server(string name): base(name)
        {
            this.Databases = new DatabaseCollection();
        }

        public Server(): this(null)
        {}

        #endregion

        #region methods

        protected override void BuildToString(StringBuilder sb)
        {
            base.BuildToString(sb);

            sb.AppendFormat(", Databases: {0}", Databases.Count);
        }

        protected internal override DBObject InternalResolveChildObject(DBObjectUrn childObjectUrn)
        {
            DBObject result = null;

            if (Util.StringEqual(childObjectUrn[DbItemType.Server], this.Name, true))
            {
                result = this;

                string dbName = childObjectUrn[DbItemType.Database];
                if (!string.IsNullOrEmpty(dbName))
                {
                    Database databaseObj = this.Databases.SingleOrDefault(database => Util.StringEqual(database.Name, dbName, true));
                    if (databaseObj != null)
                    {
                        result = databaseObj.InternalResolveChildObject(childObjectUrn);
                    }
                }
            }

            return result;
        }

        #endregion
    }

    #region class ServerCollection

    [Serializable]
    public sealed class ServerCollection: ObjectCollection<Server>
    {
        public ServerCollection(IList<Server> list): base(list) {}

        public ServerCollection()
        {
        }
    }

    #endregion
}