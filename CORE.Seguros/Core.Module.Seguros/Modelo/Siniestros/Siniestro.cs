using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Fondos;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    //[DefaultProperty( "Nombre" )]
    [System.ComponentModel.DisplayName("Siniestro")]
    [Persistent("seguros.Siniestro")]
    public class Siniestro : BasicObject
    {
        private DateTime fCerrado;
        private bool fCoaseguro;
        private string fDanmificados;
        private decimal fDaño;
        private decimal fDeducible;
        private string fDeducibleNotas;
        private string fDetalle;
        private DocumentoItem fDocumentoItem;
        private Especie fEspecie;
        private SiniestroEstado fEstado;
        private string fEstadoNotas;
        private DateTime fFechaAseguradora;
        private DateTime fFechaDenuncia;
        private DateTime fFechaSiniestro;
        private string fFolioAseguradora;
        private decimal fGraciable;
        private DateTime fInspeccionFecha;
        private string fInspeccionNotas;
        private string fJurisdiccion;
        private Liquidador fLiquidador;
        private string fNotas;
        private int fNumero;
        private Poliza fPoliza;
        private decimal fReclamado;
        private string fRiesgoAfectado;
        private bool fTerceros;
        private SiniestroTipo fTipo;
        private SiniestroTipoDePerdida fTipoDePerdida;
        private string fUbicacion;

        public Siniestro(Session session) : base(session)
        {
        }

        [Association]
        public Poliza Poliza
        {
            get { return fPoliza; }
            set { SetPropertyValue("Poliza", ref fPoliza, value); }
        }

        [Association]
        public DocumentoItem DocumentoItem
        {
            get { return fDocumentoItem; }
            set { SetPropertyValue("DocumentoItem", ref fDocumentoItem, value); }
        }

        public bool Coaseguro
        {
            get { return fCoaseguro; }
            set { SetPropertyValue("Coaseguro", ref fCoaseguro, value); }
        }

        public int Numero
        {
            get { return fNumero; }
            set { SetPropertyValue<int>("Numero", ref fNumero, value); }
        }

        public DateTime FechaSiniestro
        {
            get { return fFechaSiniestro; }
            set { SetPropertyValue<DateTime>("FechaSiniestro", ref fFechaSiniestro, value); }
        }

        public DateTime FechaDenuncia
        {
            get { return fFechaDenuncia; }
            set { SetPropertyValue<DateTime>("FechaDenuncia", ref fFechaDenuncia, value); }
        }

        public DateTime FechaAseguradora
        {
            get { return fFechaAseguradora; }
            set { SetPropertyValue<DateTime>("FechaAseguradora", ref fFechaAseguradora, value); }
        }

        [Size(20)]
        public string FolioAseguradora
        {
            get { return fFolioAseguradora; }
            set { SetPropertyValue("FolioAseguradora", ref fFolioAseguradora, value); }
        }

        public Liquidador Liquidador
        {
            get { return fLiquidador; }
            set { SetPropertyValue("Liquidador", ref fLiquidador, value); }
        }

        public SiniestroTipo Tipo
        {
            get { return fTipo; }
            set { SetPropertyValue("Tipo", ref fTipo, value); }
        }

        public SiniestroTipoDePerdida TipoDePerdida
        {
            get { return fTipoDePerdida; }
            set { SetPropertyValue("TipoDePerdida", ref fTipoDePerdida, value); }
        }

        public SiniestroEstado Estado
        {
            get { return fEstado; }
            set { SetPropertyValue("Estado", ref fEstado, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string EstadoNotas
        {
            get { return fEstadoNotas; }
            set { SetPropertyValue("EstadoNotas", ref fEstadoNotas, value); }
        }

        public DateTime Cerrado
        {
            get { return fCerrado; }
            set { SetPropertyValue<DateTime>("Cerrado", ref fCerrado, value); }
        }

        public bool Terceros
        {
            get { return fTerceros; }
            set { SetPropertyValue("Terceros", ref fTerceros, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Danmificados
        {
            get { return fDanmificados; }
            set { SetPropertyValue("Danmificados", ref fDanmificados, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string RiesgoAfectado
        {
            get { return fRiesgoAfectado; }
            set { SetPropertyValue("RiesgoAfectado", ref fRiesgoAfectado, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Ubicacion
        {
            get { return fUbicacion; }
            set { SetPropertyValue("Ubicacion", ref fUbicacion, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Jurisdiccion
        {
            get { return fJurisdiccion; }
            set { SetPropertyValue("Jurisdiccion", ref fJurisdiccion, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Detalle
        {
            get { return fDetalle; }
            set { SetPropertyValue("Detalle", ref fDetalle, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Notas
        {
            get { return fNotas; }
            set { SetPropertyValue("Notas", ref fNotas, value); }
        }

        public DateTime InspeccionFecha
        {
            get { return fInspeccionFecha; }
            set { SetPropertyValue<DateTime>("InspeccionFecha", ref fInspeccionFecha, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string InspeccionNotas
        {
            get { return fInspeccionNotas; }
            set { SetPropertyValue("InspeccionNotas", ref fInspeccionNotas, value); }
        }

        public Especie Especie
        {
            get { return fEspecie; }
            set { SetPropertyValue("Especie", ref fEspecie, value); }
        }

        public decimal Reclamado
        {
            get { return fReclamado; }
            set { SetPropertyValue<decimal>("Reclamado", ref fReclamado, value); }
        }

        public decimal Daño
        {
            get { return fDaño; }
            set { SetPropertyValue<decimal>("Daño", ref fDaño, value); }
        }

        public decimal Deducible
        {
            get { return fDeducible; }
            set { SetPropertyValue<decimal>("Deducible", ref fDeducible, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string DeducibleNotas
        {
            get { return fDeducibleNotas; }
            set { SetPropertyValue("DeducibleNotas", ref fDeducibleNotas, value); }
        }

        public decimal Graciable
        {
            get { return fGraciable; }
            set { SetPropertyValue<decimal>("Graciable", ref fGraciable, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<SiniestroLiquidacion> Liquidaciones
        {
            get { return GetCollection<SiniestroLiquidacion>("Liquidaciones"); }
        }
    }
}