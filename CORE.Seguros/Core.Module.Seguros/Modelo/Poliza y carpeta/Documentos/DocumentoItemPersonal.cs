using System;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using FDIT.Core.Personas;

namespace FDIT.Core.Seguros
{
    [System.ComponentModel.DisplayName("Item de documento (personal)")]
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class DocumentoItemPersonal : DocumentoItem
    {
        private Aseguradora fVidaAfiliacionSegSocial;
        private string fVidaIdentificacion;
        private IdentificacionTipo fVidaIdentificacionTipo;
        private DocumentoItem fVidaItemVinculado;
        private DateTime? fVidaNacimientoFecha;
        private string fVidaNombre;
        private DocumentoItemParentesco fVidaParentesco;
        private Sexo? fVidaSexo;

        public DocumentoItemPersonal(Session session)
            : base(session)
        {
        }

        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "1")]
        public string VidaNombre
        {
            get { return fVidaNombre; }
            set { SetPropertyValue("VidaNombre", ref fVidaNombre, value); }
        }

        public IdentificacionTipo VidaIdentificacionTipo
        {
            get { return fVidaIdentificacionTipo; }
            set { SetPropertyValue("VidaIdentificacionTipo", ref fVidaIdentificacionTipo, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        [ModelDefault("RowCount", "1")]
        public string VidaIdentificacion
        {
            get { return fVidaIdentificacion; }
            set { SetPropertyValue("VidaIdentificacion", ref fVidaIdentificacion, value); }
        }

        public Sexo? VidaSexo
        {
            get { return fVidaSexo; }
            set { SetPropertyValue("VidaSexo", ref fVidaSexo, value); }
        }

        public DateTime? VidaNacimientoFecha
        {
            get { return fVidaNacimientoFecha; }
            set { SetPropertyValue("VidaNacimientoFecha", ref fVidaNacimientoFecha, value); }
        }

        public DocumentoItemParentesco VidaParentesco
        {
            get { return fVidaParentesco; }
            set { SetPropertyValue("VidaParentesco", ref fVidaParentesco, value); }
        }

        public DocumentoItem VidaItemVinculado
        {
            get { return fVidaItemVinculado; }
            set { SetPropertyValue("VidaItemVinculado", ref fVidaItemVinculado, value); }
        }

        public Aseguradora VidaAfiliacionSegSocial
        {
            get { return fVidaAfiliacionSegSocial; }
            set { SetPropertyValue("VidaAfiliacionSegSocial", ref fVidaAfiliacionSegSocial, value); }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
    }
}