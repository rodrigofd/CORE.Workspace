using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Seguridad;
using FDIT.Core.Sistema;
using FDIT.Core.Ventas;

namespace FDIT.Core.CRM
{
    [Persistent(@"crm.Actividad")]
    [DefaultClassOptions]
    [System.ComponentModel.DisplayName("Actividad")]
    public class Actividad : BasicObject, IObjetoPorEmpresa
    {
        private Empresa _empresa;
        private string fAsunto;
        private Caso fCaso;
        private Cliente fCliente;
        private decimal fConsumo;
        private string fContenido;
        private Contrato fContrato;
        private ContratoItem fContratoItem;
        private Direccion fDireccion;
        private string fDireccionBCC;
        private string fDireccionCC;
        private string fDireccionDestinatario;
        private string fDireccionRemitente;
        private ActividadEstado fEstado;
        private int fEstadoPorcentaje;
        private DateTime? fFin;
        private DateTime? fInicio;
        private DateTime? fInicioPactado;
        private ActividadTipo fTipo;
        private DateTime? fVencimiento;

        public Actividad(Session session) : base(session)
        {
        }

        [VisibleInDetailView(true)]
        [VisibleInListView(true)]
        public override Image Icono => Tipo?.TipoIcono;

        [RuleRequiredField]
        [ImmediatePostData]
        public ActividadTipo Tipo
        {
            get { return fTipo; }
            set
            {
                SetPropertyValue("Tipo", ref fTipo, value);
                if (!IsSaving && !IsLoading)
                {
                    Estado = Tipo.EstadoPredeterminado;
                    if (Tipo.DireccionPredeterminada.HasValue) Direccion = Tipo.DireccionPredeterminada.Value;

                    if (Tipo.InicioAutomatico)
                        Inicio = DateTime.Now;

                    if (Inicio.HasValue) Fin = Inicio.Value.AddMinutes(Tipo.DuracionPredeterminada);
                }
            }
        }

        [RuleRequiredField]
        public Direccion Direccion
        {
            get { return fDireccion; }
            set { SetPropertyValue("Direccion", ref fDireccion, value); }
        }

        [ImmediatePostData]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? InicioPactado
        {
            get { return fInicioPactado; }
            set { SetPropertyValue("InicioPactado", ref fInicioPactado, value); }
        }

        [ImmediatePostData]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? Vencimiento
        {
            get { return fVencimiento; }
            set { SetPropertyValue("Vencimiento", ref fVencimiento, value); }
        }

        [ImmediatePostData]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? Inicio
        {
            get { return fInicio; }
            set { SetPropertyValue("Inicio", ref fInicio, value); }
        }

        [ImmediatePostData]
        [RuleValueComparison("inicio_fin", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, "Inicio",
            ParametersMode.Expression)]
        [ModelDefault("EditMask", "dd/MM/yyyy HH:mm:ss")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime? Fin
        {
            get { return fFin; }
            set { SetPropertyValue("Fin", ref fFin, value); }
        }

        [ModelDefault("AllowEdit", "false")]
        public string Duracion
        {
            get
            {
                var t = new TimeSpan(0);

                if (Inicio != null && Fin != null && !(Fin < Inicio))
                {
                    t = Fin.Value.Subtract(Inicio.Value);
                }

                var ret = "";
                if (t.Days > 0)
                    ret += t.Days + " días";
                if (t.Hours > 0)
                    ret += (ret != "" ? ", " : "") + t.Hours + " horas";
                if (t.Minutes > 0)
                    ret += (ret != "" ? ", " : "") + t.Minutes + " minutos";
                if (t.Seconds > 0)
                    ret += (ret != "" ? ", " : "") + t.Seconds + " segundos";

                return ret;
            }
        }

        [ImmediatePostData]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public Cliente Cliente
        {
            get { return fCliente; }
            set { SetPropertyValue("Cliente", ref fCliente, value); }
        }

        [ImmediatePostData]
        [DataSourceProperty("Cliente.Contratos")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public Contrato Contrato
        {
            get { return fContrato; }
            set { SetPropertyValue("Contrato", ref fContrato, value); }
        }

        [ImmediatePostData]
        [DataSourceProperty("Contrato.Items")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public ContratoItem ContratoItem
        {
            get { return fContratoItem; }
            set { SetPropertyValue("ContratoItem", ref fContratoItem, value); }
        }

        [ModelDefault("DisplayFormat", "{0:n4}")]
        [ModelDefault("EditMask", "n4")]
        [ImmediatePostData]
        public decimal Consumo
        {
            get { return fConsumo; }
            set { SetPropertyValue<decimal>("Consumo", ref fConsumo, value); }
        }

        [Association]
        [ImmediatePostData]
        public Caso Caso
        {
            get { return fCaso; }
            set { SetPropertyValue("Caso", ref fCaso, value); }
        }

        [RuleRequiredField]
        public string Asunto
        {
            get { return fAsunto; }
            set { SetPropertyValue("Asunto", ref fAsunto, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Contenido
        {
            get { return fContenido; }
            set { SetPropertyValue("Contenido", ref fContenido, value); }
        }

        public string DireccionRemitente
        {
            get { return fDireccionRemitente; }
            set { SetPropertyValue("DireccionRemitente", ref fDireccionRemitente, value); }
        }

        public string DireccionDestinatario
        {
            get { return fDireccionDestinatario; }
            set { SetPropertyValue("DireccionDestinatario", ref fDireccionDestinatario, value); }
        }

        [Appearance("act_mostrar_cc", AppearanceItemType.ViewItem, "IsNull(Tipo) or Tipo.UsaCC = false",
            Visibility = ViewItemVisibility.Hide)]
        public string DireccionCC
        {
            get { return fDireccionCC; }
            set { SetPropertyValue("DireccionCC", ref fDireccionCC, value); }
        }

        [Appearance("act_mostrar_bcc", AppearanceItemType.ViewItem, "IsNull(Tipo) or Tipo.UsaBCC = false",
            Visibility = ViewItemVisibility.Hide)]
        public string DireccionBCC
        {
            get { return fDireccionBCC; }
            set { SetPropertyValue("DireccionBCC", ref fDireccionBCC, value); }
        }

        [RuleRequiredField]
        [ImmediatePostData]
        public ActividadEstado Estado
        {
            get { return fEstado; }
            set
            {
                SetPropertyValue("Estado", ref fEstado, value);

                if (!IsLoading && !IsSaving)
                {
                    if (Estado.PorcentajePredeterminado.HasValue)
                        EstadoPorcentaje = Estado.PorcentajePredeterminado.Value;
                }
            }
        }

        [RuleRequiredField]
        [ImmediatePostData]
        [RuleRange(DefaultContexts.Save, 0, 100, CustomMessageTemplate = "El valor debe ser un entero entre 0 y 100")]
        public int EstadoPorcentaje
        {
            get { return fEstadoPorcentaje; }
            set { SetPropertyValue<int>("EstadoPorcentaje", ref fEstadoPorcentaje, value); }
        }

        [Association(@"ActividadesResponsablesReferencesActividades", typeof (ActividadResponsable))]
        public XPCollection<ActividadResponsable> Responsables => GetCollection<ActividadResponsable>("Responsables");

        [Browsable(false)]
        public Empresa Empresa
        {
            get { return _empresa; }
            set { SetPropertyValue("Empresa", ref _empresa, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Responsables.Add(new ActividadResponsable(Session)
            {
                Responsable = CoreAppLogonParameters.Instance.UsuarioActual(Session),
                Principal = true
            });
        }
    }
}