using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ System.ComponentModel.DisplayName( "Movimiento de stock" ) ]
  [ Persistent( @"stock.Movimiento" ) ]
  public class Movimiento : BasicObject
  {
    private ArticuloSerie fArticuloSerie;
    private decimal fCantidad;
    private Comprobante fComprobante;
    private DepositoUbicacion fDepositoUbicacionDestino;
    private DepositoUbicacion fDepositoUbicacionOrigen;
    private decimal fPrecioUnitario;

    public Movimiento( Session session ) : base( session )
    {
    }

    [ Association( @"MovimientosReferencesComprobantes" ) ]
    public Comprobante Comprobante
    {
      get { return fComprobante; }
      set { SetPropertyValue( "Comprobante", ref fComprobante, value ); }
    }

    [ Association( @"MovimientosReferencesArticulos_Series" ) ]
    public ArticuloSerie ArticuloSerie
    {
      get { return fArticuloSerie; }
      set { SetPropertyValue( "IdArticuloSerie", ref fArticuloSerie, value ); }
    }

    public DepositoUbicacion DepositoUbicacionOrigen
    {
      get { return fDepositoUbicacionOrigen; }
      set { SetPropertyValue( "DepositoUbicacionOrigen", ref fDepositoUbicacionOrigen, value ); }
    }

    public DepositoUbicacion DepositoUbicacionDestino
    {
      get { return fDepositoUbicacionDestino; }
      set { SetPropertyValue( "DepositoUbicacionDestino", ref fDepositoUbicacionDestino, value ); }
    }

    public decimal Cantidad
    {
      get { return fCantidad; }
      set { SetPropertyValue< decimal >( "Cantidad", ref fCantidad, value ); }
    }

    public decimal PrecioUnitario
    {
      get { return fPrecioUnitario; }
      set { SetPropertyValue< decimal >( "PrecioUnitario", ref fPrecioUnitario, value ); }
    }
  }
}
