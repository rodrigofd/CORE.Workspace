using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Fondos
{
  [ Persistent( "fondos.ComprobanteItemValor" ) ]
  [ System.ComponentModel.DisplayName( "Movimiento del valor" ) ]
  [ DefaultClassOptions ]
  public class ComprobanteItemValor : BasicObject
  {
    private ComprobanteItem fComprobanteItem;
    private Valor fValor;

    public ComprobanteItemValor( Session session ) : base( session )
    {
    }

    [ Association ]
    public ComprobanteItem ComprobanteItem
    {
      get { return fComprobanteItem; }
      set { SetPropertyValue( "ComprobanteItem", ref fComprobanteItem, value ); }
    }

    [ Association ]
    [ ExpandObjectMembers( ExpandObjectMembers.Always ) ]
    public Valor Valor
    {
      get { return fValor; }
      set
      {
        SetPropertyValue( "Valor", ref fValor, value );

        //Cuando un objeto valor se asocia a un comprobante item (via comprobante item valor)...
        if( Valor != null )
        {
          //Si se está creando un objeto valor nuevo, entonces forzar el 'ultimo movimiento' al unico que tiene (esto)
          //porque la rutina de calculo solo funciona con valores ya persistidos
          //if( Session.IsNewObject( Valor ) )
            //Valor.UltimoMovimiento = ComprobanteItem;

          //Ante un cambio en el objeto Valor, forzar el marcado de este ComprobanteItemValor como modificado (para que afecte los triggers en ComprobanteItem)
          Valor.Changed += ( sender, e ) => OnChanged( "Valor" );
        }
      }
    }
  }
}
