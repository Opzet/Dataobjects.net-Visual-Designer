//------------------------------------------------------------------------------
// <auto-generated>
//     DataObjects.Net Entity Model Designer
//     Template version: 1.0.5.0
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xtensive.Core;
using Xtensive.Orm;
namespace Model.Version1_0_5_0.Entities
{
    [Xtensive.Orm.Index("Age:DESC")]
    [Xtensive.Orm.HierarchyRoot]
    [Xtensive.Orm.TypeDiscriminatorValue(123456ul,Default=false)]
    [Xtensive.Orm.Index("Name",Unique=true,Name="idxName")]
    [System.SerializableAttribute]
    public partial class EntityBase: Xtensive.Orm.Entity,Model.Version1_0_5_0.Interfaces.IEntity
    {
        
        #region Scalar Properties
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key(Direction=Xtensive.Core.Direction.Negative,Position=5)]
        public virtual int Id
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field(DefaultValue=123456789D)]
        [Xtensive.Orm.Validation.EmailConstraint(Mode=Xtensive.Orm.Validation.ConstrainMode.OnSetValue)]
        [Xtensive.Orm.Validation.LengthConstraint(Message="Wrong Age",Min=1,Max=99)]
        public virtual string Age
        {
            get;
            set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Version(Xtensive.Orm.VersionMode.Skip)]
        public virtual string ScalarProperty4
        {
            get;
            set;
        }
        
        #endregion Scalar Properties
        
        #region Inherited Scalar Properties
        
        [Xtensive.Orm.Field]
        public virtual System.Nullable<byte> OId
        {
            get;
            set;
        }
        
        #endregion Inherited Scalar Properties
        
        #region Constructors
        
        public EntityBase(Xtensive.Orm.Session session) : base(session)
        {}
        
        public EntityBase()
        {}
        public EntityBase(Xtensive.Orm.Session session,int id) : base(session,id)
        {}
        protected EntityBase(Xtensive.Orm.Session session,int id, params System.Object[] values) : base(session,id, values)
        {}
        
        #endregion Constructors
        
    }
}
