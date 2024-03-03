using AIC.Portal.Services.Abstractions;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Web.Common;

namespace AIC.Portal.Services
{
	//MediaNotificationHandler
	public static partial class UmbracoBuilder
	{
		public static IUmbracoBuilder Addcustomnotificationservice(this IUmbracoBuilder builder)
		{
			builder.AddNotificationHandler<MediaSavingNotification, MediaNotificationHandler>();
			return builder;

		}
	}
	public class MediaNotificationHandler : INotificationHandler<MediaSavingNotification>
	{
		private readonly UmbracoHelper _umbracoHelper;
        private readonly ISiteSettingsService _siteSettings;
        public MediaNotificationHandler(UmbracoHelper umbracoHelper,
            ISiteSettingsService siteSettings)
		{
            _siteSettings = siteSettings;
            _umbracoHelper = umbracoHelper;
		}
		public void Handle(MediaSavingNotification notification)
		{	
            //fetch the setting properies form the content 
            //extentions 
            IEnumerable<string> allowedExtensions = _siteSettings.getAllowedExtentions();
			// image size
            var imagefilesize = _siteSettings.GetimageFileSize();
			// video size 
			var vidfilesize = _siteSettings.GetVdeoFileSize();
			var svgFilesize = _siteSettings.GetsvgFilesize();
			var filefilesize = _siteSettings.GetFIlesize();
			var audiofilesize = _siteSettings.GetAudioSize();
			var articalfilesize=_siteSettings.GetArticalFilesize();





            if (allowedExtensions == null)
			{
				notification.CancelOperation(new EventMessage("site setting propblem",
					"please add values in the allowed extentions property",
					EventMessageType.Error));
			}
			else
			{
                foreach (var mediaItem in notification.SavedEntities)
				{
					// if it is folder let it pass 
					if (mediaItem.ContentType.Alias!="Folder") 
					{ 
                    string fileExtension = Path.GetExtension(mediaItem.GetValue<string>("umbracoFile"));

						if (fileExtension == null)
						{
							notification.CancelOperation(new EventMessage("file type",
						"null file type",
						EventMessageType.Error));

						}


						if (!allowedExtensions.Contains(fileExtension.TrimStart('.').Trim('"', '}').ToLower()))
						{
							notification.CancelOperation(new EventMessage("file type",
						"worng  file type",
						EventMessageType.Error));
						}
						if(mediaItem.ContentType.Alias== "Image")
						{
                            if (mediaItem.GetValue<int>("umbracoBytes") > imagefilesize * 1024 * 1024)
                            {
                                // custom message 
                                notification.CancelOperation(new EventMessage("test image inviald",
                            "pic size invaild",
                            EventMessageType.Error));

                            }
                        }else if (mediaItem.ContentType.Alias.Equals("umbracoMediaVideo")) 
						{
                            if (mediaItem.GetValue<int>("umbracoBytes") > vidfilesize * 1024 * 1024)
                            {

                                notification.CancelOperation(new EventMessage("test inviald",
                            "the video size is too big",
                            EventMessageType.Error));

                            }
                        }else if (mediaItem.ContentType.Alias.Equals("File"))
                        {
                            if (mediaItem.GetValue<int>("umbracoBytes") > filefilesize * 1024 * 1024)
                            {

                                notification.CancelOperation(new EventMessage("test inviald",
                            "the File size is too big",
                            EventMessageType.Error));

                            }
                        }else  if (mediaItem.ContentType.Alias.Equals("umbracoMediaVectorGraphics"))
                        {
                            if (mediaItem.GetValue<int>("umbracoBytes") > svgFilesize * 1024 * 1024)
                            {

                                notification.CancelOperation(new EventMessage("test inviald",
                            "the File size is too big",
                            EventMessageType.Error));

                            }
                        }else if (mediaItem.ContentType.Alias.Equals("umbracoMediaAudio"))
                        {
                            if (mediaItem.GetValue<int>("umbracoBytes") > audiofilesize * 1024 * 1024)
                            {

                                notification.CancelOperation(new EventMessage("test inviald",
                            "the File size is too big",
                            EventMessageType.Error));

                            }
                        }else if (mediaItem.ContentType.Alias.Equals("umbracoMediaArticle"))
                        {
                            if (mediaItem.GetValue<int>("umbracoBytes") > articalfilesize * 1024 * 1024)
                            {

                                notification.CancelOperation(new EventMessage("test inviald",
                            "the File size is too big",
                            EventMessageType.Error));

                            }
                        }//else
                        //{
                        //    notification.CancelOperation(new EventMessage("test inviald",
                        //    "invaild media type",
                        //    EventMessageType.Error));

                        //}



                    }

                }

            }
		}
	}
}
