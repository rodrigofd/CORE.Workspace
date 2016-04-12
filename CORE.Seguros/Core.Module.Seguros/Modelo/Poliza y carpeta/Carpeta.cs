using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Seguros
{
    [ImageName("folder_stand")]
    [Persistent("seguros.Carpeta")]
    [DefaultProperty("NumeroSolicitud")]
    public class Carpeta : BasicObject, IObjetoPorEmpresa
    {
        private Empresa fEmpresa;
        private int? fNumeroSolicitud;

        public Carpeta(Session session)
            : base(session)
        {
        }

        public int? NumeroSolicitud
        {
            get { return fNumeroSolicitud; }
            set { SetPropertyValue("NumeroSolicitud", ref fNumeroSolicitud, value); }
        }

        [Association]
        public XPCollection<Cotizacion> Cotizaciones => GetCollection<Cotizacion>("Cotizaciones");

        [Association]
        public XPCollection<Poliza> Polizas => GetCollection<Poliza>("Polizas");

        [Browsable(false)]
        public Empresa Empresa
        {
            get { return fEmpresa; }
            set { SetPropertyValue("Empresa", ref fEmpresa, value); }
        }
    }
}