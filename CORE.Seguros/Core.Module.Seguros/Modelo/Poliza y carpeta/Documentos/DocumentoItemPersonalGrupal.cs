using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Seguros
{
    [System.ComponentModel.DisplayName("Item de documento (grupal)")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class DocumentoItemPersonalGrupal : DocumentoItem
    {
        private decimal fCantidad;
        private DocumentoItemClase fClase;
        private ListaDePrecios fListaDePrecios;

        public DocumentoItemPersonalGrupal(Session session)
            : base(session)
        {
        }

        [ImmediatePostData]
        [ModelDefault("DisplayFormat", "n2")]
        [ModelDefault("EditMask", "n2")]
        public decimal Cantidad
        {
            get { return fCantidad; }
            set
            {
                SetPropertyValue<decimal>("Cantidad", ref fCantidad, value);
                if (CanRaiseOnChanged)
                {
                    //TODO: actualizarPremio( );
                }
            }
        }

        public DocumentoItemClase Clase
        {
            get { return fClase; }
            set { SetPropertyValue("Clase", ref fClase, value); }
        }

        [ImmediatePostData]
        public ListaDePrecios ListaDePrecios
        {
            get { return fListaDePrecios; }
            set
            {
                SetPropertyValue("ListaDePrecios", ref fListaDePrecios, value);

                if (CanRaiseOnChanged)
                {
                    actualizarPrecioSegunPlan();
                }
            }
        }

        private void actualizarPrecioSegunPlan()
        {
            if (Plan != null && ListaDePrecios != null)
            {
                foreach (var precio in Plan.PlanVigente.PlanCobertura.Precios)
                {
                    if (precio.Lista.Oid == ListaDePrecios.Oid)
                    {
                        //TODO:cambiar
                        //PrimaGravada = precio.PrimaGravada;
                        //PrimaNoGravada = precio.PrimaNoGravada;
                        //Iva1Tasa = precio.Iva1Tasa;
                        break;
                    }
                }
            }
        }
    }
}