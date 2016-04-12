using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using FDIT.Core.StoredProcedures;

namespace FDIT.Core.Controllers
{
    public class StoredProcedureWindowController : WindowController
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            Application.CustomProcessShortcut += application_CustomProcessShortcut;
        }
        
        protected override void OnDeactivated()
        {
            Application.CustomProcessShortcut -= application_CustomProcessShortcut;
        }

        private void application_CustomProcessShortcut(object sender, CustomProcessShortcutEventArgs e)
        {
            Type tipoObjParametros;

            var vm = Application.Model.Views[e.Shortcut.ViewId];

            var consultaStoredProcedureAttribute =
                vm.AsObjectView.ModelClass.TypeInfo.FindAttribute<ConsultaStoredProcedureAttribute>();
            if (consultaStoredProcedureAttribute == null)
            {
                var type = vm.AsObjectView.ModelClass.TypeInfo.Type;
                if (typeof (IConsultaStoredProcedure).IsAssignableFrom(type))
                    tipoObjParametros = type;
                else
                    return;
            }
            else
                tipoObjParametros = consultaStoredProcedureAttribute.ConsultaStoredProcedure;

            var objectSpace = Application.CreateObjectSpace();

            var objetoParametros = Activator.CreateInstance(tipoObjParametros);
            e.View = Application.CreateDetailView(objectSpace, objetoParametros);
            e.View.Caption = $"Parámetros para vista {vm.Caption}";

            ((DetailView) e.View).ViewEditMode = ViewEditMode.Edit;

            e.Handled = true;
        }
    }
}