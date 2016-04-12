using System.ComponentModel;

namespace FDIT.Core
{
  public interface IModelLayoutItemWithCaptionWidth
  {
    [ Category( "Appearance" ) ]
    [ Localizable( true ) ]
    int CaptionWidth{ get; set; }
  }
}