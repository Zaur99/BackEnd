using Comm.Business.Abstract;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Comm.Business.Concrete
{
    public class EmailSender : IEmailSender
    {
        private string host;
        private int port;
        private bool enableSSL;
        private string userName;
        private string passsword;

        public EmailSender(string host, int port, bool enableSSL, string userName, string passsword)
        {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.userName = userName;
            this.passsword = passsword;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient(this.host, this.port)
            {
                Credentials = new NetworkCredential(this.userName,this.passsword),
                EnableSsl = this.enableSSL
            };


            return client.SendMailAsync(new MailMessage(this.userName, email, subject, message) {  IsBodyHtml = true});
        }
    }
}
