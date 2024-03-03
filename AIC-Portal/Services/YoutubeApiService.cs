using AIC.Portal.Services.Abstractions;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Channel = Google.Apis.YouTube.v3.Data.Channel;

namespace AIC.Portal.Services
{
    public class YoutubeApiService : IYoutubeApiService
    {
		private YouTubeService _youtubeService;
		private readonly ISiteSettingsService _siteSettings;
		private readonly ILogger _logger;

        public YoutubeApiService(ISiteSettingsService siteSettings, ILogger<YoutubeApiService> logger)
        {
			_siteSettings = siteSettings;
			_youtubeService = new YouTubeService();
			_logger = logger;
		}

		public IEnumerable<string> GetLatestUploadsEmbedLinks()
		{
			if (string.IsNullOrEmpty(_siteSettings.YoutubeApiKey()))
			{
				return [];
			}

			var initializer = new BaseClientService.Initializer
			{
				ApplicationName = _siteSettings.YoutubeApplicationName(), //Settings Dependency
				ApiKey = _siteSettings.YoutubeApiKey(),
			};
			var embedLinks = new List<string>();
			try
			{
				using (_youtubeService = new YouTubeService(initializer))
				{
					var channel = GetChannel();
					if (channel is null) return [];

					//Returns a List of Video Ids, to Be used in Embedding
					var playlistItems = GetChannelUploadsIds(channel);
					foreach (var item in playlistItems)
					{
						embedLinks.Add("https://www.youtube.com/embed/" + item.ContentDetails.VideoId);
					}//TO EMBED In IFRAME
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Youtube API Caused an Exception");
			}
			
			return embedLinks;
		}

		private Channel? GetChannel()
		{
			var channelRequest = _youtubeService.Channels.List("contentDetails, snippet");
			channelRequest.ForUsername = _siteSettings.YoutubeChannelUserName();
			var response = channelRequest.Execute();

			if(response.Items == null) return null;

			return response.Items.FirstOrDefault();
		}

		private IEnumerable<PlaylistItem> GetChannelUploadsIds(Channel channel)
		{
			var uploadsId = channel.ContentDetails.RelatedPlaylists.Uploads;

			var playlistRequest = _youtubeService.PlaylistItems.List("contentDetails, snippet");
			playlistRequest.PlaylistId = uploadsId;
			
			var response = playlistRequest.Execute();
			return response.Items;
		}
	}
}
