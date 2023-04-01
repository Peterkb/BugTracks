using BugTracksV3.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BugTracksV3.Services;

public class BTEmailService : IEmailSender
{
	private readonly MailSettings _mailSettings;

	public BTEmailService(IOptions<MailSettings> mailSettings)
	{
		_mailSettings = mailSettings.Value;
		MailSettings = mailSettings; // resolves error CS8618
	}

	public IOptions<MailSettings> MailSettings { get; }

	public async Task SendEmailAsync(string emailTo, string subject, string htmlMessage)
	{
		MimeMessage email = new();

		email.Sender = MailboxAddress.Parse(_mailSettings.Email ?? Environment.GetEnvironmentVariable("Email"));
		email.To.Add(MailboxAddress.Parse(emailTo));
		email.Subject = subject;

		var builder = new BodyBuilder
		{
			HtmlBody = htmlMessage
		};

		email.Body = builder.ToMessageBody();

		try
		{
			using var smtp = new SmtpClient();

			var host = _mailSettings.EmailHost ?? Environment.GetEnvironmentVariable("EmailHost");
			var port = _mailSettings.EmailPort != 0 ? _mailSettings.EmailPort : int.Parse(Environment.GetEnvironmentVariable("EmailPort"));
			var password = _mailSettings.EmailPassword ?? Environment.GetEnvironmentVariable("EmailPassword");

			await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(_mailSettings.Email, _mailSettings.EmailPassword);

			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}
		catch (Exception ex)
		{
			var error = ex.Message;
			throw;
		}
	}
}
