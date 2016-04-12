using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [ System.ComponentModel.DisplayName( "Valor de propiedad" ) ]
  [ Persistent( @"stock.PropiedadValor" ) ]
  public class PropiedadValor : BasicObject
  {
    private Propiedad fPropiedad;
    private string fValor;

    public PropiedadValor( Session session ) : base( session )
    {
    }

    [ Association( @"Propiedades_ValoresReferencesPropiedades" ) ]
    public Propiedad Propiedad
    {
      get { return fPropiedad; }
      set { SetPropertyValue( "Propiedad", ref fPropiedad, value ); }
    }

    [ Size( 50 ) ]
    public string Valor
    {
      get { return fValor; }
      set { SetPropertyValue( "Valor", ref fValor, value ); }
    }
  }
}
