using System;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.AFIP.WebServices.WSAA;

namespace FDIT.Core.AFIP
{
  [ Persistent( @"afip.Sesion" ) ]
  [ DefaultClassOptions ]
  [ System.ComponentModel.DisplayName( "Sesiones AFIP" ) ]
  public class Sesion : BasicObject
  {
    private const string XmlStrLoginTicketRequestTemplate =
      "<loginTicketRequest>" + "<header>" + "<uniqueId></uniqueId>" + "<generationTime></generationTime>" + "<expirationTime></expirationTime>" + "</header>" + "<service></service>" + "</loginTicketRequest>";

    public const string ServicioWSFE = "wsfe";
    private Ente fEnte;

    private DateTime fExpirationTime;
    private DateTime fGenerationTime;

    private string fService;
    private string fSign;
    private string fToken;
    private long fUniqueID;
    private XmlDocument fXmlLoginTicketRequest;
    private XmlDocument fXmlLoginTicketResponse;

    public Sesion( Session session ) : base( session )
    {
    }

    [ Browsable( false ) ]
    [ Association ]
    public Ente Ente
    {
      get { return fEnte; }
      set { SetPropertyValue( "Ente", ref fEnte, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Service
    {
      get { return fService; }
      set { SetPropertyValue( "Service", ref fService, value ); }
    }

    public DateTime GenerationTime
    {
      get { return fGenerationTime; }
      set { SetPropertyValue< DateTime >( "Generationtime", ref fGenerationTime, value ); }
    }

    public DateTime ExpirationTime
    {
      get { return fExpirationTime; }
      set { SetPropertyValue< DateTime >( "Expirationtime", ref fExpirationTime, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Sign
    {
      get { return fSign; }
      set { SetPropertyValue( "Sign", ref fSign, value ); }
    }

    [ Size( SizeAttribute.Unlimited ) ]
    public string Token
    {
      get { return fToken; }
      set { SetPropertyValue( "Token", ref fToken, value ); }
    }

    public long UniqueID
    {
      get { return fUniqueID; }
      set { SetPropertyValue( "uniqueID", ref fUniqueID, value ); }
    }

    public bool Valido
    {
      get { return !string.IsNullOrWhiteSpace( Token ) && !string.IsNullOrWhiteSpace( Sign ) && ExpirationTime > DateTime.Now; }
    }

    public string Obtener( string argServicio, string argRutaCertX509Firmante )
    {
      var certFirmante = CertificadosX509Lib.ObtenerCertificadoDesdeArchivo( argRutaCertX509Firmante );
      return Obtener( argServicio, certFirmante );
    }

    public string Obtener( string argServicio, StoreLocation certifStoreLocation,
                           string certifStoreName, string certifSubjectName )
    {
      var certFirmante = CertificadosX509Lib.ObtenerCertificadoDesdeAlmacen( certifStoreLocation, certifStoreName, certifSubjectName );
      return Obtener( argServicio, certFirmante );
    }

    public string Obtener( string argServicio, byte[ ] certificado )
    {
      var certFirmante = CertificadosX509Lib.ObtenerCertificado( certificado );
      return Obtener( argServicio, certFirmante );
    }

    public string Obtener( )
    {
      var cer = Ente.CertificadoWSAA.Content;
      if( cer.Length == 0 )
        throw new Exception( "El ente indicado no tiene certificado cargado" );

      var certFirmante = CertificadosX509Lib.ObtenerCertificado( cer );
      return Obtener( Service, certFirmante );
    }

    public string Obtener( string argServicio, X509Certificate2 certFirmante )
    {
      string cmsFirmadoBase64;
      string loginTicketResponse;

      // PASO 1: Genero el Login Ticket Request 
      //try
      {
        fXmlLoginTicketRequest = new XmlDocument( );
        fXmlLoginTicketRequest.LoadXml( XmlStrLoginTicketRequestTemplate );

        var xmlNodoUniqueId = fXmlLoginTicketRequest.SelectSingleNode( "//uniqueId" );
        var xmlNodoGenerationTime = fXmlLoginTicketRequest.SelectSingleNode( "//generationTime" );
        var xmlNodoExpirationTime = fXmlLoginTicketRequest.SelectSingleNode( "//expirationTime" );
        var xmlNodoService = fXmlLoginTicketRequest.SelectSingleNode( "//service" );

        xmlNodoGenerationTime.InnerText = DateTime.Now.AddMinutes( -10 ).ToString( "s" );
        xmlNodoExpirationTime.InnerText = DateTime.Now.AddHours( +24 ).ToString( "s" );
        xmlNodoUniqueId.InnerText = Convert.ToString( Environment.TickCount );
        xmlNodoService.InnerText = argServicio;
        Service = argServicio;
      }
      //catch
      {
        //  throw new Exception( "Servicio WSAA: Error al obtener ticket de autenticación. Motivo: Xml de solicitud no válido" );
      }

      // PASO 2: Firmo el Login Ticket Request 
      try
      {
        // Convierto el login ticket request a bytes, para firmar 
        var msgBytes = Encoding.UTF8.GetBytes( fXmlLoginTicketRequest.OuterXml );

        // Firmo el msg y paso a Base64 
        var encodedSignedCms = CertificadosX509Lib.FirmarBytesMensaje( msgBytes, certFirmante );
        cmsFirmadoBase64 = Convert.ToBase64String( encodedSignedCms );
      }
      catch
      {
        throw new Exception( "Servicio WSAA: Error al obtener ticket de autenticación. Motivo: Error al firmar el mensaje con el certificado" );
      }

      // PASO 3: Invoco al WSAA para obtener el Login Ticket Response 
      try
      {
        var servicioWsaa = new LoginCMSClient( );

        loginTicketResponse = servicioWsaa.loginCms( cmsFirmadoBase64 );
      }
      catch( Exception exc )
      {
        throw new Exception( "Servicio WSAA: Error al obtener ticket de autenticación. Motivo: " + exc.Message );
      }

      // PASO 4: Analizo el Login Ticket Response recibido del WSAA 
      //try
      {
        fXmlLoginTicketResponse = new XmlDocument( );
        fXmlLoginTicketResponse.LoadXml( loginTicketResponse );

        UniqueID = long.Parse( fXmlLoginTicketResponse.SelectSingleNode( "//uniqueId" ).InnerText );
        GenerationTime = DateTime.Parse( fXmlLoginTicketResponse.SelectSingleNode( "//generationTime" ).InnerText );
        ExpirationTime = DateTime.Parse( fXmlLoginTicketResponse.SelectSingleNode( "//expirationTime" ).InnerText );
        Sign = fXmlLoginTicketResponse.SelectSingleNode( "//sign" ).InnerText;
        Token = fXmlLoginTicketResponse.SelectSingleNode( "//token" ).InnerText;
      }
      //catch
      {
        //  throw new Exception( "Servicio WSAA: Error al obtener ticket de autenticación. Motivo: Xml de respuesta no válido" );
      }

      return loginTicketResponse;
    }
  }
}
