using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using Thefieldofdreams.Application.Interfaces;
using Twilio.Rest;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        private (string? host, int port, string? user, string? pass, string from, string fromName) GetSmtpSettings()
        {
            var emailSettings = _config.GetSection("EmailSettings");
            var host = emailSettings["SmtpHost"];
            var portStr = emailSettings["SmtpPort"];
            var user = emailSettings["SmtpUser"];
            var pass = emailSettings["SmtpPass"];
            var from = emailSettings["FromAddress"] ?? user ?? "noreply@maintenancepro.com";
            var fromName = emailSettings["FromName"] ?? "MaintenancePro";
            int port = int.TryParse(portStr, out var p) ? p : 587;
            return (host, port, user, pass, from, fromName);
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string resetLink)
        {
            var smtp = GetSmtpSettings();

            // If SMTP is not configured, log the reset link so it is usable in development
            if (string.IsNullOrWhiteSpace(smtp.host) || string.IsNullOrWhiteSpace(smtp.user))
            {
                // Sanitize user-supplied values to prevent log forging (CRLF injection)
                var safeEmail = toEmail.Replace("\r", "", StringComparison.Ordinal)
                                       .Replace("\n", "", StringComparison.Ordinal);
                var safeLink = resetLink.Replace("\r", "", StringComparison.Ordinal)
                                        .Replace("\n", "", StringComparison.Ordinal);
                _logger.LogInformation(
                    "[DEV] Password reset link for {Email}: {ResetLink}", safeEmail, safeLink);
                return;
            }

            using var message = new MailMessage
            {
                From = new MailAddress(smtp.from, smtp.fromName),
                Subject = "Password Reset Request",
                Body = $"<p>You requested a password reset.</p>" +
                       $"<p>Click the link below to reset your password (valid for 1 hour):</p>" +
                       $"<p><a href=\"{resetLink}\">{resetLink}</a></p>" +
                       $"<p>If you did not request this, please ignore this email.</p>",
                IsBodyHtml = true
            };
            message.To.Add(toEmail);

            using var client = new SmtpClient(smtp.host, smtp.port)
            {
                Credentials = new NetworkCredential(smtp.user, smtp.pass),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }


        private static string Encode(string value)
            => System.Web.HttpUtility.HtmlEncode(value);

        private static string FormatCurrency(decimal value)
            => value.ToString("N2", CultureInfo.InvariantCulture);

    }

}
