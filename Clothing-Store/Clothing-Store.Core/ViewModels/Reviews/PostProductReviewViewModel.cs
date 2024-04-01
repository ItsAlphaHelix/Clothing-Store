namespace Clothing_Store.Core.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;
    public class PostProductReviewViewModel
    {
        public int ProductId { get; set; }

        public string UserFullName { get; set; } = null!;

        public int Rating { get; set; }

        [Required(ErrorMessage = "Вашето ревю не може да бъде празно.")]
        [MinLength(10, ErrorMessage = "Вашето ревю трябва да има минимум 10 символа.")]
        public string Message { get; set; } = null!;

        public string? UserProfileImageUrl { get; set; }
    }
}
