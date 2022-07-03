using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Crypto.Util
{
    public class SendEmail
    {
        private static string sender_email_id= "kkwsuresh701@gmail.com";
        private static string sender_email_password= "xkaddjvlukxnybtj";

        public static void sendNotificationEmail(string recipientEmail, Double coinPrice)
        {
            String mailSubject = "Horaay!, Your expected Bitcoin price reached now | Crypto-Tracking & HelpDesk";

            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("<h4>Your expected Bitcoin price $"+ coinPrice.ToString() + " reached at " + DateTime.Now + ".</h4>");
            mailBody.Append("<p>Bitcoin price now is <b> $"+ coinPrice.ToString() + "</b></p>");
            mailBody.Append("</br></br></br></br>");
            mailBody.Append("<p>Thank You</p>");
            mailBody.Append("<p>Crypto-Tracking & HelpDesk Team</p>");

            sendEmail(recipientEmail, mailSubject, mailBody.ToString());
        }

        public static void sendInformationMail(string recipientEmail, string info)
        {
            String mailSubject = "Your Alert Package has been Expired";

            StringBuilder mailBody = new StringBuilder();
            mailBody.Append("<h4>Your Alert Package has been Expired due to out of remaining Alerts</h4>");
            mailBody.Append("<p>You can purchase a package again.</p>");
            mailBody.Append("</br></br></br></br>");
            mailBody.Append("<p>Thank You</p>");
            mailBody.Append("<p>Crypto-Tracking & HelpDesk Team</p>");

            sendEmail(recipientEmail, mailSubject, mailBody.ToString());
        }

        private static void sendEmail(string recipientEmail, string mailSubject, string mailBody)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(sender_email_id);
                    mail.To.Add(recipientEmail);
                    mail.Subject = mailSubject;
                    mail.Body = mailBody;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential(sender_email_id, sender_email_password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                        System.Diagnostics.Debug.WriteLine("===========<<< Mail has sent >>>===============");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("===========<<< ERROR: Can not Send Mail>>>===============");
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}