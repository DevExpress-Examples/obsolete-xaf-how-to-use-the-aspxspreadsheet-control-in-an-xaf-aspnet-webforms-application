Imports System
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI.WebControls
Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Web.Editors
Imports DevExpress.Spreadsheet
Imports DevExpress.Web.ASPxSpreadsheet
Imports SpreadsheetExample.Module.BusinessObjects

Namespace SpreadsheetExample.Module.Web.Editors
	<PropertyEditor(GetType(SpreadsheetDocument), True)>
	Public Class SpreadsheetEditor
		Inherits WebPropertyEditor

		Public Sub New(ByVal objectType As Type, ByVal model As IModelMemberViewItem)
			MyBase.New(objectType, model)
		End Sub

		Protected ReadOnly Property Spreadsheet() As ASPxSpreadsheet
			Get
				Return CType(If(ViewEditMode = ViewEditMode.Edit, Editor, InplaceViewModeEditor), ASPxSpreadsheet)
			End Get
		End Property

		Protected ReadOnly Property CurrentDocument() As SpreadsheetDocument
			Get
				Return DirectCast(PropertyValue, SpreadsheetDocument)
			End Get
		End Property

		Protected Overrides Function CreateEditModeControlCore() As WebControl
			Return New ASPxSpreadsheet()
		End Function

		Protected Overrides Function CreateViewModeControlCore() As WebControl
			Return New ASPxSpreadsheet()
		End Function

		Protected Overrides Sub ReadEditModeValueCore()
			OpenDocument()
		End Sub

		Protected Overrides Sub ReadViewModeValueCore()
			OpenDocument()
		End Sub

		Protected Overrides Function GetControlValueCore() As Object
			Return PropertyValue
		End Function

		Protected Overrides Sub SetupControl(ByVal control As WebControl)
            Dim spreadsheetControl As ASPxSpreadsheet = CType(control, ASPxSpreadsheet)
            spreadsheetControl.WorkDirectory = HttpContext.Current.Server.MapPath("~/App_Data/WorkDirectory")
			If ViewEditMode = ViewEditMode.Edit Then
                spreadsheetControl.RibbonMode = SpreadsheetRibbonMode.Ribbon
                spreadsheetControl.CreateDefaultRibbonTabs(True)
                spreadsheetControl.RibbonTabs(0).Visible = False
                AddHandler spreadsheetControl.Load, AddressOf spreadsheet_Load

			Else
                spreadsheetControl.RibbonMode = SpreadsheetRibbonMode.None
			End If
		End Sub

		Public Overrides Sub BreakLinksToControl(ByVal unwireEventsOnly As Boolean)
			MyBase.BreakLinksToControl(unwireEventsOnly)
			If Spreadsheet IsNot Nothing Then
				RemoveHandler Spreadsheet.Load, AddressOf spreadsheet_Load
			End If
		End Sub

		Private Sub spreadsheet_Load(ByVal sender As Object, ByVal e As EventArgs)
			If Spreadsheet.Document.Modified Then
				CurrentDocument.Content = Spreadsheet.SaveCopy(DocumentFormat.OpenXml)
			End If
		End Sub

		Private Sub OpenDocument()
			If CurrentDocument.Content Is Nothing Then
				Spreadsheet.[New]()
			Else
				Spreadsheet.Open(GetDocumentId(), DocumentFormat.OpenXml, Function() CurrentDocument.Content)
			End If
			Spreadsheet.ReadOnly = ViewEditMode = ViewEditMode.View
		End Sub

		Private Function GetDocumentId() As String
			Return String.Concat(CurrentDocument.Oid)
		End Function
	End Class
End Namespace
