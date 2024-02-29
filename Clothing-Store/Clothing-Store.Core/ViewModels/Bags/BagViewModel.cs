namespace Clothing_Store.Core.ViewModels.Bags
{
    public class BagViewModel
    {
        public int BagId { get; set; }

        public IEnumerable<ProductBagViewModel> ProductBags { get; set; } = new List<ProductBagViewModel>();
    }
}
