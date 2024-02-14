namespace Clothing_Store.Data.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    public class Cart
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }


        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public Product Product{ get; set; }
    }
}
