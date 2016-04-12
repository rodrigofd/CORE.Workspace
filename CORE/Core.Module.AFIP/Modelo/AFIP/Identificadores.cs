using System.ComponentModel;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Seguridad;
using FDIT.Core.Util;

namespace FDIT.Core.AFIP
{
  [Persistent(@"afip.Identificadores")]
  [DefaultClassOptions]
  [System.ComponentModel.DisplayName( "Preferencias de AFIP" )]
  public class Identificadores : IdentificadoresBase<Identificadores>
  {
    private Ente fFacturaElectronicaEnte;

    public Identificadores( Session session ) : base( session )
    {
    }

    [System.ComponentModel.DisplayName( "Ente activa para facturación electrónica" )]
    public Ente FacturaElectronicaEnte
    {
      get { return fFacturaElectronicaEnte; }
      set { SetPropertyValue( "FacturaElectronicaEnte", ref fFacturaElectronicaEnte, value ); }
    }
  }
}