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

Namespace Debugging.VB
    <Xtensive.Orm.HierarchyRoot>
    Public NotInheritable Partial Class Entity2
    Inherits Entity1
        
        #Region "Inherited Scalar Properties"
        
        <Xtensive.Orm.Field>
        <Xtensive.Orm.Key>
        Public Property Id as Integer
        
        <Xtensive.Orm.Field(Indexed:=true,Length:=123)>
        Public Property Name as String
        
        #End Region
        
        #Region "Constructors"
        
        Public  Sub New(ByVal session As Xtensive.Orm.Session)
        	MyBase.New(session)
        End Sub
        
        Public  Sub New()
        	MyBase.New()
        End Sub
        
        
        #End Region
        
    End Class
End Namespace
