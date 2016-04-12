using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace FDIT.Core.DatabaseUpdate
{
  public class UpdaterBase : ModuleUpdater
  {
    public UpdaterBase( IObjectSpace objectSpace, Version currentDBVersion ) :
      base( objectSpace, currentDBVersion )
    {
    }

    public void EnsureDbSchema( string schema )
    {
      var cmd = CreateCommand( string.Format( "IF NOT EXISTS ( SELECT 1 FROM information_schema.schemata WHERE schema_name = '{0}' ) EXEC sp_executesql N'CREATE SCHEMA {0}'", schema ) );
      cmd.ExecuteNonQuery( );
    }
  }
}