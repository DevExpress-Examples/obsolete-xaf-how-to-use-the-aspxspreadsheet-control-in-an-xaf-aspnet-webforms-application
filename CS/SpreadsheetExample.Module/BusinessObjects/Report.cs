using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace SpreadsheetExample.Module.BusinessObjects {
    [NavigationItem]
    public class Report :BaseObject {
        public Report(Session session) : base(session) { }

        [RuleRequiredField]
        public string Name {
            get { return GetPropertyValue<string>("Name"); }
            set { 
                bool modified = SetPropertyValue("Name", value);
                if(modified && !IsLoading)
                    Data.Subject = value;
            }
        }

        [Aggregated]
        [VisibleInListView(false)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public SpreadsheetDocument Data {
            get { return GetPropertyValue<SpreadsheetDocument>("Data"); }
            set { SetPropertyValue("Data", value); }
        }

        public override void AfterConstruction() {
            base.AfterConstruction();
            Data = new SpreadsheetDocument(Session);
        }
    }
}
