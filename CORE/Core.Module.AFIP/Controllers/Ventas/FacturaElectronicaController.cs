using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using FDIT.Core.AFIP.WebServices.WSFE;
using FDIT.Core.Controllers.Gestion;
using FDIT.Core.Controllers.Ventas;
using FDIT.Core.Gestion;
using Comprobante = FDIT.Core.Ventas.Comprobante;

namespace FDIT.Core.AFIP.Controllers
{
  public class FacturaElectronicaController : ViewController
  {
    private IContainer components;
    private SimpleAction recuperarAutorizacionAction;
    private ServiceSoapClient service;

    public FacturaElectronicaController( )
    {
      InitializeComponent( );

      RegisterActions( components );
    }

    private void InitializeComponent( )
    {
      components = new Container( );
      recuperarAutorizacionAction = new SimpleAction( components );
      // 
      // recuperarAutorizacionAction
      // 
      recuperarAutorizacionAction.Caption = "Recuperar autorización";
      recuperarAutorizacionAction.ConfirmationMessage = null;
      recuperarAutorizacionAction.Id = "WSFERecuperarAutorizacionAction";
      recuperarAutorizacionAction.ImageName = "email_authentication";
      recuperarAutorizacionAction.Shortcut = null;
      recuperarAutorizacionAction.Tag = null;
      recuperarAutorizacionAction.TargetObjectsCriteria = null;
      recuperarAutorizacionAction.TargetViewId = null;
      recuperarAutorizacionAction.ToolTip = null;
      recuperarAutorizacionAction.TypeOfView = null;
      recuperarAutorizacionAction.Execute += recuperarAutorizacionAction_Execute;
      // 
      // AutorizarComprobanteVentaController
      // 
      TargetObjectType = typeof( Comprobante );
      TargetViewNesting = Nesting.Root;
    }

    protected override void OnActivated( )
    {
      base.OnActivated( );

      service = new ServiceSoapClient( );
    }

    protected override void OnFrameAssigned( )
    {
      base.OnFrameAssigned( );

      Frame.GetController< ComprobanteController >( ).ComprobanteAutorizando += ComprobanteController_ComprobanteAutorizando;
    }

    private void ComprobanteController_ComprobanteAutorizando( object sender, ComprobanteAutorizandoArgs args )
    {
      var comprobante = (Comprobante)args.Comprobante;

      if( !Convert.ToBoolean( comprobante.Talonario.GetMemberValue( AFIPModule.PropertyNameFacturaElectronicaWsfeV1 ) ) )
        return;

      //cargar identificadores y sesión de la AFIP. Si no hay una válida, se inicia una nueva sesión y se guardan los cambios en la DB
      var objSpace = Application.CreateObjectSpace( );
      var identificadores = Identificadores.GetInstance( objSpace );

      if( identificadores.FacturaElectronicaEnte == null )
        throw new Exception( "Ente de factura electrónica no definida para esta empresa" );

      var sesionWsfe = identificadores.FacturaElectronicaEnte.GetSesion( Sesion.ServicioWSFE );
      objSpace.CommitChanges( );

      if( comprobante.AutorizadaResultado == ResultadoAutorizacion.Autorizada )
        throw new Exception( "Comprobante ya autorizado por la AFIP" );

      var resp = ObtenerAutorizacion( sesionWsfe, GetDetRequest( comprobante ) );

      comprobante.AutorizadaNotas = "";
      if( resp.Errors != null && resp.Errors.Count > 0 )
      {
        var err = resp.Errors.Aggregate( "", ( current, error ) => current + ( error.Code + " - " + error.Msg + "\n" ) );
        //TODO errores de Encoding. No encontre otra manera de arreglarlo
        err = err.Replace( "Ã³", "ó" ).Replace( "Ãº", "ú" );

        comprobante.AutorizadaNotas = err;
        throw new Exception( "Error de autorización:\n" + err );
      }

      if( resp.FeCabResp == null || resp.FeDetResp == null )
        throw new Exception( "Error de autorización:\nNo se obtuvieron datos" );

      comprobante.AutorizadaResultado = resp.FeCabResp.Resultado;

      if( resp.FeDetResp.Count == 1 )
      {
        if( resp.FeDetResp[ 0 ].Observaciones != null && resp.FeDetResp[ 0 ].Observaciones.Count > 0 )
        {
          var obs = resp.FeDetResp[ 0 ].Observaciones.Aggregate( "", ( current, observacion ) => current + ( observacion.Code + " - " + observacion.Msg + "\n" ) );
          comprobante.AutorizadaNotas += obs;
        }

        comprobante.AutorizadaCodigo = resp.FeDetResp[ 0 ].CAE;
        if( !string.IsNullOrWhiteSpace( resp.FeDetResp[ 0 ].CAEFchVto ) )
          comprobante.AutorizadaCodigoFecVto = new DateTime( Convert.ToInt32( resp.FeDetResp[ 0 ].CAEFchVto.Substring( 0, 4 ) ),
                                                                  Convert.ToInt32( resp.FeDetResp[ 0 ].CAEFchVto.Substring( 4, 2 ) ),
                                                                  Convert.ToInt32( resp.FeDetResp[ 0 ].CAEFchVto.Substring( 6, 2 ) ) );
      }

      args.Autorizado = comprobante.AutorizadaResultado == ResultadoAutorizacion.Autorizada;
    }

    public FEAuthRequest GetAuth( Sesion sesion )
    {
      var resp = new FEAuthRequest { Cuit = sesion.Ente.CUITInformante, Token = sesion.Token, Sign = sesion.Sign };
      return resp;
    }

    public int ObtenerUltimoNroComp( Sesion sesion, int PtoVta, int CbteTipo )
    {
      var resp = service.FECompUltimoAutorizado( GetAuth( sesion ), PtoVta, CbteTipo );
      return resp.CbteNro;
    }

    public FECAEResponse ObtenerAutorizacion( Sesion sesion, FECAERequest request )
    {
      var resp = service.FECAESolicitar( GetAuth( sesion ), request );
      return resp;
    }

    public FECompConsultaResponse ConsultarAutorizacion( Sesion sesion, FECompConsultaReq request )
    {
      var resp = service.FECompConsultar( GetAuth( sesion ), request );
      return resp;
    }

    public static FECAERequest GetDetRequest( Comprobante comprobante )
    {
      var req = new FECAERequest { FeCabReq = new FECAECabRequest { CantReg = 1, PtoVta = comprobante.Sector } };

      if( comprobante.Tipo != null )
        req.FeCabReq.CbteTipo = ( ( ComprobanteTipo ) comprobante.Tipo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ).Codigo;

      var detReq = new FECAEDetRequest( );

      detReq.CbteDesde = detReq.CbteHasta = comprobante.Numero;
      detReq.CbteFch = comprobante.Fecha.ToString( "yyyyMMdd" );
      detReq.FchServDesde = comprobante.InicioPrestacion.ToString( "yyyyMMdd" );
      detReq.FchServHasta = comprobante.FinPrestacion.ToString( "yyyyMMdd" );
      if( comprobante.Vencimiento != null )
        detReq.FchVtoPago = comprobante.Vencimiento.Value.ToString( "yyyyMMdd" );
      detReq.MonCotiz = ( double ) comprobante.Cambio;
      if( comprobante.Especie != null )
        detReq.MonId = ( ( Moneda ) comprobante.Especie.Moneda.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ).Codigo;

      if( comprobante.IdentificacionTipo != null )
        detReq.DocTipo = ( ( IdentificacionTipo ) comprobante.IdentificacionTipo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ).Codigo;
      detReq.DocNro = Convert.ToInt64( comprobante.IdentificacionNro );

      double impGrav = 0, impIva = 0, impTrib = 0, impNoGrav = 0, impExento = 0;

      foreach( var item in comprobante.Items )
      {
        switch( ( ConceptosGrupos ) item.Concepto.Grupo )
        {
          case ConceptosGrupos.NoGrav:
            impNoGrav += ( double ) item.ImporteTotal;
            break;
          case ConceptosGrupos.Exento:
            impExento += ( double ) item.ImporteTotal;
            break;
          case ConceptosGrupos.Grav0:
          case ConceptosGrupos.Grav105:
          case ConceptosGrupos.Grav21:
          case ConceptosGrupos.Grav27:
            impGrav += ( double ) item.ImporteTotal;
            break;
          case ConceptosGrupos.IVA0:
          case ConceptosGrupos.IVA105:
          case ConceptosGrupos.IVA21:
          case ConceptosGrupos.IVA27:
            impIva += ( double ) item.ImporteTotal;
            if( detReq.Iva == null ) detReq.Iva = new List< AlicIva >( );
            detReq.Iva.Add( new AlicIva { BaseImp = ( double ) item.BaseImponible, Id = Convert.ToInt32( item.Concepto.Grupo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ), Importe = ( double ) item.ImporteTotal } );
            break;
          default:
            if( !string.IsNullOrWhiteSpace( Convert.ToString( item.Concepto.Grupo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ) ) )
            {
              impTrib += ( double ) item.ImporteTotal;
              if( detReq.Tributos == null ) detReq.Tributos = new List< Tributo >( );
              detReq.Tributos.Add( new Tributo { BaseImp = ( double ) item.BaseImponible, Id = Convert.ToInt16( item.Concepto.Grupo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ), Importe = ( double ) item.ImporteTotal } );
            }
            break;
        }
      }

      detReq.ImpNeto = impGrav;
      detReq.ImpOpEx = impExento;
      detReq.ImpTotConc = impNoGrav;
      detReq.ImpIVA = impIva;
      detReq.ImpTrib = impTrib;
      detReq.Concepto = 2;

      detReq.ImpTotal = Math.Round( detReq.ImpNeto + detReq.ImpOpEx + detReq.ImpTotConc + detReq.ImpIVA + detReq.ImpTrib, 2 );

      req.FeDetReq = new List< FECAEDetRequest > { detReq };

      return req;
    }

    private void recuperarAutorizacionAction_Execute( object sender, SimpleActionExecuteEventArgs e )
    {
      //cargar identificadores y sesión de la AFIP. Si no hay una válida, se inicia una nueva sesión y se guardan los cambios en la DB
      var objSpace = ( XPObjectSpace ) Application.CreateObjectSpace( );
      var identificadores = Identificadores.GetInstance( objSpace );

      if( identificadores.FacturaElectronicaEnte == null )
        throw new Exception( "Ente de factura electrónica no definida para esta empresa" );

      var sesionWsfe = identificadores.FacturaElectronicaEnte.GetSesion( Sesion.ServicioWSFE );
      objSpace.CommitChanges( );

      var comprobante = ( Comprobante ) View.CurrentObject;

      //if ( comprobante.AutorizadaResultado == "A" )
      //  throw new Exception( "Comprobante ya autorizado" );

      var request = new FECompConsultaReq { CbteNro = comprobante.Numero, CbteTipo = Convert.ToInt32( ((ComprobanteTipo)comprobante.Tipo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip )).Codigo ), PtoVta = comprobante.Sector };

      var resp = ConsultarAutorizacion( sesionWsfe, request );

      if( resp.Errors != null && resp.Errors.Count > 0 )
      {
        var err = resp.Errors.Aggregate( "", ( current, error ) => current + ( error.Code + " - " + error.Msg + "\n" ) );
        //TODO errores de Encoding. No encontre otra manera de arreglarlo
        err = err.Replace( "Ã³", "ó" ).Replace( "Ãº", "ú" );

        comprobante.AutorizadaNotas = err;
        throw new Exception( "Error en la operación:\n" + err );
      }

      if( resp.ResultGet == null )
        throw new Exception( "Error en la operación:\nNo se obtuvieron datos para la consulta" );

      comprobante.AutorizadaResultado = resp.ResultGet.Resultado;
      comprobante.AutorizadaNotas = "";

      if( resp.ResultGet.Observaciones != null && resp.ResultGet.Observaciones.Count > 0 )
      {
        var obs = resp.ResultGet.Observaciones.Aggregate( "", ( current, observacion ) => current + ( observacion.Code + " - " + observacion.Msg + "\n" ) );
        comprobante.AutorizadaNotas += obs;
      }

      comprobante.AutorizadaCodigo = resp.ResultGet.CodAutorizacion;
      if( !string.IsNullOrWhiteSpace( resp.ResultGet.FchVto ) )
        comprobante.AutorizadaCodigoFecVto = new DateTime( Convert.ToInt32( resp.ResultGet.FchVto.Substring( 0, 4 ) ),
                                                           Convert.ToInt32( resp.ResultGet.FchVto.Substring( 4, 2 ) ),
                                                           Convert.ToInt32( resp.ResultGet.FchVto.Substring( 6, 2 ) ) );
    }
  }
}
