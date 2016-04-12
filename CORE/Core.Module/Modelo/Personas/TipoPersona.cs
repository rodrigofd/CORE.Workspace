using DevExpress.Persistent.Base;

namespace FDIT.Core.Personas
{
  public enum TipoPersona
  {
    [ ImageName( "user" ) ]
    Fisica = 1,

    [ ImageName( "building" ) ]
    Juridica,

    [ ImageName( "global_telecom" ) ]
    Virtual,
  }
}