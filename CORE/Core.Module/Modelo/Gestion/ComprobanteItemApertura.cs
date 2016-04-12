using System.ComponentModel;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Fondos;

namespace FDIT.Core.Gestion
{
  [Persistent( @"gestion.ComprobanteItemApertura" )]
  [System.ComponentModel.DisplayName( "Apertura del Item" )]
  public class ComprobanteItemApertura : BasicObject
  {
    private CentroDeCosto fCentroDeCosto;
    private ComprobanteItem fItem;
    private decimal fPorcentaje;

    public ComprobanteItemApertura( Session session ) : base( session )
    {
    }

    [Association]
    [Browsable( false )]
    public ComprobanteItem Item
    {
      get { return fItem; }
      set { SetPropertyValue( "Item", ref fItem, value ); }
    }

    public CentroDeCosto CentroDeCosto
    {
      get { return fCentroDeCosto; }
      set { SetPropertyValue( "CentroDeCosto", ref fCentroDeCosto, value ); }
    }

    [RuleRange( DefaultContexts.Save, 0, 100, CustomMessageTemplate = "El valor debe ser un entero entre 0 y 100" )]
    public decimal Porcentaje
    {
      get { return fPorcentaje; }
      set { SetPropertyValue< decimal >( "Porcentaje", ref fPorcentaje, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}