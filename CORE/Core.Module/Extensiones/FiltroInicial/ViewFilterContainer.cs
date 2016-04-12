using System;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace FDIT.Core.Controllers.ViewFilter
{
    [NonPersistent]
    public class ViewFilterContainer : BaseObject
    {
        private string fCriteria;
        private Type fObjectType;

        public ViewFilterContainer(Session session) : base(session)
        {
        }

        [CriteriaOptions("ObjectType")]
        [ImmediatePostData]
        public string Criteria
        {
            get { return fCriteria; }
            set { SetPropertyValue("Criteria", ref fCriteria, value); }
        }

        [MemberDesignTimeVisibility(false)]
        public Type ObjectType
        {
            get { return fObjectType; }
            set { SetPropertyValue("ObjectType", ref fObjectType, value); }
        }
    }
}