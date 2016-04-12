using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Gestion
{
  [Persistent( "gestion.MarcaComercial" )]
  [DefaultClassOptions]
  [DefaultProperty( "Nombre" )]
  [System.ComponentModel.DisplayName( "Marca comercial" )]
  public class MarcaComercial : BasicObject
  {
    private string fNombre;

    public MarcaComercial( Session session ) : base( session )
    {
    }

    [System.ComponentModel.DisplayName( "Nombre" )]
    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}