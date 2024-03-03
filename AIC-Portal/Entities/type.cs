using System.ComponentModel.DataAnnotations;

namespace AIC.Portal.Entities
{
    public class type
    {
		[Key]
		public int Id { get; set; }
		[Required]
		public string Type { get; set; }
        public string TypeAR { get; set; }
        public virtual ICollection<ContactForm> ContactForms { get; set; }



    }
}
