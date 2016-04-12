using System.ComponentModel;
using DevExpress.Xpo;
using FDIT.Core.Fondos;

namespace FDIT.Core.Seguros
{
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Item de liquidación a intermediario" ) ]
  [ Persistent( "seguros.LiquidacionAIntermediarioItem" ) ]
  public class LiquidacionAIntermediarioItem : BasicObject
  {
    private decimal fCambio;
    private decimal fComisionCobranza;
    private decimal fComisionCobranzaTasa;
    private decimal fComisionPrima;
    private decimal fComisionPrimaTasa;
    private Especie fEspecie;
    private LiquidacionAAseguradoraItem fItemLiquidacionAAseguradora;
    private RendicionDeAseguradoraItem fItemRendicionDeAseguradora;
    private LiquidacionAIntermediario fLiquidacionAIntermediario;

    public LiquidacionAIntermediarioItem( Session session )
      : base( session )
    {
    }

    [ Association ]
    public LiquidacionAIntermediario LiquidacionAIntermediario
    {
      get { return fLiquidacionAIntermediario; }
      set { SetPropertyValue( "LiquidacionAIntermediario", ref fLiquidacionAIntermediario, value ); }
    }

    [ Association ]
    public LiquidacionAAseguradoraItem ItemLiquidacionAAseguradora
    {
      get { return fItemLiquidacionAAseguradora; }
      set { SetPropertyValue( "ItemLiquidacionAAseguradora", ref fItemLiquidacionAAseguradora, value ); }
    }

    [ Association ]
    public RendicionDeAseguradoraItem ItemRendicionDeAseguradora
    {
      get { return fItemRendicionDeAseguradora; }
      set { SetPropertyValue( "ItemRendicionDeAseguradora", ref fItemRendicionDeAseguradora, value ); }
    }

    public Especie Especie
    {
      get { return fEspecie; }
      set { SetPropertyValue( "Especie", ref fEspecie, value ); }
    }

    public decimal Cambio
    {
      get { return fCambio; }
      set { SetPropertyValue< decimal >( "Cambio", ref fCambio, value ); }
    }

    public decimal ComisionPrima
    {
      get { return fComisionPrima; }
      set { SetPropertyValue< decimal >( "ComisionPrima", ref fComisionPrima, value ); }
    }

    public decimal ComisionCobranza
    {
      get { return fComisionCobranza; }
      set { SetPropertyValue< decimal >( "ComisionCobranza", ref fComisionCobranza, value ); }
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
