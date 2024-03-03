using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace AIC.Portal.Controllers
{
    public class ProjectController : RenderController
	{
		public ProjectController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
		{
		}

		public override IActionResult Index()
		{
			return base.Index();
		}

		public IActionResult ProjectDemo() 
		{
			var project = GetCurrentProject();
			return project is null ? 
				this.Problem("Project Not Found") : View("ProjectDemo", project);
		}

		public IActionResult ProjectRelatedNews()
		{
			var project = GetCurrentProject();
			return project is null ?
				this.Problem("Project Not Found") : View("ProjectRelatedNews", project);
		}

		private Project? GetCurrentProject()
		{
			return CurrentPage?.SafeCast<Project>();
		}
	}
}
