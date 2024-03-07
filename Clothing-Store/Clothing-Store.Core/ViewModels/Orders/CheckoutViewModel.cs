namespace Clothing_Store.Core.ViewModels.Orders
{
    using Clothing_Store.Core.ViewModels.Bags;
    public  class CheckoutViewModel
    {
        public CustomerViewModel OrderModel { get; set; }

        public IEnumerable<BagViewModel> ProductsInBag { get; set; } = new List<BagViewModel>();

    }
}
