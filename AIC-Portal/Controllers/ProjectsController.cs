using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace AIC.Portal.Controllers
{
    public class ProjectsController : RenderController
    {
        public static readonly string DemoTemplate = "ProjectDemo";
        public static readonly string RelatedNewsTemplate = "ProjectRelatedNews";

		private readonly UmbracoHelper _umbracoHelper;

        public ProjectsController(
            ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            UmbracoHelper umbracoHelper)
        : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _umbracoHelper = umbracoHelper;
        }

        public override IActionResult Index()
        {
            var domains = _umbracoHelper
                .ContentAtRoot()
                .OfType<ProjectDomains>()
                .First() ?? throw new Exception();
            
            return CurrentTemplate(domains);
        }
    }
}
