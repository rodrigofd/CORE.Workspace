using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.ViewVariantsModule;
using DevExpress.Persistent.Validation;
using Updater = FDIT.Core.Seguros.DatabaseUpdate.Updater;

namespace FDIT.Core.Seguros
{
  public sealed partial class SegurosModule : CoreModuleBase
  {
    public SegurosModule( )
    {
      InitializeComponent( );
    }

    public override IEnumerable< ModuleUpdater > GetModuleUpdaters( IObjectSpace objectSpace, Version versionFromDB )
    {
      ModuleUpdater updater = new Updater( objectSpace, versionFromDB );
      return new[ ] { updater };
    }

    public override void Setup( XafApplication application )
    {
      base.Setup( application );

      application.SetupComplete += application_SetupComplete;
    }

    private void application_SetupComplete( object sender, EventArgs e )
    {
      var application = ( XafApplication ) sender;

      var modelViews = ( IModelList< IModelView > ) application.Model.Application.Views;
      var modelViewVariants = ( ( IModelViewVariants ) modelViews[ "FDIT.Core.Seguros.Documento_Items_ListView" ] ).Variants;
      var rootModelViewVariants = ( IModelList< IModelVariant > ) modelViewVariants;

      var documentoItemType = XafTypesInfo.Instance.PersistentTypes.First( info => info.Type == typeof( DocumentoItem ) );

      var variantId = documentoItemType.Type.FullName + "_ListView";
      var defaultVariant = ( ( ModelNode ) rootModelViewVariants ).AddNode< IModelVariant >( variantId );
      defaultVariant.View = modelViews[ variantId ];
      defaultVariant.Caption = variantId;
      modelViewVariants.Current = defaultVariant;

      foreach( var type in documentoItemType.Descendants.OrderBy( info => info.Name ) )
      {
        variantId = type.Type.FullName + "_ListView";

        var modelViewVariant = ( ( ModelNode ) rootModelViewVariants ).AddNode< IModelVariant >( variantId );
        modelViewVariant.View = modelViews[ variantId ];
        modelViewVariant.Caption = variantId;
      }
    }

    public override void Setup( ApplicationModulesManager moduleManager )
    {
      base.Setup( moduleManager );

      ValidationRulesRegistrator.RegisterRule( moduleManager, typeof( RuleDocumentoValido ), typeof( IRuleBaseProperties ) );
      ValidationRulesRegistrator.RegisterRule( moduleManager, typeof( RuleDocumentoItemValido ), typeof( IRuleBaseProperties ) );
    }
  }
}
