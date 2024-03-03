using AIC.Portal.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AIC.Portal.Services
{
	public class GoogleCaptchService
	{
		private readonly googleCaptchKeys _googleCaptchKeys;
        public GoogleCaptchService(IOptions<googleCaptchKeys> options) 
		{
            _googleCaptchKeys = options.Value;

        }
		public virtual async Task<GoogleREspo> Recver(string _Token)
		{
			var secretKey = _googleCaptchKeys.secretKey;
			GoogleCaptchData data = new GoogleCaptchData() { response = _Token, secret = secretKey };
			HttpClient Client = new HttpClient();
			var response = await Client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?secret={data.secret}&response={data.response}");

			var capresp = JsonConvert.DeserializeObject<GoogleREspo>(response);
			return capresp;
		}



	}
	public class GoogleCaptchData
	{
		public string response { get; set; } // token
		public string secret { get; set; }
	}

	public class GoogleREspo
	{
		public bool success { get; set; }
		public double? score { get; set; }
		public string action { get; set; }

		public DateTime challange_ts { get; set; }
		public string hostname { get; set; }
	}
}
