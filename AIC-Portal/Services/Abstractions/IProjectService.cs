using Umbraco.Cms.Web.Common.PublishedModels;

namespace AIC.Portal.Services.Abstractions
{
	public interface IProjectService
	{
		public static readonly string QueryParam = "domain";
		public IEnumerable<Project> GetProjectsByTopicId(Guid topicId);
		public IEnumerable<Domain> GetAllProjectsDomains();

    }
}
