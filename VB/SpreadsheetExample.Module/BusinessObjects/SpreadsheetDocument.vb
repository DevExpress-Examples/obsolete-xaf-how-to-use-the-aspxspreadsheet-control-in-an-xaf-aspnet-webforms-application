Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo

Namespace SpreadsheetExample.Module.BusinessObjects
	Public Class SpreadsheetDocument
		Inherits BaseObject

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

		Public Property Subject() As String
			Get
				Return GetPropertyValue(Of String)("Subject")
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Subject", value)
			End Set
		End Property

		<Delayed, VisibleInListView(False)>
		Public Property Content() As Byte()
			Get
				Return GetDelayedPropertyValue(Of Byte())("Content")
			End Get
			Set(ByVal value As Byte())
				SetDelayedPropertyValue("Content", value)
			End Set
		End Property
	End Class
End Namespace
