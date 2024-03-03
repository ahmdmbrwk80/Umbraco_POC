using AIC.Portal.Services.Abstractions;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Mail;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Email;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Web.Common.PublishedModels;
using IO = System.IO;

namespace AIC.Portal.Services
{
    public class NewsletterSubscriptionService : INewsletterSubscriptionService
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IMemberService _memberService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<NewsletterSubscriptionService> _logger;
        private readonly IWebHostEnvironment _env;

        public NewsletterSubscriptionService(
            IOptions<GlobalSettings> globalSettings,
            IMemberService memberService,
            IEmailSender emailSender,
            ILogger<NewsletterSubscriptionService> logger,
            IWebHostEnvironment env)
        {
            
            _globalSettings = globalSettings.Value;
            _memberService = memberService;
            _emailSender = emailSender;
            _logger = logger;
            _env = env;
        }

        public async Task<bool> SendEmail(NewsletterMail mail)
        {
            var fromAddress = _globalSettings.Smtp?.From;
            var subject = mail.Subject;

            if (fromAddress is null)
            {
                _logger.LogError("No Smtp Sender Mail was Configured");
                return false;
            }

            if (mail.Body is null)
            {
                _logger.LogError("Mail Body message is Null or Empty!");
                return false;
            }
            
            var body = GetEmailBodyTemplate(mail.Body);
            var recipents =
                mail.SendToAll ? 
                    _memberService.GetAllMembers() : 
                    _memberService.GetAllMembers( mail.Recipents?.Select(c => c.Id).ToArray<int>() ?? [] );

            return await SendMailToRecipents(recipents, fromAddress, subject, body, mail.Attachments);
        }

        private string GetEmailBodyTemplate(IHtmlEncodedString content)
        {
            var filePath = "Views/Partials/MailTemplate/MailTemplate.cshtml";
            var template = IO.File.ReadAllText(filePath);
            var body = content.ToHtmlString();
            return template.Replace("%CONTENT%", body);
        }

        private async Task<bool> SendMailToRecipents(IEnumerable<IMember>? recipents, string fromAddress, string? subject, string body, IEnumerable<IPublishedContent>? attachements)
        {
            if (recipents is null || !recipents.Any()) return false;
            var attachementsList = Enumerable.Empty<EmailMessageAttachment>();
            if (attachements != null && attachements.Any())
            {
                attachementsList = attachements.Select(c =>
                {
                    var file = c.Value<string>(Constants.Conventions.Media.File);
                    var fullPath = _env.WebRootPath + file;
                    Stream stream = IO.File.OpenRead(fullPath);
                    return new EmailMessageAttachment(stream, $"{c.Name}.{c.Value<string>(Constants.Conventions.Media.Extension)}");
                });
            }

            try
            {
                var email = new EmailMessage(
                    fromAddress,
                    recipents.Select(m => m.Email).ToArray(),
                    [], [], [],
                    subject, 
                    body, 
                    true, 
                    attachementsList);
                await _emailSender.SendAsync(email, emailType: "Newsletter");

                _logger.LogInformation("Emails Successfully Sent to all recipents!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email Sending Caused an Exception");
                return false;
            }


        }
    }
}
