Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.Xpo

Namespace SpreadsheetExample.Module.BusinessObjects
	<NavigationItem>
	Public Class Report
		Inherits BaseObject

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

		<RuleRequiredField>
		Public Property Name() As String
			Get
				Return GetPropertyValue(Of String)("Name")
			End Get
			Set(ByVal value As String)
				Dim modified As Boolean = SetPropertyValue("Name", value)
				If modified AndAlso (Not IsLoading) Then
					Data.Subject = value
				End If
			End Set
		End Property

		<Aggregated, VisibleInListView(False), ExpandObjectMembers(ExpandObjectMembers.Never)>
		Public Property Data() As SpreadsheetDocument
			Get
				Return GetPropertyValue(Of SpreadsheetDocument)("Data")
			End Get
			Set(ByVal value As SpreadsheetDocument)
				SetPropertyValue("Data", value)
			End Set
		End Property

		Public Overrides Sub AfterConstruction()
			MyBase.AfterConstruction()
			Data = New SpreadsheetDocument(Session)
		End Sub
	End Class
End Namespace
