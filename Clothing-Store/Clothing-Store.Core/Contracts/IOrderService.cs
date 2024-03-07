namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Orders;
    public interface IOrderService
    {
        public Task CreateOrderAsync(CustomerViewModel orderModel, string userId);

        public Task<CompletedOrderViewModel> CompletedOrderAsync(string userId);

        public Task<CustomerViewModel> SaveInformationAboutCustomerForNextTime(CustomerViewModel customer, string userId);
    }
}
