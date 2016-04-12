using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;

namespace FDIT.Core.Controllers
{
    public class ArchivosAdjuntosController : ViewController<ListView>
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        public ArchivosAdjuntosController()
        {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof (ArchivoAdjunto);
            TargetViewNesting = Nesting.Nested;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            var novc = Frame.GetController<NewObjectViewController>();
            novc.ObjectCreated += NewObjectViewController_ObjectCreated;
        }

        private void NewObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            var nuevoArchivo = (ArchivoAdjunto) e.CreatedObject;
            SetupNewObject(nuevoArchivo, ObjectSpace, View);
        }

        public static void SetupNewObject(ArchivoAdjunto nuevoArchivo, IObjectSpace objectSpace, ListView listView)
        {
            var orig = (BasicObject) ((PropertyCollectionSource) listView.CollectionSource).MasterObject;

            if (orig == null)
            {
                nuevoArchivo.OriginanteType = null;
                nuevoArchivo.OriginanteKey = null;
            }
            else if (objectSpace.IsNewObject(orig))
            {
                throw new ArgumentException("No puede vincular con un objeto sin guardar.");
            }
            else
            {
                nuevoArchivo.OriginanteType = ((XPObjectSpace) objectSpace).Session.GetObjectType(orig);
                nuevoArchivo.OriginanteKey = (int) objectSpace.GetKeyValue(orig);
            }
        }

        protected override void OnDeactivated()
        {
            var novc = Frame.GetController<NewObjectViewController>();
            novc.ObjectCreated -= NewObjectViewController_ObjectCreated;

            base.OnDeactivated();
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