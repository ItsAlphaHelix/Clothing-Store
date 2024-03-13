namespace Clothing_Store.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    public class Customer
    {
        [Key]
        [Required]
        public string CustomerId { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string CityPinCode { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Region { get; set; } = null!;

        [Required]
        [Phone]
        public string Phone { get; set; } = null!;

        public bool IsInformationSaved { get; set; }

        public bool IsCustomerWantsToPayOnline { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
