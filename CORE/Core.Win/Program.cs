using System;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Adapters;
using DevExpress.ExpressApp.Security.Xpo.Adapters;
using DevExpress.ExpressApp.Win.Utils;

namespace FDIT.Core.Win
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
#if EASYTEST
			DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            EditModelPermission.AlwaysGranted = Debugger.IsAttached;
            var winApplication = new WinApplication();
            winApplication.SplashScreen = new DXSplashScreen("logotipo_big2.png");

            IsGrantedAdapter.Enable(XPOSecurityAdapterHelper.GetXpoCachedRequestSecurityAdapters());

#if EASYTEST
			if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
				winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
			}
#endif
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                winApplication.ConnectionString =
                    ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }

            try
            {
                winApplication.Setup();
                winApplication.Start();
            }
            catch (Exception e)
            {
                winApplication.HandleException(e);
            }
        }
    }
}