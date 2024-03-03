using AIC.Portal.Entities;
using AIC.Portal.ViewModels;

namespace AIC.Portal.Services.Abstractions
{
    public interface IContactUsFormService
    {
		public List<governorates> governoratesDropDown();
		public List<type> typeDropDown();
		public List<Countries> countryDown();
       


    }
    

}