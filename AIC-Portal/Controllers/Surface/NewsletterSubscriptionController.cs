using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;
using AIC.Portal.ViewModels;

namespace AIC.Portal.Controllers.Surface
{
	public class NewsletterSubscriptionController : SurfaceController
	{
        private readonly ILogger<NewsletterSubscriptionController> _logger;
        private readonly IMemberService _memberService;
		public NewsletterSubscriptionController(
			IUmbracoContextAccessor umbracoContextAccessor,
			IUmbracoDatabaseFactory databaseFactory,
			ServiceContext services,
			AppCaches appCaches,
			IProfilingLogger profilingLogger,
			IPublishedUrlProvider publishedUrlProvider,
			ILogger<NewsletterSubscriptionController> logger)
			: base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
			_logger = logger;
			if(services.MemberService is null)
			{
				_logger.LogError("Member Service was not Initiated");
				throw new ArgumentNullException(nameof(services.MemberService));
			}
			_memberService = services.MemberService;
		}

		public IActionResult Subscribe(NewsletterSubscriberViewModel model)
		{
			if (!ModelState.IsValid)
			{
				TempData["SubscriptionError"] = "Email Must be Provided";
				return RedirectToCurrentUmbracoPage();
			}

			if (_memberService.GetByEmail(model.Email) != null)
			{
                TempData["SubscriptionError"] = "A Subscription was Found with same Email";
                return RedirectToCurrentUmbracoPage();
			}
			var userName = "Subscriber #" + _memberService.Count();
			var user = _memberService.CreateMemberWithIdentity(userName, model.Email, Member.ModelTypeAlias);

			if(user is null)
			{
                TempData["SubscriptionError"] = "Could not Create Email due to an Error on our side";
				_logger.LogError("Returned Member resulted in Null");
                return RedirectToCurrentUmbracoPage();
            }

			TempData["SubscriptionError"] = "You've Subscribes Successfully";
			return RedirectToCurrentUmbracoPage();
		}
	}
}
