using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Nombre")]
    [System.ComponentModel.DisplayName("Título")]
    [Persistent("seguros.Titulo")]
    public class Titulo : BasicObject
    {
        private Aseguradora fAseguradora;
        private string fCobertura;
        private string fCodigo;
        private string fDetalle;
        private int fOrden;
        private Ramo fRamo;
        private Subramo fSubramo;
        private TituloTipo fTipo;

        public Titulo(Session session)
            : base(session)
        {
        }

        [Association]
        public TituloTipo Tipo
        {
            get { return fTipo; }
            set { SetPropertyValue("Tipo", ref fTipo, value); }
        }

        [Association]
        public Aseguradora Aseguradora
        {
            get { return fAseguradora; }
            set { SetPropertyValue("Aseguradora", ref fAseguradora, value); }
        }

        [Association]
        public Ramo Ramo
        {
            get { return fRamo; }
            set { SetPropertyValue("Ramo", ref fRamo, value); }
        }

        [Association]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        [DataSourceProperty("Ramo.Subramos")]
        public Subramo Subramo
        {
            get { return fSubramo; }
            set { SetPropertyValue("Subramo", ref fSubramo, value); }
        }

        [Size(50)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "1")]
        [Index(1)]
        public string Nombre
        {
            get { return fCobertura; }
            set { SetPropertyValue("Nombre", ref fCobertura, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Detalle
        {
            get { return fDetalle; }
            set { SetPropertyValue("Detalle", ref fDetalle, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }
    }
}