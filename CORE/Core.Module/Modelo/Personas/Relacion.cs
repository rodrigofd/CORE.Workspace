using System;
using System.ComponentModel;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Sistema;

namespace FDIT.Core.Personas
{
  [ImageName("group_link")]
  [Persistent( @"personas.Relacion" )]
  [System.ComponentModel.DisplayName( "Relación" )]
  public class Relacion : BasicObject
  {
    private DateTime fDesde;
    private DateTime fHasta;
    private string fNotas;
    private Persona fPersonaVinculado;
    private Persona fPersonaVinculante;
    private RelacionTipo fRelacionTipo;

    public Relacion( Session session ) : base( session )
    {
    }

    //[VisibleInListView(false)]
    //[PersistentAlias("Persona.OID")]
    //public Persona oid
    //{
    //    get { return ( Persona ) EvaluateAlias("oid"); }
    //}

    [RuleRequiredField]
    [Association( @"RelacionesReferencesPersonas-Vinculante" )]
    [System.ComponentModel.DisplayName( "Persona vinculante" )]
    //[Appearance("PersonaVinculanteRule", AppearanceItemType = "ViewItem", TargetItems = "PersonaVinculante", Criteria = "PersonaVinculante = oid", Context = "DetailView", Enabled = false)]
    public Persona PersonaVinculante
    {
      get { return fPersonaVinculante; }
      set { SetPropertyValue( "PersonaVinculante", ref fPersonaVinculante, value ); }
    }

    [RuleRequiredField]
    [System.ComponentModel.DisplayName("Tipo de relación")]
    [LookupEditorMode(LookupEditorMode.AllItems)]
    public RelacionTipo RelacionTipo
    {
      get { return fRelacionTipo; }
      set { SetPropertyValue( "RelacionTipo", ref fRelacionTipo, value ); }
    }

    [RuleRequiredField]
    [Association( @"RelacionesReferencesPersonas-Vinculado" )]
    [System.ComponentModel.DisplayName( "Persona vinculada" )]
    //[Appearance("PersonaVinculanteRule", AppearanceItemType = "ViewItem", TargetItems = "PersonaVinculado", Criteria = "PersonaVinculante = oid", Context = "DetailView", Enabled = false)]
    public Persona PersonaVinculado
    {
      get { return fPersonaVinculado; }
      set { SetPropertyValue( "PersonaVinculado", ref fPersonaVinculado, value ); }
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