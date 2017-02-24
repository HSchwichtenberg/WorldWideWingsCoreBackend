using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ITVisions.Mail
{
 public class MailGun
 {
  // the domain name you have verified in your Mailgun account
  const string DOMAIN = "sandboxa3679b7084c54a989eb20c072bedc40d.mailgun.org";

  // your API Key used to send mail through the Mailgun API
  const string API_KEY = "key-884c2d55c5b301c0d0fa55c66bc95e78";

  public async Task<bool> SendAsync(string from, string to, string subject, string message)
  {

   if (System.Environment.MachineName.StartsWith("E")) return false;
   var client = new HttpClient();
   client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
    Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes("api" + ":" + API_KEY)));

   var form = new Dictionary<string, string>();
   form["from"] = from;
   form["to"] = to;
   form["subject"] = subject;
   form["text"] = message;

   var response = await client.PostAsync("https://api.mailgun.net/v2/" + DOMAIN + "/messages",
    new FormUrlEncodedContent(form));

   if (response.StatusCode == HttpStatusCode.OK)
   {
    Debug.WriteLine("Success");
    return true;
   }
   else
   {
    Debug.WriteLine("StatusCode: " + response.StatusCode);
    Debug.WriteLine("ReasonPhrase: " + response.ReasonPhrase);
    return false;
   }
  }
 }
}