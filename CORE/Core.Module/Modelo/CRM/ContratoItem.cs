using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraCharts.Native;
using FDIT.Core.Stock;

namespace FDIT.Core.CRM
{
  [Persistent(@"crm.ContratoItem")]
  [DefaultProperty(@"Descripcion")]
  [System.ComponentModel.DisplayName("Item del contrato")]
  public class ContratoItem : BasicObject
  {
    private Articulo fArticulo;
    private decimal fCantidadExcedenteMax;
    private decimal fCantidadIncluida;
    private Contrato fContrato;
    private decimal fImporteFijo;
    private decimal fPrecioUnidadExcedente;
    private Servicio fServicio;
    private UnidadMedida fUnidadMedida;
    private DateTime? fVigenciaInicio;
    private DateTime? fVigenciaFin;

    public ContratoItem(Session session)
      : base(session)
    {
    }

    [PersistentAlias("Contrato.Descripcion + ': ' + ToStr(Oid)")]
    public string Descripcion
    {
      get { return Convert.ToString(EvaluateAlias("Descripcion")); }
    }

    [Association]
    public Contrato Contrato
    {
      get { return fContrato; }
      set { SetPropertyValue("Contrato", ref fContrato, value); }
    }

    public Servicio Servicio
    {
      get { return fServicio; }
      set { SetPropertyValue("Servicio", ref fServicio, value); }
    }

    public Articulo Articulo
    {
      get { return fArticulo; }
      set { SetPropertyValue("Articulo", ref fArticulo, value); }
    }

    public DateTime? VigenciaInicio
    {
      get { return fVigenciaInicio; }
      set { SetPropertyValue("VigenciaInicio", ref fVigenciaInicio, value); }
    }

    [RuleValueComparison("contrato_item_vig_inicio_fin", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, "VigenciaInicio", ParametersMode.Expression)]
    public DateTime? VigenciaFin
    {
      get { return fVigenciaFin; }
      set { SetPropertyValue("VigenciaFin", ref fVigenciaFin, value); }
    }

    public UnidadMedida UnidadMedida
    {
      get { return fUnidadMedida; }
      set { SetPropertyValue("UnidadMedida", ref fUnidadMedida, value); }
    }


    [ModelDefault("DisplayFormat", "{0:n4}")]
    [ModelDefault("EditMask", "n4")]
    public decimal CantidadIncluida
    {
      get { return fCantidadIncluida; }
      set { SetPropertyValue<decimal>("CantidadIncluida", ref fCantidadIncluida, value); }
    }

    public decimal ImporteFijo
    {
      get { return fImporteFijo; }
      set { SetPropertyValue<decimal>("ImporteFijo", ref fImporteFijo, value); }
    }

    public decimal PrecioUnidadExcedente
    {
      get { return fPrecioUnidadExcedente; }
      set { SetPropertyValue<decimal>("PrecioUnidadExcedente", ref fPrecioUnidadExcedente, value); }
    }

    [ModelDefault("DisplayFormat", "{0:n4}")]
    [ModelDefault("EditMask", "n4")]
    public decimal CantidadExcedenteMax
    {
      get { return fCantidadExcedenteMax; }
      set { SetPropertyValue<decimal>("CantidadExcedenteMax", ref fCantidadExcedenteMax, value); }
    }

    public override void AfterConstruction()
    {
      base.AfterConstruction();
    }
  }
}