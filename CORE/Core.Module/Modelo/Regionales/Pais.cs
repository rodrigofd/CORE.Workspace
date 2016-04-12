using System;
using System.ComponentModel;
using System.Drawing;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Impuestos;
using FDIT.Core.Personas;

namespace FDIT.Core.Regionales
{
  public class FiltroPorPaisAttribute : Attribute
  {
    public bool Filtrar = true;

    public FiltroPorPaisAttribute( )
    {
    }

    public FiltroPorPaisAttribute( bool filtrar )
    {
      Filtrar = filtrar;
    }
  }

  [ Persistent( @"regionales.Pais" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "País" ) ]
  public class Pais : BasicObject
  {
    private string fCapital;
    private string fCodigo;
    private string fCodigo3;
    private string fCodigoFips;
    private string fCodigoNum;
    private string fCodigoPstn;
    private string fCodigoTld;
    private Continente fContinente;
    private string fFormatoCodigoPostal;
    private string fGeoCodeAddr;
    private string fGeoLatitud;
    private string fGeoLongitud;
    private string fGeoNameid;
    private PaisIdioma fIdiomaNativo;
    private Image fImagenBandera;
    private Image fImagenBanderaChica;
    private string fPais;
    private string fPaisEng;
    private string fPaisesLimitrofes;
    private string fPatronCodigoPostal;
    private int fPoblacion;
    private decimal fSuperficie;
    private string fUrlBuscadorCodigoPostal;

    public Pais( Session session ) : base( session )
    {
    }

    [ ModelDefault( "ImageSizeMode", "Normal" ) ]
    [ Index( 0 ) ]
    [ VisibleInLookupListView( true ) ]
    [ ValueConverter( typeof( ImageValueConverter ) ) ]
    public Image ImagenBanderaChica
    {
      get { return fImagenBanderaChica; }
      set { SetPropertyValue( "ImagenBanderaChica", ref fImagenBanderaChica, value ); }
    }

    [ Association( @"PaisesReferencesContinentes" ) ]
    public Continente Continente
    {
      get { return fContinente; }
      set { SetPropertyValue( "Continente", ref fContinente, value ); }
    }

    [ Size( 2 ) ]
    [ Index( 0 ) ]
    public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [ Index( 1 ) ]
    public string Nombre
    {
      get { return fPais; }
      set { SetPropertyValue( "Nombre", ref fPais, value ); }
    }

    public string Capital
    {
      get { return fCapital; }
      set { SetPropertyValue( "Capital", ref fCapital, value ); }
    }

    [ Size( 15 ) ]
    public string CodigoPstn
    {
      get { return fCodigoPstn; }
      set { SetPropertyValue( "CodigoPstn", ref fCodigoPstn, value ); }
    }

    [ Delayed( true ) ]
    [ ValueConverter( typeof( ImageValueConverter ) ) ]
    [ Size( SizeAttribute.Unlimited ) ]
    public Image ImagenBandera
    {
      get { return fImagenBandera; }
      set { SetPropertyValue( "ImagenBandera", ref fImagenBandera, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string UrlBuscadorCodigoPostal
    {
      get { return fUrlBuscadorCodigoPostal; }
      set { SetPropertyValue( "UrlBuscadorCodigoPostal", ref fUrlBuscadorCodigoPostal, value ); }
    }

    [ Size( 60 ) ]
    public string FormatoCodigoPostal
    {
      get { return fFormatoCodigoPostal; }
      set { SetPropertyValue( "FormatoCodigoPostal", ref fFormatoCodigoPostal, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string PatronCodigoPostal
    {
      get { return fPatronCodigoPostal; }
      set { SetPropertyValue( "PatronCodigoPostal", ref fPatronCodigoPostal, value ); }
    }

    public int Poblacion
    {
      get { return fPoblacion; }
      set { SetPropertyValue< int >( "Poblacion", ref fPoblacion, value ); }
    }

    public decimal Superficie
    {
      get { return fSuperficie; }
      set { SetPropertyValue< decimal >( "Superficie", ref fSuperficie, value ); }
    }

    [ Size( 50 ) ]
    public string PaisesLimitrofes
    {
      get { return fPaisesLimitrofes; }
      set { SetPropertyValue( "Limitrofes", ref fPaisesLimitrofes, value ); }
    }

    [ Size( 50 ) ]
    public string GeoLatitud
    {
      get { return fGeoLatitud; }
      set { SetPropertyValue( "GeoLatitud", ref fGeoLatitud, value ); }
    }

    [ Size( 50 ) ]
    public string GeoLongitud
    {
      get { return fGeoLongitud; }
      set { SetPropertyValue( "GeoLongitud", ref fGeoLongitud, value ); }
    }

    [ Size( 255 ) ]
    public string GeoCodeAddr
    {
      get { return fGeoCodeAddr; }
      set { SetPropertyValue( "GeoCodeAddr", ref fGeoCodeAddr, value ); }
    }

    [ Size( 10 ) ]
    public string GeoNameid
    {
      get { return fGeoNameid; }
      set { SetPropertyValue( "GeoNameid", ref fGeoNameid, value ); }
    }

    [ Size( 3 ) ]
    public string Codigo3
    {
      get { return fCodigo3; }
      set { SetPropertyValue( "Codigo3", ref fCodigo3, value ); }
    }

    [ Size( 3 ) ]
    public string CodigoNum
    {
      get { return fCodigoNum; }
      set { SetPropertyValue( "CodigoNum", ref fCodigoNum, value ); }
    }

    [ Size( 2 ) ]
    public string CodigoFips
    {
      get { return fCodigoFips; }
      set { SetPropertyValue( "CodigoFips", ref fCodigoFips, value ); }
    }

    [ Size( 2 ) ]
    public string CodigoTld
    {
      get { return fCodigoTld; }
      set { SetPropertyValue( "CodigoTld", ref fCodigoTld, value ); }
    }

    [ Size( 50 ) ]
    public string PaisEng
    {
      get { return fPaisEng; }
      set { SetPropertyValue( "PaisEng", ref fPaisEng, value ); }
    }

    public PaisIdioma IdiomaNativo
    {
      get { return fIdiomaNativo; }
      set { SetPropertyValue( "IdiomaNativo", ref fIdiomaNativo, value ); }
    }

    [ Association( @"ImpuestosReferencesPaises", typeof( Impuesto ) ) ]
    public XPCollection< Impuesto > ImpuestosAsociados
    {
      get { return GetCollection< Impuesto >( "ImpuestosAsociados" ); }
    }

    [ Association( @"IdentificacionesTiposReferencesPaises", typeof( IdentificacionTipo ) ) ]
    public XPCollection< IdentificacionTipo > IdentificacionesTiposAsociados
    {
      get { return GetCollection< IdentificacionTipo >( "IdentificacionesTiposAsociados" ); }
    }

    [ Association( @"CiudadesReferencesPaises", typeof( Ciudad ) ) ]
    public XPCollection< Ciudad > Ciudades
    {
      get { return GetCollection< Ciudad >( "Ciudades" ); }
    }

    [ Association( @"PaisesIdiomasReferencesPaises", typeof( PaisIdioma ) ) ]
    public XPCollection< PaisIdioma > IdiomasAsociados
    {
      get { return GetCollection< PaisIdioma >( "IdiomasAsociados" ); }
    }

    [ Association( @"ProvinciasReferencesPaises", typeof( Provincia ) ) ]
    public XPCollection< Provincia > Provincias
    {
      get { return GetCollection< Provincia >( "Provincias" ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
