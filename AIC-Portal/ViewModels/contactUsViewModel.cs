using AIC.Portal.Entities;
using System.ComponentModel.DataAnnotations;

namespace AIC.Portal.ViewModels
{
    public class contactUsViewModel
    {
        [Required]
        [MaxLength(8)]
        public string FullName { get; set; }//done
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [MaxLength(11, ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string work { get; set; }
        [Required]
        public string Message { get; set; }
        public string? lang { get; set; }
        
    }
}
