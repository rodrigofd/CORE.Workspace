using System;
using Microsoft.Exchange.WebServices.Data;

namespace FDIT.Core.EWS
{
  public class UserData
  {
    public ExchangeVersion Version{ get; set; }
    public string EmailAddress{ get; set; }
    public string Password{ get; set; }
    public Uri AutodiscoverUrl{ get; set; }
  }
}
