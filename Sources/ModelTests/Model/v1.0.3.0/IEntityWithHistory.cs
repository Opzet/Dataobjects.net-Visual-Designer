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

namespace MyEntityModel.Interfaces
{
    [Xtensive.Orm.Index("Name",Unique=true,Name="idxName")]
    public partial interface IEntityWithHistory: IEntity
    {
        
        #region Navigation Properties
        
        [Xtensive.Orm.Field]
        [Xtensive.Orm.Association(PairTo="Owner")]
        MyEntityModel.TypedEntitySets.HistoryEntitySet HistoryItems
        {
            get;
        }
        
        #endregion Navigation Properties
    }
}
