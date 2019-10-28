'------------------------------------------------------------------------------
' <auto-generated>
'     DataObjects.Net Entity Model Designer
'     Template version: 1.0.4.0
'     This code was generated from a template.
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized

Imports Xtensive.Core
Imports Xtensive.Orm

Namespace MyEntityModel.Interfaces
    <Xtensive.Orm.Index("Name",Unique:=true,Name:="idxName")>
    Public Interface IEntityWithHistory
    Inherits IEntity
        
        #Region "Navigation Properties"
        
        <Xtensive.Orm.Field>
        <Xtensive.Orm.Association(PairTo:="Owner")>
        Property HistoryItems as MyEntityModel.TypedEntitySets.HistoryEntitySet
        
        #End Region
    End Interface
End Namespace