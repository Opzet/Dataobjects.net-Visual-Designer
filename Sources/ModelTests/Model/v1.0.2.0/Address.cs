//------------------------------------------------------------------------------
// <auto-generated>
//     DataObjects.Net Entity Model Designer
//     Template version: 1.0.4.0
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

namespace Model.Version1_0_2_0
{
    public partial class Address: Xtensive.Orm.Structure
    {
        
        #region Scalar Properties
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Validation.RangeConstraint(Min=0,Max=50)]
        [Xtensive.Orm.Key]
        public virtual sbyte SByteField
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key]
        public virtual short Int16Field
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key]
        public virtual int Int32Field
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key]
        public virtual long Int64Field
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key]
        public virtual byte ByteField
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key]
        public virtual ushort UInt16Field
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key]
        public virtual uint UInt32Field
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key]
        public virtual ulong UInt64Field
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Validation.FutureConstraint]
        [Xtensive.Orm.Validation.PastConstraint(Mode=Xtensive.Orm.Validation.ConstrainMode.OnValidate)]
        [Xtensive.Orm.Key]
        public virtual System.DateTime DateTimeField
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key(Direction=Xtensive.Core.Direction.Default,Position=0)]
        public virtual System.Guid GuidField
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key]
        public virtual byte[] ByteArrayField
        {
            get;
            private set;
        }
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Key]
        public virtual string StringField
        {
            get;
            private set;
        }
        
        #endregion Scalar Properties
    }
}
