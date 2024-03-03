using System.ComponentModel.DataAnnotations;

namespace AIC.Portal.ViewModels
{
    public class SearchViewModel
    {
        [Required]
        public string? Query { get; set; }
        public Guid? SearchableTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set;}
	}
}
