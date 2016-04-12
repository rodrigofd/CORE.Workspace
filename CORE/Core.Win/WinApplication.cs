using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.AuditTrail;
using DevExpress.ExpressApp.Chart;
using DevExpress.ExpressApp.Chart.Win;
using DevExpress.ExpressApp.CloneObject;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.FileAttachments.Win;
using DevExpress.ExpressApp.HtmlPropertyEditor.Win;
using DevExpress.ExpressApp.Kpi;
using DevExpress.ExpressApp.PivotGrid;
using DevExpress.ExpressApp.Reports;
using DevExpress.ExpressApp.Reports.Win;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.TreeListEditors;
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Validation.Win;
using DevExpress.ExpressApp.ViewVariantsModule;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.Xpo;
using FDIT.Core.AFIP;
using FDIT.Core.Module.Win;
using FDIT.Core.Seguridad;
using FDIT.Core.Seguros;
using FDIT.Core.Seguros.Win;

namespace FDIT.Core.Win
{
    public class WinApplication : DevExpress.ExpressApp.Win.WinApplication
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private readonly IContainer components = null;

        private AFIPModule afipModule1;
        private AFIPWsMtxcaModule afipWsMtxcaModule;
        private AuditTrailModule auditTrailModule1;
        private CoreAuthentication CoreAuthenticationApp1;
        private ChartModule chartModule1;
        private ChartWindowsFormsModule chartWindowsFormsModule1;
        private CloneObjectModule cloneObjectModule1;
        private ConditionalAppearanceModule conditionalAppearanceModule1;
        private CoreModule coreModule1;
        private FileAttachmentsWindowsFormsModule fileAttachmentsWindowsFormsModule1;
        private HtmlPropertyEditorWindowsFormsModule htmlPropertyEditorWindowsFormsModule1;
        private KpiModule kpiModule1;
        private SystemModule module1;
        private SystemWindowsFormsModule module2;
        private WinModule module4;
        private PivotGridModule pivotGridModule1;
        private ReportsModule reportsModule1;
        private ReportsWindowsFormsModule reportsWindowsFormsModule1;
        private SecurityModule securityModule1;
        private SecurityStrategyComplex securityStrategyComplex1;
        private SegurosModule segurosModule1;
        private SegurosWinModule segurosWinModule1;
        private SqlConnection sqlConnection1;
        private StateMachineModule stateMachineModule1;
        private TreeListEditorsModuleBase treeListEditorsModuleBase1;
        private TreeListEditorsWindowsFormsModule treeListEditorsWindowsFormsModule1;
        private ValidationModule validationModule1;
        private ValidationWindowsFormsModule validationWindowsFormsModule1;
        private ViewVariantsModule viewVariantsModule1;

        public WinApplication()
        {
            InitializeComponent();

            var fileSystemStoreLoc = Convert.ToString(ConfigurationManager.AppSettings["FileAttachmentBasePath"]);
            fileSystemStoreLoc = fileSystemStoreLoc.Replace("~",
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            coreModule1.FileAttachmentsBasePath = fileSystemStoreLoc;

            DelayedViewItemsInitialization = true;
        }

        /// <summary>
        ///     Clean up any resources being used.
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

        protected override void OnLoggedOff()
        {
            base.OnLoggedOff();
        }

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection, false);
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }

        protected override IObjectSpace CreateLogonWindowObjectSpace(object logonParameters)
        {
            return ((CoreAppLogonParameters) logonParameters).ObjectSpace = CreateObjectSpace();
        }

        private void CoreWindowsFormsApplication_CustomizeLanguagesList(object sender, CustomizeLanguagesListEventArgs e)
        {
            var userLanguageName = Thread.CurrentThread.CurrentUICulture.Name;
            if (userLanguageName != "en-US" &&
                e.Languages.IndexOf(userLanguageName) == -1)
            {
                e.Languages.Add(userLanguageName);
            }
        }

        private void CoreWindowsFormsApplication_DatabaseVersionMismatch(object sender,
            DatabaseVersionMismatchEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                e.Updater.Update();
                e.Handled = true;
            }
            else
            {
                throw new InvalidOperationException(
                    "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                    "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
                    "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
                    "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                    "or manually create a database using the 'DBUpdater' tool.\r\n" +
                    "Anyway, refer to the 'Update Application and Database Versions' help topic at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm " +
                    "for more detailed information. If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/");
            }
        }

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule();
            this.module4 = new FDIT.Core.Module.Win.WinModule();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.cloneObjectModule1 = new DevExpress.ExpressApp.CloneObject.CloneObjectModule();
            this.reportsModule1 = new DevExpress.ExpressApp.Reports.ReportsModule();
            this.validationModule1 = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.conditionalAppearanceModule1 =
                new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.stateMachineModule1 = new DevExpress.ExpressApp.StateMachine.StateMachineModule();
            this.securityStrategyComplex1 = new DevExpress.ExpressApp.Security.SecurityStrategyComplex();
            this.CoreAuthenticationApp1 = new FDIT.Core.Seguridad.CoreAuthentication();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.coreModule1 = new FDIT.Core.CoreModule();
            this.afipModule1 = new FDIT.Core.AFIP.AFIPModule();
            this.afipWsMtxcaModule = new FDIT.Core.AFIP.AFIPWsMtxcaModule();
            this.segurosModule1 = new FDIT.Core.Seguros.SegurosModule();
            this.segurosWinModule1 = new FDIT.Core.Seguros.Win.SegurosWinModule();
            this.fileAttachmentsWindowsFormsModule1 =
                new DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule();
            this.reportsWindowsFormsModule1 = new DevExpress.ExpressApp.Reports.Win.ReportsWindowsFormsModule();
            this.viewVariantsModule1 = new DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule();
            this.auditTrailModule1 = new DevExpress.ExpressApp.AuditTrail.AuditTrailModule();
            this.pivotGridModule1 = new DevExpress.ExpressApp.PivotGrid.PivotGridModule();
            this.treeListEditorsModuleBase1 = new DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase();
            this.chartModule1 = new DevExpress.ExpressApp.Chart.ChartModule();
            this.kpiModule1 = new DevExpress.ExpressApp.Kpi.KpiModule();
            this.validationWindowsFormsModule1 = new DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule();
            this.htmlPropertyEditorWindowsFormsModule1 =
                new DevExpress.ExpressApp.HtmlPropertyEditor.Win.HtmlPropertyEditorWindowsFormsModule();
            this.treeListEditorsWindowsFormsModule1 =
                new DevExpress.ExpressApp.TreeListEditors.Win.TreeListEditorsWindowsFormsModule();
            this.chartWindowsFormsModule1 = new DevExpress.ExpressApp.Chart.Win.ChartWindowsFormsModule();
            ((System.ComponentModel.ISupportInitialize) (this)).BeginInit();
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // reportsModule1
            // 
            this.reportsModule1.EnableInplaceReports = true;
            this.reportsModule1.ReportDataType = typeof (DevExpress.Persistent.BaseImpl.ReportData);
            // 
            // validationModule1
            // 
            this.validationModule1.AllowValidationDetailsAccess = true;
            // 
            // stateMachineModule1
            // 
            this.stateMachineModule1.StateMachineStorageType =
                typeof (DevExpress.ExpressApp.StateMachine.Xpo.XpoStateMachine);
            // 
            // securityStrategyComplex1
            // 
            this.securityStrategyComplex1.Authentication = this.CoreAuthenticationApp1;
            this.securityStrategyComplex1.RoleType = typeof (FDIT.Core.Seguridad.Rol);
            this.securityStrategyComplex1.UserType = typeof (FDIT.Core.Seguridad.Usuario);
            // 
            // coreModule1
            // 
            this.coreModule1.FileAttachmentsBasePath = "";
            // 
            // viewVariantsModule1
            // 
            this.viewVariantsModule1.GenerateVariantsNode = true;
            this.viewVariantsModule1.ShowAdditionalNavigation = false;
            // 
            // auditTrailModule1
            // 
            this.auditTrailModule1.AuditDataItemPersistentType =
                typeof (DevExpress.Persistent.BaseImpl.AuditDataItemPersistent);
            // 
            // WinApplication
            // 
            this.ApplicationName = "CORE";
            this.Connection = this.sqlConnection1;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.cloneObjectModule1);
            this.Modules.Add(this.reportsModule1);
            this.Modules.Add(this.validationModule1);
            this.Modules.Add(this.conditionalAppearanceModule1);
            this.Modules.Add(this.stateMachineModule1);
            this.Modules.Add(this.viewVariantsModule1);
            this.Modules.Add(this.auditTrailModule1);
            this.Modules.Add(this.pivotGridModule1);
            this.Modules.Add(this.treeListEditorsModuleBase1);
            this.Modules.Add(this.chartModule1);
            this.Modules.Add(this.kpiModule1);
            this.Modules.Add(this.coreModule1);
            this.Modules.Add(this.fileAttachmentsWindowsFormsModule1);
            this.Modules.Add(this.reportsWindowsFormsModule1);
            this.Modules.Add(this.validationWindowsFormsModule1);
            this.Modules.Add(this.securityModule1);
            this.Modules.Add(this.htmlPropertyEditorWindowsFormsModule1);
            this.Modules.Add(this.treeListEditorsWindowsFormsModule1);
            this.Modules.Add(this.chartWindowsFormsModule1);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.afipModule1);
            this.Modules.Add(this.afipWsMtxcaModule);
            this.Modules.Add(this.segurosModule1);
            this.Modules.Add(this.segurosWinModule1);
            this.Security = this.securityStrategyComplex1;

            this.DatabaseVersionMismatch +=
                new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(
                    this.CoreWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList +=
                new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(
                    this.CoreWindowsFormsApplication_CustomizeLanguagesList);

            ((System.ComponentModel.ISupportInitialize) (this)).EndInit();
        }

        #endregion
    }
}