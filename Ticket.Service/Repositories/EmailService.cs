using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Ticket.Service.DTOs;
using Ticket.Service.Interfaces;

namespace Ticket.Service.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task SendEmail(EmailDTO emailDto)
        {
            _logger.LogInformation($"SendEmail: Request Body {emailDto}");
            var smtpServer = _configuration.GetValue<string>("EmailSettings:SmtpServer");
            var smtpPort = _configuration.GetValue<int>("EmailSettings:SmtpPort");
            var senderEmail = _configuration.GetValue<string>("EmailSettings:SenderEmail");
            var senderPassword = _configuration.GetValue<string>("EmailSettings:SenderPassword");

            var sendTo = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            try
            {
                var message = new MailMessage(from: senderEmail, to: emailDto.Email, subject: emailDto.Subject, body: emailDto.Message);
                await sendTo.SendMailAsync(message);
            }
            catch (SmtpException ex) when (ex.Message.Contains("5.7.139"))
            {
                _logger.LogError("Account is locked. Please unlock your account and try again.", ex);
                throw new Exception("Account is locked. Please unlock your account and try again.", ex);
            }
            catch (SmtpException ex)
            {
                _logger.LogError("Email sending failed. " + ex.Message, ex);
                throw new Exception("Email sending failed. " + ex.Message, ex);
            }
        }
    }
}