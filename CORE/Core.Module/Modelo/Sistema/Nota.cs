using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace FDIT.Core
{
    [ImageName("sticky-note-pin")]
    [Persistent("sistema.Nota")]
    [DefaultProperty("")]
    [DefaultClassOptions]
    public class Nota : XPObject
    {
        private string fContenido;
        private DateTime fFecha;

        public Nota(Session session) : base(session)
        {
        }

        [NonPersistent]
        [Browsable(false)]
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

        [RuleRequiredField]
        [Persistent]
        [Browsable(false)]
        public XPObjectType OriginanteType { get; set; }

        [Persistent]
        [Browsable(false)]
        public int? OriginanteKey { get; set; }

        [RuleRequiredField]
        [Index(0)]
        [ModelDefault("EditMask", "G")]
        [ModelDefault("DisplayFormat", "{0:G}")]
        public DateTime Fecha
        {
            get { return fFecha; }
            set { SetPropertyValue<DateTime>("Fecha", ref fFecha, value); }
        }

        [RuleRequiredField]
        [Index(1)]
        [VisibleInListView(true)]
        [VisibleInDetailView(true)]
        [Size(SizeAttribute.Unlimited)]
        public string Contenido
        {
            get { return fContenido; }
            set { SetPropertyValue("Contenido", ref fContenido, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Fecha = DateTime.Now;
        }
    }
}