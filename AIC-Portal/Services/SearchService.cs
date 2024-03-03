using AIC.Portal.Services.Abstractions;
using Examine;
using Examine.Lucene.Indexing;
using Examine.Search;
using Lucene.Net.Documents;
using System.Globalization;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;

namespace AIC.Portal.Services
{
	public class SearchService : ISearchService
	{
		private readonly IExamineManager _examineManager;
		private readonly UmbracoHelper _umbracoHelper;
		private readonly ISiteSettingsService _siteSettings;

		public SearchService(
			IExamineManager examineManager, 
			UmbracoHelper umbracoHelper,
			ISiteSettingsService siteSettings)
		{
			_examineManager = examineManager;
			_umbracoHelper = umbracoHelper;
			_siteSettings = siteSettings;
		}

		public IEnumerable<IPublishedContent> Search(string query, Guid? containerDocumentId = null, DateTime? fromDate = null, DateTime? toDate = null)
		{
			string indexName = Umbraco.Cms.Core.Constants.UmbracoIndexes.ExternalIndexName;
			bool isEmptyQuery = string.IsNullOrEmpty(query);

			var allowedSearchTypes = _siteSettings.GetAllowedSearchTypes(containerDocumentId);
			bool isEmptySearchTypes = !allowedSearchTypes.Any();

			bool canGetIndex = _examineManager.TryGetIndex(indexName, out var index);

			if (!canGetIndex || isEmptyQuery || isEmptySearchTypes)
			{
				return Enumerable.Empty<IPublishedContent>();
			}

			var searchResults =
				BuildSearchQuery(index.Searcher, allowedSearchTypes, fromDate, toDate)
				.ManagedQuery(query)
				.Execute()
				.Select(result => result.Id);

				
			if (searchResults is null || !searchResults.Any()) return Enumerable.Empty<IPublishedContent>();
			return _umbracoHelper.Content(searchResults);
		}

		//Builds are returns a constructed query, restricted on given searchable types
		private IQuery BuildSearchQuery(
			ISearcher searcher, 
			IEnumerable<IPublishedContent> searchableTypes,
			DateTime? fromDate, DateTime? toDate )
		{
			var queryType = Umbraco.Cms.Core.Constants.Applications.Content;
			var query = searcher.CreateQuery(queryType);

			if (fromDate != null)
			{
				toDate ??= DateTime.UtcNow;
				query.RangeQuery<DateTime>(
					new[] { "createDate" },
					DateTime.Parse(fromDate.ToString() ?? ""),
					DateTime.Parse(toDate.ToString() ?? ""),
					true,
					true
				);
			}

			foreach (var type in searchableTypes)
            {
				if(searchableTypes.IndexOf(type) == searchableTypes.Count() - 1)
				{
					query
					.ParentId(type.Id)
					.And();

					break;
					//Append "And" instead of Or for further query logic
				}

				query
					.ParentId(type.Id)
					.Or();
            }
			return query;
        }
	}
}
