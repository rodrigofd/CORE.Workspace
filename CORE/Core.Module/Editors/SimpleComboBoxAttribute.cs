using System;

namespace FDIT.Core.Editors
{
  public class SimpleComboBoxAttribute : Attribute
  {
    public bool AllowNullValues;
    public string Values;
    public bool AllowEdit;
    public bool DisplayValues;
    public string DataSourceProperty;
  }
}