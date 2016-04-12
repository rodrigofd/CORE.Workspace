using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;

namespace FDIT.Core
{
  public class CoreModuleBase : ModuleBase
  {
    protected void AddObjectProperty( Type classType, Type propertyType, string propertyName )
    {
      AddObjectProperty( classType, propertyType, propertyName, null );
    }

    protected void AddObjectProperty( Type classType, Type propertyType, string propertyName, List<Attribute> attributeList )
    {
      /*var xpDictionary = XpoTypesInfoHelper.GetXpoTypeInfoSource( ).XPDictionary;

      if( xpDictionary.GetClassInfo( classType ).FindMember( propertyName ) == null )
        xpDictionary.GetClassInfo( classType ).CreateMember( propertyName, propertyType );
      XafTypesInfo.Instance.RefreshInfo( classType );*/

      var typeInfo = XafTypesInfo.Instance.FindTypeInfo( classType );
      var member = typeInfo.FindMember( propertyName );
      if( member == null ) member = typeInfo.CreateMember( propertyName, propertyType );

      if( attributeList != null )
        foreach( var attribute in attributeList )
      member.AddAttribute( attribute );
    }

    public static string TranslatePersistentName( string typeName, string defaultName )
    {
      if( typeName.Contains( "DevExpress" ) )
      {
        switch( defaultName )
        {
          case "SecuritySystemUser":
          case "SecuritySystemRole":
          case "SecuritySystemMemberPermissionsObject":
          case "SecuritySystemObjectPermissionsObject":
          case "SecuritySystemRoleParentRoles_SecuritySystemRoleChildRoles":
          case "SecuritySystemTypePermissionsObject":
          case "SecuritySystemUserUsers_SecuritySystemRoleRoles":
            defaultName = "seguridad." + defaultName.Replace( "SecuritySystem", "" );
            break;
          default:
            defaultName = "internal." + defaultName;
            break;
        }
      }

      return defaultName;
    }

    public override void CustomizeTypesInfo( ITypesInfo typesInfo )
    {
      base.CustomizeTypesInfo( typesInfo );

    CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);

            foreach ( var type in XafTypesInfo.Instance.PersistentTypes )
        ModelNodesGeneratorSettings.SetIdPrefix( type.Type, type.FullName );

      foreach( var ci in typesInfo.PersistentTypes )
      {
        if( !ci.IsPersistent ) continue;

        /*var tableName = TranslatePersistentName( ci.AssemblyInfo.FullName, ci.Name );
          ci.AddAttribute( new PersistentAttribute( tableName ) );*/

        var oidMember = ci.FindMember( "Oid" );
        if( oidMember != null )
        {
          var browsableAttribute = oidMember.FindAttribute< BrowsableAttribute >( );
          if( browsableAttribute == null )
            oidMember.AddAttribute( new BrowsableAttribute( false ) );

          typesInfo.RefreshInfo( ci );
        }
      }
    }
  }
}
