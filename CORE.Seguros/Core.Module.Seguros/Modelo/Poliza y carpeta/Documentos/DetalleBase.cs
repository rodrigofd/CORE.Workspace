using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Personas;

namespace FDIT.Core.Seguros
{
    [NonPersistent]
    [System.ComponentModel.DisplayName("Detalle")]
    [RuleCriteria("TituloODescripcion", DefaultContexts.Save, "Not IsNull(Titulo) OR LEN(Detalle) > 0",
        "Debe ingresar al menos un titulo predefinido, o bien un texto")]
    public abstract class DetalleBase : BasicObject
    {
        private string fDetalle;
        private Interes fInteres;
        private int fOrden;
        private TituloTipo fTipoTitulo;
        private Titulo fTitulo;
        private XPCollection<Titulo> titulosPosibles;

        public DetalleBase(Session session)
            : base(session)
        {
        }

        [VisibleInListView(true)]
        [Index(0)]
        public Interes Interes
        {
            get { return fInteres; }
            set { SetPropertyValue("Interes", ref fInteres, value); }
        }

        [VisibleInListView(true)]
        [ImmediatePostData]
        [RuleRequiredField]
        [Index(1)]
        public TituloTipo TipoTitulo
        {
            get { return fTipoTitulo; }
            set
            {
                SetPropertyValue("TipoTitulo", ref fTipoTitulo, value);

                if (CanRaiseOnChanged)
                {
                    RefreshTitulosPosibles();
                    Titulo = null;
                    //reiniciar el valor, para asegurarnos que no sea un valor invalido para este referente
                }
            }
        }

        [Browsable(false)]
        private XPCollection<Titulo> TitulosPosibles
        {
            get
            {
                if (titulosPosibles == null)
                {
                    titulosPosibles = new XPCollection<Titulo>(Session);
                    RefreshTitulosPosibles();
                }
                return titulosPosibles;
            }
        }

        [VisibleInListView(true)]
        [DataSourceProperty("TitulosPosibles")]
        [Index(2)]
        public Titulo Titulo
        {
            get { return fTitulo; }
            set { SetPropertyValue("Titulo", ref fTitulo, value); }
        }

        [VisibleInListView(true)]
        [Size(SizeAttribute.Unlimited)]
        [Index(3)]
        public string Detalle
        {
            get { return fDetalle; }
            set { SetPropertyValue("Detalle", ref fDetalle, value); }
        }

        [VisibleInListView(true)]
        [Index(4)]
        public int Orden
        {
            get { return fOrden; }
            set { SetPropertyValue<int>("Orden", ref fOrden, value); }
        }

        [Browsable(false)]
        public abstract Persona AseguradoraAsociada { get; }

        [Browsable(false)]
        public abstract Ramo RamoAsociado { get; }

        [Browsable(false)]
        public abstract Subramo SubramoAsociado { get; }

        public void RefreshTitulosPosibles()
        {
            if (TitulosPosibles == null) return;
            TitulosPosibles.Criteria = CriteriaOperator.Parse("Tipo = ? AND " +
                                                              "(ISNULL(Ramo) Or Ramo = ?) AND " +
                                                              "(ISNULL(Subramo) OR Subramo = ?) AND " +
                                                              "(ISNULL(Aseguradora) OR Aseguradora.Persona = ?)",
                TipoTitulo,
                RamoAsociado,
                SubramoAsociado,
                AseguradoraAsociada);
        }
    }
}