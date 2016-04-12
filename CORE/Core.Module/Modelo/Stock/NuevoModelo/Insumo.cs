using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ System.ComponentModel.DisplayName( "Insumo" ) ]
  [ Persistent( @"stock.Insumo" ) ]
  public class Insumo : BasicObject
  {
    private decimal fCantidad;
    private Articulo fArticulo;
    private Articulo fArticuloInsumo;

    public Insumo( Session session ) : base( session )
    {
    }

    [ Association( @"InsumosReferencesArticulos" ) ]
    public Articulo Articulo
    {
      get { return fArticulo; }
      set { SetPropertyValue( "Articulo", ref fArticulo, value ); }
    }

    public Articulo ArticuloInsumo
    {
      get { return fArticuloInsumo; }
      set { SetPropertyValue( "ArticuloInsumo", ref fArticuloInsumo, value ); }
    }

    public decimal Cantidad
    {
      get { return fCantidad; }
      set { SetPropertyValue< decimal >( "Cantidad", ref fCantidad, value ); }
    }
  }
}
