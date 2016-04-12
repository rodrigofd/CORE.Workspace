using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using FDIT.Core.CRM;
using FDIT.Core.Personas;
using Updater = FDIT.Core.EWS.DatabaseUpdate.Updater;

namespace FDIT.Core.EWS
{
  public sealed partial class EWSModule : CoreModuleBase
  {
    internal const string PropertyCodigoEWS = "CodigoEWS";
    internal const string PropertyModifFechaEWS = "ModifFechaEWS";

    public static int NumContactosPorLote = 150;    //TODO: hacer configurable

    public EWSModule( )
    {
      InitializeComponent( );
    }

    public override IEnumerable< ModuleUpdater > GetModuleUpdaters( IObjectSpace objectSpace, Version versionFromDB )
    {
      ModuleUpdater updater = new Updater( objectSpace, versionFromDB );
      return new[ ] { updater };
    }

    public override void CustomizeTypesInfo( ITypesInfo typesInfo )
    {
      base.CustomizeTypesInfo( typesInfo );

      AddObjectProperty( typeof( Persona ), typeof( string ), PropertyCodigoEWS, new List<Attribute>{ new VisibleInListViewAttribute(false), new VisibleInDetailViewAttribute(false) } );
      AddObjectProperty( typeof( Persona ), typeof( DateTime? ), PropertyModifFechaEWS, new List<Attribute> { new VisibleInListViewAttribute( false ), new VisibleInDetailViewAttribute( false ) } );

      AddObjectProperty( typeof( Actividad ), typeof( string ), PropertyCodigoEWS, new List<Attribute> { new VisibleInListViewAttribute( false ), new VisibleInDetailViewAttribute( false ) } );
      AddObjectProperty( typeof( Actividad ), typeof( DateTime? ), PropertyModifFechaEWS, new List<Attribute> { new VisibleInListViewAttribute( false ), new VisibleInDetailViewAttribute( false ) } );
    }
  }
}
