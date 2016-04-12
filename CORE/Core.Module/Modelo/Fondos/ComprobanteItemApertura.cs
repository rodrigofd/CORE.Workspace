using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace FDIT.Core.Fondos
{
  [ Persistent( @"fondos.ComprobanteItemApertura" ) ]
  [ System.ComponentModel.DisplayName( "Apertura por item" ) ]
  public class ComprobanteItemApertura : BasicObject
  {
    private CentroDeCosto fCentroDeCosto;
    private ComprobanteItem fItem;
    private decimal fPorcentaje;

    public ComprobanteItemApertura( Session session ) : base( session )
    {
    }

    [ Association ]
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

    [ RuleRange( DefaultContexts.Save, 0, 100, CustomMessageTemplate = "El valor debe ser un entero entre 0 y 100" ) ]
    [ ModelDefault( "DisplayFormat", "n2" ) ]
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
