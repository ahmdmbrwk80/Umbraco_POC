using System.ComponentModel.DataAnnotations;

namespace AIC.Portal.Entities
{
    public class Countries
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CountryName { get; set; }
        public string CountryNameAR { get; set; }

        public virtual ICollection<governorates> Governorates { get; set; }
        //public virtual ICollection<ContactForm> ContactForms { get; set; }

    }
}
