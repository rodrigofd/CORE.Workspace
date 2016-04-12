using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DisplayNameAttribute = System.ComponentModel.DisplayNameAttribute;

namespace FDIT.Core.Personas
{
  [ImageName("user-worker")]
  [NonPersistent]
  [DisplayName("Rol de persona")]
  public class Rol : BasicObject
  {
    private Persona fPersona;

    public Rol(Session session)
      : base(session)
    {
    }

    [VisibleInDetailView(false)]
    public string TipoRol
    {
      get
      {
        return ClassInfo.HasAttribute(typeof (DisplayNameAttribute))
          ? ((DisplayNameAttribute) ClassInfo.GetAttributeInfo(typeof (DisplayNameAttribute))).DisplayName
          : ClassInfo.FullName;
      }
    }

    //[ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
    [ImmediatePostData]
    [VisibleInListView(true)]
    public Persona Persona
    {
      get { return fPersona; }
      set { SetPropertyValue("Persona", ref fPersona, value); }
    }
  }
}