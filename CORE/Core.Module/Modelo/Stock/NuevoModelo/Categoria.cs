using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ DefaultClassOptions ]
  [ NavigationItem( "Stock" ) ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Categoría de artículo" ) ]
  [ Persistent( @"stock.Categoria" ) ]
  public class Categoria : BasicObject
  {
    private Categoria fCategoriaPadre;
    private string fNombre;

    public Categoria( Session session ) : base( session )
    {
    }

    [ Size( 50 ) ]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [ Association( @"CategoriasReferencesCategorias" ) ]
    public Categoria CategoriaPadre
    {
      get { return fCategoriaPadre; }
      set { SetPropertyValue( "CategoriaPadre", ref fCategoriaPadre, value ); }
    }

    [ Association( @"ArticulosReferencesCategorias", typeof( Articulo ) ) ]
    public XPCollection< Articulo > Articulos
    {
      get { return GetCollection< Articulo >( "Articulos" ); }
    }

    [ Association( @"CategoriasReferencesCategorias", typeof( Categoria ) ) ]
    public XPCollection< Categoria > CategoriasHijas
    {
      get { return GetCollection<Categoria>( "CategoriasHijas" ); }
    }
  }
}
