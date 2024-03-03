using AIC.Portal.ViewModels;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace AIC.Portal.Services.Abstractions
{
    public interface IMasterPageService
    {
        public IEnumerable<IPublishedContent> ReturnHeaderLinks();
        public IEnumerable<IPublishedContent> ReturnFooterLinks();
        public SiteLogosVM ReturnSiteLogos();

        public bool IsHomePage(int nodeId);
        public IEnumerable<IPublishedElement> SocialMediaInHome();
        public IEnumerable<IPublishedElement> SocialMediaInInnerPages();

    }
}
