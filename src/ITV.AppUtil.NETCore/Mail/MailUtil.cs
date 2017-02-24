//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Mail;
//using System.Threading.Tasks;

//namespace ITVisions.Mail
//{
//    public class NetworkUtil
//    {

//  public static bool MailSendenAktiv = true;
//  public static string Standardsender = "";
//  public static string Standardreceiver = "";


//  public string SendMailTollerant(string von, string an, string subject, string body, bool HTML = false, string CC = "")
//  {
//   if (string.IsNullOrEmpty(von)) von = NetworkUtil.Standardsender;

//   if (string.IsNullOrEmpty(an)) an = NetworkUtil.Standardreceiver;
//   if (String.IsNullOrEmpty(von))
//   {
//    return "Ungültiger Mailsender. Von=" + von;
//   }

//   if (String.IsNullOrEmpty(an))
//   {
//    return "Ungültiger Mailempfänger. An=" + an;
//   }
   
  
//   System.Net.Mail.MailMessage. smtp = new System.Net.Mail.SmtpClient();
//   if (subject == "") subject = "((Kein Betreff))";
//   if (body == "") body = "((Kein Inhalt))";
//   body = body.Truncate(1024 * 1024 * 2, true, "\n... Body abgeschnitten weil zu gross: " + body.Length);


//   // WICHTIG: ggf in app.config eintragen:
//   //   <system.net>
//   // <mailSettings>
//   //  <smtp deliveryMethod="Network"> 
//   //   <network host="smtp.it-visions.de" port="25" userName="xxx@IT-Visions.de" password="xxx" defaultCredentials="false"/>
//   //  </smtp>
//   // </mailSettings>
//   //</system.net>

//   MailMessage message = new MailMessage(von.Trim(), an.Trim());
//   if (!String.IsNullOrEmpty(CC)) message.CC.Add(CC);
//   message.Subject = subject.Trim();
//   message.Body = body.Trim();
//   message.IsBodyHtml = HTML;
//   message.se
//   if (NetworkUtil.MailSendenAktiv)
//   {
//    CUI.Print("Sende E-Mail an: " + an + " Betreff: " + subject);

//    try
//    {
//     if (String.IsNullOrEmpty(von) || String.IsNullOrEmpty(an))
//     {
//      CUI.Print(" ---> SendMail-Fehler: Es fehlen Sender oder Empfänger", ConsoleColor.Red); return "";
//     }

//     smtp.Send(message);
//     return "";
//    }
//    catch (Exception ex)
//    {
//     CUI.Print(" ---> SendMail-Fehler: Von: " + von + " An: " + an + "Betreff: " + subject + " Inhalt: " + body + " FEHLER: " + ex.GetFullMessage(), ConsoleColor.Red);
//     return ex.Message;
//    }

//   }
//   else
//   {
//    CUI.Print("E-Mail an: " + an + " Betreff: " + subject + " wurde unterdrückt.");
//   }
//   return ""; // alles OK!

//  }
// }
//}
