using AIC.Portal.Services.Abstractions;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace AIC.Portal.Services
{
	public class ProjectService : IProjectService
	{
        public static readonly string QueryParam = "domain";
        private readonly UmbracoHelper _umbracoHelper;
		private readonly Projects? _projectsContainer;

		public ProjectService(UmbracoHelper umbracoHelper)
		{
			_umbracoHelper = umbracoHelper;
			_projectsContainer = _umbracoHelper
					.ContentAtRoot()
					.OfType<AppliedInnovationCenter>()
					.First()
					.FirstChildOfType(Projects.ModelTypeAlias)?
					.SafeCast<Projects>();
		}

		public IEnumerable<Project> GetProjectsByTopicId(Guid topicId)
		{
			if ( _projectsContainer is null ) return Enumerable.Empty<Project>();

			return _projectsContainer.Children
					.OfType<Project>()
					.Where( x => x.Topic?.Any(t => t.Key.Equals(topicId)) ?? false);	
		}

		public IEnumerable<Domain> GetAllProjectsDomains()
		{
			return _umbracoHelper
				.ContentAtRoot()
				.OfType<ProjectDomains>()
				.First()
				.Children<Domain>() ?? Enumerable.Empty<Domain>();
		}
	}
}
