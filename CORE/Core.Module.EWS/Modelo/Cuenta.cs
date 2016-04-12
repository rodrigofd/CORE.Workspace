using System;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using FDIT.Core.Sistema;
using Microsoft.Exchange.WebServices.Data;

namespace FDIT.Core.EWS
{
  [ Persistent( @"ews.Cuenta" ) ]
  [ DefaultClassOptions ]
  [ DefaultProperty( "LoginUsuario" ) ]
  [ System.ComponentModel.DisplayName( "Cuenta de Exchange" ) ]
  public class Cuenta : CuentaEmail
  {
    private ExchangeService exchangeService;
    private string fAutodiscoverURL;
    private ExchangeVersion fVersionExchange;

    public Cuenta( Session session )
      : base( session )
    {
    }

    [ RuleRequiredField ]
    public string AutodiscoverURL
    {
      get { return fAutodiscoverURL; }
      set { SetPropertyValue( "AutodiscoverURL", ref fAutodiscoverURL, value ); }
    }

    [ RuleRequiredField ]
    public ExchangeVersion VersionExchange
    {
      get { return fVersionExchange; }
      set { SetPropertyValue( "VersionExchange", ref fVersionExchange, value ); }
    }

    public ExchangeService ExchangeService
    {
      get
      {
        if( exchangeService == null )
        {
          var auth = new UserData { EmailAddress = Usuario, Password = Contraseña };

          if( !string.IsNullOrWhiteSpace( AutodiscoverURL ) )
            auth.AutodiscoverUrl = new Uri( AutodiscoverURL );

          //auth.Version = ExchangeVersion.Exchange2013;
          //if (!string.IsNullOrWhiteSpace(VersionExchange)
          auth.Version = VersionExchange;

          exchangeService = Service.ConnectToService( auth );
        }

        return exchangeService;
      }
    }

    [ Action ]
    public void EsCuentaOffice365( )
    {
      AutodiscoverURL = "https://outlook.office365.com/EWS/Exchange.asmx";
    }

    public override void SendMail( MailMessage mail )
    {
      var svc = ExchangeService;
      
      var message = new EmailMessage( svc );
      
      message.Subject = mail.Subject;
      message.Body = mail.Body;

      message.From = new EmailAddress( mail.From.DisplayName, mail.From.Address );
      foreach( var recip in mail.To )
        message.ToRecipients.Add( new EmailAddress( recip.DisplayName, recip.Address ) );
      foreach( var recip in mail.CC )
        message.CcRecipients.Add( new EmailAddress( recip.DisplayName, recip.Address ) );
      foreach( var recip in mail.Bcc )
        message.BccRecipients.Add( new EmailAddress( recip.DisplayName, recip.Address ) );
      foreach( var recip in mail.ReplyToList )
        message.ReplyTo.Add( new EmailAddress( recip.DisplayName, recip.Address ) );

      foreach( var att in mail.Attachments )
      {
        att.ContentStream.Seek( 0L, SeekOrigin.Begin );
        message.Attachments.AddFileAttachment( att.Name, att.ContentStream );
      }

      message.SendAndSaveCopy( );
    }
  }
}
