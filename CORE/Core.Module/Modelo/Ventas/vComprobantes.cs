using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Modelo.Ventas
{
    [DefaultClassOptions, Persistent("gestion.vComprobantes")]
    public class vComprobantes : XPObject, IObjetoPorEmpresa //XPLiteObject
    {
        public vComprobantes(Session session) : base(session) { }
        //[Key, Persistent, Browsable(false)]
        //public vComprobantesViewKey Key;
        public int Sector { get; set; }
        public int Numero { get; set; }
        public DateTime Fecha { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        [Browsable(false)]
        public Empresa Empresa { get; set; }
    }
/*    public struct vComprobantesViewKey
    {
        [Persistent("OID"), Browsable(false)]
        public int OID;
    }
*/
    //protected override void DoEndEditAction() { }

    /*
        [Persistent(@"gestion.vComprobantes")]
        //[ DefaultProperty( "Descripcion" ) ]
        [System.ComponentModel.DisplayName("vComprobantes")]
        [ImageName("document-invoice")]
        [DefaultClassOptions]
        public class vComprobantes : BasicObject, IObjetoPorEmpresa
        {
            protected Empresa fEmpresa;

            private int fSector;
            private int fNumero;
            private DateTime fFecha;
            private string fNombre;
            private string fDomicilio;

            public vComprobantes(Session session)
                : base(session)
            {
            }
    // OID, Sector, Numero, Fecha, Nombre, Domicilio, Empresa, OptimisticLockField, GCRecord, ObjectType

            [ VisibleInDetailView( false ) ]
            [System.ComponentModel.DisplayName("Sector")]
            public int Sector { get; set; }

            //{
            //    get { return fSector; }
            //   set { SetPropertyValue("Sector", ref fSector, value); }
            //}

            [VisibleInDetailView(false)]
            //[PersistentAlias("(Numero)")]
            [System.ComponentModel.DisplayName("Numero")]
            public int Numero
            {
                get { return fNumero; }
                set { SetPropertyValue("Numero", ref fNumero, value); }
            }

            [VisibleInDetailView(false)]
            //[PersistentAlias("(Fecha)")]
            [System.ComponentModel.DisplayName("Fecha")]
            public DateTime Fecha
            {
                get { return fFecha; }
                set { SetPropertyValue("Fecha", ref fFecha, value); }
            }

            [VisibleInDetailView(false)]
            //[PersistentAlias("(Nombre)")]
            [System.ComponentModel.DisplayName("Nombre")]
            public string Nombre
            {
                get { return fNombre; }
                set { SetPropertyValue("Nombre", ref fNombre, value); }
            }

            [VisibleInDetailView(false)]
            //[PersistentAlias("(Domicilio)")]
            [System.ComponentModel.DisplayName("Domicilio")]
            public string Domicilio
            {
                get { return fDomicilio; }
                set { SetPropertyValue("Domicilio", ref fDomicilio, value); }
            }

            [Browsable(false)]
            public Empresa Empresa
            {
                get { return fEmpresa; }
                set { SetPropertyValue("Empresa", ref fEmpresa, value); }
            }

        }
    */
}
