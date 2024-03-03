using AIC.Portal.Services.Abstractions;
using AIC.Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Infrastructure.Persistence.Dtos;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Common.Views;
using Umbraco.Cms.Web.Website.Controllers;

namespace AIC.Portal.Controllers.Surface
{
    public class SearchController : SurfaceController
    {
        private readonly ISearchService _searchService;
		private readonly UmbracoHelper _umbracoHelper;
        private readonly ILogger _logger;

		public SearchController(
            IUmbracoContextAccessor umbracoContextAccessor, 
            IUmbracoDatabaseFactory databaseFactory, 
            ServiceContext services, 
            AppCaches appCaches, 
            IProfilingLogger profilingLogger, 
            IPublishedUrlProvider publishedUrlProvider,
            ISearchService searchService,
            UmbracoHelper umbracoHelper,
            ILogger<SearchController> logger) 
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _searchService = searchService;
			_umbracoHelper = umbracoHelper;
            _logger = logger;
		}

        [HttpPost]
        [ValidateUmbracoFormRouteString]
        public IActionResult Search(SearchViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(model.Query))
            {
                return CurrentUmbracoPage();
            }

            var page = _umbracoHelper
                .ContentAtRoot()
                .OfType<AppliedInnovationCenter>()
                .FirstOrDefault()?
                .FirstChild<SearchPage>();

            if( page is null ) 
            {
                _logger.LogError("No Search Page was Found!");
                return CurrentUmbracoPage(); 
            }

			var results = _searchService.Search(model.Query, model.SearchableTypeId, model.FromDate, model.ToDate);
            if(results is null || !results.Any())
            {
                TempData["SearchError"] = "Nothing found with what you specified";
            }
            page.SearchResults = results ?? new List<IPublishedContent>();
            
            return View( "SearchPage",page );
        }
    }
}
