using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Descripcion")]
    [System.ComponentModel.DisplayName("Aseguradora")]
    [Persistent(@"seguros.Aseguradora")]
    public class Aseguradora : Personas.Rol
    {
        private string fCodigo;
        private int fOrden;
        private TipoLiquidacion fTipoLiquidacion;

        public Aseguradora(Session session) : base(session)
        {
        }

        [VisibleInDetailView(false)]
        [PersistentAlias("Persona.Nombre")]
        public string Descripcion
        {
            get { return Convert.ToString(EvaluateAlias("Descripcion")); }
        }

        [Size(10)]
        [Index(0)]
        public string Codigo
        {
            get { return fCodigo; }
            set { SetPropertyValue("Codigo", ref fCodigo, value); }
        }

        public TipoLiquidacion TipoLiquidacion
        {
            get { return fTipoLiquidacion; }
            set { SetPropertyValue("TipoLiquidacion", ref fTipoLiquidacion, value); }
        }

        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }

        [Association]
        public XPCollection<AseguradoraProductor> ProductoresInscriptos
        {
            get { return GetCollection<AseguradoraProductor>("ProductoresInscriptos"); }
        }

        [Association]
        public XPCollection<Titulo> Titulos
        {
            get { return GetCollection<Titulo>("Titulos"); }
        }

        [Association]
        [Aggregated]
        public XPCollection<LineaAseguradora> Negocios
        {
            get { return GetCollection<LineaAseguradora>("Negocios"); }
        }

        [Association]
        [Aggregated]
        public XPCollection<PlanDePago> PlanesDePago
        {
            get { return GetCollection<PlanDePago>("PlanesDePago"); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            TipoLiquidacion = TipoLiquidacion.Bruta;
        }
    }
}