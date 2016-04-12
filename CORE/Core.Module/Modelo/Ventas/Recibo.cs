using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Fondos;
using FDIT.Core.Gestion;
using FDIT.Core.Personas;
using ComprobanteItem = FDIT.Core.Fondos.ComprobanteItem;

namespace FDIT.Core.Ventas
{
  //[Appearance("CollectionPropertyDisable", TargetItems = "*;Recibo", Criteria = "1=1", Enabled = false)]
  [Appearance("VentasRecibosOcultarPagoImpuestos", AppearanceItemType.ViewItem, "1=1", Visibility = ViewItemVisibility.Hide, TargetItems = "Impuestos")]
  [ ImageName( "receipt_invoice" ) ]
  [ Persistent( @"ventas.Recibo" ) ]
  [ System.ComponentModel.DisplayName( "Recibo de cobranza" ) ]
  public class Recibo : Pago
  {
    public Recibo( Session session )
      : base( session )
    {
      ActualizarOrigDest( );
    }

    [ ImmediatePostData ]
    [ DataSourceProperty( "DestinatariosDisponibles" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    [ System.ComponentModel.DisplayName( "Cliente" ) ]
    [ RuleRequiredField ]
    public override Persona Destinatario
    {
      set
      {
        base.Destinatario = value;

        if( CanRaiseOnChanged )
        {
          if( value != null )
          {
            var patronConceptoRecibo = Identificadores.GetInstance( Session ).PatronConceptoRecibo;
            if( !string.IsNullOrEmpty( patronConceptoRecibo ) )
              Concepto = string.Format( patronConceptoRecibo, value.Nombre );
          }
        }
      }
    }

    [ PersistentAlias( "Items[DebeHaber='Debe'].SUM(ImporteAlCambio)" ) ]
    public decimal ImporteTotal
    {
      get { return ( decimal ) EvaluateAlias( "ImporteTotal" ); }
    }

    internal void ActualizarOrigDest( )
    {
      //Para Recibos, los destinatarios posibles son todos los clientes
      OriginantesDisponibles.Criteria = CriteriaOperator.Parse( "[<Empresa>][^.Oid = Persona.Oid]" );
      DestinatariosDisponibles.Criteria = CriteriaOperator.Parse( "[<Cliente>][^.Oid = Persona.Oid]" );
    }

    protected override void OnPagoAplicacionesChanged( object sender, ListChangedEventArgs e )
    {
      var cuentaDeudoresPorVentas = Identificadores.GetInstance( Session ).CuentaDeudoresPorVentas;
      if( cuentaDeudoresPorVentas == null ) return;

      Items.ToList( ).ForEach( comprobanteItem => { if( comprobanteItem.Cuenta.Oid == cuentaDeudoresPorVentas.Oid ) comprobanteItem.Delete( ); } );

      var importeAplicado = Evaluate( "Aplicaciones.SUM(ImporteAplicado*Cuota.Comprobante.Tipo.DebitoCredito)" );

      if( importeAplicado == null ) return;

      var aplicado = ( decimal ) importeAplicado;

      Items.Add( new ComprobanteItem( Session )
                 {
                   Cuenta = cuentaDeudoresPorVentas,
                   Importe = Math.Abs( aplicado ),
                   DebeHaber = aplicado > 0 ? DebeHaber.Haber : DebeHaber.Debe,
                   Autogenerado = true,
                   Cambio = 1
                 } );
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Tipo = Identificadores.GetInstance( Session ).TipoComprobanteRecibo;
    }
  }
}
