using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using FDIT.Core.Sistema;
using FDIT.Core.Impuestos;

namespace FDIT.Core.Personas
{
  [ImageName("bank--pencil")]
  [Persistent( @"personas.PersonaImpuesto" )]
  [System.ComponentModel.DisplayName( "Datos impositivos" )]
  public class PersonaImpuesto : BasicObject
  {
    private Persona fPersona;
    private Impuesto fImpuesto;
    private Categoria fCategoria;
    private DateTime fDesde;
    private DateTime fHasta;
    private string fNotas;
    private bool fPercepcion;
    private Regimen fRegimen;
    private bool fRetencion;

    public PersonaImpuesto( Session session ) : base( session )
    {
    }

    [Association( @"PersonasImpuestosReferencesPersonas" )]
    public Persona Persona
    {
      get { return fPersona; }
      set { SetPropertyValue( "Persona", ref fPersona, value ); }
    }

      //TODO: en la grid aparece en null
//    [ NonPersistent ]
//    [ImmediatePostData]
//    public Impuesto Impuesto{ get; set; }

    [ImmediatePostData]
    public Impuesto Impuesto
    {
        get { return fImpuesto; }
        set { SetPropertyValue("Impuesto", ref fImpuesto, value); }
    }

    [DataSourceProperty("Impuesto.Categorias")]
    [System.ComponentModel.DisplayName("Categoría de impuesto")]
    [LookupEditorMode(LookupEditorMode.AllItems)]
    public Categoria Categoria
    {
      get { return fCategoria; }
      set
      {
        SetPropertyValue( "Categoria", ref fCategoria, value );
        //Impuesto = Categoria.Impuesto;
      }
    }

    [ImmediatePostData]
    [DataSourceProperty("Impuesto.Regimenes")]
    [System.ComponentModel.DisplayName("Regimen del impuesto")]
    [LookupEditorMode(LookupEditorMode.AllItems)]
    public Regimen Regimen
    {
        get { return fRegimen; }
        set { SetPropertyValue("Regimen", ref fRegimen, value); }
    }

    [System.ComponentModel.DisplayName("Agente de retención")]
    public bool AgenteRetencion
    {
      get { return fRetencion; }
      set { SetPropertyValue( "Retencion", ref fRetencion, value ); }
    }

    [System.ComponentModel.DisplayName( "Agente de percepción" )]
    public bool AgentePercepcion
    {
      get { return fPercepcion; }
      set { SetPropertyValue( "Percepcion", ref fPercepcion, value ); }
    }
    public DateTime Desde
    {
      get { return fDesde; }
      set { SetPropertyValue< DateTime >( "Desde", ref fDesde, value ); }
    }
    public DateTime Hasta
    {
      get { return fHasta; }
      set { SetPropertyValue< DateTime >( "Hasta", ref fHasta, value ); }
    }

    [Size( SizeAttribute.Unlimited )]
    public string Notas
    {
      get { return fNotas; }
      set { SetPropertyValue( "Notas", ref fNotas, value ); }
    }

    public override void AfterConstruction( )
    {
      base.AfterConstruction( );
    }
  }
}