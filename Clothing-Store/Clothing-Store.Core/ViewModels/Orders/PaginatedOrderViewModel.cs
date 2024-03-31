namespace Clothing_Store.Views.Orders
{
    using Clothing_Store.Core.ViewModels.Customers;
    using Clothing_Store.Core.ViewModels.Shared;

    public class PaginatedOrderViewModel<T>
    {
        public PaginatedList<T> Models { get; set; }

        public CustomerViewModel CustomerModel { get; set; }
    }
}
