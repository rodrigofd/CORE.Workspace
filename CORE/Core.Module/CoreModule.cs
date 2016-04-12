using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.TreeListEditors;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.AuditTrail;
using FDIT.Core.Controllers.Fondos;
using FDIT.Core.Seguridad;
using FDIT.Core.Util;
using Updater = FDIT.Core.DatabaseUpdate.Updater;

namespace FDIT.Core
{
  public sealed class CoreModule : CoreModuleBase
  {
    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private readonly IContainer components = null;

    public CoreModule( )
    {
      InitializeComponent( );
    }

    /// <summary>
    ///   Indica la ruta base donde el módulo de archivos adjuntos a objetos, almacena los archivos reales
    /// </summary>
    public string FileAttachmentsBasePath
    {
      get { return Archivo.FileSystemStoreLocation; }
      set { Archivo.FileSystemStoreLocation = value; }
    }

    /// <summary>
    ///   Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if( disposing && ( components != null ) )
      {
        components.Dispose( );
      }
      base.Dispose( disposing );
    }

    public override void Setup( XafApplication application )
    {
      base.Setup( application );

      application.SetupComplete += application_SetupComplete;
      application.CreateCustomLogonWindowObjectSpace += ( sender, args ) => { args.ObjectSpace = ( ( CoreAppLogonParameters ) args.LogonParameters ).ObjectSpace = application.CreateObjectSpace( ); };

      if( CriteriaOperator.GetCustomFunctions( ).GetCustomFunction( "ConvertToStr" ) == null )
        CriteriaOperator.RegisterCustomFunction( new ConvertToStrFunction( ) );
    }

    private void application_SetupComplete( object sender, EventArgs e )
    {
      var application = ( XafApplication ) sender;

      ComprobanteItemDetailViewController.UpdateModel( application );

      AuditTrailService.Instance.SaveAuditTrailData += Instance_SaveAuditTrailData;
    }

    void Instance_SaveAuditTrailData( object sender, SaveAuditTrailDataEventArgs e )
    {
      /*for(var i = e.AuditTrailDataItems.Count-1; i >= 0; i-- )
        if( e.Session.IsNewObject( e.AuditTrailDataItems[i] ) )
          e.AuditTrailDataItems.RemoveAt( i );*/
      e.Handled = true;
    }

    public override IEnumerable< ModuleUpdater > GetModuleUpdaters( IObjectSpace objectSpace, Version versionFromDB )
    {
      return new[ ] { new Updater( objectSpace, versionFromDB ) };
    }

    public override void ExtendModelInterfaces( ModelInterfaceExtenders extenders )
    {
      base.ExtendModelInterfaces( extenders );

      extenders.Add< IModelLayoutItem, IModelLayoutItemWithCaptionWidth >( );
    }

    #region Component Designer generated code

    /// <summary>
    ///   Required method for Designer support - do not modify
    ///   the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
            // 
            // CoreModule
            // 
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.CloneObject.CloneObjectModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.StateMachine.StateMachineModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Reports.ReportsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.AuditTrail.AuditTrailModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.PivotGrid.PivotGridModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Chart.ChartModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Kpi.KpiModule));

    }

    #endregion
  }
}
