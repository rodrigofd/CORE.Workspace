using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Personas;

namespace FDIT.Core.Seguros
{
  [ DefaultClassOptions ]
  [ NavigationItem( "Seguros" ) ]
  //[DefaultProperty( "Nombre" )]
  [ System.ComponentModel.DisplayName( "Intermediario" ) ]
  [ Persistent( "seguros.Intermediario" ) ]
  public class Intermediario : BasicObject
  {
    private Aseguradora fAseguradora;
    private decimal fComisionCobranzaTasa;
    private decimal fComisionPrimaTasa;

    private Persona fIntermediarioPersona;
    private LineaNegocio fLineaNegocio;
    private Productor fProductor;
    private Ramo fRamo;
    private Subramo fSubramo;
    private Persona fTomador;

    public Intermediario( Session session )
      : base( session )
    {
    }

    public Persona IntermediarioPersona
    {
      get { return fIntermediarioPersona; }
      set { SetPropertyValue( "IntermediarioPersona", ref fIntermediarioPersona, value ); }
    }

    public Aseguradora Aseguradora
    {
      get { return fAseguradora; }
      set { SetPropertyValue( "Aseguradora", ref fAseguradora, value ); }
    }

    public Persona Tomador
    {
      get { return fTomador; }
      set { SetPropertyValue( "Tomador", ref fTomador, value ); }
    }

    public Productor Productor
    {
      get { return fProductor; }
      set { SetPropertyValue( "Productor", ref fProductor, value ); }
    }

    public LineaNegocio LineaNegocio
    {
      get { return fLineaNegocio; }
      set { SetPropertyValue( "LineaNegocio", ref fLineaNegocio, value ); }
    }

    public Ramo Ramo
    {
      get { return fRamo; }
      set { SetPropertyValue( "Ramo", ref fRamo, value ); }
    }

    [ DataSourceProperty( "Ramo.Subramos" ) ]
    [ LookupEditorMode( LookupEditorMode.AllItems ) ]
    public Subramo Subramo
    {
      get { return fSubramo; }
      set { SetPropertyValue( "Subramo", ref fSubramo, value ); }
    }

    public decimal ComisionPrimaTasa
    {
      get { return fComisionPrimaTasa; }
      set { SetPropertyValue< decimal >( "ComisionPrimaTasa", ref fComisionPrimaTasa, value ); }
    }

    public decimal ComisionCobranzaTasa
    {
      get { return fComisionCobranzaTasa; }
      set { SetPropertyValue< decimal >( "ComisionCobranzaTasa", ref fComisionCobranzaTasa, value ); }
    }
  }
}
