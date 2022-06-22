using System;
using System.Net;
using System.Net.Mail;

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
                using (MailMessage mm = new MailMessage("krishnaa9121@gmail.com", email))
                {
                    mm.Subject = subject;
                    mm.Body = body;
                    mm.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("krishnaa9121@gmail.com", "Kri$hn@@91");
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
