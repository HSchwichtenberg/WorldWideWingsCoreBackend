//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MailKit;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;

//namespace ITVisions.Mail
//{
//// /// <summary>
//// /// https://github.com/jstedfast/MailKit
//// /// </summary>

//using System;
//using System.Text;

//public class MailUtil
//{

// public string SendEmail(string fromName, string fromEMail, string toName, string toEmail, string subject, string message)
//{
// Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
// var emailMessage = new MimeMessage();

// emailMessage.From.Add(new MailboxAddress(fromEMail));
// emailMessage.To.Add(new MailboxAddress(toEmail));
// emailMessage.Subject = subject;
// emailMessage.Body = new TextPart("plain") { Text = message };
// //emailMessage.Prepare(EncodingConstraint.SevenBit);
//   using (var client = new SmtpClient(new ProtocolLogger(@"c:\temp\smtp.log")))
//   {


//    client.Connect("smtp.it-visions.de", 25, SecureSocketOptions.None);
//    client.AuthenticationMechanisms.Remove("XOAUTH2");
//    Console.WriteLine("Auth...");
//    client.Authenticate("itvsmtp@it-visions.de", "itvsmtp");
//    Console.WriteLine("Send Mail...");
//    client.Send(emailMessage);
//    Console.WriteLine("Disconnect...");
//    client.Disconnect(true);
//    return "";
//   }
//}

////  public async Task SendEmailAsync(string fromName, string fromEMail, string toName, string toEmail, string subject, string message)
////  {
////   Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
////   var emailMessage = new MimeMessage();

////   emailMessage.From.Add(new MailboxAddress(fromName, fromEMail));
////   emailMessage.To.Add(new MailboxAddress("", toEmail));
////   emailMessage.Subject = subject;
////   emailMessage.Body = new TextPart("plain") { Text = message };

////   using (var client = new SmtpClient())
////   {
////    //client.LocalDomain = "dev.it-visions.de";
////    await client.ConnectAsync("itvsmtp.it-visions.de", 25, SecureSocketOptions.None).ConfigureAwait(false);
////    await client.AuthenticateAsync("itvsmtp@it-visions.de", "itvsmtp");
////    await client.SendAsync(emailMessage).ConfigureAwait(false);
////    await client.DisconnectAsync(true).ConfigureAwait(false);
////   }
////  }
// }
//}
