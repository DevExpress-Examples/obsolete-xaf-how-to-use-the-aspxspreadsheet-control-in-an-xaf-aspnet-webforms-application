using System;
using System.Web;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.Spreadsheet;
using DevExpress.Web.ASPxSpreadsheet;
using SpreadsheetExample.Module.BusinessObjects;

namespace SpreadsheetExample.Module.Web.Editors {
    [PropertyEditor(typeof(SpreadsheetDocument), true)]
    public class SpreadsheetEditor :WebPropertyEditor {
        public SpreadsheetEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }

        protected ASPxSpreadsheet Spreadsheet {
            get { return (ASPxSpreadsheet)(ViewEditMode == ViewEditMode.Edit ? Editor : InplaceViewModeEditor); }
        }

        protected SpreadsheetDocument CurrentDocument {
            get { return (SpreadsheetDocument)PropertyValue; }
        }

        protected override WebControl CreateEditModeControlCore() {
            return new ASPxSpreadsheet();
        }

        protected override WebControl CreateViewModeControlCore() {
            return new ASPxSpreadsheet();
        }

        protected override void ReadEditModeValueCore() {
            OpenDocument();
        }

        protected override void ReadViewModeValueCore() {
            OpenDocument();
        }

        protected override object GetControlValueCore() {
            return PropertyValue;
        }

        protected override void SetupControl(WebControl control) {
            ASPxSpreadsheet spreadsheet = (ASPxSpreadsheet)control;
            spreadsheet.WorkDirectory = HttpContext.Current.Server.MapPath("~/App_Data/WorkDirectory");
            if(ViewEditMode == ViewEditMode.Edit) {
                spreadsheet.RibbonMode = SpreadsheetRibbonMode.Ribbon;
                spreadsheet.CreateDefaultRibbonTabs(true);
                spreadsheet.RibbonTabs[0].Visible = false;
                spreadsheet.Load += spreadsheet_Load;

            } else spreadsheet.RibbonMode = SpreadsheetRibbonMode.None;
        }

        public override void BreakLinksToControl(bool unwireEventsOnly) {
            base.BreakLinksToControl(unwireEventsOnly);
            if (Spreadsheet != null)
                Spreadsheet.Load -= spreadsheet_Load;
        }

        private void spreadsheet_Load(object sender, EventArgs e) {
            if(Spreadsheet.Document.Modified)
                CurrentDocument.Content = Spreadsheet.SaveCopy(DocumentFormat.OpenXml);
        }

        private void OpenDocument() {
            if(CurrentDocument.Content == null)
                Spreadsheet.New();
            else Spreadsheet.Open(GetDocumentId(), DocumentFormat.OpenXml, () => CurrentDocument.Content);
            Spreadsheet.ReadOnly = ViewEditMode == ViewEditMode.View;
        }

        private string GetDocumentId() {
            return string.Concat(CurrentDocument.Oid);
        }
    }
}
