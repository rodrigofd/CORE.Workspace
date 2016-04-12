using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace FDIT.Core.Module.Win
{
  [ ToolboxItemFilter( "Xaf.Platform.Win" ) ]
  public sealed class WinModule : ModuleBase
  {
    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private readonly IContainer components = null;

    public WinModule( )
    {
      InitializeComponent( );
    }

    public override IEnumerable< ModuleUpdater > GetModuleUpdaters( IObjectSpace objectSpace, Version versionFromDB )
    {
      return ModuleUpdater.EmptyModuleUpdaters;
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

    #region Component Designer generated code

    /// <summary>
    ///   Required method for Designer support - do not modify
    ///   the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
            // 
            // WinModule
            // 
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule));
            this.RequiredModuleTypes.Add(typeof(FDIT.Core.CoreModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Reports.ReportsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Reports.Win.ReportsWindowsFormsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.CloneObject.CloneObjectModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Security.SecurityModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.HtmlPropertyEditor.Win.HtmlPropertyEditorWindowsFormsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.TreeListEditors.Win.TreeListEditorsWindowsFormsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Chart.Win.ChartWindowsFormsModule));

    }

    #endregion
  }
}
