using AIC.Portal.Services;
using AIC.Portal.Services.Abstractions;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace AIC.Portal.NotificationHandlers
{
    public class NewsletterEmailSenderHandler : INotificationAsyncHandler<ContentPublishedNotification>
    {
        private readonly INewsletterSubscriptionService _newsletterService;
        private readonly ILogger<NewsletterEmailSenderHandler> _logger;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly string _mailDocumentAlias = NewsletterMail.ModelTypeAlias;
        public NewsletterEmailSenderHandler(
            INewsletterSubscriptionService newsletterService,
            ILogger<NewsletterEmailSenderHandler> logger,
            UmbracoHelper umbracoHelper)
        {
            
            _newsletterService = newsletterService;
            _logger = logger;
            _umbracoHelper = umbracoHelper;
        }

        public async Task HandleAsync(ContentPublishedNotification notification, CancellationToken cancellationToken)
        {
            //Current Element being Published
            var element = notification.PublishedEntities.First();
            if (!element.ContentType.Alias.Equals(_mailDocumentAlias))
            {
                return;
            }

            var content = 
                notification
                .PublishedEntities
                .First();

            var mailToSend = _umbracoHelper.Content(content.Id) as NewsletterMail;

            if (mailToSend is null)
            {
                _logger.LogError("Mail Being published returned Null");
                return;
            }

            await _newsletterService.SendEmail(mailToSend);
        }
    }
}
