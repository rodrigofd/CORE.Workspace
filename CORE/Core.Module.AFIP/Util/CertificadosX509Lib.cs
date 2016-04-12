using System;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace FDIT
{
  /// <summary> 
  /// Libreria de utilidades para manejo de certificados 
  /// </summary>
  internal class CertificadosX509Lib
  {
    /// <summary> 
    /// Firma mensaje 
    /// </summary> 
    /// <param name="argBytesMsg">Bytes del mensaje</param> 
    /// <param name="argCertFirmante">Certificado usado para firmar</param> 
    /// <returns>Bytes del mensaje firmado</returns> 
    public static byte[ ] FirmarBytesMensaje( byte[ ] argBytesMsg, X509Certificate2 argCertFirmante )
    {
      // Pongo el mensaje en un objeto ContentInfo (requerido para construir el obj SignedCms) 
      var cmsFirmado = new SignedCms( new ContentInfo( argBytesMsg ) );

      // Creo objeto CmsSigner que tiene las caracteristicas del firmante 
      var cmsFirmante = new CmsSigner( argCertFirmante ) { IncludeOption = X509IncludeOption.EndCertOnly };

      // Firmo el mensaje PKCS #7 
      cmsFirmado.ComputeSignature( cmsFirmante );

      // Encodeo el mensaje PKCS #7. 
      return cmsFirmado.Encode( );
    }

    /// <summary> 
    /// Lee certificado de un stream
    /// </summary> 
    /// <returns>Un objeto certificado X509</returns> 
    public static X509Certificate2 ObtenerCertificado( byte[] data )
    {
      var objCert = new X509Certificate2( );

      try
      {
        objCert.Import( data );
        return objCert;
      }
      catch
      {
        throw new Exception( "Servicio WSAA: Error al obtener certificado a partir de datos en memoria" );
      }
    }
    /// <summary> 
    /// Lee certificado de disco 
    /// </summary> 
    /// <param name="argArchivo">Ruta del certificado a leer.</param> 
    /// <returns>Un objeto certificado X509</returns> 
    public static X509Certificate2 ObtenerCertificadoDesdeArchivo( string argArchivo )
    {
      var objCert = new X509Certificate2( );

      try
      {
        objCert.Import( File.ReadAllBytes( argArchivo ) );
        return objCert;
      }
      catch ( Exception excepcionAlImportarCertificado )
      {
        throw new Exception( string.Format( "Servicio WSAA: Error al obtener certificado a partir de archivo \"{0}\". Motivo: {1} {2}",
                                            argArchivo, excepcionAlImportarCertificado.Message, excepcionAlImportarCertificado.StackTrace ) );
      }
    }

    /// <summary> 
    /// Lee certificado de almacen del equipo 
    /// </summary> 
    /// <returns>Un objeto certificado X509</returns> 
    public static X509Certificate2 ObtenerCertificadoDesdeAlmacen( StoreLocation location, string storeName, string subjectName )
    {
      try
      {
        var store = new X509Store( storeName, location );
        store.Open( OpenFlags.ReadOnly | OpenFlags.MaxAllowed );
        return store.Certificates.Find( X509FindType.FindBySubjectName, subjectName, false )[ 0 ];
      }
      catch ( Exception excepcionAlImportarCertificado )
      {
        throw new Exception( string.Format( "Servicio WSAA: Error al obtener certificado a partir de almacén. location={0} storeName={1} subjectName={2}. Motivo: {3} {4}",
                                            location, storeName, subjectName, excepcionAlImportarCertificado.Message, excepcionAlImportarCertificado.StackTrace ) );
      }
    }
  }
}