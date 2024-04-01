using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.ViewModels.Reviews
{
    public class GetProductReviewViewModel
    {
        public int ProductId { get; set; }

        public string UserFullName { get; set; } = null!;

        public double Rating { get; set; }

        public string Message { get; set; } = null!;

        public DateTime Date { get; set; }

        public string? UserProfileImageUrl { get; set; }
    }
}
