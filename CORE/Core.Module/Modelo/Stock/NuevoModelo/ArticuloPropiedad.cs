using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [System.ComponentModel.DisplayName( "Propiedad de artículo" )]
  [ Persistent( @"stock.ArticuloPropiedad" ) ]
  public class ArticuloPropiedad : BasicObject
  {
    private Articulo fArticulo;
    private ClasePropiedad fClasePropiedad;
    private string fValor;

    public ArticuloPropiedad( Session session ) : base( session )
    {
    }

    [ Association( @"Articulos_PropiedadesReferencesArticulos" ) ]
    public Articulo Articulo
    {
      get { return fArticulo; }
      set { SetPropertyValue( "Articulo", ref fArticulo, value ); }
    }

    public ClasePropiedad ClasePropiedad
    {
      get { return fClasePropiedad; }
      set { SetPropertyValue( "ClasePropiedad", ref fClasePropiedad, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Valor
    {
      get { return fValor; }
      set { SetPropertyValue( "Valor", ref fValor, value ); }
    }
  }
}
