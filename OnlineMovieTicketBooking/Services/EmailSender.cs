
using System;

using System.Collections.Generic;

using System.Linq;

using System.Net;       //NetworkCredential

using System.Net.Mail;  //SmtpClient

using System.Threading.Tasks;



namespace OnlineMovieTicketBooking.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string To_Email_Address, string subject, string message)

        {

            //------------< SendEmailAsync() >------------

            //< check >

            if (To_Email_Address == null) return;  //todo <bool>=false

            //</ check >



            try

            {

                //< init >

                string sFrom_Email_Address = "info@OnlineMovieTicketBookings.com";

                string sFrom_Email_DisplayName = "Mailservice";

                string sSenderAppname = "OnlineMovieTicketBookings.com";

                string sHost = "smtp.gmail.com";

                int intPort = 587;

                string sEmail_Login = "your_mail@gmail.com";

                string sEmail_Passwort = "your_password";

                //</ init >



                //-< setup Email >-

                MailMessage email = new MailMessage();

                email.To.Add(new MailAddress(To_Email_Address));

                email.From = new MailAddress(sFrom_Email_Address, sFrom_Email_DisplayName);

                email.Subject = sSenderAppname + " " + subject;

                email.Body = message;

                email.IsBodyHtml = true;

                //email.Priority = MailPriority.High;

                //-</ setup Email >-



                //-< EmailClient >-

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");

               // smtp.Host = sHost;

                smtp.Port = intPort;

                  smtp.Credentials = new NetworkCredential(sEmail_Login, sEmail_Passwort);
             //   smtp.Credentials = CredentialCache.DefaultNetworkCredentials;
                smtp.EnableSsl = true;

                //-</ EmailClient >-



                //< send >

                await smtp.SendMailAsync(email);

                //</ send >

            }



            catch (Exception ex)

            {

                //do something here

                Console.WriteLine("Error EmailSender.cs error:" + ex.InnerException);

            }





            //return true;

            //------------</ SendEmailAsync() >------------

        }
    }
}
