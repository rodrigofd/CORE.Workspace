using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.AFIP
{
  [ Persistent( @"afip.Ente" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "CuitInformante" ) ]
  [ System.ComponentModel.DisplayName( "Preferencias de AFIP" ) ]
  public class Ente : BasicObject, IObjetoPorEmpresa
  {
    private string fCertifStoreLocation;
    private string fCertifStoreName;
    private string fCertifSubjectName;
    private FileData fCertificadoWSAA;
    private long fCuitInformante;
    private Empresa fEmpresa;

    private bool fPrestaServicios;
    private string fWsUrlWSAA;
    private string fWsUrlWSFE;

    public Ente( Session session ) : base( session )
    {
    }

    [ Aggregated ]
    [ ExpandObjectMembers( ExpandObjectMembers.Never ) ]
    public FileData CertificadoWSAA
    {
      get { return fCertificadoWSAA; }
      set { SetPropertyValue( "CertificadoWSAA", ref fCertificadoWSAA, value ); }
    }

    [ Indexed( Name = "IX_CUIT", Unique = true ) ]
    [ ModelDefault( "DisplayFormat", "{0:D}" ) ]
    public long CUITInformante
    {
      get { return fCuitInformante; }
      set { SetPropertyValue( "CUITInformante", ref fCuitInformante, value ); }
    }

    public bool PrestaServicios
    {
      get { return fPrestaServicios; }
      set { SetPropertyValue( "PrestaServicios", ref fPrestaServicios, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string CertifStoreLocation
    {
      get { return fCertifStoreLocation; }
      set { SetPropertyValue( "CertifStoreLocation", ref fCertifStoreLocation, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string CertifStoreName
    {
      get { return fCertifStoreName; }
      set { SetPropertyValue( "CertifStoreName", ref fCertifStoreName, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string CertifSubjectName
    {
      get { return fCertifSubjectName; }
      set { SetPropertyValue( "CertifSubjectName", ref fCertifSubjectName, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string WsUrlWSAA
    {
      get { return fWsUrlWSAA; }
      set { SetPropertyValue( "WsUrlWSAA", ref fWsUrlWSAA, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string WsUrlWSFE
    {
      get { return fWsUrlWSFE; }
      set { SetPropertyValue( "WsUrlWSFE", ref fWsUrlWSFE, value ); }
    }

    [ Association ]
    public XPCollection< Sesion > Sesiones
    {
      get { return GetCollection< Sesion >( "Sesiones" ); }
    }

    public Empresa Empresa
    {
      get { return fEmpresa; }
      set { SetPropertyValue( "Empresa", ref fEmpresa, value ); }
    }

    public Sesion GetSesion( string servicio )
    {
      var sesion = Sesiones.FirstOrDefault( s => s.Service == servicio );
      if( sesion != null )
      {
        if( !sesion.Valido )
        {
          sesion.Delete( );
          sesion.Save( );
          sesion = null;
        }
      }

      if( sesion == null )
      {
        sesion = new Sesion( Session ) { Service = servicio, Ente = this };
        Sesiones.Add( sesion );
        sesion.Obtener( );
        Save( );
      }

      return sesion;
    }
  }
}
