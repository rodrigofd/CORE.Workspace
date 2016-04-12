using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Impuestos;

namespace FDIT.Core.Fondos
{
  [ Persistent( "fondos.RetencionImpuestos" ) ]
  [ System.ComponentModel.DisplayName( "Retención de impuestos" ) ]
  [ DefaultClassOptions ]
  public class RetencionImpuestos : Valor
  {
    private Impuesto fImpuesto;
    private int fNumero;
    private int fSector;

    public RetencionImpuestos( Session session ) : base( session )
    {
    }

    public Impuesto Impuesto
    {
      get { return fImpuesto; }
      set
      {
        SetPropertyValue( "Impuesto", ref fImpuesto, value ); 
        
      }
    }

    public int Sector
    {
      get { return fSector; }
      set { SetPropertyValue<int>( "Sector", ref fSector, value ); }
    }

    public int Numero
    {
      get { return fNumero; }
      set { SetPropertyValue<int>( "Numero", ref fNumero, value ); }
    }
  }
}