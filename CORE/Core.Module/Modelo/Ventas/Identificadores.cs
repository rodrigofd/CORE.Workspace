using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Fondos;
using FDIT.Core.Gestion;
using FDIT.Core.Sistema;
using FDIT.Core.Util;

namespace FDIT.Core.Ventas
{
  [ Persistent( @"ventas.Identificadores" ) ]
  [ System.ComponentModel.DisplayName( "Preferencias de Ventas" ) ]
  public class Identificadores : IdentificadoresBase< Identificadores >
  {
    private ComprobanteTipo fAnticipoComprobanteTipo;
    private Concepto fAnticipoConcepto;
    private Cuenta fAnticipoCuenta;
    private Cuenta fCuentaDeudoresPorVentas;
    private CuentaEmail fCuentaEmailFacturacion;
    private string fPatronConceptoRecibo;
    private Concepto fPercepcionIibbbaConcepto;
    private PlantillaMensaje fPlantillaMensajeFacturacion;
    private string fRutaExpComprobantes;
    private ComprobanteTipo fTipoComprobanteRecibo;

    public Identificadores( Session Session ) : base( Session )
    {
    }

    [ DataSourceCriteria( "Modulo = 'Fondos'" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    public ComprobanteTipo TipoComprobanteRecibo
    {
      get { return fTipoComprobanteRecibo; }
      set { SetPropertyValue( "TipoComprobanteRecibo", ref fTipoComprobanteRecibo, value ); }
    }

    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    public Cuenta CuentaDeudoresPorVentas
    {
      get { return fCuentaDeudoresPorVentas; }
      set { SetPropertyValue( "CuentaDeudoresPorVentas", ref fCuentaDeudoresPorVentas, value ); }
    }

    public string PatronConceptoRecibo
    {
      get { return fPatronConceptoRecibo; }
      set { SetPropertyValue( "PatronConceptoRecibo", ref fPatronConceptoRecibo, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    [ DisplayName( "Ruta para exportación de comprobantes" ) ]
    public string RutaExpComprobantes
    {
      get { return fRutaExpComprobantes; }
      set { SetPropertyValue( "RutaExpComprobantes", ref fRutaExpComprobantes, value ); }
    }

    [ DataSourceCriteria( "Modulo = 'Gestion' OR Modulo = 'Venta'" ) ]
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

    public Concepto PercepcionIIBBBAConcepto
    {
      get { return fPercepcionIibbbaConcepto; }
      set { SetPropertyValue( "PercepcionIIBBBAConcepto", ref fPercepcionIibbbaConcepto, value ); }
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
  }
}
