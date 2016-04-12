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
using FDIT.Core.Controllers.Gestion;
using FDIT.Core.Controllers.Ventas;
using FDIT.Core.Gestion;
using Comprobante = FDIT.Core.Ventas.Comprobante;

namespace FDIT.Core.AFIP.Controllers
{
  public class FacturaElectronicaDetalleController : ViewController
  {
    public const string ServiceName = "wsmtxca";

    private IContainer components;
    private SimpleAction recuperarAutorizacionAction;
    private MTXCAServicePortTypeClient service;

    public FacturaElectronicaDetalleController( )
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
      recuperarAutorizacionAction.Caption = "Recuperar autorización det";
      recuperarAutorizacionAction.ConfirmationMessage = null;
      recuperarAutorizacionAction.Id = "WSMTXCARecuperarAutorizacionAction";
      recuperarAutorizacionAction.ImageName = null;
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

      service = new MTXCAServicePortTypeClient( );
      ServicePointManager.ServerCertificateValidationCallback += ( sender, certificate, chain, errors ) => true;
    }

    protected override void OnFrameAssigned( )
    {
      base.OnFrameAssigned( );

      Frame.GetController<ComprobanteController>( ).ComprobanteAutorizando += ComprobanteController_ComprobanteAutorizando;
    }

    private void ComprobanteController_ComprobanteAutorizando( object sender, ComprobanteAutorizandoArgs args )
    {
      var comprobante = ( Comprobante ) args.Comprobante;

      if( !Convert.ToBoolean( comprobante.Talonario.GetMemberValue( AFIPWsMtxcaModule.PropertyNameFacturaElectronicaMtxca ) ) )
        return;

      //cargar identificadores y sesión de la AFIP. Si no hay una válida, se inicia una nueva sesión y se guardan los cambios en la DB
      var objSpace = Application.CreateObjectSpace( );
      var identificadores = Identificadores.GetInstance( objSpace );

      if( identificadores.FacturaElectronicaEnte == null )
        throw new Exception( "Ente de factura electrónica no definida para esta empresa" );

      var sesionWsmtxca = identificadores.FacturaElectronicaEnte.GetSesion( ServiceName );
      objSpace.CommitChanges( );

      if( comprobante.AutorizadaResultado == ResultadoAutorizacion.Autorizada )
        throw new Exception( "Comprobante ya autorizado por la AFIP" );

      ComprobanteCAEResponseType comprobanteResponse;
      CodigoDescripcionType[ ] arrayObservaciones;
      CodigoDescripcionType[ ] arrayErrores;
      CodigoDescripcionType evento;

      var resp = service.autorizarComprobante( GetAuth( sesionWsmtxca ),
                                               GetComprobanteType( ( Comprobante ) sender ),
                                               out comprobanteResponse,
                                               out arrayObservaciones,
                                               out arrayErrores,
                                               out evento );

      comprobante.AutorizadaNotas = "";
      if( arrayErrores != null && arrayErrores.Length > 0 )
      {
        var err = arrayErrores.Aggregate( "", ( current, error ) => current + ( error.codigo + " - " + error.descripcion + "\n" ) );
        //TODO errores de Encoding. No encontre otra manera de arreglarlo
        err = err.Replace( "Ã³", "ó" ).Replace( "Ãº", "ú" );

        comprobante.AutorizadaNotas = err;
        throw new Exception( "Error de autorización:\n" + err );
      }

      if( comprobanteResponse == null )
        throw new Exception( "Error de autorización:\nNo se obtuvieron datos" );

      comprobante.AutorizadaResultado = resp.ToString( );

      if( arrayObservaciones != null && arrayObservaciones.Length > 0 )
      {
        var obs = arrayObservaciones.Aggregate( "", ( current, observacion ) => current + ( observacion.codigo + " - " + observacion.descripcion + "\n" ) );
        comprobante.AutorizadaNotas += obs;
      }

      comprobante.AutorizadaCodigo = comprobanteResponse.CAE.ToString( );
      comprobante.AutorizadaCodigoFecVto = comprobanteResponse.fechaVencimientoCAE;

      args.Autorizado = comprobante.AutorizadaResultado == ResultadoAutorizacion.Autorizada;
    }

    private void recuperarAutorizacionAction_Execute( object sender, SimpleActionExecuteEventArgs e )
    {
      //cargar identificadores y sesión de la AFIP. Si no hay una válida, se inicia una nueva sesión y se guardan los cambios en la DB
      var objSpace = ( XPObjectSpace ) Application.CreateObjectSpace( );
      var identificadores = Identificadores.GetInstance( objSpace );

      if( identificadores.FacturaElectronicaEnte == null )
        throw new Exception( "Ente de factura electrónica no definida para esta empresa" );

      var sesionWsmtxca = identificadores.FacturaElectronicaEnte.GetSesion( ServiceName );
      objSpace.CommitChanges( );

      var comprobante = ( Comprobante ) View.CurrentObject;

      //if ( comprobante.AutorizadaResultado == "A" )
      //  throw new Exception( "Comprobante ya autorizado" );

      
      CodigoDescripcionType[ ] arrayObservaciones;
      CodigoDescripcionType[ ] arrayErrores;
      CodigoDescripcionType evento;

      var consultaComprobanteRequest = new ConsultaComprobanteRequestType( )
                                       {
                                         codigoTipoComprobante = Convert.ToInt16( ((ComprobanteTipo) comprobante.Tipo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip )).Codigo ),
                                         numeroPuntoVenta = ( short ) comprobante.Sector,
                                         numeroComprobante = comprobante.Numero
                                       };
      
      var resp = service.consultarComprobante( GetAuth( sesionWsmtxca ),
                                               consultaComprobanteRequest,
                                               out arrayObservaciones,
                                               out arrayErrores,
                                               out evento );

      if( arrayErrores != null && arrayErrores.Length > 0 )
      {
        var err = arrayErrores.Aggregate( "", ( current, error ) => current + ( error.codigo + " - " + error.descripcion + "\n" ) );
        //TODO errores de Encoding. No encontre otra manera de arreglarlo
        err = err.Replace( "Ã³", "ó" ).Replace( "Ãº", "ú" );

        comprobante.AutorizadaNotas = err;
        throw new Exception( "Error en la operación:\n" + err );
      }

      if( resp == null )
        throw new Exception( "Error en la operación:\nNo se obtuvieron datos para la consulta" );

      //comprobante.AutorizadaResultado = resp.;
      comprobante.AutorizadaNotas = "";

      if( arrayObservaciones != null && arrayObservaciones.Length > 0 )
      {
        var obs = arrayObservaciones.Aggregate( "", ( current, observacion ) => current + ( observacion.codigo + " - " + observacion.descripcion + "\n" ) );
        comprobante.AutorizadaNotas += obs;
      }

      comprobante.AutorizadaCodigo = resp.codigoAutorizacion.ToString( );
      comprobante.AutorizadaCodigoFecVto = resp.fechaVencimiento;
    }

    public AuthRequestType GetAuth( Sesion sesion )
    {
      return new AuthRequestType { cuitRepresentada = sesion.Ente.CUITInformante, token = sesion.Token, sign = sesion.Sign };
    }

    public static ComprobanteType GetComprobanteType( Comprobante comprobante )
    {
      var req = new ComprobanteType( );

      if( comprobante.Tipo != null )
        req.codigoTipoComprobante = ( ( ComprobanteTipo ) comprobante.Tipo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ).Codigo;

      if( comprobante.ConceptosIncluidos == ConceptosIncluidos.Otro )
        throw new UserFriendlyException( "La autorización electrónica por AFIP solo permite comprobantes de Productos, Servicios, o Prod. y Serv." );

      req.codigoConcepto = ( short ) comprobante.ConceptosIncluidos;

      req.numeroPuntoVenta = ( short ) comprobante.Sector;
      req.numeroComprobante = comprobante.Numero;
      req.fechaEmision = comprobante.Fecha;
      req.fechaServicioDesde = comprobante.InicioPrestacion;
      req.fechaServicioHasta = comprobante.FinPrestacion;

      if( comprobante.Vencimiento != null )
        req.fechaVencimientoPago = comprobante.Vencimiento.Value;

      req.cotizacionMoneda = comprobante.Cambio;

      if( comprobante.Especie != null )
        req.codigoMoneda = ( ( Moneda ) comprobante.Especie.Moneda.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ).Codigo;

      if( comprobante.IdentificacionTipo != null )
      {
        req.codigoTipoDocumento = ( ( IdentificacionTipo ) comprobante.IdentificacionTipo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ).Codigo;
        req.codigoTipoDocumentoSpecified = true;
      }
      if( !string.IsNullOrEmpty( comprobante.IdentificacionNro ) )
      {
        req.numeroDocumento = Convert.ToInt64( comprobante.IdentificacionNro );
        req.numeroDocumentoSpecified = true;
      }

      decimal impGrav = 0, impIVA = 0, impTrib = 0, impNoGrav = 0, impExento = 0;

      var arraySubtotalesIVA = new List<SubtotalIVAType>( );
      var arrayOtrosTributos = new List<OtroTributoType>( );
      var arrayItems = new List<ItemType>( );

      foreach( var item in comprobante.Items )
      {
        if( item.Concepto.Clase != ConceptoClase.Impuesto )
        {
          var itemType = new ItemType( );
          itemType.descripcion = item.Detalle;
          itemType.cantidad = item.Cantidad;
          itemType.cantidadSpecified = true;

          if( comprobante.Tipo.DiscriminaImpuestos )
          {
            itemType.precioUnitario = item.PrecioUnitario;
            itemType.importeBonificacion = item.ImporteDescuento;
            itemType.importeIVA = Math.Round( item.ImporteImpuestos, 2 );
            itemType.importeIVASpecified = true;
          }
          else
          {
            itemType.precioUnitario = item.PrecioUnitarioConImpuesto;
            itemType.importeBonificacion = item.ImporteDescuentoConImpuesto;
          }

          itemType.precioUnitarioSpecified = true;
          itemType.importeBonificacionSpecified = true;

          itemType.importeItem = Math.Round( item.ImporteTotalConImpuesto, 2 );

          itemType.codigoCondicionIVA = Convert.ToInt16( item.Concepto.AlicuotaImpuesto.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) );

          if( item.UnidadMedida == null )
            throw new UserFriendlyException( "Debe indicar unidad de medida para todos los items." );

          itemType.codigoUnidadMedida = ( short ) item.UnidadMedida.GetMemberValue( AFIPModule.PropertyNameCodigoAfip );
          itemType.unidadesMtx = ( int ) item.UnidadMedida.UnidadesFraccionamiento;
          itemType.unidadesMtxSpecified = true;

          if( item.Articulo != null )
          {
            itemType.codigo = item.Articulo.Codigo;
            itemType.codigoMtx = item.Articulo.CodigoLegal;
          }

          if( string.IsNullOrEmpty( itemType.codigoMtx ) )
          {
            itemType.codigoMtx = item.Concepto.CodigoLegal;
          }

          arrayItems.Add( itemType );
        }

        switch( ( ConceptosGrupos ) item.Concepto.Grupo )
        {
          case ConceptosGrupos.NoGrav:
            impNoGrav += item.ImporteTotal;
            break;
          case ConceptosGrupos.Exento:
            impExento += item.ImporteTotal;
            break;
          case ConceptosGrupos.Grav0:
          case ConceptosGrupos.Grav105:
          case ConceptosGrupos.Grav21:
          case ConceptosGrupos.Grav27:
            impGrav += item.ImporteTotal;
            break;
          case ConceptosGrupos.IVA0:
          case ConceptosGrupos.IVA105:
          case ConceptosGrupos.IVA21:
          case ConceptosGrupos.IVA27:
            impIVA += item.ImporteTotal;

            arraySubtotalesIVA.Add( new SubtotalIVAType { codigo = Convert.ToInt16( item.Concepto.Grupo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ), importe = Math.Round( item.ImporteTotal, 2 ) } );
            break;
          default:
            if( !string.IsNullOrWhiteSpace( Convert.ToString( item.Concepto.Grupo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ) ) )
            {
              impTrib += item.ImporteTotal;
              arrayOtrosTributos.Add( new OtroTributoType { baseImponible = item.BaseImponible, codigo = Convert.ToInt16( item.Concepto.Grupo.GetMemberValue( AFIPModule.PropertyNameCodigoAfip ) ), importe = Math.Round( item.ImporteTotal, 2 ) } );
            }
            break;
        }
      }

      req.arrayItems = arrayItems.Count > 0 ? arrayItems.ToArray( ) : null;
      req.arraySubtotalesIVA = arraySubtotalesIVA.Count > 0 ? arraySubtotalesIVA.ToArray( ) : null;
      req.arrayOtrosTributos = arrayOtrosTributos.Count > 0 ? arrayOtrosTributos.ToArray( ) : null;

      req.importeGravado = Math.Round( impGrav, 2 );
      req.importeGravadoSpecified = true;
      req.importeNoGravado = Math.Round( impNoGrav, 2 );
      req.importeNoGravadoSpecified = true;
      req.importeExento = Math.Round( impExento, 2 );
      req.importeExentoSpecified = true;
      req.importeSubtotal = req.importeGravado + req.importeNoGravado + req.importeExento;
      
      req.importeOtrosTributos = Math.Round( impTrib, 2 );
      req.importeOtrosTributosSpecified = arrayOtrosTributos.Count > 0;

      req.importeTotal = req.importeSubtotal + req.importeOtrosTributos + Math.Round( impIVA, 2 );

      return req;
    }

  }
}
