using System;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Stock
{
  [Persistent( @"stock.Precio" )]
  [System.ComponentModel.DisplayName( "Precio por artículo" )]
  public class Precio : BasicObject
  {
    private Articulo fArticulo;
    private bool fEstimado;
    private ListaDePrecios fListaDePrecios;
    private DateTime fModificadoFecha;
    private decimal fPrecioUnitario;

    public Precio( Session session )
      : base( session )
    {
    }

    [LookupEditorMode( LookupEditorMode.AllItems )]
    [Association]
    [System.ComponentModel.DisplayName( "Artículo" )]
    public Articulo Articulo
    {
      get { return fArticulo; }
      set { SetPropertyValue( "Articulo", ref fArticulo, value ); }
    }

    [Association]
    [System.ComponentModel.DisplayName( "Lista de precios" )]
    public ListaDePrecios ListaDePrecios
    {
      get { return fListaDePrecios; }
      set { SetPropertyValue( "ListaDePrecios", ref fListaDePrecios, value ); }
    }

    [System.ComponentModel.DisplayName( "Precio unitario" )]
    public decimal PrecioUnitario
    {
      get { return fPrecioUnitario; }
      set { SetPropertyValue< decimal >( "PrecioUnitario", ref fPrecioUnitario, value ); }
    }

    [System.ComponentModel.DisplayName( "Es estimado" )]
    public bool Estimado
    {
      get { return fEstimado; }
      set { SetPropertyValue( "Estimado", ref fEstimado, value ); }
    }

    [System.ComponentModel.DisplayName( "Fecha Modif." )]
    public DateTime ModificadoFecha
    {
      get { return fModificadoFecha; }
      set { SetPropertyValue< DateTime >( "ModificadoFecha", ref fModificadoFecha, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}