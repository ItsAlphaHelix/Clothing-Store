using System.ComponentModel.DataAnnotations.Schema;

namespace Clothing_Store.Data.Data.Models
{
    public class ProductFavorites
    {
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;


        [ForeignKey(nameof(Favorite))]
        public int FavoriteId { get; set; }

        public Favorite Favorite { get; set; } = null!;
    }
}