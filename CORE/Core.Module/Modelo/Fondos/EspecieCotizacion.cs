using System;
using System.ComponentModel;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Fondos
{
  [Persistent( @"fondos.EspecieCotizacion" )]
  [System.ComponentModel.DisplayName( "Cotización de especie" )]
  public class EspecieCotizacion : BasicObject, IObjetoPorEmpresa
  {
    private decimal fComprador;
    private Especie fEspecieDestino;
    private Especie fEspecieOrigen;
    private DateTime fFecha;
    private Empresa _empresa;
    private decimal fPromedio;
    private decimal fVendedor;

    public EspecieCotizacion( Session session ) : base( session )
    {
    }
    
    public DateTime Fecha
    {
      get { return fFecha; }
      set { SetPropertyValue< DateTime >( "Fecha", ref fFecha, value ); }
    }

    public Especie EspecieOrigen
    {
      get { return fEspecieOrigen; }
      set { SetPropertyValue( "EspecieOrigen", ref fEspecieOrigen, value ); }
    }

    public decimal Comprador
    {
      get { return fComprador; }
      set { SetPropertyValue< decimal >( "Comprador", ref fComprador, value ); }
    }

    public decimal Vendedor
    {
      get { return fVendedor; }
      set { SetPropertyValue< decimal >( "Vendedor", ref fVendedor, value ); }
    }

    public decimal Promedio
    {
      get { return fPromedio; }
      set { SetPropertyValue< decimal >( "Promedio", ref fPromedio, value ); }
    }

    public Especie EspecieDestino
    {
      get { return fEspecieDestino; }
      set { SetPropertyValue( "EspecieDestino", ref fEspecieDestino, value ); }
    }

    [Browsable( false )]
    public Empresa Empresa
    {
      get { return _empresa; }
      set { SetPropertyValue("Empresa", ref _empresa, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}