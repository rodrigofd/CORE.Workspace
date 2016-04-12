using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ System.ComponentModel.DisplayName( "Articulo Serie" ) ]
  [ Persistent( @"stock.ArticuloSerie" ) ]
  public class ArticuloSerie : BasicObject
  {
    private Articulo fArticulo;
    private Partida fPartida;
    private Serie fSerie;

    public ArticuloSerie( Session session ) : base( session )
    {
    }

    [ Association( @"Articulos_SeriesReferencesArticulos" ) ]
    public Articulo Articulo
    {
      get { return fArticulo; }
      set { SetPropertyValue( "IdArticulo", ref fArticulo, value ); }
    }

    [ Association( @"Articulos_SeriesReferencesSeries" ) ]
    public Serie Serie
    {
      get { return fSerie; }
      set { SetPropertyValue( "Serie", ref fSerie, value ); }
    }

    [ Association( @"Articulos_SeriesReferencesPartidas" ) ]
    public Partida Partida
    {
      get { return fPartida; }
      set { SetPropertyValue( "Partida", ref fPartida, value ); }
    }

    [ Association( @"MovimientosReferencesArticulos_Series", typeof( Movimiento ) ) ]
    public XPCollection< Movimiento > Movimientos
    {
      get { return GetCollection< Movimiento >( "Movimientos" ); }
    }
  }
}
