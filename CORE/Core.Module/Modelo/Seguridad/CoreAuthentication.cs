using System;
using System.Collections.Generic;
using System.Diagnostics;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Utils;

namespace FDIT.Core.Seguridad
{
    public interface ISupportResetLogonParameters
    {
        void Reset();
    }

    public class CoreAuthentication : AuthenticationBase, IAuthenticationStandard
    {
        private CoreAppLogonParameters _logonParameters;

        public CoreAuthentication()
        {
            _logonParameters = new CoreAppLogonParameters();
        }

        private bool autoLogin => false && Debugger.IsAttached;

        public override bool AskLogonParametersViaUI => !autoLogin;

        public override object LogonParameters => _logonParameters;

        public override bool IsLogoffEnabled => true;

        public override void Logoff()
        {
            base.Logoff();
            _logonParameters = new CoreAppLogonParameters();
        }

        public override void ClearSecuredLogonParameters()
        {
            _logonParameters.Password = "";
            base.ClearSecuredLogonParameters();
        }

        public override object Authenticate(IObjectSpace objectSpace)
        {
            Usuario usuario;

            if (autoLogin)
            {
                usuario = objectSpace.FindObject<Usuario>(new BinaryOperator("UserName", "admin"));

                _logonParameters.ObjectSpace = objectSpace;
                _logonParameters.LoginUserName = usuario.UserName;
                _logonParameters.LoginEmpresa = usuario.EmpresaPredeterminada;
            }
            else
            {
                if (_logonParameters.UsuarioActual() == null)
                {
                    throw new Exception(CaptionHelper.GetLocalizedText("Security", "UsuarioNotNull"));
                }

                if (_logonParameters.EmpresaActual() == null)
                {
                    throw new Exception(CaptionHelper.GetLocalizedText("Security", "EmpresaNotNull"));
                }

                if (!_logonParameters.UsuarioActual().ComparePassword(_logonParameters.Password))
                {
                    throw new AuthenticationException(_logonParameters.UsuarioActual().UserName,
                        SecurityExceptionLocalizer.GetExceptionMessage(SecurityExceptionId.PasswordsAreDifferent));
                }

                usuario = objectSpace.GetObjectByKey<Usuario>(_logonParameters.UsuarioActualId);
            }

            return usuario;
        }

        public override IList<Type> GetBusinessClasses()
        {
            return new[] {typeof (CoreAppLogonParameters)};
        }
    }
}