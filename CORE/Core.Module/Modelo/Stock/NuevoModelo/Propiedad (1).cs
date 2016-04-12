using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ DefaultClassOptions ]
  [ NavigationItem( "Stock" ) ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Propiedad de artículo" ) ]
  [ Persistent( @"stock.Propiedad" ) ]
  public class Propiedad : BasicObject
  {
    private string fNombre;

    public Propiedad( Session session ) : base( session )
    {
    }

    [ Size( 50 ) ]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [ Association( @"Clases_por_PropiedadesReferencesPropiedades", typeof( ClasePropiedad ) ) ]
    public XPCollection< ClasePropiedad > ClasesPropiedades
    {
      get { return GetCollection< ClasePropiedad >( "ClasesPropiedades" ); }
    }

    [ Aggregated ]
    [ Association( @"Propiedades_ValoresReferencesPropiedades", typeof( PropiedadValor ) ) ]
    public XPCollection< PropiedadValor > Valores
    {
      get { return GetCollection< PropiedadValor >( "Valores" ); }
    }
  }
}
