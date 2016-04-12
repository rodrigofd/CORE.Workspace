using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Modelo.Sistema
{
    public class BasicObjectCloner : Cloner
    {
        public bool IgnoreOnChanged;

        public BasicObjectCloner(bool ignoreOnChanged)
        {
            IgnoreOnChanged = ignoreOnChanged;
        }

        public override IXPSimpleObject CreateObject(Session session, Type type)
        {
            var xpSimpleObject = base.CreateObject(session, type);

            var basicObject = xpSimpleObject as BasicObject;
            if (basicObject != null)
                basicObject.IgnoreOnChanged = IgnoreOnChanged;

            return xpSimpleObject;
        }
    }
}