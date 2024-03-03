using AIC.Portal.Entities;

namespace AIC.Portal.ViewModels
{
    public class DropDownViewModel
{
        // add id for the county
        public List<type> DropDownTypes { get; set; }
        public List<Countries> countries { get; set; }
        public List<governorates> Governorates { get; set; }
    }
}
