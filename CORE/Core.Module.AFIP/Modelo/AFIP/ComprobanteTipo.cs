using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.AFIP
{
  [ Persistent( @"afip.ComprobanteTipo" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Tipos de comprobantes AFIP" ) ]
  public class ComprobanteTipo : BasicObject
  {
    private short fCodigo;
    private string fComprobanteTipo;

    public ComprobanteTipo( Session session )
      : base( session )
    {
    }

    public short Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    public string Nombre
    {
      get { return fComprobanteTipo; }
      set { SetPropertyValue( "Nombre", ref fComprobanteTipo, value ); }
    }
  }
}
