using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Gestion
{
  [Persistent( @"gestion.ComprobanteSector" )]
  [DefaultClassOptions]
  [DefaultProperty( "Descripcion" )]
  [System.ComponentModel.DisplayName( "Sectores de comprobante" )]
  public class ComprobanteSector : BasicObject, IObjetoPorEmpresa
  {
    private Empresa fIdEmpresa;
    private bool fImprime;
    private int fNumero;
    private string fNotas;

    public ComprobanteSector( Session session ) : base( session )
    {
    }

    [VisibleInDetailView(false)]
    [PersistentAlias( "PadLeft(ToStr(Numero), 4, '0' ) + ' ' + Notas" )]
    public string Descripcion
    {
      get { return Convert.ToString( EvaluateAlias( "Descripcion" ) ); }
    }

    [RuleRequiredField]
    public int Numero
    {
      get { return fNumero; }
      set { SetPropertyValue<int>( "Numero", ref fNumero, value ); }
    }

    public bool Imprime
    {
      get { return fImprime; }
      set { SetPropertyValue( "Imprime", ref fImprime, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    public string Notas
    {
      get { return fNotas; }
      set { SetPropertyValue( "Notas", ref fNotas, value ); }
    }

    [Browsable( false )]
    public Empresa Empresa
    {
      get { return fIdEmpresa; }
      set { SetPropertyValue( "Empresa", ref fIdEmpresa, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}