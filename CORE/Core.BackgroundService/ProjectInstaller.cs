using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;

namespace FDIT.Core
{
  [RunInstaller( true )]
  public partial class ProjectInstaller : Installer
  {
    public ProjectInstaller( )
    {
      InitializeComponent( );
    }

    public override void Install( IDictionary stateSaver )
    {
      base.Install( stateSaver );

      if( !EventLog.SourceExists( BackgroundService.fCoreBackgroundService ) )
        EventLog.CreateEventSource( BackgroundService.fCoreBackgroundService, BackgroundService.fFdItCore );
    }

    public override void Uninstall( IDictionary savedState )
    {
      // Delete the source, if it exists.
      if( EventLog.SourceExists( BackgroundService.fCoreBackgroundService ) )
      {
        EventLog.DeleteEventSource( BackgroundService.fCoreBackgroundService );
        EventLog.Delete( BackgroundService.fFdItCore );
      }

      base.Uninstall( savedState );
    }
  }
}
