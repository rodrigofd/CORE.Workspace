using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Stock;

namespace FDIT.Core.Compras
{
  [ Persistent( @"compras.ArticuloProveedor" ) ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Art. según proveedor" ) ]
  public class ArticuloProveedor : BasicObject
  {
    private Articulo fArticulo;
    private string fCodigo;
    private Proveedor fProveedor;

    public ArticuloProveedor( Session session )
      : base( session )
    {
    }

    [ System.ComponentModel.DisplayName( "Artículo" ) ]
    public Articulo Articulo
    {
      get { return fArticulo; }
      set { SetPropertyValue( "Articulo", ref fArticulo, value ); }
    }

    [ Association( @"ArticuloProveedorReferencesProveedores" ) ]
    [ System.ComponentModel.DisplayName( "Proveedor" ) ]
    public Proveedor Proveedor
    {
      get { return fProveedor; }
      set { SetPropertyValue( "Proveedor", ref fProveedor, value ); }
    }

    [ Size( 30 ) ]
    [ System.ComponentModel.DisplayName( "Cód. por proveedor" ) ]
    [ Index( 0 ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
