using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Regionales
{
  [Persistent( @"regionales.Idioma" )]
  [DefaultClassOptions]
  [DefaultProperty( "Nombre" )]
  [System.ComponentModel.DisplayName( "Idioma" )]
  public class Idioma : BasicObject
  {
    private string fCodigo;
    private string fCodigo1;
    private string fCodigo2;
    private string fNombre;
    private string fNombreEng;
    private int fCodigoMsLangId;
    private string fCodigoSqlServer;

    public Idioma( Session session ) : base( session )
    {
    }

    [Size( 50 )]
    [ Index(0) ]
public string Codigo
    {
      get { return fCodigo; }
      set { SetPropertyValue( "Codigo", ref fCodigo, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    [Index(1)]
public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [Size( 60 )]
    public string NombreEng
    {
      get { return fNombreEng; }
      set { SetPropertyValue( "NombreEng", ref fNombreEng, value ); }
    }

    [Size( 50 )]
    public string Codigo1
    {
      get { return fCodigo1; }
      set { SetPropertyValue( "Codigo1", ref fCodigo1, value ); }
    }

    [Size( 10 )]
    public string Codigo2
    {
      get { return fCodigo2; }
      set { SetPropertyValue( "Codigo2", ref fCodigo2, value ); }
    }

    public int CodigoMsLangId
    {
      get { return fCodigoMsLangId; }
      set { SetPropertyValue<int>( "Mslangid", ref fCodigoMsLangId, value ); }
    }
    public string CodigoSqlServer
    {
      get { return fCodigoSqlServer; }
      set { SetPropertyValue( "IdiomaSql", ref fCodigoSqlServer, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}