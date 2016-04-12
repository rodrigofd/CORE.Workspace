using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Web;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security.Adapters;
using DevExpress.ExpressApp.Security.Xpo.Adapters;
using DevExpress.ExpressApp.Web;
using DevExpress.Web;

namespace FDIT.Core.Web
{
    public class GlobalBase<T> : HttpApplication where T : WebApplication, new()
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            IsGrantedAdapter.Enable(XPOSecurityAdapterHelper.GetXpoCachedRequestSecurityAdapters());
            ASPxWebControl.CallbackError += Application_Error;
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            DevExpress.ExpressApp.Web.WebApplication.SetInstance(Session, new T());
            DevExpress.ExpressApp.Web.WebApplication.Instance.SwitchToNewStyle();

            if (ConfigurationManager.ConnectionStrings["ConnectionString"] == null)
                throw new Exception("Error de configuración. No se indicó la conexión a la base de datos.");
            else
            {
                DevExpress.ExpressApp.Web.WebApplication.Instance.ConnectionString =
                    ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
            if (Debugger.IsAttached &&
                DevExpress.ExpressApp.Web.WebApplication.Instance.CheckCompatibilityType ==
                CheckCompatibilityType.DatabaseSchema)
            {
                DevExpress.ExpressApp.Web.WebApplication.Instance.DatabaseUpdateMode =
                    DatabaseUpdateMode.UpdateDatabaseAlways;
            }
            DevExpress.ExpressApp.Web.WebApplication.Instance.Setup();
            DevExpress.ExpressApp.Web.WebApplication.Instance.Start();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            var filePath = HttpContext.Current.Request.PhysicalPath;
            if (!string.IsNullOrEmpty(filePath) && (filePath.IndexOf("Images") >= 0) && !File.Exists(filePath))
            {
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            ErrorHandling.Instance.ProcessApplicationError();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            DevExpress.ExpressApp.Web.WebApplication.LogOff(Session);
            DevExpress.ExpressApp.Web.WebApplication.DisposeInstance(Session);
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}