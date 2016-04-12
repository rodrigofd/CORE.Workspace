using System.Globalization;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;

namespace FDIT.Core.Controllers.Globales
{
  public class CambiarIdiomaController : WindowController
  {
    private readonly string defaultLanguageCaption;
    public readonly string defaultCultureCaption;
    
    private readonly SingleChoiceAction chooseFormattingCulture;
    private readonly SingleChoiceAction chooseLanguage;
    private readonly string defaultCultureName = CultureInfo.InvariantCulture.TwoLetterISOLanguageName;
    
    private string systemUserLanguage;

    public CambiarIdiomaController( )
    {
      TargetWindowType = WindowType.Main;
      defaultLanguageCaption = CaptionHelper.DefaultLanguage;
      defaultCultureCaption = CaptionHelper.GetLocalizedText( "Texts", "DefaultCulture" );
      
      StoreDefaultCulture( );

      chooseLanguage = new SingleChoiceAction( this, "ChooseLanguage", PredefinedCategory.Tools );
      chooseLanguage.Items.Add( new ChoiceActionItem( defaultLanguageCaption, defaultLanguageCaption, defaultLanguageCaption ) );
      chooseLanguage.Items.Add( new ChoiceActionItem( "es-AR", "Español (Argentina)", "es-AR" ) );
      chooseLanguage.SelectedIndex = 0;

      chooseFormattingCulture = new SingleChoiceAction( this, "ChooseFormattingCulture", PredefinedCategory.Tools );
      chooseFormattingCulture.Items.Add( new ChoiceActionItem( defaultCultureCaption, defaultCultureCaption, systemUserLanguage ) );
      chooseFormattingCulture.Items.Add( new ChoiceActionItem( "es-AR", "Español (Argentina)", "es-AR" ) );
      chooseFormattingCulture.SelectedIndex = 0;
    }

    private void StoreDefaultCulture( )
    {
      systemUserLanguage = CultureInfo.CurrentCulture.Name;
    }

    public SingleChoiceAction ChooseLanguage
    {
      get { return chooseLanguage; }
    }

    public SingleChoiceAction ChooseFormattingCulture
    {
      get { return chooseFormattingCulture; }
    }

    protected override void OnActivated( )
    {
      base.OnActivated( );
      //StoreDefaultCulture();

      //ChoiceActionItem currentLanguageItem = ChooseLanguage.Items.Find(Application.Model.CurrentAspect, ChoiceActionItemFindType.NonRecursive, ChoiceActionItemFindTarget.Leaf);
      var currentLanguageItem = ChooseLanguage.Items.Find( ( ( IModelApplicationServices ) ( Application.Model ) ).CurrentAspect, ChoiceActionItemFindType.NonRecursive, ChoiceActionItemFindTarget.Leaf );
      if( currentLanguageItem != null )
      {
        ChooseLanguage.SelectedIndex = ChooseLanguage.Items.IndexOf( currentLanguageItem );
      }
      //ChooseFormattingCulture.SelectedIndex = ChooseFormattingCulture.Items.IndexOf(ChooseFormattingCulture.Items.Find(System.Threading.Thread.CurrentThread.CurrentCulture.Name, ChoiceActionItemFindType.Recursive, ChoiceActionItemFindTarget.Any));

      chooseFormattingCulture.Execute += ChooseFormattingCulture_Execute;
      chooseLanguage.Execute += ChooseLanguage_Execute;
    }

    protected override void OnDeactivated( )
    {
      chooseLanguage.Execute -= ChooseLanguage_Execute;
      chooseFormattingCulture.Execute -= ChooseFormattingCulture_Execute;

      base.OnDeactivated( );
    }

    private void ChooseLanguage_Execute( object sender, SingleChoiceActionExecuteEventArgs e )
    {
      var newLanguageName = e.SelectedChoiceActionItem.Data as string;
      if( newLanguageName == defaultLanguageCaption )
        newLanguageName = defaultCultureName;

      Application.SetLanguage( newLanguageName );
    }

    private void ChooseFormattingCulture_Execute( object sender, SingleChoiceActionExecuteEventArgs e )
    {
      var newCultureName = e.SelectedChoiceActionItem.Data as string;
      if( newCultureName == defaultLanguageCaption )
        newCultureName = systemUserLanguage;

      Application.SetFormattingCulture( newCultureName );
    }
  }
}