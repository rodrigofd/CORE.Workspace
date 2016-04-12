using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Updating;
using FDIT.Core.Gestion;
using Updater = FDIT.Core.DatabaseUpdate.Updater;

namespace FDIT.Core.AFIP
{
  public sealed class AFIPWsMtxcaModule : CoreModuleBase
  {
    public const string PropertyNameFacturaElectronicaMtxca = "HabilitadoFacElectronicaMtxca";

    /// <summary>
    ///   Required designer variable.
    /// </summary>
    private readonly IContainer components = null;

    public AFIPWsMtxcaModule( )
    {
      InitializeComponent( );
    }

    public override IEnumerable< ModuleUpdater > GetModuleUpdaters( IObjectSpace objectSpace, Version versionFromDB )
    {
      return new[ ] { new Updater( objectSpace, versionFromDB ) };
    }

    public override void CustomizeTypesInfo( ITypesInfo typesInfo )
    {
      base.CustomizeTypesInfo( typesInfo );

      AddObjectProperty( typeof( Talonario ), typeof( bool ), PropertyNameFacturaElectronicaMtxca );
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
    }

    #endregion
  }
}
