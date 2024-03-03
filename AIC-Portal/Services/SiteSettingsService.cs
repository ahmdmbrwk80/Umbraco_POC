using AIC.Portal.Services.Abstractions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Common;

namespace AIC.Portal.Services
{
	public class SiteSettingsService : ISiteSettingsService
	{
		private readonly SiteSettings _siteSettings;
		public static readonly string DefaultApplicationName = "AIC";

		public SiteSettingsService(UmbracoHelper umbracoHelper)
		{
			_siteSettings = 
				umbracoHelper.ContentAtRoot()
					.OfType<SiteSettings>()
					.First().SafeCast<SiteSettings>() ?? throw new Exception("Site Settings Not Found");
			
		}

		public IEnumerable<IPublishedContent> GetAllowedSearchTypes(Guid? containerDocumentId = null)
		{
			if (containerDocumentId is null)
			{
				return _siteSettings.SearchableTypes ?? Enumerable.Empty<IPublishedContent>();
			}

			return _siteSettings
				.SearchableTypes?
				.Where(type => type.Key.Equals(containerDocumentId)) ?? Enumerable.Empty<IPublishedContent>();
		}
        public IEnumerable<String> getAllowedExtentions()
		{
			return _siteSettings.AllowedExtentions ?? Enumerable.Empty<String>();

        }
		public int GetimageFileSize()
		{
			return _siteSettings.ImageFileSize;
		}
		public int GetVdeoFileSize()
		{
			return _siteSettings.VideoFileSize;
		}
		public int GetFIlesize()
		{
			return _siteSettings.FileMediaTypeFileSize;
		}
		public int GetAudioSize() 
		{
			return _siteSettings.UmbracoMediaAudioFileSize;
		}
		public int GetArticalFilesize() 
		{
			return _siteSettings.UmbracoMediaArticle;

		}
		public int GetsvgFilesize() 
		{
			return _siteSettings.UmbracoMediaVectorGraphicsFileSize;
		}
        public int GetDefaultTimeZone()
		{
			return 2;
		}

		public string YoutubeApiKey()
		{
			return _siteSettings.YoutubeApiToken ?? string.Empty;
		}
		public string YoutubeChannelUserName()
		{
			return _siteSettings.YoutubeChannelUserName ?? string.Empty;
		}
		public string YoutubeApplicationName()
		{
			return _siteSettings.YoutubeApplicationName ?? DefaultApplicationName;
		}

        public string GetAnalyticsTrackingCode()
        {
			return _siteSettings.AnalyticsTrackingCode ?? string.Empty;
        }
    }
}
