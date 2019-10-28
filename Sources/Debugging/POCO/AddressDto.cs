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
namespace MyEntityModel
{
    [System.Runtime.Serialization.DataContractAttribute(Name="xAddress",Namespace="xNamespace1")]
    public partial struct AddressDto: IObjectDto
    {
        
        #region Scalar Properties
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public sbyte SByteField
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public short Int16Field
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public int Int32Field
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public long Int64Field
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public byte ByteField
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public ushort UInt16Field
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public uint UInt32Field
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public ulong UInt64Field
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public System.DateTime DateTimeField
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public System.Guid GuidField
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public byte[] ByteArrayField
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public string StringField
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public bool BoolField
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public System.TimeSpan TimeSpanField
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMemberAttribute]
        public System.Guid ExtraKey
        {
            get;
            set;
        }
        
        #endregion Scalar Properties
        
        #region Implementation of IObjectDto
        
        public IObjectDto OriginalObject { get; set; }
        
        public string _MappingKey { get; set; }
        
        public void UpdateKeyFields(IObjectDto sourceDto)
        {
        	if (sourceDto is AddressDto)
        	{
        		var _sourceDto = (AddressDto)sourceDto;
        		this.SByteField = _sourceDto.SByteField;
        		this.Int16Field = _sourceDto.Int16Field;
        		this.Int32Field = _sourceDto.Int32Field;
        		this.Int64Field = _sourceDto.Int64Field;
        		this.ByteField = _sourceDto.ByteField;
        		this.UInt16Field = _sourceDto.UInt16Field;
        		this.UInt32Field = _sourceDto.UInt32Field;
        		this.UInt64Field = _sourceDto.UInt64Field;
        		this.DateTimeField = _sourceDto.DateTimeField;
        		this.GuidField = _sourceDto.GuidField;
        		this.ByteArrayField = _sourceDto.ByteArrayField;
        		this.StringField = _sourceDto.StringField;
        		this.ExtraKey = _sourceDto.ExtraKey;
        	}
        }
        
        #endregion
    }
}
