using DevExpress.Persistent.Base;

namespace FDIT.Core.Gestion
{
  public enum ComprobanteEstado
  {
    [ ImageName( "web_template_editor" ) ]
    Pendiente = 1,

    [ ImageName( "document-stamp" ) ]
    Confirmado = 2,

    [ ImageName( "font_red_delete" ) ]
    Anulado = 3
  }
}
