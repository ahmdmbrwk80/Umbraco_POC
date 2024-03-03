using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace AIC.Portal.Services.Abstractions
{
    public interface ISiteSettingsService
    {
		public IEnumerable<IPublishedContent> GetAllowedSearchTypes(Guid? containerDocumentId = null);
		public int GetDefaultTimeZone();
		public string YoutubeApiKey();
		public string YoutubeChannelUserName();
		public string YoutubeApplicationName();
		public int GetVdeoFileSize();
		public int GetimageFileSize();
		public IEnumerable<String> getAllowedExtentions();
		public int GetFIlesize();
		public int GetAudioSize();
		public int GetArticalFilesize();
		public int GetsvgFilesize();



        public string GetAnalyticsTrackingCode();

    }
}
