using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Regionales
{
  [Persistent( @"regionales.Ciudad" )]
  [DefaultClassOptions]
  [DefaultProperty( "Nombre" )]
  [System.ComponentModel.DisplayName( "Ciudad" )]
  public class Ciudad : BasicObject
  {
    private string fNombre;
    private string fCodigo;
    private int fPoblacion;
    private string fGeoLatitud;
    private string fGeoLongitud;
    private Localidad fLocalidad;

    private Pais fPais;
    private Provincia fProvincia;

    public Ciudad( Session session ) : base( session )
    {
    }

    [Association( @"CiudadesReferencesPaises" )]
    public Pais Pais
    {
      get { return fPais; }
      set { SetPropertyValue( "Pais", ref fPais, value ); }
    }

    [Association( @"CiudadesReferencesProvincias" )]
    public Provincia Provincia
    {
      get { return fProvincia; }
      set { SetPropertyValue( "Provincia", ref fProvincia, value ); }
    }

    [Association( @"CiudadesReferencesLocalidades" )]
    public Localidad Localidad
    {
      get { return fLocalidad; }
      set { SetPropertyValue( "Localidad", ref fLocalidad, value ); }
    }

    [Size( 20 )]
    [ Index(0) ]
public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [Size( 200 )]
    [Index(1)]
public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [Size( 50 )]
    public string GeoLatitud
    {
      get { return fGeoLatitud; }
      set { SetPropertyValue( "GeoLatitud", ref fGeoLatitud, value ); }
    }

    [Size( 50 )]
    public string GeoLongitud
    {
      get { return fGeoLongitud; }
      set { SetPropertyValue( "GeoLongitud", ref fGeoLongitud, value ); }
    }

    public int Poblacion
    {
      get { return fPoblacion; }
      set { SetPropertyValue<int>( "Poblacion", ref fPoblacion, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}