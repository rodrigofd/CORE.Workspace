using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace FDIT.Core.Gestion
{
  public enum ConceptosGrupos
  {
    Grav21 = 1,
    Grav105 = 2,
    Grav27 = 3,
    Grav0 = 4,
    NoGrav = 5,
    Otros = 6,
    PercepIVA = 7,
    IIBBCABA = 8,
    IIBBBSAS = 9,
    Exento = 10,
    IVA21 = 11,
    IVA105 = 12,
    IVA27 = 13,
    IVA0 = 14,
    Ganancias = 15,
  }

  [ Persistent( @"gestion.ConceptoGrupo" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "Nombre" ) ]
  [ System.ComponentModel.DisplayName( "Grupos de conceptos" ) ]
  public class ConceptoGrupo : BasicObject
  {
    private bool fActivo;
    private string fNombre;
    private int fOrdenTotal;

    public ConceptoGrupo( Session session ) : base( session )
    {
    }

    public string Nombre
    {
      get { return fNombre; }
      set { SetPropertyValue( "Nombre", ref fNombre, value ); }
    }

    public int OrdenTotal
    {
      get { return fOrdenTotal; }
      set { SetPropertyValue< int >( "OrdenTotal", ref fOrdenTotal, value ); }
    }

    public bool Activo
    {
      get { return fActivo; }
      set { SetPropertyValue( "Activo", ref fActivo, value ); }
    }

    public bool EsIVA
    {
      get
      {
        var conceptosGrupo = ( ConceptosGrupos ) Oid;
        return ( conceptosGrupo == ConceptosGrupos.IVA0 ||
                 conceptosGrupo == ConceptosGrupos.IVA105 ||
                 conceptosGrupo == ConceptosGrupos.IVA21 ||
                 conceptosGrupo == ConceptosGrupos.IVA27 );
      }
    }

    [ Association( @"ConceptosReferencesConceptosGrupos", typeof( Concepto ) ) ]
    public XPCollection< Concepto > Conceptos
    {
      get { return GetCollection< Concepto >( "Conceptos" ); }
    }

    public static explicit operator ConceptosGrupos( ConceptoGrupo e )
    {
      if( e.Oid < 1 ) throw new InvalidCastException( );
      return ( ConceptosGrupos ) e.Oid;
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }

    protected override void OnSaving( )
    {
      base.OnSaving( );
    }
  }

  [ NonPersistent ]
  [ DefaultClassOptions ]
  public class LibroIva : BasicObject
  {
    private Comprobante fComprobante;

    public LibroIva( Session session ) : base( session )
    {
    }

    public Comprobante Comprobante
    {
      get { return fComprobante; }
      set { SetPropertyValue( "Comprobante", ref fComprobante, value ); }
    }
  }
}
