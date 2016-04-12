using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using FDIT.Core.Editors;

namespace FDIT.Core.Win.Editors
{
  [ PropertyEditor( typeof( String ), false ) ]
  public class WinSimpleComboBoxEditor : DXPropertyEditor
  {
    protected bool AllowNullValues;
    protected Dictionary< string, string > PredefinedValuesList;

    public WinSimpleComboBoxEditor( Type objectType, IModelMemberViewItem info )
      : base( objectType, info )
    {
    }

    protected override object CreateControlCore( )
    {
      var attrib = MemberInfo.FindAttribute< SimpleComboBoxAttribute >( );
      PredefinedValuesList = new Dictionary< string, string >( );

      var control = new LookUpEdit( );

      control.Properties.Columns.Add( new LookUpColumnInfo { FieldName = "Key", Visible = false } );
      control.Properties.Columns.Add( new LookUpColumnInfo { FieldName = "Value" } );

      control.Properties.NullText = CaptionHelper.NullValueText;

      if( !string.IsNullOrWhiteSpace( attrib.DataSourceProperty ) )
      {
        PredefinedValuesList = ( Dictionary< string, string > ) MemberInfo.Owner.FindMember( attrib.DataSourceProperty ).GetValue( CurrentObject );
      }
      else if( !string.IsNullOrWhiteSpace( attrib.Values ) )
      {
        var lookupValues = attrib.Values.Split( ';' );

        foreach( var _lookupValue in lookupValues.Select( lookupValue => lookupValue.Split( ':' ) ) )
          PredefinedValuesList.Add( _lookupValue[ 0 ], _lookupValue[ _lookupValue.Length > 1 ? 1 : 0 ] );
      }

      control.Properties.AllowNullInput = ( attrib.AllowNullValues ? DefaultBoolean.True : DefaultBoolean.False );
      control.Properties.Columns[ 0 ].Visible = attrib.DisplayValues;
      control.Properties.TextEditStyle = attrib.AllowEdit ? TextEditStyles.Standard : TextEditStyles.DisableTextEditor;

      control.Properties.ShowHeader = false;
      control.Properties.DataSource = PredefinedValuesList;
      control.Properties.DisplayMember = "Value";
      control.Properties.ValueMember = "Key";

      return control;
    }
  }
}
