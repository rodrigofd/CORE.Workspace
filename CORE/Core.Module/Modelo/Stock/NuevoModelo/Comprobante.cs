using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Gestion;
using FDIT.Core.Personas;

namespace FDIT.Core.Stock
{
  [ DefaultClassOptions ]
  [ NavigationItem( "Stock" ) ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Comprobante de stock" ) ]
  [ Persistent( @"stock.Comprobante" ) ]
  public class Comprobante : ComprobanteBase
  {
    private Deposito fDepositoDestino;
    private Deposito fDepositoOrigen;
    private Direccion fDireccionDestino;
    private Direccion fDireccionOrigen;

    public Comprobante( Session session ) : base( session )
    {
    }

    public Deposito DepositoOrigen
    {
      get { return fDepositoOrigen; }
      set { SetPropertyValue( "DepositoOrigen", ref fDepositoOrigen, value ); }
    }

    public Direccion DireccionOrigen
    {
      get { return fDireccionOrigen; }
      set { SetPropertyValue( "DireccionOrigen", ref fDireccionOrigen, value ); }
    }

    public Deposito DepositoDestino
    {
      get { return fDepositoDestino; }
      set { SetPropertyValue( "DepositoDestino", ref fDepositoDestino, value ); }
    }

    public Direccion DireccionDestino
    {
      get { return fDireccionDestino; }
      set { SetPropertyValue( "DireccionDestino", ref fDireccionDestino, value ); }
    }

    [ Aggregated ]
    [ Association( @"MovimientosReferencesComprobantes", typeof( Movimiento ) ) ]
    public XPCollection< Movimiento > Movimientos
    {
      get { return GetCollection< Movimiento >( "Movimientos" ); }
    }
  }
}
