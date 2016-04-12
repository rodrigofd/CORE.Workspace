using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Fondos;
using FDIT.Core.Sistema;

namespace FDIT.Core.Seguros
{
    [ImageName("document_signature")]
    [Persistent("seguros.Poliza")]
    [NavigationItem("Seguros")]
    [DefaultProperty("NumeroSolicitud")]
    [DefaultClassOptions]
    public class Poliza : BasicObject, IObjetoPorEmpresa
    {
        private DocumentoInterviniente _aseguradora;
        private decimal _cambio;
        private Carpeta _carpeta;
        private Empresa _empresa;
        private Especie fEspecie;
        private LineaNegocio fLineaNegocio;
        private string fNumero;
        private int? fNumeroSolicitud;
        private DocumentoInterviniente fOrganizador;
        private Poliza fPolizaPiloto;
        private Poliza fPolizaRenovada;
        private Ramo fRamo;
        private bool fRenueva;
        private string fRenuevaMotivo;
        private DocumentoPilotoRol fRolPiloto;
        private Subramo fSubramo;
        private decimal fSumaAsegurada;
        private VentaTipo fTipoVenta;
        private DocumentoInterviniente fTomador;
        private DateTime? fVigenciaFin;
        private DateTime fVigenciaInicio;

        public Poliza(Session session)
            : base(session)
        {
        }

        public int? NumeroSolicitud
        {
            get { return fNumeroSolicitud; }
            set { SetPropertyValue("NumeroSolicitud", ref fNumeroSolicitud, value); }
        }

        [Size(20)]
        [System.ComponentModel.DisplayName("Nro. Póliza")]
        public string Numero
        {
            get { return fNumero; }
            set { SetPropertyValue("Numero", ref fNumero, value); }
        }

        [Association]
        public Carpeta Carpeta
        {
            get { return _carpeta; }
            set { SetPropertyValue("Carpeta", ref _carpeta, value); }
        }

        public Poliza PolizaRenovada
        {
            get { return fPolizaRenovada; }
            set { SetPropertyValue("PolizaRenovada", ref fPolizaRenovada, value); }
        }

        public Poliza PolizaPiloto
        {
            get { return fPolizaPiloto; }
            set { SetPropertyValue("PolizaPiloto", ref fPolizaPiloto, value); }
        }

        public DocumentoPilotoRol RolPiloto
        {
            get { return fRolPiloto; }
            set { SetPropertyValue("RolPiloto", ref fRolPiloto, value); }
        }

        [RuleRequiredField]
        public VentaTipo TipoVenta
        {
            get { return fTipoVenta; }
            set { SetPropertyValue("TipoVenta", ref fTipoVenta, value); }
        }

        public bool Renueva
        {
            get { return fRenueva; }
            set { SetPropertyValue("Renueva", ref fRenueva, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string RenuevaMotivo
        {
            get { return fRenuevaMotivo; }
            set { SetPropertyValue("RenuevaMotivo", ref fRenuevaMotivo, value); }
        }

        [ImmediatePostData]
        public DocumentoInterviniente Aseguradora
        {
            get { return _aseguradora; }
            set { SetPropertyValue("Aseguradora", ref _aseguradora, value); }
        }

        [ImmediatePostData]
        public DocumentoInterviniente Tomador
        {
            get { return fTomador; }
            set { SetPropertyValue("Tomador", ref fTomador, value); }
        }

        [ImmediatePostData]
        public DocumentoInterviniente Organizador
        {
            get { return fOrganizador; }
            set { SetPropertyValue("Organizador", ref fOrganizador, value); }
        }

        [RuleRequiredField]
        public LineaNegocio LineaNegocio
        {
            get { return fLineaNegocio; }
            set { SetPropertyValue("LineaNegocio", ref fLineaNegocio, value); }
        }

        [RuleRequiredField]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public Ramo Ramo
        {
            get { return fRamo; }
            set { SetPropertyValue("Ramo", ref fRamo, value); }
        }

        [RuleRequiredField]
        [DataSourceProperty("Ramo.Subramos")]
        [LookupEditorMode(LookupEditorMode.AllItems)]
        public Subramo Subramo
        {
            get { return fSubramo; }
            set { SetPropertyValue("Subramo", ref fSubramo, value); }
        }

        [RuleRequiredField]
        public Especie Especie
        {
            get { return fEspecie; }
            set { SetPropertyValue("Especie", ref fEspecie, value); }
        }

        [RuleRequiredField]
        [ModelDefault("DisplayFormat", "n4")]
        public decimal Cambio
        {
            get { return _cambio; }
            set { SetPropertyValue<decimal>("Cambio", ref _cambio, value); }
        }

        [Association]
        [Aggregated]
        public XPCollection<Siniestro> Siniestros => GetCollection<Siniestro>("Siniestros");

        [Association]
        public XPCollection<Documento> Documentos => GetCollection<Documento>("Documentos");

        [Association]
        [Aggregated]
        [System.ComponentModel.DisplayName("Items vigentes")]
        public XPCollection<PolizaItem> Items => GetCollection<PolizaItem>("Items");

        [Association]
        [Aggregated]
        [System.ComponentModel.DisplayName("Planes vigentes")]
        public XPCollection<PolizaPlan> Planes => GetCollection<PolizaPlan>("Planes");

        [Association]
        [Aggregated]
        [System.ComponentModel.DisplayName("Intervinientes vigentes")]
        public XPCollection<PolizaInterviniente> Intervinientes => GetCollection<PolizaInterviniente>("Intervinientes");

        [Browsable(false)]
        public Empresa Empresa
        {
            get { return _empresa; }
            set { SetPropertyValue("Empresa", ref _empresa, value); }
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();

            //eliminar la carpeta, si es la ultima poliza
            if (Carpeta.Polizas.Count == 1)
                Carpeta.Delete();
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            Especie = Fondos.Identificadores.GetInstance(Session).EspeciePredeterminada;
        }

        [Action(Caption = "Calcular Póliza Vigente")]
        public void CalcularPolizaVigente()
        {
            CalcularVersionesVigentes(typeof (DocumentoItem), typeof (PolizaItem), "PolizaItem", "Documento",
                "ItemVigente");
            CalcularVersionesVigentes(typeof (DocumentoInterviniente), typeof (PolizaInterviniente),
                "PolizaInterviniente", "Documento", "IntervinienteVigente");
            CalcularVersionesVigentes(typeof (DocumentoIntervinienteItem), typeof (PolizaIntervinienteItem),
                "PolizaIntervinienteItem", "DocumentoItem.Documento", "IntervinienteItemVigente");
            CalcularVersionesVigentes(typeof (DocumentoItemDetalle), typeof (PolizaItemDetalle), "PolizaItemDetalle",
                "DocumentoItem.Documento", "DetalleVigente");
            CalcularVersionesVigentes(typeof (DocumentoPlan), typeof (PolizaPlan), "PolizaPlan", "Documento",
                "PlanVigente");
            CalcularVersionesVigentes(typeof (DocumentoPlanDetalle), typeof (PolizaPlanDetalle), "PolizaPlanDetalle",
                "DocumentoPlan.Documento", "DetalleVigente");

            //Calcular fecha
            var v = new XPView(Session, typeof (Documento))
            {
                Criteria = CriteriaOperator.Parse("Poliza = ? AND NOT ISNULL(EmitidaFecha)", this)
            };
            v.AddProperty("MaxVigenciaInicio", CriteriaOperator.Parse("MAX(VigenciaInicio)"));
            v.AddProperty("MaxVigenciaFin", CriteriaOperator.Parse("MAX(VigenciaFin)"));

            foreach (ViewRecord version in v)
            {
                if (version["MaxVigenciaInicio"] != null)
                    VigenciaInicio = (DateTime) version["MaxVigenciaInicio"];
                VigenciaFin = version["MaxVigenciaFin"] as DateTime?;
            }
            Save();
        }

        private void CalcularVersionesVigentes(Type tipoObjMovimiento,
            Type tipoObjActual,
            string propName,
            string rutaDocumento,
            string propVigente)
        {
            var historial = new Dictionary<int, int>();

            var v = new XPView(Session, tipoObjMovimiento)
            {
                Criteria = CriteriaOperator.Parse($"{rutaDocumento}.Poliza = ? AND NOT ISNULL({propName})", this)
            };

            v.AddProperty(propName, propName, false, true, SortDirection.Ascending);
            v.AddProperty("EmitidaFecha", $"{rutaDocumento}.EmitidaFecha", false, true, SortDirection.Ascending);
            v.AddProperty("NumeroSolicitud", $"{rutaDocumento}.NumeroSolicitud", false, true, SortDirection.Ascending);
            v.AddProperty("Oid", "Oid", false, true, SortDirection.None);

            foreach (ViewRecord version in v)
            {
                var pi = (int) version[0];
                if (!historial.ContainsKey(pi)) historial[pi] = -1;
                historial[pi] = (int) version[3];
            }

            foreach (var elemento in historial)
            {
                var pi = (BasicObject) Session.GetObjectByKey(tipoObjActual, elemento.Key);
                pi.SetMemberValue(propVigente, Session.GetObjectByKey(tipoObjMovimiento, elemento.Value));
            }
        }

        #region Redundancia (los valores vigentes del ultimo endoso)

        public DateTime VigenciaInicio
        {
            get { return fVigenciaInicio; }
            set { SetPropertyValue<DateTime>("VigenciaInicio", ref fVigenciaInicio, value); }
        }

        public DateTime? VigenciaFin
        {
            get { return fVigenciaFin; }
            set { SetPropertyValue("VigenciaFin", ref fVigenciaFin, value); }
        }

        [PersistentAlias(
            "IIF(ISNULL(VigenciaInicio) OR ISNULL(VigenciaFin), 0, DATEDIFFDAY(VigenciaInicio, VigenciaFin))")]
        public int VigenciaDias => (int) EvaluateAlias("VigenciaDias");

        [PersistentAlias(
            "IIF(ISNULL(VigenciaInicio) OR ISNULL(VigenciaFin), 0, DATEDIFFMONTH(VigenciaInicio, VigenciaFin))")]
        public int VigenciaMeses => (int) EvaluateAlias("VigenciaMeses");

        [PersistentAlias("IIF(ISNULL(VigenciaFin), 0, DATEDIFFDAY(VigenciaFin, LOCALDATETIMETODAY()))")]
        public int VigenciaDiasAlVencimiento => (int) EvaluateAlias("VigenciaDiasAlVencimiento");

        [ModelDefault("DisplayFormat", "n2")]
        public decimal SumaAsegurada
        {
            get { return fSumaAsegurada; }
            set { SetPropertyValue<decimal>("SumaAsegurada", ref fSumaAsegurada, value); }
        }

        #endregion
    }
}