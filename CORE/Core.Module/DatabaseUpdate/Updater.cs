using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using FDIT.Core.Personas;
using FDIT.Core.Seguridad;
using FDIT.Core.Sistema;
using Rol = FDIT.Core.Seguridad.Rol;

namespace FDIT.Core.DatabaseUpdate
{
    public class Updater : UpdaterBase
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion)
        {
        }

        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            
            var administrativeRole = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "Administradores"));
            if (administrativeRole == null)
            {
                administrativeRole = ObjectSpace.CreateObject<Rol>();
                administrativeRole.Name = "Administradores";
                administrativeRole.IsAdministrative = true;
            }

            const string adminName = "admin";

            var administratorUser = ObjectSpace.FindObject<Usuario>(new BinaryOperator("UserName", adminName));
            if (administratorUser == null)
            {
                administratorUser = ObjectSpace.CreateObject<Usuario>();
                administratorUser.UserName = adminName;
                administratorUser.IsActive = true;
                administratorUser.SetPassword("$");
                administratorUser.Roles.Add(administrativeRole);
            }

            var empresa =
                ObjectSpace.FindObject<Empresa>(new BinaryOperator("Persona.Nombre", "Fernández Díaz, Rodrigo"));
            if (empresa == null)
            {
                var empresaPers = ObjectSpace.CreateObject<Persona>();

                empresaPers.Nombre = "Fernández Díaz, Rodrigo";
                empresaPers.Tipo = TipoPersona.Fisica;

                empresa = ObjectSpace.CreateObject<Empresa>();
                empresa.Persona = empresaPers;
                empresaPers.Roles.Add(empresa);

                empresa.Usuarios.Add(administratorUser);
            }

            if (ObjectSpace.IsNewObject(administratorUser))
                administratorUser.EmpresaPredeterminada = empresa;
        }

        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();

            EnsureDbSchema("afip");

            EnsureDbSchema("internal");
            EnsureDbSchema("gestion");
            EnsureDbSchema("compras");
            EnsureDbSchema("ventas");
            EnsureDbSchema("contabilidad");
            EnsureDbSchema("crm");
            EnsureDbSchema("sistema");
            EnsureDbSchema("fondos");
            EnsureDbSchema("impuestos");
            EnsureDbSchema("personas");
            EnsureDbSchema("regionales");
            EnsureDbSchema("seguridad");
            EnsureDbSchema("stock");

            //if( CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0") )
            //{
            //  RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}