using System;
using DevExpress.ExpressApp;
using FDIT.Core.DatabaseUpdate;

namespace FDIT.Core.EWS.DatabaseUpdate
{
  public class Updater : UpdaterBase
  {
    public Updater( IObjectSpace objectSpace, Version currentDBVersion ) : base( objectSpace, currentDBVersion )
    {
    }

    public override void UpdateDatabaseAfterUpdateSchema( )
    {
      base.UpdateDatabaseAfterUpdateSchema( );
    }

    public override void UpdateDatabaseBeforeUpdateSchema( )
    {
      base.UpdateDatabaseBeforeUpdateSchema( );

      EnsureDbSchema( "ews" );
    }
  }
}
