using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using FDIT.Core.Compras;
using FDIT.Core.Controllers.Gestion;
using FDIT.Core.Gestion;
using FDIT.Core.Seguridad;
using Comprobante = FDIT.Core.Compras.Comprobante;

namespace FDIT.Core.Controllers.Compras
{
  public class OrdenPagoController : PagoController
  {
    public OrdenPagoController( )
    {
      TargetObjectType = typeof( OrdenPago );
    }

    protected override void GenerarPagoAnticipo( Dictionary< int, decimal > saldos )
    {
      //TODO: dependencia fuerte a XPO
      var session = ( ( XPObjectSpace ) ObjectSpace ).Session;
      var identificadores = Identificadores.GetInstance( ObjectSpace );

      var anticipoComprobanteTipo = identificadores.AnticipoComprobanteTipo;
      var anticipoConcepto = identificadores.AnticipoConcepto;
      var anticipoCuenta = identificadores.AnticipoCuenta;
      var empresaActual = CoreAppLogonParameters.Instance.EmpresaActual( ObjectSpace );
      var especiePredeterminada = Core.Fondos.Identificadores.GetInstance( ObjectSpace ).EspeciePredeterminada;

      if( anticipoComprobanteTipo == null || anticipoConcepto == null || anticipoCuenta == null || especiePredeterminada == null )
      {
        throw new UserFriendlyException( "Faltan valores de configuración para la generación de anticipos. Por favor revise." );
      }

      var compAnticipo = ObjectSpace.CreateObject< Comprobante >( );

      var ordenPago = ( OrdenPago ) View.CurrentObject;
      var originante = ordenPago.Destinatario;

      var ultimoNumero = session.Evaluate< Comprobante >( CriteriaOperator.Parse( "MAX(Numero)" ), CriteriaOperator.Parse( "Originante = ? AND Tipo = ? AND Sector = 1", originante, anticipoComprobanteTipo ) );
      ultimoNumero = ultimoNumero != null ? ( int ) ultimoNumero + 1 : 1;

      compAnticipo.Empresa = empresaActual;
      compAnticipo.Sector = 1;
      compAnticipo.Numero = ( int ) ultimoNumero;
      compAnticipo.Destinatario = empresaActual.Persona;
      compAnticipo.Originante = originante;
      compAnticipo.Cuenta = anticipoCuenta;
      compAnticipo.Especie = especiePredeterminada;
      compAnticipo.Cambio = 1;

      decimal valorTotalLocalAnticipo = 0;

      foreach( var saldo in saldos )
      {
        if( saldo.Key == especiePredeterminada.Oid )
        {
          valorTotalLocalAnticipo += saldo.Value;
        }
        else
        {
          var itm = ordenPago.Items.FirstOrDefault( item => item.Especie.Moneda.Oid == saldo.Key );
          if( itm == null ) //nunca deberia pasar esto
            throw new UserFriendlyException( "No se pudo convertir el saldo del pago, a un anticipo en moneda local." );

          valorTotalLocalAnticipo += saldo.Value * itm.Cambio;
        }
      }

      var comprobanteItem = ObjectSpace.CreateObject< ComprobanteItem >( );
      comprobanteItem.Cantidad = 1;
      comprobanteItem.Concepto = anticipoConcepto;
      comprobanteItem.PrecioUnitario = valorTotalLocalAnticipo;

      compAnticipo.Items.Add( comprobanteItem );

      ordenPago.ComprobanteAnticipo = compAnticipo;
    }
  }
}
