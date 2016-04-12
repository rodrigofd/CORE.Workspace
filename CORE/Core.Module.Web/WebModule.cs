using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.TreeListEditors.Web;
using DevExpress.ExpressApp.Updating;

namespace FDIT.Core.Web
{
  [ ToolboxItemFilter( "Xaf.Platform.Web" ) ]
  public sealed class WebModule : ModuleBase
  {
    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private readonly IContainer components = null;

    public WebModule( )
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

    public override void ExtendModelInterfaces( ModelInterfaceExtenders extenders )
    {
      base.ExtendModelInterfaces( extenders );

      extenders.Add<IModelOptions, IModelOptionsHighlightFocusedLayoutItem>( );
      extenders.Add<IModelDetailView, IModelDetailViewHighlightFocusedLayoutItem>( );
    }

    #region Component Designer generated code

    /// <summary>
    ///   Required method for Designer support - do not modify
    ///   the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent( )
    {
            // 
            // WebModule
            // 
            this.RequiredModuleTypes.Add(typeof(FDIT.Core.CoreModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Reports.ReportsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.FileAttachments.Web.FileAttachmentsAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.CloneObject.CloneObjectModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Reports.Web.ReportsAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Security.SecurityModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.TreeListEditors.Web.TreeListEditorsAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.PivotGrid.Web.PivotGridAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Chart.Web.ChartAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Chart.ChartModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.PivotGrid.PivotGridModule));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxCriteriaPropertyEditorLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxGridViewResourceLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxGridViewControlLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxImagePropertyEditorLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxEditorsResourceLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Web.Localization.ASPxperienceResourceLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.Reports.Web.ASPxReportControlLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.HtmlPropertyEditor.Web.Localization.ASPxHtmlEditorResourceLocalizer));
            this.ResourcesExportedToModel.Add(typeof(DevExpress.ExpressApp.HtmlPropertyEditor.Web.Localization.ASPxSpellCheckerResourceLocalizer));

    }

    #endregion
  }
}


public interface IModelOptionsHighlightFocusedLayoutItem : IModelNode
{
  [DefaultValue( true )]
  [Category( "Behavior" )]
  bool EnableHighlightFocusedLayoutItem { get; set; }
}

public interface IModelDetailViewHighlightFocusedLayoutItem : IModelNode
{
  [Category( "Behavior" )]
  bool HighlightFocusedLayoutItem { get; set; }
}

[DomainLogic( typeof( IModelDetailViewHighlightFocusedLayoutItem ) )]
public class ModelDetailViewHighlightFocusedLayoutItemLogic
{
  public static bool Get_HighlightFocusedLayoutItem( IModelDetailViewHighlightFocusedLayoutItem model )
  {
    if( model != null )
      return ( ( IModelOptionsHighlightFocusedLayoutItem ) model.Application.Options ).EnableHighlightFocusedLayoutItem;
    return false;
  }
}