using System.ComponentModel.DataAnnotations;

namespace ShopxBase.Domain.Entities
{
    public class Slider : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        [Required]
        public string Description { get; set; }

        public string Link { get; set; }

        public int DisplayOrder { get; set; }

        public int Status { get; set; }

        // Business Methods
        public bool IsActive()
        {
            return Status == 1;
        }
    }
}