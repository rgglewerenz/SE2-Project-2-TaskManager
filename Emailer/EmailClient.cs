using DatabaseInterop.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using Ganss.XSS;

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

        public async Task SendEmail(string to, string? subject, string? body, bool IsHTML = false)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(_credentials.Email),
                Body = body,
                Subject = subject,
                IsBodyHtml = IsHTML
            };

            message.To.Add(to);

            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendPasswordResetEmail(string to, string code)
        {
            var tags = new Dictionary<string, string>()
            {
                {"$code", HttpUtility.UrlEncode(code) },
                {"$BaseApplicationUrl", BaseApplicationUrl }
            };

            var body_html = ReplaceTags(await GetFileContent("Passreset.html"),  tags);


            var message = new MailMessage()
            {
                From = new MailAddress(_credentials.Email),
                Body = body_html,
                IsBodyHtml = true,
                Subject = "Password Reset",
            };

            message.To.Add(to);

            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendEmailValidationEmail(string to, string code)
        {
            var tags = new Dictionary<string, string>()
            {
                {"$code", HttpUtility.UrlEncode(code) },
                {"$BaseApplicationUrl", BaseApplicationUrl }
            };

            var body_html = ReplaceTags(await GetFileContent("ValidateEmail.html"), tags);


            var message = new MailMessage()
            {
                From = new MailAddress(_credentials.Email),
                Body = body_html,
                IsBodyHtml = true,
                Subject = "Welcome to your new account",
            };

            message.To.Add(to);

            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendErrorEmail(string to, Exception ex)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(_credentials.Email),
                Body = $"<div style=\"text-align=center;\"><h4>Hello, it seems as if something went wrong while running.<br/>Error message = {ex.Message}<br/>Source:{ex.Source}<br/>Stack Trace:{ex.StackTrace}</h4></div>",
                IsBodyHtml = true,
                Subject = "An error has occurred",
            };

            message.To.Add(to);

            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendTaskReminderEmail(string to, TaskModal taskInfo)
        {
            var tags = new Dictionary<string, string>()
            {
                {"$Description", taskInfo.Description }
            };

            var body_html = ReplaceTags(await GetFileContent("TaskReminder.html"), tags);

            var message = new MailMessage()
            {
                From = new MailAddress(_credentials.Email),
                Body = body_html,
                IsBodyHtml = true,
                Subject = $"Upcomming task {taskInfo.Title}",
            };

            message.To.Add(to);

            await _smtpClient.SendMailAsync(message);
        }

        public async Task<string> GetFileContent(string fileName)
        {
            var loc = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();

            return await File.ReadAllTextAsync(loc + "/Templates/" + fileName);
        }

        public string ReplaceTags(string content, Dictionary<string, string> tagPairs)
        {
            foreach (var item in tagPairs)
            {
                content = content.Replace(item.Key, HttpUtility.HtmlEncode(item.Value)); 
            }
            return content;
        }
    }
}