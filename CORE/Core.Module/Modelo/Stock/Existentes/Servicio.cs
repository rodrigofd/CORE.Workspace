using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Gestion;

namespace FDIT.Core.Stock
{
  [ImageName("box-share")]
  [Persistent(@"stock.Servicio")]
  [DefaultClassOptions]
  [DefaultProperty("Nombre")]
  [System.ComponentModel.DisplayName("Servicio")]
  public class Servicio : BasicObject
  {
    private string fCodigo;
    private string fNombre;

    public Servicio(Session session)
      : base(session)
    {
    }

    [Size(50)]
    [System.ComponentModel.DisplayName("Código")]
    [ Index(0) ]
public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue("codigo_articulo", ref fCodigo, value); }
    }

    [System.ComponentModel.DisplayName("Nombre")]
    [Index(1)]
public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue("Nombre", ref fNombre, value); }
    }
  }
}