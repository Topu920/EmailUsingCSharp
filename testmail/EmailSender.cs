//using System.IO;
//using System.Net;
//using System.Net.Mail;

using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace testmail
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);

            //Send();
        }

        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);
            await SendAsync(mailMessage);
        }

        //private MimeMessage CreateEmailMessage(Message message)
        //{
        //    var emailMessage = new MimeMessage();
        //    emailMessage.From.Add(new MailboxAddress("iubatnemesis@gmail.com"));
        //    emailMessage.To.AddRange(message.To);
        //    emailMessage.Subject = message.Subject;
        //    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

        //    return emailMessage;
        //}

        //private void Send()
        //{
        //    using (MailMessage mm = new MailMessage("16303039@iubat.edu", "topu920@gmail.com"))
        //{
        //    mm.Subject = "test mail";
        //    mm.Body = "sdfsdfsdfsdf";
        //    //if (model.Attachment.Length > 0)
        //    //{
        //    //    string fileName = Path.GetFileName(model.Attachment.FileName);
        //    //    mm.Attachments.Add(new Attachment(model.Attachment.OpenReadStream(), fileName));
        //    //}
        //    mm.IsBodyHtml = false;
        //    using (SmtpClient smtp = new SmtpClient())
        //    {
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.EnableSsl = true;
        //        NetworkCredential NetworkCred = new NetworkCredential("16303039@iubat.edu", "");
        //        smtp.UseDefaultCredentials = true;
        //        smtp.Credentials = NetworkCred;
        //        smtp.Port = 587;
        //        smtp.Send(mm);
        //           // smtp.co
        //        //ViewBag.Message = "Email sent.";
        //    }
        //}
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("16303039@iubat.edu"));
            emailMessage.To.AddRange(message.To);
            emailMessage.Cc.AddRange(message.To);
            emailMessage.Bcc.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            string str = "<p>Hey Alice,<br></p><p>What are you up to this weekend? Monica is throwing one of her parties on Saturday and I was hoping you could make it.</p><p><span style='background-color: rgb(255, 255, 255);'><font color='#ff0000'>Zipper timely tor company khaiya dimu.</font><font color='#000000'>&nbsp;</font></span></p><p><span style='background-color: rgb(255, 255, 255);'><font color='#000000'><br></font></span></p><table class='table table-bordered' border=1><tbody><tr><td>werwer</td><td>werwer</td><td>werwer</td></tr><tr><td>werwer</td><td>werwerwerwerwer</td><td>werwerwerwerwer</td></tr></tbody></table><p><img src='data:image/png;base64,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' style='width: 225px;' data-filename='HAMEEM.png'><span style='background-color: rgb(255, 255, 255);'><font color='#000000'><br></font></span></p><p><b>Will you be my?</b><br></p><p>-- Joey<br></p>";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message.Content;

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, 587, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate("topu.it", "Rh%tp&u5!d");//pass parameter username and password

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("mail.hameemgroup.com", 465, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate("homayun.it", "Nu%Ha5!Mk@7");//pass parameter username and password

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

    }

}
