using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Ventas;

namespace FDIT.Core.CRM
{
  public enum ContratoEstado
  {
    Pendiente = 1,
    Vigente = 2,
    Cancelado = 3,
    EnSuspension = 4,
    Finalizado = 5,
    Renovado = 6,
  }

  public enum Periodicidad
  {
    Diaria,
    Semanal,
    Mensual,
    Bimestral,
    Trimestral,
    Cuatrimestral,
    Semestral,
    Anual,
    Bianual,
    UnicaVez
  }

  [ Persistent( @"crm.Contrato" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Descripcion" ) ]
  [ System.ComponentModel.DisplayName( "Contrato de cliente" ) ]
  public class Contrato : BasicObject
  {
    private Cliente fCliente;
    private string fCodigo;
    private ContratoEstado fEstado;
    private string fNombre;
    private Periodicidad fPeriodicidad;
    private DateTime? fVigenciaFin;
    private DateTime? fVigenciaInicio;

    public Contrato( Session session )
      : base( session )
    {
    }

    [PersistentAlias("Codigo + ' - ' + Nombre + ' (' + IsNull(ToStr(ConvertToStr(VigenciaInicio,103)),'') + '-' + IsNull(ToStr(ConvertToStr(VigenciaFin,103)),'') + ')'")]
    public string Descripcion
    {
      get { return Convert.ToString( EvaluateAlias( "Descripcion" ) ); }
    }

    [ Size( 50 ) ]
    [ Index(0) ]
    [VisibleInListView(false)]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ Size( 50 ) ]
    [Index(1)]
    [VisibleInListView(false)]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [ System.ComponentModel.DisplayName( "Inicio de vigencia" ) ]
    [VisibleInListView(false)]
    public DateTime? VigenciaInicio
    {
      get { return fVigenciaInicio; }
      set { SetPropertyValue( "VigenciaInicio", ref fVigenciaInicio, value ); }
    }

    [ RuleValueComparison( "vig_inicio_fin", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, "VigenciaInicio", ParametersMode.Expression ) ]
    [ System.ComponentModel.DisplayName( "Fin de vigencia" ) ]
    [VisibleInListView(false)]
    public DateTime? VigenciaFin
    {
      get { return fVigenciaFin; }
      set { SetPropertyValue( "VigenciaFin", ref fVigenciaFin, value ); }
    }

    public ContratoEstado Estado
    {
      get { return fEstado; }
      set { SetPropertyValue( "Estado", ref fEstado, value ); }
    }

    //[ VisibleInListView( true ) ]
    [ Association ]
    public Cliente Cliente
    {
      get { return fCliente; }
      set { SetPropertyValue( "Cliente", ref fCliente, value ); }
    }

    [ System.ComponentModel.DisplayName( "Periodicidad" ) ]
    public Periodicidad Periodicidad
    {
      get { return fPeriodicidad; }
      set { SetPropertyValue( "Periodicidad", ref fPeriodicidad, value ); }
    }

    [ System.ComponentModel.DisplayName( "Detalle" ) ]
    [ Aggregated ]
    [ Association ]
    public XPCollection< ContratoItem > Items
    {
      get { return GetCollection< ContratoItem >( "Items" ); }
    }

    [ System.ComponentModel.DisplayName( "Períodos de facturación" ) ]
    [ Aggregated ]
    [ Association ]
    public XPCollection< ContratoPeriodo > PeriodosFacturacion
    {
      get { return GetCollection< ContratoPeriodo >( "PeriodosFacturacion" ); }
    }

    public override void AfterConstruction()
    {
        base.AfterConstruction();

        Estado = ContratoEstado.Pendiente;
        Periodicidad = Periodicidad.Mensual;
    }	
  }
}
