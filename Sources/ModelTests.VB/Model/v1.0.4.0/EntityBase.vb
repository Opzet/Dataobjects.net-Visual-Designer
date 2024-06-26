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

Namespace MyEntityModel.Entities
    <Xtensive.Orm.Index("Age:DESC")>
    <Xtensive.Orm.HierarchyRoot>
    <Xtensive.Orm.TypeDiscriminatorValue(123456UL,Default:=false)>
    <Xtensive.Orm.Index("Name",Unique:=true,Name:="idxName")>
    Public MustInherit Partial Class EntityBase
    Inherits Xtensive.Orm.Entity
    Implements MyEntityModel.Interfaces.IEntity
        
        #Region "Scalar Properties"
        
        <Xtensive.Orm.Field>
        <Xtensive.Orm.Key(Direction:=Xtensive.Core.Direction.Negative,Position:=5)>
        Public Overridable Property Id as Integer Implements MyEntityModel.Interfaces.IEntity.Id
        
        <Xtensive.Orm.Field(DefaultValue:=123456789R)>
        <Xtensive.Orm.Validation.EmailConstraint(Mode:=Xtensive.Orm.Validation.ConstrainMode.OnSetValue)>
        <Xtensive.Orm.Validation.LengthConstraint(Message:="Wrong Age",Min:=1,Max:=99)>
        Public Overridable Property Age as String
        
        <Xtensive.Orm.Field>
        <Xtensive.Orm.Version(Xtensive.Orm.VersionMode.Skip)>
        Public Overridable Property ScalarProperty4 as String
        
        #End Region
        
        #Region "Inherited Scalar Properties"
        
        <Xtensive.Orm.Field>
        Public Overridable Property OId as System.Nullable(Of Byte) Implements MyEntityModel.Interfaces.IEntity.OId
        
        #End Region
        
        #Region "Constructors"
        
        Public  Sub New(ByVal session As Xtensive.Orm.Session)
        	MyBase.New(session)
        End Sub
        
        Public  Sub New()
        	MyBase.New()
        End Sub
        
        Public  Sub New(ByVal session As Xtensive.Orm.Session,id As Integer)
        	MyBase.New(session, id)
        End Sub
        
        #End Region
        
    End Class
End Namespace
