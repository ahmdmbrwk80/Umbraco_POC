using AIC.Portal.DbContexts;
using AIC.Portal.NotificationHandlers;
using AIC.Portal.Models;
using AIC.Portal.Services;
using AIC.Portal.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Serilog.Context;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISiteSettingsService, SiteSettingsService>();
builder.Services.AddScoped<IYoutubeApiService, YoutubeApiService>();
builder.Services.AddTransient<GoogleCaptchService>();
builder.Services.AddScoped<IContactUsFormService, ContactUsFormService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IMasterPageService, MasterPageService>();
builder.Services.AddScoped<INewsletterSubscriptionService, NewsletterSubscriptionService>();

//Notification Handler for SendingEmails
builder.Services.AddScoped<ISEOService, SEOService>();

builder.Services.Configure<googleCaptchKeys>(builder.Configuration.GetSection("GoogleRecaptchaSettings"));
builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
	.Addcustomnotificationservice()
    .AddNotificationAsyncHandler<ContentPublishedNotification, NewsletterEmailSenderHandler>()
    .Build();


builder.Services.AddUnique<IBackOfficeUserPasswordChecker, ActiveDirectoryAuthenticationService>();
//builder.Services.AddUmbracoEFCoreContext<ContactFormContext>("Server=.;Database=AIC-Portal;Integrated Security=true;TrustServerCertificate=true;", "Microsoft.Data.SqlClient");
builder.Services.AddUmbracoEFCoreContext<ContactFormContext>(
  (options, connectionString, providerName) => options.UseSqlServer(connectionString));
WebApplication app = builder.Build();

await app.BootUmbracoAsync();


app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseInstallerEndpoints();
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();
