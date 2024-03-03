using Umbraco.Cms.Core.Models.PublishedContent;

namespace AIC.Portal.Services.Abstractions
{
    public interface ISearchService
    {
        public IEnumerable<IPublishedContent> Search(string query, Guid? containerDocumentId = null, DateTime? fromDate = null, DateTime? toDate = null);

	}
}
