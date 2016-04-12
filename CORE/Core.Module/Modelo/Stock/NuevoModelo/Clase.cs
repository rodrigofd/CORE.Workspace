using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ DefaultClassOptions ]
  [ NavigationItem( "Stock" ) ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Clase de artículo" ) ]
  [ Persistent( @"stock.Clase" ) ]
  public class Clase : BasicObject
  {
    private string fNombre;

    public Clase( Session session ) : base( session )
    {
    }

    [ Size( 50 ) ]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Clase", ref fNombre, value ); }
    }

    [ Association( @"ArticulosReferencesClases", typeof( Articulo ) ) ]
    public XPCollection< Articulo > Articulos
    {
      get { return GetCollection< Articulo >( "Articulos" ); }
    }

    [ Aggregated ]
    [ Association( @"Clases_por_PropiedadesReferencesClases", typeof( ClasePropiedad ) ) ]
    public XPCollection< ClasePropiedad > Propiedades
    {
      get { return GetCollection< ClasePropiedad >( "Propiedades" ); }
    }
  }
}
