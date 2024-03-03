using System.ComponentModel.DataAnnotations;

namespace AIC.Portal.ViewModels
{
    public class NewsletterSubscriberViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
