using System;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;

namespace FDIT.Core.Controllers
{
    public class NotasController : ViewController
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private IContainer components;

        public NotasController()
        {
            InitializeComponent();
            RegisterActions(components);

            TargetObjectType = typeof (Nota);
            TargetViewType = ViewType.ListView;
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

            var novc = Frame.GetController<NewObjectViewController>();
            novc.ObjectCreated += NewObjectViewController_ObjectCreated;
        }

        private void NewObjectViewController_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            var nuevaNota = (Nota) e.CreatedObject;
            var orig = (BasicObject) ((PropertyCollectionSource) ((ListView) View).CollectionSource).MasterObject;

            if (orig == null)
            {
                nuevaNota.OriginanteType = null;
                nuevaNota.OriginanteKey = null;
            }
            else if (ObjectSpace.IsNewObject(nuevaNota))
            {
                throw new ArgumentException("No puede vincular con un objeto sin guardar.");
            }
            else
            {
                nuevaNota.OriginanteType = ((XPObjectSpace) ObjectSpace).Session.GetObjectType(orig);
                nuevaNota.OriginanteKey = (int) ObjectSpace.GetKeyValue(orig);
            }
        }

        protected override void OnDeactivated()
        {
            var novc = Frame.GetController<NewObjectViewController>();
            novc.ObjectCreated -= NewObjectViewController_ObjectCreated;

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