using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.AFIP
{
  [ Persistent( @"afip.IdentificacionTipo" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "IdentificacionesTipos" ) ]
  public class IdentificacionTipo : BasicObject
  {
    private short fCodigo;

    private string fNombre;

    public IdentificacionTipo( Session session )
      : base( session )
    {
    }

    [ Size( 10 ) ]
    public short Codigo
    {
      get
      {
        return fCodigo;
      }
      set
      {
        SetPropertyValue( "Codigo", ref fCodigo, value );
      }
    }

    public string Nombre
    {
      get
      {
        return fNombre;
      }
      set
      {
        SetPropertyValue( "Nombre", ref fNombre, value );
      }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
