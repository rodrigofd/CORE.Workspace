using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;

namespace FDIT.Core.Controllers
{
  public class ResetsPropertyAttribute : Attribute
  {
    public string DestinationProperty;

    public ResetsPropertyAttribute( string destinationProperty )
    {
      DestinationProperty = destinationProperty;
    }
  }

  public class GeneralDetailController : ViewController< DetailView >
  {
    protected override void OnActivated( )
    {
      base.OnActivated( );

      View.ControlsCreated += View_ControlsCreated;
    }

    protected override void OnDeactivated( )
    {
      View.ControlsCreated -= View_ControlsCreated;

      base.OnDeactivated( );
    }

    private Dictionary< string, IMemberInfo > relatedPropertyCache = new Dictionary< string, IMemberInfo >();

    private void View_ControlsCreated( object sender, EventArgs e )
    {
      if( relatedPropertyCache.Count > 0 ) return;

      foreach( var pe in View.Items.OfType< PropertyEditor >( ).Select( viewItem => viewItem ) )
      {
        var resetsPropertyAttribute = pe.MemberInfo.FindAttribute<ResetsPropertyAttribute>( );
        if( resetsPropertyAttribute != null )
        {
          relatedPropertyCache.Add( pe.Id, pe.ObjectTypeInfo.FindMember( resetsPropertyAttribute.DestinationProperty ) );
          pe.ValueStored += pe_ValueStored;
        }
      }
    }

    private void pe_ValueStored( object sender, EventArgs e )
    {
      var propertyEditor = (PropertyEditor)sender;
      var memberInfo = relatedPropertyCache[ propertyEditor.Id ];
      memberInfo.SetValue( View.CurrentObject, null );
    }
  }
}
