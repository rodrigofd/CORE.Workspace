using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Gestion;
using FDIT.Core.Personas;
using FDIT.Core.Sistema;
using FDIT.Core.Util;
using Comprobante = FDIT.Core.Ventas.Comprobante;

namespace FDIT.Core.Seguros
{
    [ImageName("document_index")]
    //[Appearance( "BloquearEdit", "Estado='Aceptada'", Enabled = false, TargetItems = "*")]
    [DefaultClassOptions]
    [NavigationItem("Seguros")]
    [DefaultProperty("Descripcion")]
    [System.ComponentModel.DisplayName("Documento")]
    [Persistent("seguros.Documento")]
    [RuleCriteria("MinimoUnItem", DefaultContexts.Save, "Items.Count() > 0",
        CustomMessageTemplate = "El documento debe tener al menos un item")]
    public class Documento : BasicObject, IObjetoPorEmpresa, IObjetoConImportes
    {
        [Browsable(false)] public bool CambioDeEstado;

        [Browsable(false)] public bool CambioImportesOCuotas;

        private int fCantidadCuotas;
        private Comprobante fComprobante;
        private string fComprobanteReferencia;
        private DateTime? fEmitidaFecha;
        private Empresa fEmpresa;
        private DocumentoEstado fEstado;
        private FormaDePago fFormaDePago;
        private string fNotaAseguradora;
        private string fNotaInterna;
        private string fNotaTomador;
        private string fNumeroEndoso;
        private int fNumeroSolicitud;
        private int fNumeroSolicitudProductor;
        private bool fPedidoDeEmision;
        private PeriodicidadFacturacion fPeriodicidadFacturacion;
        private PlanDePago fPlanDePago;
        private XPCollection<PlanDePago> fPlanesDePago;
        private Poliza fPoliza;
        private bool fProductorEnPoliza;
        private DocumentoTipo fTipo;
        private DateTime? fVigenciaFin;
        private DateTime fVigenciaInicio;

        public Documento(Session session) : base(session)
        {
        }

        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [PersistentAlias(
            "IIF(ISNULL(Tipo),'',Tipo.Codigo + ' ' ) + IIF(NumeroSolicitud=0,' (NUEVO)', TOSTR(NumeroSolicitud))")]
        public string Descripcion
        {
            get { return (string) EvaluateAlias("Descripcion"); }
        }

        [ModelDefault("AllowEdit", "false")]
        public int NumeroSolicitud
        {
            get { return fNumeroSolicitud; }
            set { SetPropertyValue<int>("NumeroSolicitud", ref fNumeroSolicitud, value); }
        }

        [ModelDefault("AllowEdit", "false")]
        public int NumeroSolicitudProductor
        {
            get { return fNumeroSolicitudProductor; }
            set { SetPropertyValue<int>("NumeroSolicitudProductor", ref fNumeroSolicitudProductor, value); }
        }

        [Association]
        [ExpandObjectMembers(ExpandObjectMembers.Always)]
        public Poliza Poliza
        {
            get { return fPoliza; }
            set { SetPropertyValue("Poliza", ref fPoliza, value); }
        }

        [NonPersistent]
        [ImmediatePostData]
        [RuleRequiredField]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public Ramo Ramo
        {
            get { return Poliza == null ? null : Poliza.Ramo; }
            set
            {
                Poliza.Ramo = value;
                if (CanRaiseOnChanged)
                {
                    Subramo = value.Subramos.Count == 1 ? value.Subramos[0] : null;
                }
            }
        }

        [NonPersistent]
        [ImmediatePostData]
        [RuleRequiredField]
        [DataSourceProperty("Ramo.Subramos")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public Subramo Subramo
        {
            get { return Poliza == null ? null : Poliza.Subramo; }
            set
            {
                Poliza.Subramo = value;
                GenerarImportesPredeterminados();
            }
        }

        [NonPersistent]
        public Persona Aseguradora
        {
            get { return Poliza == null || Poliza.Aseguradora == null ? null : Poliza.Aseguradora.Interviniente; }
            set
            {
                Poliza.Aseguradora.Interviniente = value;
                if (CanRaiseOnChanged)
                {
                    var a = Aseguradora;
                    PlanesDePago.Criteria = a == null ? null : CriteriaOperator.Parse("Aseguradora.Persona.Oid = ?", a);
                }
            }
        }

        public XPCollection<PlanDePago> PlanesDePago
        {
            get { return fPlanesDePago ?? (fPlanesDePago = new XPCollection<PlanDePago>(Session)); }
        }

        [Size(20)]
        public string NumeroEndoso
        {
            get { return fNumeroEndoso; }
            set { SetPropertyValue("NumeroEndoso", ref fNumeroEndoso, value); }
        }

        [RuleRequiredField]
        public DocumentoTipo Tipo
        {
            get { return fTipo; }
            set { SetPropertyValue("Tipo", ref fTipo, value); }
        }

        [RuleRequiredField]
        [ModelDefault("AllowEdit", "false")]
        public DocumentoEstado Estado
        {
            get { return fEstado; }
            set { SetPropertyValue("Estado", ref fEstado, value); }
        }

        public bool PedidoDeEmision
        {
            get { return fPedidoDeEmision; }
            set { SetPropertyValue("PedidoDeEmision", ref fPedidoDeEmision, value); }
        }

        [ImmediatePostData]
        public DateTime? EmitidaFecha
        {
            get { return fEmitidaFecha; }
            set
            {
                SetPropertyValue("EmitidaFecha", ref fEmitidaFecha, value);
                if (CanRaiseOnChanged)
                {
                    GenerarCuotas();
                }
            }
        }

        [RuleRequiredField]
        public DateTime VigenciaInicio
        {
            get { return fVigenciaInicio; }
            set { SetPropertyValue<DateTime>("VigenciaInicio", ref fVigenciaInicio, value); }
        }

        [RuleValueComparison("PolDocVigenciaInicioFin", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual,
            "VigenciaInicio", ParametersMode.Expression)]
        public DateTime? VigenciaFin
        {
            get { return fVigenciaFin; }
            set { SetPropertyValue("VigenciaFin", ref fVigenciaFin, value); }
        }

        [PersistentAlias("IIF(IsNull(VigenciaFin),0,DateDiffDay(VigenciaInicio,VigenciaFin))")]
        public int VigenciaDias
        {
            get { return (int) EvaluateAlias("VigenciaDias"); }
        }

        [PersistentAlias("IIF(IsNull(VigenciaFin),0,DateDiffMonth(VigenciaInicio,VigenciaFin))")]
        public int VigenciaMeses
        {
            get { return (int) EvaluateAlias("VigenciaMeses"); }
        }

        [PersistentAlias("IIF(IsNull(VigenciaFin),0,DateDiffDay(VigenciaFin,LocalDateTimeToday()))")]
        public int VigenciaDiasAlVencimiento
        {
            get { return (int) EvaluateAlias("VigenciaDiasAlVencimiento"); }
        }

        [PersistentAlias(
            "(Estado='Aceptada' OR Estado='PendienteDeDespacho' or Estado = 'PendienteDeAceptacion') AND (NOT IsNull(Poliza) AND (IsNull(Poliza.VigenciaFin) OR Poliza.VigenciaFin >= LocalDateTimeNow()))"
            )]
        public bool Vigente
        {
            get { return (bool) EvaluateAlias("Vigente"); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string NotaAseguradora
        {
            get { return fNotaAseguradora; }
            set { SetPropertyValue("NotaAseguradora", ref fNotaAseguradora, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string NotaInterna
        {
            get { return fNotaInterna; }
            set { SetPropertyValue("NotaInterna", ref fNotaInterna, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string NotaTomador
        {
            get { return fNotaTomador; }
            set { SetPropertyValue("NotaTomador", ref fNotaTomador, value); }
        }

        public FormaDePago FormaDePago
        {
            get { return fFormaDePago; }
            set { SetPropertyValue("LugarDePago", ref fFormaDePago, value); }
        }

        public PeriodicidadFacturacion PeriodicidadFacturacion
        {
            get { return fPeriodicidadFacturacion; }
            set { SetPropertyValue("PeriodicidadFacturacion", ref fPeriodicidadFacturacion, value); }
        }

        [ImmediatePostData]
        [RuleValueComparison("CantidadCuotasMin", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        public int CantidadCuotas
        {
            get { return fCantidadCuotas; }
            set
            {
                SetPropertyValue<int>("CantidadCuotas", ref fCantidadCuotas, value);
                if (CanRaiseOnChanged)
                {
                    GenerarCuotas();
                }
            }
        }

        [Size(50)]
        public string ComprobanteReferencia
        {
            get { return fComprobanteReferencia; }
            set { SetPropertyValue("ComprobanteReferencia", ref fComprobanteReferencia, value); }
        }

        public Comprobante Comprobante
        {
            get { return fComprobante; }
            set { SetPropertyValue("Comprobante", ref fComprobante, value); }
        }

        [DataSourceProperty("PlanesDePago")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public PlanDePago PlanDePago
        {
            get { return fPlanDePago; }
            set { SetPropertyValue("PlanDePago", ref fPlanDePago, value); }
        }

        public bool ProductorEnPoliza
        {
            get { return fProductorEnPoliza; }
            set { SetPropertyValue("ProductorEnPoliza", ref fProductorEnPoliza, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<DocumentoImporte> Importes
        {
            get { return GetCollection<DocumentoImporte>("Importes"); }
        }

        [Association]
        [Aggregated]
        public XPCollection<DocumentoItem> Items
        {
            get { return GetCollection<DocumentoItem>("Items"); }
        }

        [Association]
        [Aggregated]
        public XPCollection<DocumentoInterviniente> Intervinientes
        {
            get { return GetCollection<DocumentoInterviniente>("Intervinientes"); }
        }

        [Association]
        [Aggregated]
        public XPCollection<DocumentoCuota> Cuotas
        {
            get
            {
                var documentoCuotas = GetCollection<DocumentoCuota>("Cuotas");
                return documentoCuotas;
            }
        }

        [Association]
        [Aggregated]
        public XPCollection<DocumentoPlan> Planes
        {
            get { return GetCollection<DocumentoPlan>("Planes"); }
        }

        [PersistentAlias("Importes[NOT ISNULL(Tipo.ConceptoFacturacion)].Sum(Importe)")]
        public decimal ImporteTotal
        {
            get { return Convert.ToDecimal(EvaluateAlias("ImporteTotal")); }
        }

        public void RecalcularImportes(DocumentoImporteBase excluir)
        {
            DocumentoImporteBase.RefreshImportes(Importes, excluir);
        }

        [Browsable(false)]
        public Empresa Empresa
        {
            get { return fEmpresa; }
            set { SetPropertyValue("Empresa", ref fEmpresa, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            //Valores iniciales
            Estado = Identificadores.GetInstance(Session).DocumentoEstadoInicial;
            CantidadCuotas = 1;
        }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);

            if (CanRaiseOnChanged)
            {
                //Al modificar la propiedad Estado, activar una flag
                if (propertyName == "Estado") CambioDeEstado = true;
            }
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();

            //Si es el ultimo documento, eliminar la poliza
            if (Poliza.Documentos.Count == 1) Poliza.Delete();
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();

            //Restablecer flag de modificación de importes/cuotas
            CambioImportesOCuotas = false;
            //Restablecer flag de modificación de estado
            CambioDeEstado = false;
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            #region Consideraciones al guardar un documento nuevo

            if (Session.IsNewObject(this))
            {
                //Asignar numero secuencial de solicitud, (secuencia por empresa)
                NumeroSolicitud = GetProxNroSolicitud();

                //Si la poliza y su carpeta contenedoras, se estan creando con este documento, arrastrar el numero de solicitud a esos dos niveles
                if (Poliza != null)
                {
                    if (Session.IsNewObject(Poliza))
                        Poliza.NumeroSolicitud = NumeroSolicitud;

                    if (Poliza.Carpeta != null && Session.IsNewObject(Poliza.Carpeta))
                        Poliza.Carpeta.NumeroSolicitud = NumeroSolicitud;
                }
            }

            #endregion

            //Coherencia: si existe un comprobante asociado, y se intenta guardar una modificación que lo, regenerar el comprobante
            if (CambioImportesOCuotas && Comprobante != null)
            {
                if (Comprobante.TienePagosAplicados)
                    throw new UserFriendlyException(
                        "No se puede guardar esta modificación al documento, porque ya tiene pagos aplicados.");

                ActualizarComprobanteVenta(Comprobante);
            }

            //Cambio de estado
            if (CambioDeEstado)
            {
                var estado = Estado;

                if (estado.AccionGeneraComprobante)
                {
                    GenerarComprobanteVenta();
                }

                if (estado.AccionConfirma)
                {
                    Confirmar();
                }
            }
        }

        protected override void OnSaved()
        {
            base.OnSaved();
        }

        /// <summary>
        ///     Notifica a este documento que se modificaron sus importes
        /// </summary>
        public void OnImporteModificado()
        {
            //Al modificar importes, activar una flag de comprobante modificado
            CambioImportesOCuotas = true;

            GenerarCuotas(); //TODO: siempre automatico? quiza parametrizable
            OnChanged();
        }

        /// <summary>
        ///     Notifica a este documento que se modificaron sus cuotas
        /// </summary>
        public void OnCuotasModificadas()
        {
            //Al modificar cuotas, activar una flag de comprobante modificado
            CambioImportesOCuotas = true;

            OnChanged();
        }

        public void ActualizarImportesSegunItems()
        {
            var subtotales = new Dictionary<int, decimal>();

            foreach (var itemImp in Items.SelectMany(item => item.Importes))
            {
                if (!subtotales.ContainsKey(itemImp.Tipo.Oid))
                    subtotales[itemImp.Tipo.Oid] = 0;
                subtotales[itemImp.Tipo.Oid] += itemImp.Importe;
            }

            Importes.Empty();

            foreach (var subt in subtotales)
                Importes.Add(new DocumentoImporte(Session)
                {
                    Importe = subt.Value,
                    Tipo = Session.GetObjectByKey<TipoImporte>(subt.Key)
                });
        }

        public int GetProxNroSolicitud()
        {
            var proximoNro =
                Session.Evaluate(typeof (Documento), CriteriaOperator.Parse("Max(NumeroSolicitud)"),
                    CriteriaOperator.Parse("Empresa = ?", Empresa)) ?? 0;
            return (int) proximoNro + 1;
        }

        /// <summary>
        ///     Vaciar el plan de plagos actual, y recrearlo, calculando las fechas e importes
        ///     Depende de la Fecha de Emision y de CantidadCuotas
        /// </summary>
        public void GenerarCuotas()
        {
            if (EmitidaFecha == null) return;

            var importeTotalItems = ImporteTotal;

            Cuotas.Empty();

            if (CantidadCuotas != 0)
            {
                var importeCuota = Math.Round(importeTotalItems/CantidadCuotas, 2);

                for (var c = 0; c < CantidadCuotas; c++)
                {
                    Cuotas.Add(new DocumentoCuota(Session)
                    {
                        Numero = c + 1,
                        Importe = importeCuota,
                        Fecha = EmitidaFecha.Value.AddMonths(c)
                    });
                }

                //Agregar a la primer cuota, la diferencia con el total, por redondeo a dos decimales
                Cuotas[0].Importe += ImporteTotal - importeCuota*CantidadCuotas;
            }
        }

        public void GenerarImportesPredeterminados()
        {
            Importes.Empty();

            var tipos = DocumentoImporteBase.ImportesPredeterminados(Session, Ramo, Subramo);

            foreach (
                var nuevoImp in
                    tipos.Select(
                        tipo =>
                            new DocumentoImporte(Session)
                            {
                                Tipo = Session.GetObjectByKey<TipoImporte>(tipo),
                                Importe = 0
                            }))
                Importes.Add(nuevoImp);
        }

        [Action]
        public void Confirmar()
        {
            if (Comprobante == null)
                throw new UserFriendlyException(
                    string.Format(
                        "No se puede pasar al estado {0}. El documento aún no tiene comprobante generado.\nEsto puede deberse a una configuración inválida. Comunique el error al Administrador.",
                        Estado.Nombre));

            Poliza.CalcularPolizaVigente();
        }

        [Action]
        public void GenerarComprobanteVenta()
        {
            if (Comprobante != null)
                throw new UserFriendlyException(
                    string.Format(
                        "No se puede pasar al estado {0}. El documento aún no tiene comprobante generado.\nEsto puede deberse a una configuración inválida. Comunique el error al Administrador.",
                        Estado.Nombre));

            if (Cuotas.Count == 0)
                throw new UserFriendlyException("Debe ingresar al menos una cuota.");

            if (EmitidaFecha == null)
                throw new UserFriendlyException("Debe ingresar la Fecha de Emisión.");

            var talonario = Identificadores.GetInstance(Session).TalonarioDocumentos;
            if (talonario == null)
                throw new UserFriendlyException(
                    "Debe indicar el talonario asignado para los comprobantes de venta de documentos.");

            var nuevoComprobante = new Comprobante(Session)
            {
                Propio = false,
                Destinatario = Poliza.Tomador.Interviniente,
                Fecha = EmitidaFecha.Value,
                Tipo = talonario.ComprobanteTipo,
                Sector = talonario.Sector,
                Especie = Poliza.Especie,
                Cambio = Poliza.Cambio,
                CantidadCuotas = CantidadCuotas,
                Estado = ComprobanteEstado.Confirmado
            };

            ActualizarComprobanteVenta(nuevoComprobante);

            Comprobante = nuevoComprobante;
        }

        public void ActualizarComprobanteVenta(Comprobante comprobante)
        {
            comprobante.Items.Empty();

            foreach (var nuevoItem in from importe in Importes
                where importe.Tipo != null && importe.Tipo.ConceptoFacturacion != null
                select new ComprobanteItem(Session)
                {
                    Concepto = importe.Tipo.ConceptoFacturacion,
                    Cantidad = 1,
                    PrecioUnitario = importe.Importe
                })
            {
                nuevoItem.ActualizarDescripcion();
                nuevoItem.ActualizarImporteTotal();
                comprobante.Items.Add(nuevoItem);
            }

            comprobante.Cuotas.Empty();

            foreach (var cuota in Cuotas)
            {
                var nuevaCuota = new ComprobanteCuota(Session)
                {
                    Numero = cuota.Numero,
                    Fecha = cuota.Fecha,
                    Importe = cuota.Importe
                };
                cuota.ComprobanteCuota = nuevaCuota;
                comprobante.Cuotas.Add(nuevaCuota);
            }

            //Si se llegó a este punto, desde una sesión hija (ej. modificando una cuota puntual), no se debe guardar el comprobante aún
            //(porque la validación podría fallar, si estamos variando el total del monto de cuotas)
            //Solo guardar y validar cuando se guarda la sesión principal (documento)
            if (!(Session is NestedUnitOfWork))
                comprobante.Save();
        }
    }
}