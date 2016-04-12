using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using FDIT.Core.Editors;

namespace FDIT.Core.Web.Editors
{
  [ PropertyEditor( typeof( String ), false ) ]
  public class WebSimpleComboBoxEditor : ASPxPropertyEditor
  {
    protected bool AllowNullValues;
    protected Dictionary< string, string > PredefinedValuesList;
    private ASPxComboBox dropDownControl;

    public WebSimpleComboBoxEditor( Type objectType, IModelMemberViewItem info )
      : base( objectType, info )
    {
    }

    protected override void SetupControl( WebControl control )
    {
      var attrib = MemberInfo.FindAttribute< SimpleComboBoxAttribute >( );
      PredefinedValuesList = new Dictionary< string, string >( );

      var comboControl = ( control as ASPxComboBox );
      if( comboControl == null ) return;

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

      AllowNullValues = ( attrib.AllowNullValues );
      comboControl.DropDownStyle = attrib.AllowEdit ? DropDownStyle.DropDown : DropDownStyle.DropDownList;

      if( ViewEditMode == ViewEditMode.Edit )
      {
        if( AllowNullValues )
          comboControl.Items.Add( CaptionHelper.NullValueText, "" );

        foreach( var l in PredefinedValuesList )
          comboControl.Items.Add( l.Value, l.Key );
      }
    }

    protected override WebControl CreateEditModeControlCore( )
    {
      dropDownControl = RenderHelper.CreateASPxComboBox( );
      dropDownControl.SelectedIndexChanged += ExtendedEditValueChangedHandler;
      return dropDownControl;
    }

    public override void BreakLinksToControl( bool unwireEventsOnly )
    {
      if( dropDownControl != null )
        dropDownControl.SelectedIndexChanged -= ExtendedEditValueChangedHandler;

      base.BreakLinksToControl( unwireEventsOnly );
    }

    protected override void ReadViewModeValueCore( )
    {
      var currentValue = PropertyValue as string;
      if( currentValue == null ) return;
      string val;
      if( PredefinedValuesList.TryGetValue( currentValue, out val ) && InplaceViewModeEditor is ITextControl )
      {
        ( InplaceViewModeEditor as ITextControl ).Text = val;
      }
      else
      {
        base.ReadViewModeValueCore( );
      }
    }

    protected override void SetImmediatePostDataScript( string script )
    {
      base.SetImmediatePostDataScript( script );
      if( dropDownControl != null )
      {
        dropDownControl.EnableClientSideAPI = true;
        dropDownControl.ClientSideEvents.SelectedIndexChanged = script;
      }
    }
  }
}
