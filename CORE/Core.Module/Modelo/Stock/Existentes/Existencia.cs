//using DevExpress.Persistent.Base;
//using DevExpress.Xpo;

//namespace FDIT.Core.Stock
//{
//  [Persistent( @"stock.Existencia" )]
//  [System.ComponentModel.DisplayName( "Existencia en stock" )]
//  public class Existencia : BasicObject
//  {
//    private Almacen fAlmacen;
//    private Articulo fArticulo;
//    private decimal fCantidad;

//    public Existencia( Session session )
//      : base( session )
//    {
//    }

//    [Association]
//    [System.ComponentModel.DisplayName( "Almacén" )]
//    public Almacen Almacen
//    {
//      get { return fAlmacen; }
//      set { SetPropertyValue( "Almacen", ref fAlmacen, value ); }
//    }

//    [Association]
//    [System.ComponentModel.DisplayName( "Artículo" )]
//    public Articulo Articulo
//    {
//      get { return fArticulo; }
//      set { SetPropertyValue( "Articulo", ref fArticulo, value ); }
//    }

//    [System.ComponentModel.DisplayName( "Cantidad" )]
//    public decimal Cantidad
//    {
//      get { return fCantidad; }
//      set { SetPropertyValue< decimal >( "Cantidad", ref fCantidad, value ); }
//    }

//    public override void AfterConstruction( )
//    {
//      base.AfterConstruction( );
//    }
//  }
//}