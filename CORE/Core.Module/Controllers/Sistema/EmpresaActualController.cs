using System.ComponentModel;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Xpo;
using FDIT.Core.Regionales;
using FDIT.Core.Seguridad;
using FDIT.Core.Sistema;

namespace FDIT.Core.Controllers.Sistema
{
    public class EmpresaActualController : ViewController
    {
        private SingleChoiceAction _cambiarEmpresaActualAction;

        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        public EmpresaActualController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (!SecuritySystem.IsAuthenticated ||
                CoreAppLogonParameters.Instance == null ||
                !(ObjectSpace is XPObjectSpace)) return;

            CoreAppLogonParameters.Instance.ObjectSpace = ObjectSpace;

            CargarListaEmpresas();

            MarcarEmpresaActual();

            FiltrarVistaPorEmpresa();
            FiltrarVistaPorPais();

            var logoffController = Frame.GetController<LogoffController>();
            if (logoffController != null) logoffController.LogoffAction.Execute += LogoffAction_Execute;

            var newObjectViewController = Frame.GetController<NewObjectViewController>();
            if (newObjectViewController != null)
                newObjectViewController.ObjectCreated += NewObjectViewController_ObjectCreated;
        }

        protected override void OnDeactivated()
        {
            if (SecuritySystem.IsAuthenticated && ObjectSpace is XPObjectSpace)
            {
                var logoffController = Frame.GetController<LogoffController>();
                if (logoffController != null) logoffController.LogoffAction.Execute -= LogoffAction_Execute;

                var newObjectViewController = Frame.GetController<NewObjectViewController>();
                if (newObjectViewController != null)
                    newObjectViewController.ObjectCreated -= NewObjectViewController_ObjectCreated;
            }

            base.OnDeactivated();
        }

        private void LogoffAction_Execute(object sender, SimpleActionExecuteEventArgs simpleActionExecuteEventArgs)
        {
            if (Application != null)
            {
                var parameters = SecuritySystem.Instance.LogonParameters as ISupportResetLogonParameters;
                parameters?.Reset();

                Application.LogOff();
            }
        }

        public void NewObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            var objetoPorEmpresa = e.CreatedObject as IObjetoPorEmpresa;
            if (objetoPorEmpresa != null)
                objetoPorEmpresa.Empresa = CoreAppLogonParameters.Instance.EmpresaActual(e.ObjectSpace);

            //TODO proponer tambien pais predeterminado? o en cada caso?
        }

        private void FiltrarVistaPorEmpresa()
        {
            if (!(View is ListView)) return;
            
            var empresaActualId = CoreAppLogonParameters.Instance.EmpresaActualId;
            var filtroPorEmpresaAttribute = View.ObjectTypeInfo.FindAttribute<FiltroPorEmpresaAttribute>();
            
            if (typeof (IObjetoPorEmpresa).IsAssignableFrom(View.ObjectTypeInfo.Type))
                ((ListView) View).CollectionSource.Criteria["filtro_empresa"] =
                    CriteriaOperator.Parse("ISNULL(Empresa) OR Empresa.Oid = ?", empresaActualId);
            else if (filtroPorEmpresaAttribute != null)
                ((ListView) View).CollectionSource.Criteria["filtro_empresa"] =
                    CriteriaOperator.Parse(filtroPorEmpresaAttribute.CriteriaEmpresa, empresaActualId);
        }

        /// <summary>
        ///     Aplica un filtro predeterminado del pais actual, si la clase tiene el atributo correspondiente
        /// </summary>
        private void FiltrarVistaPorPais()
        {
            if (!(View is ListView)) return;

            var typeInfo = View.ObjectTypeInfo;
            var filtroPorPaisAttribute = typeInfo.FindAttributes<FiltroPorPaisAttribute>().Any(c => c.Filtrar);
            var paisPredeterminado = Identificadores.GetInstance(ObjectSpace).PaisPredeterminado;

            if (!filtroPorPaisAttribute || paisPredeterminado == null) return;

            if (typeInfo.FindMember("Pais") != null)
                ((ListView) View).CollectionSource.Criteria["filtro_pais_actual"] =
                    new GroupOperator(GroupOperatorType.Or,
                        new BinaryOperator("Pais.Oid", paisPredeterminado.Oid, BinaryOperatorType.Equal),
                        new NullOperator("Pais"));
            else if (typeInfo.FindMember("NacimientoPais") != null)
                ((ListView) View).CollectionSource.Criteria["filtro_pais_actual"] =
                    new GroupOperator(GroupOperatorType.Or,
                        new BinaryOperator("NacimientoPais.Oid", paisPredeterminado.Oid, BinaryOperatorType.Equal),
                        new NullOperator("NacimientoPais"));
        }

        /// <summary>
        ///     Cargar selector de empresas con las empresas disponibles
        /// </summary>
        private void CargarListaEmpresas()
        {
            _cambiarEmpresaActualAction.Items.Clear();

            foreach (
                var empresaParaUsuarioActual in CoreAppLogonParameters.Instance.EmpresasParaUsuarioActual(ObjectSpace))
                _cambiarEmpresaActualAction.Items.Add(new ChoiceActionItem(empresaParaUsuarioActual.Persona.Nombre, empresaParaUsuarioActual.Oid));
        }

        /// <summary>
        ///     Marcar la empresa actual en el selector de empresas
        /// </summary>
        private void MarcarEmpresaActual()
        {
            var item = _cambiarEmpresaActualAction.Items.Find(CoreAppLogonParameters.Instance.EmpresaActualId);
            if (item != null)
                _cambiarEmpresaActualAction.SelectedItem = item;
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();

            _cambiarEmpresaActualAction = new SingleChoiceAction(components);
            // 
            // CambiarEmpresaActual
            // 
            _cambiarEmpresaActualAction.Caption = "Empresa actual";
            _cambiarEmpresaActualAction.Category = "Tools";
            _cambiarEmpresaActualAction.ConfirmationMessage = null;
            _cambiarEmpresaActualAction.Id = "CambiarEmpresaActualAction";
            _cambiarEmpresaActualAction.ImageName = null;
            _cambiarEmpresaActualAction.PaintStyle = ActionItemPaintStyle.Default;
            _cambiarEmpresaActualAction.Shortcut = null;
            _cambiarEmpresaActualAction.Tag = null;
            _cambiarEmpresaActualAction.TargetObjectsCriteria = null;
            _cambiarEmpresaActualAction.TargetViewId = null;
            _cambiarEmpresaActualAction.ToolTip = null;
            _cambiarEmpresaActualAction.TypeOfView = null;
            _cambiarEmpresaActualAction.Execute += CambiarEmpresaActualAction_Execute;
        }

        #endregion

        private void CambiarEmpresaActualAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            CoreAppLogonParameters.Instance.EmpresaActualId = (int) e.SelectedChoiceActionItem.Data;
            FiltrarVistaPorEmpresa();

            var refreshTemplate = Frame.Template as IRefreshable;
            refreshTemplate?.Refresh();

            View.Refresh();
        }
    }
}