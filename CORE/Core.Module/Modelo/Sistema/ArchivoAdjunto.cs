using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core
{
    [ImageName("attach")]
    [DefaultClassOptions]
    [Persistent("sistema.ArchivoAdjunto")]
    [FileAttachment("ArchivoAsociado")]
    public class ArchivoAdjunto : XPObject
    {
        private string fDescripcion;
        private DateTime fFecha;

        public ArchivoAdjunto(Session session) : base(session)
        {
        }

        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [NoForeignKey]
        [ImmediatePostData]
        public Archivo ArchivoAsociado
        {
            get { return GetPropertyValue<Archivo>("ArchivoAsociado"); }
            set { SetPropertyValue("ArchivoAsociado", value); }
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

        [Persistent]
        [Browsable(false)]
        public XPObjectType OriginanteType { get; set; }

        [Persistent]
        [Browsable(false)]
        public int? OriginanteKey { get; set; }

        [Index(0)]
        [ModelDefault("EditMask", "G")]
        [ModelDefault("DisplayFormat", "{0:G}")]
        public DateTime Fecha
        {
            get { return fFecha; }
            set { SetPropertyValue<DateTime>("Fecha", ref fFecha, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Descripcion
        {
            get { return fDescripcion; }
            set { SetPropertyValue("Descripcion", ref fDescripcion, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Fecha = DateTime.Now;
        }
    }
}