namespace Clothing_Store.Data.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
