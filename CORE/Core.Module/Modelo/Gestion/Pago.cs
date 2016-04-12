using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace FDIT.Core.Gestion
{
  [ Persistent( @"gestion.Pago" ) ]
  [ System.ComponentModel.DisplayName( "Pago" ) ]
  public class Pago : Fondos.Comprobante
  {
    private Comprobante fComprobanteAnticipo;

    public Pago( Session session ) : base( session )
    {
    }

    public Comprobante ComprobanteAnticipo
    {
      get { return fComprobanteAnticipo; }
      set { SetPropertyValue( "ComprobanteAnticipo", ref fComprobanteAnticipo, value ); }
    }

    [ Aggregated ]
    [ Association ]
    [ CollectionOperationSet( AllowAdd = false, AllowRemove = true ) ]
    public XPCollection< PagoAplicacion > Aplicaciones
    {
      get
      {
        var pagoAplicaciones = GetCollection< PagoAplicacion >( "Aplicaciones" );
        return pagoAplicaciones;
      }
    }

    [ Aggregated ]
    [ Association ]
    [ CollectionOperationSet( AllowAdd = false, AllowRemove = true ) ]
    public XPCollection< PagoImpuesto > Impuestos
    {
      get { return GetCollection< PagoImpuesto >( "Impuestos" ); }
    }

    protected override XPCollection< T > CreateCollection< T >( XPMemberInfo property )
    {
      //interceptar la creación de colección Valores; para actuar ante cualquier A/B/M de los mismos (totalizadores)
      var collection = base.CreateCollection< T >( property );

      if( property.Name == "Aplicaciones" )
        collection.ListChanged += OnPagoAplicacionesChanged;

      return collection;
    }

    protected virtual void OnPagoAplicacionesChanged( object sender, ListChangedEventArgs e )
    {
    }

    public Dictionary< int, decimal > CalcularSaldoPago( XPView saldosAplicaciones, XPView saldoValores )
    {
      var saldo = new Dictionary< int, decimal >( );

      foreach( ViewRecord row in saldosAplicaciones )
      {
        var moneda = ( ( int ) row[ "Moneda" ] );
        if( !saldo.ContainsKey( moneda ) ) saldo[ moneda ] = 0;
        saldo[ moneda ] += ( decimal ) row[ "Importe" ];
      }

      foreach( ViewRecord row in saldoValores )
      {
        var moneda = ( ( int ) row[ "Moneda" ] );
        if( !saldo.ContainsKey( moneda ) ) saldo[ moneda ] = 0;
        saldo[ moneda ] -= ( decimal ) row[ "Importe" ];
      }

      return saldo;
    }

    public XPView CalcularSaldosAplicaciones( )
    {
      var subtotalesAplic = new XPView( Session, typeof( PagoAplicacion ) );
      subtotalesAplic.AddProperty( "Importe", CriteriaOperator.Parse( "SUM(Importe)" ) );
      subtotalesAplic.AddProperty( "Moneda", "Moneda", true );
      subtotalesAplic.Criteria = CriteriaOperator.Parse( "Pago = ?", Oid );

      return subtotalesAplic;
    }

    public XPView CalcularSaldosValores( )
    {
      var subtotalesVal = new XPView( Session, typeof( Fondos.ComprobanteItem ) );
      subtotalesVal.AddProperty( "Importe", CriteriaOperator.Parse( "SUM(Importe * DebeHaber)" ) );
      subtotalesVal.AddProperty( "Moneda", "Especie.Moneda", true );
      subtotalesVal.Criteria = CriteriaOperator.Parse( "Autogenerado = FALSE AND Comprobante = ?", Oid ); //recordar que yo (Pago) también es un Comprobante de fondos

      return subtotalesVal;
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
