using Microsoft.Extensions.Configuration;
using Parking.FindingSlotManagement.Application.Contracts.Infrastructure;
using Parking.FindingSlotManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Parking.FindingSlotManagement.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendMail(EmailModel emailModel)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(_configuration.GetSection("EmailUserName").Value);
                message.To.Add(new MailAddress(emailModel.To));
                message.Subject = emailModel.Subject;
                message.Body = emailModel.Body;
                message.IsBodyHtml = true;
                var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_configuration.GetSection("EmailUserName").Value, _configuration.GetSection("EmailPassword").Value),
                    EnableSsl = true
                };
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
