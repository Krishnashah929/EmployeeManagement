#region Using
using System;
using System.Net.Mail;
using System.Net;
#endregion

namespace EM.Common
{
    public class MailService
    {
        /// <summary>
        ///  In this method we are sending main set password template to user. where they can set their new password.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        #region SendEmail
        public static void SendEmail(string email, string body, string subject)
        {
            try
            {
                //For sending mails to multiple customers usinf for loop and split emails woth , sign.
                foreach (var emailaddress in email.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    using (MailMessage mm = new MailMessage("krishnaa9121@gmail.com", emailaddress))
                    {
                        mm.Subject = subject;
                        mm.Body = body;
                        mm.IsBodyHtml = true;

                        using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
                        {
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            NetworkCredential NetworkCred = new NetworkCredential("krishnaa9121@gmail.com", "tswjwrvxzbmmphle");
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = NetworkCred;
                            smtp.Port = 587;
                            smtp.Send(mm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
