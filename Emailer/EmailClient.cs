using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Emailer
{
    public class EmailClient
    {
        public class EmailCredentials
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private readonly string BaseApplicationUrl;

        private readonly EmailCredentials _credentials;

        private readonly SmtpClient _smtpClient;
        

        public EmailClient(IConfiguration _config) {
            var emailSection = _config.GetSection("EmailCredentials");

            _credentials = new EmailCredentials();
            _credentials.Email = emailSection.GetSection("username").Value;
            _credentials.Password = emailSection.GetSection("password").Value;

            BaseApplicationUrl = _config.GetSection("BaseApplicationUrl").Value;

            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_credentials.Email, _credentials.Password),
                EnableSsl = true
            };
        }

        public async Task SendEmail(string to, string? subject, string? body)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(_credentials.Email),
                Body = body,
                Subject = subject,
            };

            message.To.Add(to);

            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendPasswordResetEmail(string to, string code)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(_credentials.Email),
                Body = $"<div style=\"text-align=center;\"><h4>Hello, it seems as if you have reqested a new password.<br/>Please click the button below to change the password on your account.</h4><a href=\"{BaseApplicationUrl}/ForgotPassword/{code}\">click me</a></div>",
                IsBodyHtml = true,
                Subject = "Password Reset",
            };

            message.To.Add(to);

            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendEmailValidationEmail(string to, string code)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(_credentials.Email),
                Body = $"<div style=\"text-align=center;\"><h4>Hello, it seems as if you have created a new account.<br/>Please click the link below to activate your account.</h4><a href=\"{BaseApplicationUrl}/EmailValidation/{code}\">validate now</a></div>",
                IsBodyHtml = true,
                Subject = "Welcome to your new account",
            };

            message.To.Add(to);

            await _smtpClient.SendMailAsync(message);
        }

    }
}