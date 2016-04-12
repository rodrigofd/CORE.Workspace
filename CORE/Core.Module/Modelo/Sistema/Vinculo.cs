using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core
{
    [ImageName("link")]
    [Persistent("sistema.Vinculo")]
    public class Vinculo : XPObject
    {
        public Vinculo(Session session)
            : base(session)
        {
        }

        [NonPersistent]
        public BasicObject Originante
        {
            get
            {
                if (OriginanteKey <= 0 || OriginanteType == null)
                    return null;
                if (!OriginanteType.IsValidType)
                    return null;
                return (BasicObject) Session.GetObjectByKey(OriginanteType.TypeClassInfo, OriginanteKey);
            }
            set
            {
                if (value == null)
                {
                    OriginanteType = null;
                    OriginanteKey = null;
                }
                else if (Session.IsNewObject(value))
                {
                    throw new ArgumentException("No puede vincular con un objeto sin guardar.");
                }
                else
                {
                    OriginanteType = Session.GetObjectType(value);
                    OriginanteKey = (int) Session.GetKeyValue(value);
                }
                OnChanged();
            }
        }

        [Browsable(false)]
        [Persistent]
        protected XPObjectType OriginanteType { get; set; }

        [Browsable(false)]
        [Persistent]
        protected int? OriginanteKey { get; set; }

        [NonPersistent]
        public BasicObject Destinatario
        {
            get
            {
                if (DestinatarioKey <= 0 || DestinatarioType == null)
                    return null;
                if (!DestinatarioType.IsValidType)
                    return null;
                return (BasicObject) Session.GetObjectByKey(DestinatarioType.TypeClassInfo, DestinatarioKey);
            }
            set
            {
                if (value == null)
                {
                    DestinatarioType = null;
                    DestinatarioKey = null;
                }
                else if (Session.IsNewObject(value))
                {
                    throw new ArgumentException("No puede vincular con un objeto sin guardar.");
                }
                else
                {
                    DestinatarioType = Session.GetObjectType(value);
                    DestinatarioKey = (int) Session.GetKeyValue(value);
                }
                OnChanged();
            }
        }

        [Browsable(false)]
        [Persistent]
        protected XPObjectType DestinatarioType { get; set; }

        [Browsable(false)]
        [Persistent]
        protected int? DestinatarioKey { get; set; }
    }
}