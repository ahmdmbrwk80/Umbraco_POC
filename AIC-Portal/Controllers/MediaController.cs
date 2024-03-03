using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System.Runtime.InteropServices;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Website.Controllers;
namespace AIC.Portal.Controllers
{
    public class MediaController : SurfaceController
    {
        private readonly IMediaService _mediaService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IContentTypeBaseServiceProvider _contentTypeBaseServiceProvider;
        private readonly MediaFileManager _mediaFileManager;
        private readonly MediaUrlGeneratorCollection _mediaUrlGeneratorCollection;
        private readonly IShortStringHelper _shortStringHelper;
        public MediaController(
           IUmbracoContextAccessor umbracoContextAccessor,
           MediaUrlGeneratorCollection mediaUrlGenerators,
           IShortStringHelper shortStringHelper,
           MediaFileManager mediaFileManager,
           IMediaService mediaService,
           IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
           UmbracoHelper umbracoHelper,
           IUmbracoDatabaseFactory databaseFactory,
           ServiceContext services, AppCaches appCaches,
           IProfilingLogger profilingLogger,
           IPublishedUrlProvider publishedUrlProvider,
            IWebHostEnvironment hostingEnvironment)
           : base(umbracoContextAccessor,
                 databaseFactory,
                 services,
                 appCaches,
                 profilingLogger,
                 publishedUrlProvider)
        {
            _umbracoHelper = umbracoHelper;
            _mediaService = mediaService;
            _contentTypeBaseServiceProvider = contentTypeBaseServiceProvider;
            _mediaFileManager = mediaFileManager;
            _mediaUrlGeneratorCollection = mediaUrlGenerators;
            _shortStringHelper = shortStringHelper;
            _hostingEnvironment=hostingEnvironment;
        }
        public IActionResult Index()
        {
            try
            {
                // Specify the path to the folder within wwwroot
                string wwwrootPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "SocialMediaIcons");

                // Specify the Umbraco media folder where you want to copy/move the files
                int targetMediaFolderId = 1076; // Replace with the actual ID of your target media folder

                // Get a list of files in the wwwroot folder
                string[] files = Directory.GetFiles(wwwrootPath);

                // Loop through the files and copy/move them to the Umbraco media folder
                foreach (string filePath in files)
                {
                    // Get the file name
                    string fileName = Path.GetFileName(filePath);

                    // Create a new media item
                    // create the media item
                    IMedia mediaItem = _mediaService.CreateMedia(fileName, targetMediaFolderId, Constants.Conventions.MediaTypes.Image);
                    // Open the file stream for reading
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        // Create an IFormFile instance to represent the file
                        IFormFile file = new FormFile(fileStream, 0, fileStream.Length, null, fileName)
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "application/octet-stream" // Set the content type appropriately
                        };

                        // Set media item values from the uploaded file
                        SetMediaItemValueFromFileUpload(file, mediaItem, true);
                    }



                }

                return Content("Files synchronized successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return Content($"An error occurred: {ex.Message}");
            }

        }

        public void SetMediaItemValueFromFileUpload(IFormFile file, IMedia mediaItem, bool returnUdi = true)
        {   //set media item to the uploaded file

            mediaItem.SetValue(_mediaFileManager, _mediaUrlGeneratorCollection, _shortStringHelper, _contentTypeBaseServiceProvider, Constants.Conventions.Media.File, file.FileName, file.OpenReadStream());

            //save media item in the media folder in the backoffice

            _mediaService.Save(mediaItem);
        }
    }
}

