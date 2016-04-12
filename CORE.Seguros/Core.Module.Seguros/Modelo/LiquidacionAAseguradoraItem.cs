using DevExpress.Xpo;
using FDIT.Core.Gestion;

namespace FDIT.Core.Seguros
{
  //[DefaultProperty( "Nombre" )]
  [ System.ComponentModel.DisplayName( "Item de liquidación a aseguradora" ) ]
  [ Persistent( "seguros.LiquidacionAAseguradoraItem" ) ]
  public class LiquidacionAAseguradoraItem : BasicObject
  {
    private decimal fComisionCobranza;
    private decimal fComisionPrima;
    private LiquidacionAAseguradora fLiquidacionAAseguradora;
    private PagoAplicacion fPagoAplicacion;

    public LiquidacionAAseguradoraItem( Session session ) : base( session )
    {
    }

    [ Association ]
    public LiquidacionAAseguradora LiquidacionAAseguradora
    {
      get { return fLiquidacionAAseguradora; }
      set { SetPropertyValue( "LiquidacionAAseguradora", ref fLiquidacionAAseguradora, value ); }
    }

    public PagoAplicacion PagoAplicacion
    {
      get { return fPagoAplicacion; }
      set { SetPropertyValue( "PagoAplicacion", ref fPagoAplicacion, value ); }
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

    [ Association ]
    public XPCollection< LiquidacionAIntermediarioItem > LiquidacionAIntermediarioItems
    {
      get { return GetCollection< LiquidacionAIntermediarioItem >( "LiquidacionAIntermediarioItems" ); }
    }
  }
}
