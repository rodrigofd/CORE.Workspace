using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.ServiceModel.Description;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using FDIT.Core.AFIP.WebServices.WSFE;
using FDIT.Core.AFIP.WebServices.WSMTXCA;
using FDIT.Core.Controllers.Ventas;
using FDIT.Core.Fondos;
using FDIT.Core.Gestion;
using Comprobante = FDIT.Core.Ventas.Comprobante;

namespace FDIT.Core.AFIP.Controllers
{
  public class MonedaCotizacionController : ViewController
  {
    public const string ServiceName = "wsmtxca";

    private IContainer components;
    private SimpleAction obtenerCotizacionAction;
    private MTXCAServicePortTypeClient service;

    public MonedaCotizacionController( )
    {
      InitializeComponent( );

      RegisterActions( components );
    }

    private void InitializeComponent( )
    {
      components = new Container( );
      obtenerCotizacionAction = new SimpleAction( components );
      // 
      // recuperarAutorizacionAction
      // 
      obtenerCotizacionAction.Caption = "Obtener cotización de AFIP";
      obtenerCotizacionAction.ConfirmationMessage = null;
      obtenerCotizacionAction.Id = "WSMTXCAObtenerCotizacionActionn";
      obtenerCotizacionAction.ImageName = null;
      obtenerCotizacionAction.Shortcut = null;
      obtenerCotizacionAction.Tag = null;
      obtenerCotizacionAction.TargetObjectsCriteria = null;
      obtenerCotizacionAction.TargetViewId = null;
      obtenerCotizacionAction.ToolTip = null;
      obtenerCotizacionAction.TypeOfView = null;
      obtenerCotizacionAction.Execute += ObtenerCotizacionActionExecute;
      // 
      // AutorizarComprobanteVentaController
      // 
      TargetObjectType = typeof( Fondos.Especie );
      TargetViewNesting = Nesting.Root;
    }

    protected override void OnActivated( )
    {
      base.OnActivated( );

      service = new MTXCAServicePortTypeClient( );
      ServicePointManager.ServerCertificateValidationCallback += ( sender, certificate, chain, errors ) => true;
    }

    private void ObtenerCotizacionActionExecute( object sender, SimpleActionExecuteEventArgs e )
    {
      //cargar identificadores y sesión de la AFIP. Si no hay una válida, se inicia una nueva sesión y se guardan los cambios en la DB
      var objSpace = ( XPObjectSpace ) Application.CreateObjectSpace( );
      var identificadores = Identificadores.GetInstance( objSpace );

      if( identificadores.FacturaElectronicaEnte == null )
        throw new Exception( "Ente de factura electrónica no definida para esta empresa" );

      var sesionWsmtxca = identificadores.FacturaElectronicaEnte.GetSesion( ServiceName );
      objSpace.CommitChanges( );

      var especie = ( Especie ) View.CurrentObject;

      CodigoDescripcionType[ ] arrayObservaciones;
      CodigoDescripcionType[ ] arrayErrores;
      CodigoDescripcionType evento;

      if( especie.Moneda == null ) throw new UserFriendlyException( "La especie actual no tiene moneda asignada." );

      var monedaAfip = especie.Moneda.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) as AFIP.Moneda;
      if( monedaAfip == null ) throw new UserFriendlyException( "La moneda asociada no tiene equivalencia con la AFIP." );

      var especiePredeterminada = Fondos.Identificadores.GetInstance( objSpace ).EspeciePredeterminada;
      if( especiePredeterminada == null)
        throw new UserFriendlyException( "No hay especie predeterminada definida en el sistema." );

      var cotiz = service.consultarCotizacionMoneda( GetAuth( sesionWsmtxca ), monedaAfip.Codigo, out arrayErrores, out evento );

      if( arrayErrores != null && arrayErrores.Length > 0 )
      {
        var err = arrayErrores.Aggregate( "", ( current, error ) => current + ( error.codigo + " - " + error.descripcion + "\n" ) );
        //TODO errores de Encoding. No encontre otra manera de arreglarlo
        err = err.Replace( "Ã³", "ó" ).Replace( "Ãº", "ú" );
        throw new Exception( "Error en la operación:\n" + err );
      }

      var ec = objSpace.CreateObject< EspecieCotizacion >( );
      ec.EspecieOrigen = objSpace.GetObject(especie);
      ec.EspecieDestino = especiePredeterminada;
      ec.Vendedor = cotiz;
      ec.Fecha = DateTime.Today;

      ec.Save( );
      objSpace.CommitChanges( );

      View.Refresh(  );
    }

    public AuthRequestType GetAuth( Sesion sesion )
    {
      return new AuthRequestType { cuitRepresentada = sesion.Ente.CUITInformante, token = sesion.Token, sign = sesion.Sign };
    }
  }
}
