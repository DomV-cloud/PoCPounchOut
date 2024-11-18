using System.ComponentModel.DataAnnotations;

namespace Template.Domain.Entities
{
    public class Order : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string OrderNumber { get; set; } = null!;

        public User Customer { get; set; } = null!;

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public ICollection<OrderItem> OrderItems{ get; set; } = [];

        [Required]
        public double TotalPrice { get; set; }
    }
}
