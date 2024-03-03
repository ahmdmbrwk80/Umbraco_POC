using AIC.Portal.DbContexts;
using AIC.Portal.Entities;
using AIC.Portal.Services;
using AIC.Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog.Context;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Mail;
using Umbraco.Cms.Core.Models.Email;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Persistence.EFCore.Scoping;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Website.Controllers;

namespace AIC.Portal.Controllers
{
    public class ContactUsformController : SurfaceController
    {
        private readonly IUserService _userService;
        private readonly GlobalSettings _globalSettings;
        private readonly ContactFormContext _context;
        private readonly GoogleCaptchService _GoogleCaptchService;
        private readonly UmbracoHelper _umbracoHelper;
        private readonly IEmailSender _emailSender;
        private readonly IPublishedSnapshotAccessor _snapshotAccessor;
        public ContactUsformController(IUmbracoContextAccessor umbracoContextAccessor,
            IOptions<GlobalSettings> globalSettings,
            GoogleCaptchService _googleCaptchService,
            IUserService userService,
            ContactFormContext context,
            IPublishedSnapshotAccessor snapshotAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            IEmailSender emailSender,
            ServiceContext services,
            UmbracoHelper umbracoHelper,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider)
            : base(umbracoContextAccessor,
                  databaseFactory,
                  services,
                  appCaches,
                  profilingLogger,
                  publishedUrlProvider)
        {
            _userService = userService;
            _globalSettings = globalSettings.Value;
            _umbracoHelper = umbracoHelper;
            _context = context;
            _GoogleCaptchService = _googleCaptchService;
            _snapshotAccessor = snapshotAccessor;
            _emailSender = emailSender;
        }
        [HttpGet]
        public async Task<IActionResult> getall()
        {
            DropDownViewModel dropdowndata = new DropDownViewModel();
            dropdowndata.countries = _context.Countries.ToList();
            dropdowndata.DropDownTypes = _context.Types.ToList();
            return Json(dropdowndata);
        }

        [HttpGet]
        public async Task<IActionResult> getgovernates(int countryId)
        {
            //if (countries == null)
            //{
            //	return NotFound();

            //}
            var cities = _context.Governorates
                    .Where(g => g.country.Id == countryId)
                    .Select(g => g) // Assuming you want English names
                    .ToList();
            return Json(cities);
        }



        [HttpPost]
        [ValidateUmbracoFormRouteString]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertComment(contactUsViewModel comment)
        {
            // check the recpacta 
            var _googlecaphca = _GoogleCaptchService.Recver(Request.Form["GoogleCaptchaToken"]);
            if (!_googlecaphca.Result.success)
            {
                ModelState.AddModelError("Error", "please check the from");

                return CurrentUmbracoPage();

            }

            if (!ModelState.IsValid)
            {

                ModelState.AddModelError("Error", "please check the from");
                TempData["status"] = "Fail";
                return CurrentUmbracoPage();
            }



            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }


            // save into database 
            ContactForm contactForm = new ContactForm();
            contactForm.FullName = comment.FullName;
            contactForm.EmailAddress = comment.EmailAddress;
            contactForm.Message = comment.Message;
            contactForm.PhoneNumber = comment.PhoneNumber;
            contactForm.city = _context.Governorates.FirstOrDefault(g => g.GovernorateNameEn == comment.city);
            contactForm.Type = _context.Types.FirstOrDefault(g => g.Type == comment.Type);
            contactForm.work = comment.work;
            // save as a doc type
            var ContactFormsRoot = _umbracoHelper.ContentAtRoot().OfType<ContactUsSubmissions>().First();
            if (ContactFormsRoot != null)
            {
                var newcontact = Services.ContentService.Create(comment.EmailAddress, ContactFormsRoot.Id, ContactUsSubmission.ModelTypeAlias);

                newcontact.SetValue(ContactUsSubmission.GetModelPropertyType(_snapshotAccessor, y => y.FullName).Alias, comment.FullName);
                newcontact.SetValue(ContactUsSubmission.GetModelPropertyType(_snapshotAccessor, y => y.Email).Alias, comment.EmailAddress);
                newcontact.SetValue(ContactUsSubmission.GetModelPropertyType(_snapshotAccessor, y => y.PhoneNumber).Alias, comment.PhoneNumber);
                newcontact.SetValue(ContactUsSubmission.GetModelPropertyType(_snapshotAccessor, y => y.City).Alias, comment.city);
                newcontact.SetValue(ContactUsSubmission.GetModelPropertyType(_snapshotAccessor, y => y.Country).Alias, comment.Country);
                newcontact.SetValue(ContactUsSubmission.GetModelPropertyType(_snapshotAccessor, y => y.Type).Alias, comment.Type);
                newcontact.SetValue(ContactUsSubmission.GetModelPropertyType(_snapshotAccessor, y => y.Work).Alias, comment.work);
                newcontact.SetValue(ContactUsSubmission.GetModelPropertyType(_snapshotAccessor, y => y.Message).Alias, comment.Message);
                Services.ContentService.SaveAndPublish(newcontact);
                //sned email to the form writer
                var fromAddress = _globalSettings.Smtp?.From;
                var subject = "AIC contact Us";
                var body = "thank you " + comment.FullName + " for you submission ";
                var email = new EmailMessage(
                    fromAddress,
                    comment.EmailAddress,
                    subject,
                    body,
                    true
                    );

                await _emailSender.SendAsync(email, emailType: "Contact");
            }
            var attachementsList = Enumerable.Empty<EmailMessageAttachment>();
            var contactSubmissionsGroup = _userService.GetUserGroupByAlias("contactSubmissionsGroup");
            if (contactSubmissionsGroup != null)
            {
                var membersEmails = _userService.GetAllInGroup(contactSubmissionsGroup?.Id)?.Select(x => x.Email)?.ToArray();
                if (membersEmails != null)
                {
                    var fromAddress = _globalSettings.Smtp?.From;
                    var subject = "AIC From submission";
                    var body = "New submission from Contact Us with message is "+ comment.Message;
                    var email = new EmailMessage(
                        fromAddress,
                        membersEmails, [], [], [],
                        subject,
                        body,
                        true,
                        attachementsList
                        );
                    await _emailSender.SendAsync(email, emailType: "Contact submission review");
                }

            }

            _context.ContactForms.Add(contactForm);


            _context.SaveChanges();
            //to do send eamil to the from owner
            TempData["status"] = "OK";


            return RedirectToCurrentUmbracoPage();

        }

    }
}
