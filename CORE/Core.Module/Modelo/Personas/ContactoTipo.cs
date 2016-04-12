using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Personas
{
  [Persistent( @"personas.ContactoTipo" )]
  [DefaultClassOptions]
  [DefaultProperty( "Nombre" )]
  [System.ComponentModel.DisplayName( "Tipo de contacto" )]
  public class ContactoTipo : BasicObject
  {
    private string fContactoTipo;

    public ContactoTipo( Session session ) : base( session )
    {
    }

    public string Nombre
    {
      get { return fContactoTipo; }
      set { SetPropertyValue( "Nombre", ref fContactoTipo, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}