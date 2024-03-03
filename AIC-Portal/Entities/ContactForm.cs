using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIC.Portal.Entities
{
    public class ContactForm
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(80)]
        public string FullName { get; set; }//done
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        [Required]
        [ForeignKey("typeId")]
        public type Type { get; set; }
        //[Required]
        //public Countries Country { get; set; }
        [Required]
        [ForeignKey("governoratesId")]
        public governorates city { get; set; }
        [Required]
        public string work { get; set; }
        [Required]
        public string Message { get; set; }
    }
    }
