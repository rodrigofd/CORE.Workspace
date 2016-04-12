using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace FDIT.Core.Gestion
{
  [System.ComponentModel.DisplayName( "Modelo de formulario de comprobante" )]
  [DefaultClassOptions]
  [Persistent("gestion.ModeloFormulario")]
  public class ModeloFormulario : ReportData
  {
    public ModeloFormulario( Session session ) : base( session )
    {
      
    }
  }
}
