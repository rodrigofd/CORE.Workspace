using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Personas
{
  [Persistent( @"personas.PropiedadValor" )]
  [DefaultProperty( "Valor" )]
  [System.ComponentModel.DisplayName( "Valor predefinido de propiedad" )]
  public class PropiedadValor : BasicObject
  {
    private int fOrden;
    private Propiedad fPropiedad;
    private string fValor;

    public PropiedadValor( Session session ) : base( session )
    {
    }

    [Association( @"PropiedadesValoresReferencesPropiedades" )]
    public Propiedad Propiedad
    {
      get { return fPropiedad; }
      set { SetPropertyValue( "Propiedad", ref fPropiedad, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    public string Valor
    {
      get { return fValor; }
      set { SetPropertyValue( "Valor", ref fValor, value ); }
    }

    public int Orden
    {
      get { return fOrden; }
      set { SetPropertyValue< int >( "Orden", ref fOrden, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}