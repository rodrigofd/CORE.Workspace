using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [DefaultProperty("Descripcion")]
    [System.ComponentModel.DisplayName("Interviniente")]
    [Persistent("seguros.DocumentoInterviniente")]
    [RuleCombinationOfPropertiesIsUnique("UnPolizaIntervinientePorDocumento", DefaultContexts.Save,
        "Documento,PolizaInterviniente",
        CustomMessageTemplate = "No puede modificar el mismo interviniente, mas de una vez, en el mismo documento")]
    public class DocumentoInterviniente : DocumentoIntervinienteBase,
        IMovimientoHistorial<PolizaInterviniente, DocumentoInterviniente>
    {
        protected Documento fDocumento;
        protected PolizaInterviniente fPolizaInterviniente;

        public DocumentoInterviniente(Session session)
            : base(session)
        {
        }

        [Association]
        [System.ComponentModel.DisplayName("Documento asociado")]
        public virtual Documento Documento
        {
            get { return fDocumento; }
            set { SetPropertyValue("Documento", ref fDocumento, value); }
        }

        [VisibleInListView(false)]
        [ImmediatePostData]
        [Association]
        [DataSourceProperty("Documento.Poliza.Intervinientes")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        [Appearance("mostrarpolinterviniente", "TipoMovimiento='Alta'", Visibility = ViewItemVisibility.Hide)]
        [RuleRequiredField(DefaultContexts.Save, TargetCriteria = "TipoMovimiento != 'Alta'")]
        public virtual PolizaInterviniente PolizaInterviniente
        {
            get { return fPolizaInterviniente; }
            set
            {
                if (CanRaiseOnChanged)
                {
                    if (value == null && fPolizaInterviniente != null)
                        if (fPolizaInterviniente.Intervinientes.Count == 0) fPolizaInterviniente.Delete();
                }
                SetPropertyValue("PolizaInterviniente", ref fPolizaInterviniente, value);
            }
        }

        [Browsable(false)]
        public XPCollection<PolizaInterviniente> PadreObjetoConHistorial
        {
            get { return Documento.Poliza.Intervinientes; }
        }

        [Browsable(false)]
        public PolizaInterviniente ObjetoConHistorial
        {
            get { return PolizaInterviniente; }
            set { PolizaInterviniente = value; }
        }

        protected override void OnDeleting()
        {
            base.OnDeleting();

            //caso especial: si estamos eliminando el interviniente, como parte de eliminar la poliza, lo permitimos siempre
            if (Session.IsObjectMarkedDeleted(Documento.Poliza)) return;

            //bloquear la eliminación de intervinientes básicos de la póliza (referenciados en el objeto poliza)
            var ident = Identificadores.GetInstance(Session);

            if (Rol != null && ident.RolAseguradora != null && Rol.Oid == ident.RolAseguradora.Oid)
                throw new UserFriendlyException(
                    "No se puede eliminar este registro porque define la aseguradora principal de la póliza");
            if (Rol != null && ident.RolTomador != null && Rol.Oid == ident.RolTomador.Oid)
                throw new UserFriendlyException(
                    "No se puede eliminar este registro porque define el tomador principal de la póliza");
            if (Rol != null && ident.RolOrganizador != null && Rol.Oid == ident.RolOrganizador.Oid)
                throw new UserFriendlyException(
                    "No se puede eliminar este registro porque define el organizador principal de la póliza");
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            //Valores por defecto
            TipoMovimiento = TipoMovimiento.Alta;
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            this.OnSavingMovimientoHistorial();

            if (!IsDeleted)
            {
                //Regla de coherencia: si el interviniente se definió PRINCIPAL, asegurarse que el resto (de ese rol/interviniente) no lo sea
                if (Principal)
                {
                    var intervRelacionados = Documento.Intervinientes;

                    foreach (var interv in intervRelacionados.Where(interv => interv.Rol.Oid == Rol.Oid))
                        interv.Principal = false;

                    Principal = true;
                }
            }
        }
    }
}