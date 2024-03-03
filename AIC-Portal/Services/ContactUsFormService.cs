using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Common;
using AIC.Portal.DbContexts;
using AIC.Portal.Entities;
using static Umbraco.Cms.Core.Constants.WebhookEvents;
using AIC.Portal.Services.Abstractions;
using AIC.Portal.ViewModels;


namespace AIC.Portal.Services
{
    public class ContactUsFormService : IContactUsFormService
    {
        private readonly ContactFormContext _context;

        public ContactUsFormService(ContactFormContext context)
        {
            _context = context;
        }

		public List<governorates> governoratesDropDown()
		{
			var Governorates = _context.Governorates.ToList();
			return Governorates;
		}
		public List<type> typeDropDown()
		{
			var types = _context.Types.ToList();

			return types;
		}
		public List<Countries> countryDown()
		{
			var countries = _context.Countries.ToList();

			return countries;
		}
		
	}
}
