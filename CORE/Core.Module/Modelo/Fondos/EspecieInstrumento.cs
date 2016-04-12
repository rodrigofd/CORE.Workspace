using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Fondos
{
  [ Persistent( @"fondos.EspecieInstrumento" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Instrumento de pago" ) ]
  public class EspecieInstrumento : BasicObject
  {
    private string fClase;
    private string fCodigo;
    private string fEspecieInstrumento;

    public EspecieInstrumento( Session session ) : base( session )
    {
    }

    [ Size( 10 ) ]
    [ Index( 0 ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ Size( 50 ) ]
    [ Index( 1 ) ]
    public string Nombre
    {
      get { return fEspecieInstrumento; }
      set { SetPropertyValue( "Nombre", ref fEspecieInstrumento, value ); }
    }

    [ Size( 1 ) ]
    public string Clase
    {
      get { return fClase; }
      set { SetPropertyValue( "Clase", ref fClase, value ); }
    }

    [ Association( @"EspeciesReferencesEspeciesInstrumentos", typeof( Especie ) ) ]
    public XPCollection< Especie > Especies
    {
      get { return GetCollection< Especie >( "Especies" ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
