namespace Clothing_Store.Core.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;
    public class PostProductReviewViewModel
    {
        public int ProductId { get; set; }

        [Required]
        [MinLength(3)]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;

        public int Rating { get; set; }

        [Required]
        [MinLength(5)]
        public string Message { get; set; } = null!;
    }
}
