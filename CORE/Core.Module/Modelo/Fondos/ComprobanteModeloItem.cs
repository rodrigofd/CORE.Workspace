using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Fondos
{
  [ Persistent( @"fondos.ComprobanteModeloItem" ) ]
  [ System.ComponentModel.DisplayName( "Item Modelo de comprobante de fondo" ) ]
  [DefaultProperty("Cuenta")]
    public class ComprobanteModeloItem : BasicObject
  {
    private ComprobanteModelo fComprobanteModelo;
    private Cuenta fCuenta;
    private DebeHaber fDebeHaber;
    private Especie fEspecie;

    public ComprobanteModeloItem( Session session ) : base( session )
    {
    }

    [ Association ]
    public ComprobanteModelo ComprobanteModelo
    {
      get { return fComprobanteModelo; }
      set { SetPropertyValue( "ComprobanteModelo", ref fComprobanteModelo, value ); }
    }

    [LookupEditorMode( LookupEditorMode.AllItems )]
    [ ImmediatePostData ]
    public Cuenta Cuenta
    {
      get { return fCuenta; }
      set
      {
        SetPropertyValue( "Cuenta", ref fCuenta, value );
        if( CanRaiseOnChanged )
        {
          if( value != null )
            Especie = value.EspeciePredeterminada;
        }
      }
    }

    public DebeHaber DebeHaber
    {
      get { return fDebeHaber; }
      set { SetPropertyValue( "DebeHaber", ref fDebeHaber, value ); }
    }

    [LookupEditorMode( LookupEditorMode.AllItems )]
    public Especie Especie
    {
      get { return fEspecie; }
      set { SetPropertyValue( "Especie", ref fEspecie, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );

      DebeHaber = DebeHaber.Debe;
    }
  }
}
