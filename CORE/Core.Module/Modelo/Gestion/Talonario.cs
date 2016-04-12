using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Gestion
{
  [ Persistent( @"gestion.Talonario" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Descripcion" ) ]
  [ System.ComponentModel.DisplayName( "Talonario" ) ]
  public class Talonario : BasicObject, IObjetoPorEmpresa
  {
    private ComprobanteTipo fComprobanteTipo;
    private Empresa fEmpresa;
    private bool fNumeroAutomatico;
    private int fNumeroDesde;
    private int fNumeroHasta;
    private int fSector;
    private int fUltimoNumero;

    public Talonario( Session session ) : base( session )
    {
    }

    [ PersistentAlias( "CONCAT(IIF(ISNULL(ComprobanteTipo),'',ComprobanteTipo.Codigo),'-', PADLEFT(TOSTR(Sector),4,'0'))" ) ]
    public string Descripcion
    {
      get { return ( string ) EvaluateAlias( "Descripcion" ); }
    }

    [RuleRequiredField]
    [ Association ]
    [LookupEditorMode(LookupEditorMode.AllItems)]
    public ComprobanteTipo ComprobanteTipo
    {
      get { return fComprobanteTipo; }
      set { SetPropertyValue( "ComprobanteTipo", ref fComprobanteTipo, value ); }
    }

    [RuleRequiredField]
    [ ModelDefault( "DisplayFormat", "{0:0000}" ) ]
    public int Sector
    {
      get { return fSector; }
      set { SetPropertyValue< int >( "Sector", ref fSector, value ); }
    }

    [RuleRequiredField]
    [ ModelDefault( "DisplayFormat", "{0:00000000}" ) ]
    public int NumeroDesde
    {
      get { return fNumeroDesde; }
      set { SetPropertyValue< int >( "NumeroDesde", ref fNumeroDesde, value ); }
    }

    [RuleRequiredField]
    [ ModelDefault( "DisplayFormat", "{0:00000000}" ) ]
    public int NumeroHasta
    {
      get { return fNumeroHasta; }
      set { SetPropertyValue< int >( "NumeroHasta", ref fNumeroHasta, value ); }
    }

    [RuleRequiredField]
    [ ModelDefault( "DisplayFormat", "{0:00000000}" ) ]
    public int UltimoNumero
    {
      get { return fUltimoNumero; }
      set { SetPropertyValue< int >( "UltimoNumero", ref fUltimoNumero, value ); }
    }

    [RuleRequiredField]
    public bool NumeroAutomatico
    {
      get { return fNumeroAutomatico; }
      set { SetPropertyValue( "NumeroAutomatico", ref fNumeroAutomatico, value ); }
    }

    [ Browsable( false ) ]
    public Empresa Empresa
    {
      get { return fEmpresa; }
      set { SetPropertyValue( "Empresa", ref fEmpresa, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}
