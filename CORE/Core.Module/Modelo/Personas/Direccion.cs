using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Sistema;
using FDIT.Core.Regionales;
using FDIT.Core.Seguridad;

namespace FDIT.Core.Personas
{
  [Persistent( @"personas.Direccion" )]
  [DefaultProperty( "DireccionCompleta" )]
  [ImageName("postage-stamp-at-sign")]
  [System.ComponentModel.DisplayName( "Dirección" )]
  public class Direccion : BasicObject
  {
    private Provincia fProvincia;
    private string fCalle;
    private string fEntreCalles;
    private Ciudad fCiudad;
    private string fCiudadOtra;
    private string fCodigoPostal;
    private string fDepto;
    private string fEdificio;
    private string fFax;
    private string fGeoCodeAddr;
    private string fGeoLatitud;
    private string fGeoLongitud;
        
    private Localidad fLocalidad;
    private string fLocalidadOtra;
    private string fNotas;
    private string fNumero;
    private Pais fPais;
    private string fPaisOtro;
    private string fPiso;
    private string fProvinciaOtra;
    private string fTelefono;

    private string fDireccionCompleta;

    public Direccion( Session session ) : base( session )
    {
    }

    [Size( SizeAttribute.Unlimited )]
    [System.ComponentModel.DisplayName( "Calle" )]
    public string Calle
    {
      get { return fCalle; }
      set { SetPropertyValue( "Calle", ref fCalle, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    [System.ComponentModel.DisplayName( "Entre calles" )]
    public string EntreCalles
    {
      get { return fEntreCalles; }
      set { SetPropertyValue( "EntreCalles", ref fEntreCalles, value ); }
    }

    [Size( 20 )]
    [System.ComponentModel.DisplayName( "Número" )]
    public string Numero
    {
      get { return fNumero; }
      set { SetPropertyValue( "Numero", ref fNumero, value ); }
    }

    [Size( 20 )]
    [System.ComponentModel.DisplayName( "Edificio" )]
    public string Edificio
    {
      get { return fEdificio; }
      set { SetPropertyValue( "Edificio", ref fEdificio, value ); }
    }

    [Size( 20 )]
    [System.ComponentModel.DisplayName( "Piso" )]
    public string Piso
    {
      get { return fPiso; }
      set { SetPropertyValue( "Piso", ref fPiso, value ); }
    }

    [Size( 20 )]
    [System.ComponentModel.DisplayName( "Depto." )]
    public string Depto
    {
      get { return fDepto; }
      set { SetPropertyValue( "Depto", ref fDepto, value ); }
    }

    [Size( 20 )]
    [System.ComponentModel.DisplayName( "Código Postal" )]
    public string CodigoPostal
    {
      get { return fCodigoPostal; }
      set { SetPropertyValue( "CodigoPostal", ref fCodigoPostal, value ); }
    }

    [ImmediatePostData]
    [DataSourceProperty( "Localidad.Ciudades" )]
    [System.ComponentModel.DisplayName( "Ciudad" )]
    public Ciudad Ciudad
    {
      get { return fCiudad; }
      set { SetPropertyValue( "Ciudad", ref fCiudad, value ); }
    }

    [Appearance( "manual_ciudad", Criteria = "Not IsNull(Ciudad)", Visibility = ViewItemVisibility.Hide )]
    [Size( SizeAttribute.Unlimited )]
    [System.ComponentModel.DisplayName( "Ingrese ciudad" )]
    public string CiudadOtra
    {
      get { return fCiudadOtra; }
      set { SetPropertyValue( "CiudadOtra", ref fCiudadOtra, value ); }
    }

    [ImmediatePostData]
    [LookupEditorMode( LookupEditorMode.AllItems )]
    [DataSourceProperty( "Provincia.Localidades" )]
    [System.ComponentModel.DisplayName( "Localidad" )]
    public Localidad Localidad
    {
      get { return fLocalidad; }
      set { SetPropertyValue( "Localidad", ref fLocalidad, value ); }
    }

    [Appearance( "manual_localidad", Criteria = "Not IsNull(Localidad)", Visibility = ViewItemVisibility.Hide )]
    [Size( SizeAttribute.Unlimited )]
    [System.ComponentModel.DisplayName( "Ingrese localidad" )]
    public string LocalidadOtra
    {
      get { return fLocalidadOtra; }
      set { SetPropertyValue( "LocalidadOtra", ref fLocalidadOtra, value ); }
    }

    [ImmediatePostData]
    [LookupEditorMode( LookupEditorMode.AllItems )]
    [DataSourceProperty( "Pais.Provincias" )]
    [System.ComponentModel.DisplayName( "Provincia" )]
    public Provincia Provincia
    {
      get { return fProvincia; }
      set { SetPropertyValue( "Provincia", ref fProvincia, value ); }
    }

    [Appearance( "manual_provincia", Criteria = "Not IsNull(Provincia)", Visibility = ViewItemVisibility.Hide )]
    [Size( SizeAttribute.Unlimited )]
    [System.ComponentModel.DisplayName( "Ingrese provincia" )]
    public string ProvinciaOtra
    {
      get { return fProvinciaOtra; }
      set { SetPropertyValue( "ProvinciaOtra", ref fProvinciaOtra, value ); }
    }

    [ImmediatePostData]
    [LookupEditorMode( LookupEditorMode.AllItems )]
    [System.ComponentModel.DisplayName( "País" )]
    public Pais Pais
    {
      get { return fPais; }
      set { SetPropertyValue( "Pais", ref fPais, value ); }
    }

    [Appearance( "manual_pais", Criteria = "Not IsNull(Pais)", Visibility = ViewItemVisibility.Hide )]
    [Size( SizeAttribute.Unlimited )]
    [System.ComponentModel.DisplayName( "Ingrese país" )]
    public string PaisOtro
    {
      get { return fPaisOtro; }
      set { SetPropertyValue( "PaisOtro", ref fPaisOtro, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    [System.ComponentModel.DisplayName( "Teléfono" )]
    public string Telefono
    {
      get { return fTelefono; }
      set { SetPropertyValue( "Telefono", ref fTelefono, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    [System.ComponentModel.DisplayName( "Fax" )]
    public string Fax
    {
      get { return fFax; }
      set { SetPropertyValue( "Fax", ref fFax, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    [System.ComponentModel.DisplayName( "Notas" )]
    public string Notas
    {
      get { return fNotas; }
      set { SetPropertyValue( "Notas", ref fNotas, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    [Browsable( false )]
    [System.ComponentModel.DisplayName( "Geo. latitud" )]
    public string GeoLatitud
    {
      get { return fGeoLatitud; }
      set { SetPropertyValue( "GeoLatitud", ref fGeoLatitud, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    [Browsable( false )]
    [System.ComponentModel.DisplayName( "Geo. longitud" )]
    public string GeoLongitud
    {
      get { return fGeoLongitud; }
      set { SetPropertyValue( "GeoLongitud", ref fGeoLongitud, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    [Browsable( false )]
    [System.ComponentModel.DisplayName( "Geo. Cod." )]
    public string GeoCodeAddr
    {
      get { return fGeoCodeAddr; }
      set { SetPropertyValue( "GeoCodeAddr", ref fGeoCodeAddr, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    [Browsable( false )]
    [VisibleInDetailView(false)]
    [System.ComponentModel.DisplayName( "Dirección completa" )]
    public string DireccionCompleta
    {
      get
      {
        return fDireccionCompleta;
      }
      set
      {
        SetPropertyValue( "DireccionCompleta", ref fDireccionCompleta, value ); 
      }
    }

    protected override void OnChanged( string propertyName, object oldValue, object newValue )
    {
      base.OnChanged( propertyName, oldValue, newValue );

      if( CanRaiseOnChanged && propertyName != "DireccionCompleta" )
        CalcularDireccionCompleta( );
    }

    private void CalcularDireccionCompleta( )
    {
      var ciudad = ( !string.IsNullOrWhiteSpace( CiudadOtra ) ? CiudadOtra : ( Ciudad != null ? Ciudad.Nombre : "" ) );
      var localidad = ( !string.IsNullOrWhiteSpace( LocalidadOtra ) ? LocalidadOtra : ( Localidad != null ? Localidad.Nombre : "" ) );
      var provincia = ( !string.IsNullOrWhiteSpace( ProvinciaOtra ) ? ProvinciaOtra : ( Provincia != null ? Provincia.Nombre : "" ) );
      var pais = ( !string.IsNullOrWhiteSpace( PaisOtro ) ? PaisOtro : ( Pais != null ? Pais.Nombre : "" ) );

      if( Calle == null ) Calle = "";

      var ret = Calle.Replace( Environment.NewLine, " " );
      if( !string.IsNullOrWhiteSpace( Numero ) ) ret += " " + Numero;
      if( !string.IsNullOrWhiteSpace( Piso ) ) ret += " Piso " + Piso;
      if( !string.IsNullOrWhiteSpace( Depto ) ) ret += " Depto. " + Depto;
      if( !string.IsNullOrWhiteSpace( CodigoPostal ) ) ret += " (" + CodigoPostal + ")";

      var ubicaciones = new List< string >( );

      if( !string.IsNullOrWhiteSpace( ciudad ) ) ubicaciones.Add( ciudad );
      if( !string.IsNullOrWhiteSpace( localidad ) && localidad != ciudad ) ubicaciones.Add( localidad );
      if( !string.IsNullOrWhiteSpace( provincia ) && provincia != localidad ) ubicaciones.Add( provincia );
      if( !string.IsNullOrWhiteSpace( pais ) ) ubicaciones.Add( pais );

      ret += ( ubicaciones.Count > 0 ? " " : "" ) + string.Join( ", ", ubicaciones );

      DireccionCompleta = ret;
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      Pais = Regionales.Identificadores.GetInstance(Session).PaisPredeterminado;
    }
  }
}