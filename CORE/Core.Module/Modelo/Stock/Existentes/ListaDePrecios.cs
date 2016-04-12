using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [Persistent( @"stock.ListaDePrecios" )]
  [NavigationItem("Stock")]
  [DefaultClassOptions]
  [DefaultProperty( "Nombre" )]
  [System.ComponentModel.DisplayName( "Lista de precios" )]
  public class ListaDePrecios : BasicObject
  {
    private decimal fAlicuota;
    private decimal fCoeficienteListaBase;
    private bool fIncluyeImpuestos;
    private ListaDePrecios fListaBase;

    private string fNombre;

    public ListaDePrecios( Session session )
      : base( session )
    {
    }

    [Size( 50 )]
    [System.ComponentModel.DisplayName( "Nombre" )]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [System.ComponentModel.DisplayName( "Incluye Impuestos" )]
    public bool IncluyeImpuestos
    {
      get { return fIncluyeImpuestos; }
      set { SetPropertyValue( "IncluyeImpuestos", ref fIncluyeImpuestos, value ); }
    }

    [System.ComponentModel.DisplayName( "Alícuota" )]
    public decimal Alicuota
    {
      get { return fAlicuota; }
      set { SetPropertyValue< decimal >( "Alicuota", ref fAlicuota, value ); }
    }

    [System.ComponentModel.DisplayName( "Lista base" )]
    public ListaDePrecios ListaBase
    {
      get { return fListaBase; }
      set { SetPropertyValue( "ListaBase", ref fListaBase, value ); }
    }

    [System.ComponentModel.DisplayName( "Coef. sobre lista base" )]
    public decimal CoeficienteListaBase
    {
      get { return fCoeficienteListaBase; }
      set { SetPropertyValue<decimal>( "CoeficienteListaBase", ref fCoeficienteListaBase, value ); }
    }

    [Delayed]
    [Association]
    [System.ComponentModel.DisplayName( "Precios" )]
    public XPCollection< Precio > Precios
    {
      get { return GetCollection< Precio >( @"Precios" ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}