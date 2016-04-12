using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Regionales
{
  [Persistent( @"regionales.Provincia" )]
  [DefaultClassOptions]
  [DefaultProperty( "Nombre" )]
  [System.ComponentModel.DisplayName( "Provincia" )]
  public class Provincia : BasicObject
  {
    private string fCodigo;
    private Pais fPais;
    private string fProvincia;

    public Provincia( Session session ) : base( session )
    {
    }

    [Association( @"ProvinciasReferencesPaises" )]
    public Pais Pais
    {
      get { return fPais; }
      set { SetPropertyValue( "Pais", ref fPais, value ); }
    }

    [Size( 10 )]
    [ Index(0) ]
public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [Size( 50 )]
    [Index(1)]
public string Nombre
    {
      get { return fProvincia; }
      set { SetPropertyValue( "Nombre", ref fProvincia, value ); }
    }

    [Association( @"CiudadesReferencesProvincias", typeof( Ciudad ) )]
    public XPCollection< Ciudad > Ciudades
    {
      get { return GetCollection< Ciudad >( "Ciudades" ); }
    }

    [Association( @"LocalidadesReferencesProvincias", typeof( Localidad ) )]
    public XPCollection< Localidad > Localidades
    {
      get { return GetCollection< Localidad >( "Localidades" ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}