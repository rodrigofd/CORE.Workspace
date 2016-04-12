using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Sistema
{
    [ImageName("mail_server_setting")]
    [Persistent("sistema.CuentaEmail")]
    [DefaultProperty("DireccionEmail")]
    [DefaultClassOptions]
    public class CuentaEmail : BasicObject
    {
        private string fContraseña;
        private string fDireccionEmail;
        private string fNombreMostrar;
        private int fPuertoSmtp;
        private string fServidorSmtp;
        private bool fUsarSSL;
        private string fUsuario;
        private SmtpClient smtpClient;

        public CuentaEmail(Session session) : base(session)
        {
        }

        public string ServidorSmtp
        {
            get { return fServidorSmtp; }
            set { SetPropertyValue("ServidorSmtp", ref fServidorSmtp, value); }
        }

        public string Usuario
        {
            get { return fUsuario; }
            set { SetPropertyValue("Usuario", ref fUsuario, value); }
        }

        [ModelDefault("IsPassword", "true")]
        public string Contraseña
        {
            get { return fContraseña; }
            set { SetPropertyValue("Contraseña", ref fContraseña, value); }
        }

        public int PuertoSmtp
        {
            get { return fPuertoSmtp; }
            set { SetPropertyValue<int>("PuertoSmtp", ref fPuertoSmtp, value); }
        }

        public bool UsarSSL
        {
            get { return fUsarSSL; }
            set { SetPropertyValue("UsarSSL", ref fUsarSSL, value); }
        }

        public string NombreMostrar
        {
            get { return fNombreMostrar; }
            set { SetPropertyValue("NombreMostrar", ref fNombreMostrar, value); }
        }

        public string DireccionEmail
        {
            get { return fDireccionEmail; }
            set { SetPropertyValue("DireccionEmail", ref fDireccionEmail, value); }
        }

        private SmtpClient SmtpClient
        {
            get
            {
                return smtpClient ?? (smtpClient = new SmtpClient
                {
                    Host = ServidorSmtp,
                    Port = PuertoSmtp,
                    Credentials = new NetworkCredential(Usuario, Contraseña),
                    EnableSsl = UsarSSL,
                    UseDefaultCredentials = false
                });
            }
        }

        public virtual void SendMail(MailMessage mail)
        {
            var client = SmtpClient;

            client.Send(mail);
        }
    }
}