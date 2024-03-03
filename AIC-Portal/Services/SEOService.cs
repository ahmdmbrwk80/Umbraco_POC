using AIC.Portal.Services.Abstractions;
using AIC.Portal.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace AIC.Portal.Services
{
    public class SEOService : ISEOService
    {
        private readonly UmbracoHelper _umbracoHelper;

        public SEOService(UmbracoHelper umbracoHelper)
        {
            _umbracoHelper = umbracoHelper;
        }

        public SeoVM ReturnSeoData(int nodeId)
        {
            SeoVM seoVM = new SeoVM();
            var appliedInnovationCenter = _umbracoHelper.ContentAtRoot().OfType<AppliedInnovationCenter>().First();
            var homepage = appliedInnovationCenter?.FirstChildOfType(HomePage.ModelTypeAlias)?.SafeCast<HomePage>();
            var settingPage = _umbracoHelper.ContentAtRoot()
                    .OfType<SiteSettings>()
                    .First().SafeCast<SiteSettings>() ?? throw new Exception("Site Settings Not Found");
            IPublishedContent content = _umbracoHelper.Content(nodeId);
            if (content != null)
            {
                seoVM.Title = content.HasValue("metaTitle") ? content.Value<string>("metaTitle") : appliedInnovationCenter?.Name ;
                seoVM.Description = content.HasValue("metaDescription") ? content.Value<string>("metaDescription") : settingPage?.MetaDescription ;
                seoVM.SocialMediaShareImage = content.HasValue("socialShareImage") ? content.Value<IPublishedContent>("socialShareImage")?.MediaUrl() : appliedInnovationCenter?.LogoInHome?.MediaUrl() ;
                seoVM.Tags = content.HasValue("metakeywords") ? content.Value<List<string>>("metakeywords") : settingPage?.MetaKeyWords?.ToList() ?? new List<string>();
                seoVM.URL = appliedInnovationCenter?.Url();
            }
            return seoVM;
        }
    }
}
