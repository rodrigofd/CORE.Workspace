using System;
using DevExpress.Xpo;
using FDIT.Core.Stock;

namespace FDIT.Core.CRM
{
  [Persistent(@"crm.ContratoPeriodo")]
  [System.ComponentModel.DisplayName("Item del contrato")]
  public class ContratoPeriodo : BasicObject
  {
    private Contrato fContrato;
    private int fNumero;
    private DateTime? fVigenciaDesde;
    private DateTime? fVigenciaHasta;

    public ContratoPeriodo(Session session)
      : base(session)
    {
    }

    [Association]
    public Contrato Contrato
    {
      get { return fContrato; }
      set { SetPropertyValue("Contrato", ref fContrato, value); }
    }

    public int Numero
    {
      get { return fNumero; }
      set { SetPropertyValue<int>("Numero", ref fNumero, value); }
    }

    public DateTime? VigenciaDesde
    {
      get { return fVigenciaDesde; }
      set { SetPropertyValue("VigenciaDesde", ref fVigenciaDesde, value); }
    }

    public DateTime? VigenciaHasta
    {
      get { return fVigenciaHasta; }
      set { SetPropertyValue("VigenciaHasta", ref fVigenciaHasta, value); }
    }

    public override void AfterConstruction()
    {
      base.AfterConstruction();
    }
  }
}