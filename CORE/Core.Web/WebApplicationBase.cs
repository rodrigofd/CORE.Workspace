using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.CloneObject;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Reports;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.StateMachine.Xpo;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using FDIT.Core.Seguridad;
using FDIT.Core.Web.Controllers;

namespace FDIT.Core.Web
{
    public class WebApplicationBase : DevExpress.ExpressApp.Web.WebApplication
    {
        protected CoreAuthentication CoreAuthenticationApp1;
        protected CloneObjectModule cloneObjectModule1;
        protected ConditionalAppearanceModule conditionalAppearanceModule1;
        protected CoreModule coreModule1;
        protected ReportsModule reportsModule1;
        protected SecurityModule securityModule1;
        protected SecurityStrategyComplex securityStrategyComplex1;
        protected SqlConnection sqlConnection1;
        protected StateMachineModule stateMachineModule1;
        protected SystemAspNetModule systemAspNetModule;
        protected SystemModule systemModule;
        protected ValidationModule validationModule1;
        protected WebModule webModule;

        public WebApplicationBase()
        {
            InitializeComponent();

            var fileSystemStoreLoc = Convert.ToString(ConfigurationManager.AppSettings["FileAttachmentBasePath"]);
            if (!string.IsNullOrEmpty(fileSystemStoreLoc))
                fileSystemStoreLoc = HttpContext.Current.Server.MapPath(fileSystemStoreLoc);
            coreModule1.FileAttachmentsBasePath = fileSystemStoreLoc;
        }

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            CreateXPObjectSpaceProvider(args.ConnectionString, args);

            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }

        private void CreateXPObjectSpaceProvider(string connectionString, CreateCustomObjectSpaceProviderEventArgs e)
        {
            var application = HttpContext.Current != null ? HttpContext.Current.Application : null;
            IXpoDataStoreProvider dataStoreProvider = null;
            if (application != null && application["DataStoreProvider"] != null)
            {
                dataStoreProvider = application["DataStoreProvider"] as IXpoDataStoreProvider;
                e.ObjectSpaceProvider = new XPObjectSpaceProvider(dataStoreProvider, true);
            }
            else
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    connectionString = XpoDefault.GetConnectionPoolString(connectionString);
                    dataStoreProvider = new ConnectionStringDataStoreProvider(connectionString, true);
                }
                else if (e.Connection != null)
                {
                    dataStoreProvider = new ConnectionDataStoreProvider(e.Connection);
                }
                if (application != null)
                {
                    application["DataStoreProvider"] = dataStoreProvider;
                }
                e.ObjectSpaceProvider = new XPObjectSpaceProvider(dataStoreProvider, true);
            }
        }

        protected override void OnLoggedOn(LogonEventArgs args)
        {
            base.OnLoggedOn(args);

            var coreAppLogonParameters = (CoreAppLogonParameters) args.LogonParameters;

            SetSessionRegionalSettings(coreAppLogonParameters);
        }

        protected virtual void SetSessionRegionalSettings(CoreAppLogonParameters logonParameters)
        {
            var usuario = logonParameters.UsuarioActual();

            var language = "";
            
            if (usuario.EmpresaPredeterminada.IdiomaPredeterminado != null)
                language = usuario.EmpresaPredeterminada.IdiomaPredeterminado.Codigo1;
            if (usuario.IdiomaPredeterminado != null)
                language = usuario.IdiomaPredeterminado.Codigo1;

            if (language != "")
                SetLanguage(language);

            var culture = "";
            
            if (usuario.EmpresaPredeterminada.CulturaPredeterminada != null)
                culture = usuario.EmpresaPredeterminada.CulturaPredeterminada.Codigo;
            if (usuario.CulturaPredeterminada != null)
                culture = usuario.CulturaPredeterminada.Codigo;

            if (culture != "")
                SetFormattingCulture(culture);
        }

        protected override List<Controller> CreateLogonWindowControllers()
        {
            var controllers = base.CreateLogonWindowControllers();

            //foreach( var c in controllers.OfType<FocusAcceptButtonController>( ) )
            //  c.Active[ "NotInLogonWindow" ] = false;

            controllers.Add(CreateController<WebFocusDefaultDetailViewItemController>());

            return controllers;
        }

        private void CoreAspNetApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e)
        {
            e.Updater.Update();
            e.Handled = true;
        }

        private void InitializeComponent()
        {
            systemModule = new SystemModule();
            systemAspNetModule = new SystemAspNetModule();
            webModule = new WebModule();
            sqlConnection1 = new SqlConnection();
            cloneObjectModule1 = new CloneObjectModule();
            reportsModule1 = new ReportsModule();
            validationModule1 = new ValidationModule();
            conditionalAppearanceModule1 = new ConditionalAppearanceModule();
            stateMachineModule1 = new StateMachineModule();
            coreModule1 = new CoreModule();
            securityStrategyComplex1 = new SecurityStrategyComplex();
            securityModule1 = new SecurityModule();
            CoreAuthenticationApp1 = new CoreAuthentication();
            ((ISupportInitialize) this).BeginInit();
            // 
            // sqlConnection1
            //
            sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // reportsModule1
            // 
            reportsModule1.EnableInplaceReports = true;
            reportsModule1.ReportDataType = typeof (ReportData);
            // 
            // stateMachineModule1
            // 
            stateMachineModule1.StateMachineStorageType = typeof (XpoStateMachine);
            // 
            // securityStrategyComplex1
            // 
            securityStrategyComplex1.Authentication = CoreAuthenticationApp1;
            securityStrategyComplex1.RoleType = typeof (Rol);
            securityStrategyComplex1.UserType = typeof (Usuario);
            // 
            // CoreAspNetApplication
            // 
            ApplicationName = "CORE";
            LinkNewObjectToParentImmediately = false;
            CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            Connection = sqlConnection1;
            Modules.Add(systemModule);
            Modules.Add(systemAspNetModule);
            Modules.Add(coreModule1);
            Modules.Add(cloneObjectModule1);
            Modules.Add(reportsModule1);
            Modules.Add(validationModule1);
            Modules.Add(conditionalAppearanceModule1);
            Modules.Add(stateMachineModule1);
            Modules.Add(webModule);
            Modules.Add(securityModule1);
            // TODO: migrate
            // CollectionsEditMode = ViewEditMode.Edit;
            Security = securityStrategyComplex1;
            DatabaseVersionMismatch += CoreAspNetApplication_DatabaseVersionMismatch;
            ((ISupportInitialize) this).EndInit();
        }
    }
}