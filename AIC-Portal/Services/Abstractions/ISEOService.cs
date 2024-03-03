using AIC.Portal.ViewModels;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace AIC.Portal.Services.Abstractions
{
    public interface ISEOService
    {
        public SeoVM ReturnSeoData(int nodeId);

    }
}
