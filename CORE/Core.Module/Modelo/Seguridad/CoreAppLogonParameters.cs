using System;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Seguridad
{
    [NonPersistent]
    [ModelDefault("Caption", "Log On")]
    public class CoreAppLogonParameters : ICustomObjectSerialize, ISupportResetLogonParameters
    {
        private Empresa _loginEmpresa;
        private string _loginUserName;

        [Browsable(false)] public int EmpresaActualId = -1;
        
        private XPCollection<Empresa> seleccionEmpresa;

        [Browsable(false)] public Guid UsuarioActualId = Guid.Empty;

        [Browsable(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [Browsable(false)]
        [CollectionOperationSet(AllowAdd = false)]
        public XPCollection<Empresa> SeleccionEmpresa
        {
            get
            {
                if (seleccionEmpresa == null)
                {
                    var empresas = (XPCollection<Empresa>) ObjectSpace.GetObjects<Empresa>();
                    seleccionEmpresa = empresas;
                    seleccionEmpresa.Criteria = CriteriaOperator.Parse("1 = 0");
                    seleccionEmpresa.BindingBehavior = CollectionBindingBehavior.AllowNone;
                }
                return seleccionEmpresa;
            }
        }

        public static CoreAppLogonParameters Instance => (CoreAppLogonParameters) SecuritySystem.LogonParameters;

        void ICustomObjectSerialize.ReadPropertyValues(SettingsStorage storage)
        {
            if (!string.IsNullOrWhiteSpace(storage.LoadOption("", "UserName")))
                LoginUserName = storage.LoadOption("", "UserName");

            if (!string.IsNullOrWhiteSpace(storage.LoadOption("", "IdEmpresa")))
                LoginEmpresa = SeleccionEmpresa.Lookup(Convert.ToInt32(storage.LoadOption("", "IdEmpresa")));
        }

        void ICustomObjectSerialize.WritePropertyValues(SettingsStorage storage)
        {
            storage.SaveOption("", "UserName", LoginUserName);
            if (EmpresaActualId != -1)
                storage.SaveOption("", "IdEmpresa", EmpresaActualId.ToString());
        }

        void ISupportResetLogonParameters.Reset()
        {
            if (ObjectSpace != null)
            {
                ObjectSpace.Dispose();
                ObjectSpace = null;
            }

            LoginUserName = null;
            LoginEmpresa = null;
            Password = null;

            seleccionEmpresa = null;
        }

        public Usuario UsuarioActual()
        {
            return UsuarioActual(ObjectSpace);
        }

        public Usuario UsuarioActual(Session session)
        {
            return UsuarioActualId == Guid.Empty || session == null
                ? null
                : session.GetObjectByKey<Usuario>(UsuarioActualId);
        }

        public Usuario UsuarioActual(IObjectSpace objSpace)
        {
            return UsuarioActualId == Guid.Empty || objSpace == null
                ? null
                : objSpace.GetObjectByKey<Usuario>(UsuarioActualId);
        }

        public Empresa EmpresaActual()
        {
            return EmpresaActual(ObjectSpace);
        }

        public Empresa EmpresaActual(Session session)
        {
            return EmpresaActualId == -1 || session == null ? null : session.GetObjectByKey<Empresa>(EmpresaActualId);
        }

        public Empresa EmpresaActual(IObjectSpace objSpace)
        {
            return EmpresaActualId == -1 || objSpace == null ? null : objSpace.GetObjectByKey<Empresa>(EmpresaActualId);
        }

        public XPCollection<Empresa> EmpresasParaUsuarioActual(IObjectSpace objSpace)
        {
            var empresas = (XPCollection<Empresa>) objSpace.GetObjects<Empresa>();
            empresas.BindingBehavior = CollectionBindingBehavior.AllowNone;
            FiltrarEmpresas(empresas, UsuarioActual(objSpace));
            return empresas;
        }

        private void FiltrarEmpresas(XPCollection<Empresa> empresas, Usuario usuario)
        {
            empresas.Criteria = null;
        }

        #region Campos de la pantalla login

        [ImmediatePostData]
        [Index(0)]
        public string LoginUserName
        {
            get { return _loginUserName; }
            set
            {
                _loginUserName = value;
                var usuario = ObjectSpace.FindObject<Usuario>(new BinaryOperator("UserName", _loginUserName));
                UsuarioActualId = usuario?.Oid ?? Guid.Empty;

                FiltrarEmpresas(SeleccionEmpresa, UsuarioActual());
            }
        }

        [DataSourceProperty("SeleccionEmpresa")]
        [Index(2)]
        public Empresa LoginEmpresa
        {
            get { return _loginEmpresa; }
            set
            {
                _loginEmpresa = value;
                EmpresaActualId = value?.Oid ?? -1;
            }
        }

        [PasswordPropertyText(true)]
        [Index(2)]
        public string Password { get; set; }

        #endregion
    }
}