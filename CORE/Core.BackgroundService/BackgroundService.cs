using System;
using System.Configuration;
using System.Timers;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using ServiceBase = System.ServiceProcess.ServiceBase;

namespace FDIT.Core
{
  public partial class BackgroundService : ServiceBase
  {
    public const string fCoreBackgroundService = "Core Background Service";
    public const string fFdItCore = "FD-IT CORE";

    private readonly Timer scheduleTimer;
    private XPCollection< EWS.Cuenta > cuentas;
    
    public BackgroundService( )
    {
      InitializeComponent( );

      eventLog.Source = fCoreBackgroundService;
      eventLog.Log = fFdItCore;

      if( ConfigurationManager.ConnectionStrings[ "ConnectionString" ] == null )
        throw new Exception( "No se encontró la cadena de conexión a la base de datos" );

      XpoDefault.DataLayer = XpoDefault.GetDataLayer( ConfigurationManager.ConnectionStrings[ "ConnectionString" ].ConnectionString, AutoCreateOption.None );
      
      cuentas = new XPCollection<EWS.Cuenta>( );
      
      scheduleTimer = new Timer { Interval = 10 * 1000 };
      scheduleTimer.Elapsed += scheduleTimer_Elapsed;
    }

    protected override void OnStart( string[ ] args )
    {
      scheduleTimer.Start( );
      eventLog.WriteEntry( "Started" );
    }

    protected void scheduleTimer_Elapsed( object sender, ElapsedEventArgs e )
    {
      ServiceEmailMethod( );
    }

    public void ServiceEmailMethod( )
    {
      foreach( EWS.Cuenta cuenta in cuentas )
      {
        eventLog.WriteEntry( "Cargado la cuenta" + cuenta.Grupo.Persona.Nombre + " id " + cuenta.Usuario );
      }
    }

    protected override void OnStop( )
    {
      scheduleTimer.Stop( );
      eventLog.WriteEntry( "Stopped" );
    }

    protected override void OnPause( )
    {
      scheduleTimer.Stop( );
      eventLog.WriteEntry( "Paused" );
    }

    protected override void OnContinue( )
    {
      scheduleTimer.Start( );
      eventLog.WriteEntry( "Continuing" );
    }

    protected override void OnShutdown( )
    {
      scheduleTimer.Stop( );
      eventLog.WriteEntry( "ShutDowned" );
    }

    public void DoStart( )
    {
      OnStart( new string[ ] { } );
    }

    public void DoStop( )
    {
      OnStop( );
    }
  }
}
