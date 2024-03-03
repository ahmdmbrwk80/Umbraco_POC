namespace AIC.Portal.Services.Abstractions
{
    public interface IYoutubeApiService
    {
        public IEnumerable<string> GetLatestUploadsEmbedLinks();
    }
}
