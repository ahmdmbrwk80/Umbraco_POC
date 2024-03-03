using Umbraco.Cms.Web.Common.PublishedModels;

namespace AIC.Portal.Services.Abstractions
{
    public interface INewsletterSubscriptionService
    {
        Task<bool> SendEmail(NewsletterMail mail);
    }
}