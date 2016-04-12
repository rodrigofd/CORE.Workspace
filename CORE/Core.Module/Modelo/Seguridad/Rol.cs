using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;

namespace FDIT.Core.Seguridad
{
    [Persistent(@"seguridad.Rol")]
    [System.ComponentModel.DisplayName("Rol del sistema")]
    public class Rol : SecuritySystemRole
    {
        public Rol(Session session) :
            base(session)
        {
        }
    }
}