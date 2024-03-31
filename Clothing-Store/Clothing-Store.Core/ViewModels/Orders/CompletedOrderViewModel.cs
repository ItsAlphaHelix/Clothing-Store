using Clothing_Store.Core.ViewModels.Customers;

namespace Clothing_Store.Core.ViewModels.Orders
{
    public class CompletedOrderViewModel : CustomerViewModel
    {
        public string OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public IEnumerable<ProductOrderViewModel> ProductOrderModel { get; set; } = new List<ProductOrderViewModel>();
    }
}
