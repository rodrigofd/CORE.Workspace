using System.Diagnostics;
using DevExpress.ExpressApp.AuditTrail;
using FDIT.Core.AFIP;
using FDIT.Core.Seguros;
using FDIT.Core.Seguros.Web;

namespace FDIT.Core.Web
{
  public class WebApplication : WebApplicationBase
  {
    protected AFIPModule afipModule1;
    protected SegurosModule segurosModule1;
    protected SegurosWebModule segurosWebModule1;

    public WebApplication( ) : base( )
    {
      InitializeComponent( );
    }

    private void InitializeComponent( )
    {
      afipModule1 = new AFIPModule( );
      segurosModule1 = new SegurosModule( );
      segurosWebModule1 = new SegurosWebModule( );

      
      Modules.Add( afipModule1 );
      Modules.Add( segurosModule1 );
      Modules.Add( segurosWebModule1 );
    }
  }
}
