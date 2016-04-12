using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Compras;

namespace FDIT.Core.Stock
{
  [ System.ComponentModel.DisplayName( "Articulo por proveedor" ) ]
  [ Persistent( @"stock.ArticuloPorProveedor" ) ]
  public class ArticuloPorProveedor : BasicObject
  {
    private string fCodigo;
    private string fDescripcion;
    private Articulo fArticulo;
    private Proveedor fProveedor;

    public ArticuloPorProveedor( Session session ) : base( session )
    {
    }

    [ Association( @"Articulos_por_ProveedoresReferencesArticulos" ) ]
    public Articulo Articulo
    {
      get { return fArticulo; }
      set { SetPropertyValue( "Articulo", ref fArticulo, value ); }
    }

    public Proveedor Proveedor
    {
      get { return fProveedor; }
      set { SetPropertyValue( "Proveedor", ref fProveedor, value ); }
    }

    [ Size( 50 ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ Size( 200 ) ]
    public string Descripcion
    {
      get { return fDescripcion; }
      set { SetPropertyValue( "Descripcion", ref fDescripcion, value ); }
    }
  }
}
