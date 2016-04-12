using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace Core.BackgroundService
{
  internal static class Program
  {
    /// <summary>
    ///   Punto de entrada principal para la aplicación.
    /// </summary>
    private static void Main( )
    {
      if( !Debugger.IsAttached )
      {
        var ServicesToRun = new ServiceBase[ ] { new FDIT.Core.BackgroundService( ) };
        ServiceBase.Run( ServicesToRun );
      }
      else
      {
        // Debug code: this allows the process to run as a non-service.
        // It will kick off the service start point, but never kill it.
        // Shut down the debugger to exit
        var service = new FDIT.Core.BackgroundService( );
        service.DoStart( );

        Thread.Sleep( Timeout.Infinite );
      }
    }
  }
}
