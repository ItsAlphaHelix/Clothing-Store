namespace Clothing_Store.Core.ViewModels.Orders
{
    using Clothing_Store.Core.ViewModels.Bags;
    using Clothing_Store.Core.ViewModels.Customers;

    public  class CheckoutViewModel
    {
        public CustomerViewModel CustomerModel { get; set; }

        public IEnumerable<ProductBagViewModel> ProductsInBag { get; set; } = new List<ProductBagViewModel>();

    }
}
