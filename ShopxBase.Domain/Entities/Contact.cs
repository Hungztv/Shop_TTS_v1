using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Domain.Entities
{
    public class Contact : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Map { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Address { get; set; }

        [Required]
        public string Description { get; set; }

        public string LogoImg { get; set; }

        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string Youtube { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}