using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace SpreadsheetExample.Module.BusinessObjects {
    public class SpreadsheetDocument :BaseObject {
        public SpreadsheetDocument(Session session) : base(session) { }

        public string Subject {
            get { return GetPropertyValue<string>("Subject"); }
            set { SetPropertyValue("Subject", value); }
        }

        [Delayed]
        [VisibleInListView(false)]
        public byte[] Content {
            get { return GetDelayedPropertyValue<byte[]>("Content"); }
            set { SetDelayedPropertyValue("Content", value); }
        }
    }
}
