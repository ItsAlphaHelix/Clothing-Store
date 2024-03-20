namespace Clothing_Store.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    public class ProductFavorites
    {
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; } = null!;


        [ForeignKey(nameof(Favorite))]
        public int FavoriteId { get; set; }

        public virtual Favorite Favorite { get; set; } = null!;

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}