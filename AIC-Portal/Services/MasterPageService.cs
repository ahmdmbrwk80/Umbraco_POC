using AIC.Portal.Services.Abstractions;
using AIC.Portal.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace AIC.Portal.Services
{
    public class MasterPageService : IMasterPageService
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly AppliedInnovationCenter _appliedInnovationCenter;
        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;

        public MasterPageService(UmbracoHelper umbracoHelper,IPublishedSnapshotAccessor publishedSnapshotAccessor)
        {
            _umbracoHelper = umbracoHelper;
            _appliedInnovationCenter = _umbracoHelper.ContentAtRoot()?.OfType<AppliedInnovationCenter>().FirstOrDefault();
            _publishedSnapshotAccessor = publishedSnapshotAccessor;
        }



        public IEnumerable<IPublishedContent> ReturnFooterLinks()
        {
            if (_appliedInnovationCenter != null)
            {
                var footerItems = _appliedInnovationCenter.Children
                    ?.Where(child => child.HasProperty("showInFooter") && child.Value<bool>("showInFooter"));

                return footerItems?.Any() == true ? footerItems : Enumerable.Empty<IPublishedContent>();
            }

            return Enumerable.Empty<IPublishedContent>();
        }

        public IEnumerable<IPublishedContent> ReturnHeaderLinks()
        {
            if (_appliedInnovationCenter != null)
            {
                var headerItems = _appliedInnovationCenter.Children
                    ?.Where(child => child.HasProperty("showInHeader") && child.Value<bool>("showInHeader"));

                return headerItems?.Any() == true ? headerItems : Enumerable.Empty<IPublishedContent>();
            }

            return Enumerable.Empty<IPublishedContent>();
        }

        public SiteLogosVM ReturnSiteLogos()
        {
            var siteLogos = new SiteLogosVM
            {
                LogoInFooter = _appliedInnovationCenter?.LogoInFooter.MediaUrl(),
                LogoInInnerPages = _appliedInnovationCenter?.LogoInInnerPages.MediaUrl(),
                LogoInHome = _appliedInnovationCenter?.LogoInHome.MediaUrl()
            };

            return siteLogos;
        }

        public bool IsHomePage(int nodeId)
        {
            IPublishedContent content = _umbracoHelper.Content(nodeId);
            if(content == null)
            return false;
            if(content.ContentType.Alias == "homePage" || content.ContentType.Alias == "appliedInnovationCenter")
            {
                return true;
            }
            return false;

        }

        public IEnumerable<IPublishedElement> SocialMediaInHome()
        {
            if (_appliedInnovationCenter != null)
            {
                var SocialMediaInHome = _appliedInnovationCenter.SocialMediaInHome?.Select(x => x.Content);

                return SocialMediaInHome?.Any() == true ? SocialMediaInHome : Enumerable.Empty<IPublishedElement>();
            }

            return Enumerable.Empty<IPublishedElement>();
        }
        public IEnumerable<IPublishedElement> SocialMediaInInnerPages()
        {
            if (_appliedInnovationCenter != null)
            {
                var SocialMediaInInnerPages = _appliedInnovationCenter.SocialInnerPages?.Select(x => x.Content);

                return SocialMediaInInnerPages?.Any() == true ? SocialMediaInInnerPages : Enumerable.Empty<IPublishedElement>();
            }

            return Enumerable.Empty<IPublishedElement>();
        }
    }
}
