using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using FDIT.Core.Gestion;
using FDIT.Core.Impuestos;
using FDIT.Core.Regionales;
using FDIT.Core.Stock;
using Categoria = FDIT.Core.Impuestos.Categoria;
using Updater = FDIT.Core.DatabaseUpdate.Updater;

namespace FDIT.Core.AFIP
{
  public sealed class AFIPModule : CoreModuleBase
  {
    public const string PropertyNameCodigoAfip = "CodigoAFIP";
    public const string PropertyNameFacturaElectronicaWsfeV1 = "HabilitadoFacElectronicaWsfeV1";

    public AFIPModule( )
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

      AddObjectProperty( typeof( Provincia ), typeof( int ), PropertyNameCodigoAfip );
      AddObjectProperty( typeof( Categoria ), typeof( int ), PropertyNameCodigoAfip );

      AddObjectProperty( typeof( Fondos.Moneda ), typeof( Moneda ), PropertyNameCodigoAfip );
      AddObjectProperty( typeof( Gestion.ComprobanteTipo ), typeof( ComprobanteTipo ), PropertyNameCodigoAfip );
      AddObjectProperty( typeof( Personas.IdentificacionTipo ), typeof( IdentificacionTipo ), PropertyNameCodigoAfip );
      AddObjectProperty( typeof( ConceptoGrupo ), typeof( short ), PropertyNameCodigoAfip );
      AddObjectProperty( typeof( UnidadMedida ), typeof( short ), PropertyNameCodigoAfip );
      AddObjectProperty( typeof( Alicuota ), typeof( short ), PropertyNameCodigoAfip );

      AddObjectProperty( typeof( Talonario ), typeof( bool ), PropertyNameFacturaElectronicaWsfeV1 );
    }

    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      // 
      // AFIPModule
      // 

    }

    #endregion
  }
}
