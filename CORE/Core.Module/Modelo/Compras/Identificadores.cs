using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Fondos;
using FDIT.Core.Gestion;
using FDIT.Core.Sistema;
using FDIT.Core.Util;

namespace FDIT.Core.Compras
{
  [ Persistent( @"compras.Identificadores" ) ]
  [ System.ComponentModel.DisplayName( "Preferencias de Compras" ) ]
  public class Identificadores : IdentificadoresBase< Identificadores >
  {
    private ComprobanteTipo fAnticipoComprobanteTipo;
    private Concepto fAnticipoConcepto;
    private Cuenta fAnticipoCuenta;
    private Especie fRetencionGanEspecie;
    private Cuenta fRetencionGanCuenta;
    private int fRetencionIibbbaEspecie;
    private int fRetencionIibbbaCuenta;
    private string fPagosEncabezado;
    private string fPagosPie;
    private CuentaEmail fCuentaEmailFacturacion;
    private PlantillaMensaje fPlantillaMensajeFacturacion;
    private string fRutaExpComprobantes;
    private ComprobanteTipo fTipoComprobanteOrdenPago;

    public Identificadores( Session Session ) : base( Session )
    {
    }

    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ DataSourceCriteria( "Modulo = 'Fondos'" ) ]
    public ComprobanteTipo TipoComprobanteOrdenPago
    {
      get { return fTipoComprobanteOrdenPago; }
      set { SetPropertyValue( "TipoComprobanteOrdenPago", ref fTipoComprobanteOrdenPago, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    [ DisplayName( "Ruta para exportación de comprobantes" ) ]
    public string RutaExpComprobantes
    {
      get { return fRutaExpComprobantes; }
      set { SetPropertyValue( "RutaExpComprobantes", ref fRutaExpComprobantes, value ); }
    }

    [ DataSourceCriteria( "Modulo = 'Gestion' OR Modulo = 'Compra'" ) ]
    public ComprobanteTipo AnticipoComprobanteTipo
    {
      get { return fAnticipoComprobanteTipo; }
      set { SetPropertyValue( "AnticipoComprobanteTipo", ref fAnticipoComprobanteTipo, value ); }
    }

    public Concepto AnticipoConcepto
    {
      get { return fAnticipoConcepto; }
      set { SetPropertyValue( "AnticipoConcepto", ref fAnticipoConcepto, value ); }
    }

    public Cuenta AnticipoCuenta
    {
      get { return fAnticipoCuenta; }
      set { SetPropertyValue( "AnticipoCuenta", ref fAnticipoCuenta, value ); }
    }

    public Cuenta RetencionGanCuenta
    {
      get { return fRetencionGanCuenta; }
      set { SetPropertyValue( "RetencionGanCuenta", ref fRetencionGanCuenta, value ); }
    }

    public Especie RetencionGanEspecie
    {
      get { return fRetencionGanEspecie; }
      set { SetPropertyValue( "RetencionGanEspecie", ref fRetencionGanEspecie, value ); }
    }

    public int RetencionIIBBBACuenta
    {
      get { return fRetencionIibbbaCuenta; }
      set { SetPropertyValue<int>( "RetencionIIBBBACuenta", ref fRetencionIibbbaCuenta, value ); }
    }

    public int RetencionIIBBBAEspecie
    {
      get { return fRetencionIibbbaEspecie; }
      set { SetPropertyValue< int >( "ComprasRetencionIibbBaEspecie", ref fRetencionIibbbaEspecie, value ); }
    }

    public CuentaEmail CuentaEmailFacturacion
    {
      get { return fCuentaEmailFacturacion; }
      set { SetPropertyValue( "CuentaEmailFacturacion", ref fCuentaEmailFacturacion, value ); }
    }

    public PlantillaMensaje PlantillaMensajeFacturacion
    {
      get { return fPlantillaMensajeFacturacion; }
      set { SetPropertyValue( "PlantillaMensajeFacturacion", ref fPlantillaMensajeFacturacion, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string PagosEncabezado
    {
      get { return fPagosEncabezado; }
      set { SetPropertyValue( "PagosEncabezado", ref fPagosEncabezado, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string PagosPie
    {
      get { return fPagosPie; }
      set { SetPropertyValue( "PagosPie", ref fPagosPie, value ); }
    }
  }
}
