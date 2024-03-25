using System.Net.Http.Headers;
using Clothing_Store.Core.ViewModels.Reviews;

namespace Clothing_Store.Core.ViewModels.Products
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string ClearInfo { get; set; }

        public bool IsMale { get; set; }

        public int CountOfReviews { get; set; }

        public double FiveStarts { get; set; }

        public double FourStarts { get; set; }

        public double ThreeStars { get; set; }

        public double TwoStars { get; set; }

        public double OneStar { get; set; }

        public double AverageRating { get; set; }

        public double PercentageOfAverageStars { get; set; }

        public bool IsProductInStock { get; set; }

        public IEnumerable<ProductViewModel> RecommendedProducts = new List<ProductViewModel>();

        public IEnumerable<GetProductReviewViewModel> Reviews { get; set; } = new List<GetProductReviewViewModel>();

        public IEnumerable<SizeViewModel> Sizes { get; set; } = new List<SizeViewModel>();

        public IEnumerable<SizeViewModel> ProductSizes { get; set; } = new List<SizeViewModel>();

        public IEnumerable<string> Images { get; set; } = new List<string>();
    }
}
