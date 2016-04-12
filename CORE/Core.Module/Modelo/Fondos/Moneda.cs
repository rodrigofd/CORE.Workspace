using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Fondos
{
  [Persistent( @"fondos.Moneda" )]
  [DefaultClassOptions]
  [DefaultProperty( "Nombre" )]
  [System.ComponentModel.DisplayName( "Moneda" )]
    public class Moneda : BasicObject
  {
    private string fCodigo;
    private string fFraccional;
    private int fFraccionalRelacion;
    private string fMoneda;
    private string fMonedaEng;
    private string fSimbolo;

    public Moneda( Session session ) : base( session )
    {
    }

    [Size( 50 )]
    [ Index(0) ]
public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [Size( 70 )]
    [Index(1)]
public string Nombre
    {
      get { return fMoneda; }
      set { SetPropertyValue( "Nombre", ref fMoneda, value ); }
    }

    [Size( 4 )]
    [System.ComponentModel.DisplayName( "S�mbolo" )]
    public string Simbolo
    {
      get { return fSimbolo; }
      set { SetPropertyValue( "Simbolo", ref fSimbolo, value ); }
    }

    [Size( 70 )]
    [System.ComponentModel.DisplayName( "Nombre (ingl�s)" )]
    public string MonedaEng
    {
      get { return fMonedaEng; }
      set { SetPropertyValue( "MonedaEng", ref fMonedaEng, value ); }
    }

    [Size( 15 )]
    [System.ComponentModel.DisplayName( "Fraccional" )]
    public string Fraccional
    {
      get { return fFraccional; }
      set { SetPropertyValue( "Fraccional", ref fFraccional, value ); }
    }

    [System.ComponentModel.DisplayName( "Relaci�n fraccional" )]
    public int FraccionalRelacion
    {
      get { return fFraccionalRelacion; }
      set { SetPropertyValue<int>( "FraccionalRelacion", ref fFraccionalRelacion, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}