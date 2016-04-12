using System.ComponentModel;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;

namespace FDIT.Core.Impuestos
{
  [Persistent( @"impuestos.Alicuota" )]
  [DefaultProperty( "Nombre" )]
  [System.ComponentModel.DisplayName( "Alícuota por impuesto" )]
  public class Alicuota : BasicObject
  {
    private Impuesto fImpuesto;
    private string fNombre;
    private decimal fValor;

    public Alicuota( Session session ) : base( session )
    {
    }

    [Association( @"AlicuotasReferencesImpuestos" )]
    public Impuesto Impuesto
    {
      get { return fImpuesto; }
      set { SetPropertyValue("Impuesto", ref fImpuesto, value); }
    }

    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    [ModelDefault( "DisplayFormat", "n2" )]
    public decimal Valor
    {
      get { return fValor; }
      set { SetPropertyValue( "Valor", ref fValor, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}