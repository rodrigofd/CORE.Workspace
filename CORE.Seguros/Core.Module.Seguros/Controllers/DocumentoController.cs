using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.ViewVariantsModule;

namespace FDIT.Core.Seguros.Controllers
{
    public class DocumentoController : ViewController
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        public DocumentoController()
        {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof (Documento);
            TargetViewNesting = Nesting.Root;
        }

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            var doc = e.Object as Poliza;
            if (doc != null && e.PropertyName == "Subramo" && doc.Subramo != null)
            {
                var listPropertyEditor = ((DetailView) View).FindItem("Items") as ListPropertyEditor;

                if (listPropertyEditor.Frame == null)
                    listPropertyEditor.CreateControl();

                var documentoItemType = string.IsNullOrEmpty(doc.Subramo.TipoItem)
                    ? typeof (DocumentoItem)
                    : XafTypesInfo.Instance.FindTypeInfo(doc.Subramo.TipoItem).Type;

                var variantCtrl = listPropertyEditor.Frame.GetController<ChangeVariantController>();
                if (variantCtrl == null) return;
                foreach (var item in variantCtrl.ChangeVariantAction.Items)
                {
                    if (item.Id == documentoItemType.FullName + "_ListView")
                    {
                        variantCtrl.ChangeVariantAction.DoExecute(item);
                        return;
                    }
                }
            }
        }

        protected override void OnDeactivated()
        {
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;

            base.OnDeactivated();
        }

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }

        #endregion
    }
}