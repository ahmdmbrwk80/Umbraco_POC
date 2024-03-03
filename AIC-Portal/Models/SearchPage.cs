using Umbraco.Cms.Core.Models.PublishedContent;

namespace Umbraco.Cms.Web.Common.PublishedModels
{
    public partial class SearchPage
    {
        public IEnumerable<IPublishedContent> SearchResults { get; set; } = new List<IPublishedContent>();
    }
}
