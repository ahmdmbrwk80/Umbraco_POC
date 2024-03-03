using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIC.Portal.Entities
{
    public class governorates
    {
        [Key]
        [Required]
        public int Id { get; set; }
		[Required]
		public byte province_id { get; set; }
		[Required]
        public string GovernorateNameAr { get; set; }
        [Required]
		public string GovernorateNameEn { get; set; }
        
        [ForeignKey("CountriesId")]
        public Countries country { get; set; }
        public virtual ICollection<ContactForm> ContactForms { get; set; }




    }
}
