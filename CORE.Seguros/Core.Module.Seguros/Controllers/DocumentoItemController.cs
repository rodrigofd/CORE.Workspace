using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using FDIT.Core.Controllers;

namespace FDIT.Core.Seguros.Controllers
{
    //Controlador que captura la accion CREAR NUEVO para el tipo DocumentoItem 
    public class DocumentoItemController : ViewController
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        public DocumentoItemController()
        {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof (DocumentoItem);
            TargetViewNesting = Nesting.Nested;
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

            Frame.GetController<NewObjectViewController>().ObjectCreating += DocumentoItemController_ObjectCreating;
        }

        private void DocumentoItemController_ObjectCreating(object sender, ObjectCreatingEventArgs e)
        {
            var documento = this.GetMasterObject<Documento>();

            var subramo = documento.Subramo;
            if (subramo == null)
                throw new UserFriendlyException("Debe indicar el Ramo/Subramo para crear un Item.");

            var documentoItemType = string.IsNullOrEmpty(documento.Subramo.TipoItem)
                ? typeof (DocumentoItem)
                : XafTypesInfo.Instance.FindTypeInfo(documento.Subramo.TipoItem).Type;

            e.NewObject = e.ObjectSpace.CreateObject(documentoItemType);
        }

        protected override void OnDeactivated()
        {
            Frame.GetController<NewObjectViewController>().ObjectCreating -= DocumentoItemController_ObjectCreating;

            base.OnDeactivated();
        }

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
        }

        #endregion
    }
}