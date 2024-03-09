namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Data.Data.Models;

    public interface IOrderService
    {
        /// <summary>
        /// The method helps to create order async.
        /// </summary>
        /// <param name="customerModel">Customer model comes from model binder, with his help you can create the customer.</param>
        /// <param name="userId">The user's id helps to find current customer in the app.</param>
        /// <returns></returns>
        public Task CreateOrderAsync(CustomerViewModel newCustomer, string userId);

        /// <summary>
        /// The method helps to getting current user's order.
        /// </summary>
        /// <param name="userId">The user's id helps to find the user's order.</param>
        /// <returns>Returns </returns>
        public Task<CompletedOrderViewModel> GetCurrentUserOrderAsync(string userId);

        /// <summary>
        /// The method helps to saves the information about the customer for the next time.
        /// </summary>
        /// <param name="customer">The customer comes from model binder to bind the new data to the old.</param>
        /// <param name="userId">The user's id helps to find the current customer in the app.</param>
        /// <returns></returns>
        public Task<CustomerViewModel> SaveInformationAboutCustomerForNextTime(CustomerViewModel customer, string userId);

        /// <summary>
        /// The method helps to getting current customer with his orders async.
        /// </summary>
        /// <param name="userId">THe user's id helps to getting the customer's orders async.</param>
        /// <returns></returns>
        public Task<MineOrdersViewModel> GetCustomerWithHisOrdersAsync(string userId);

        /// <summary>
        /// The method helps to getting all products in order as queryable.
        /// </summary>
        /// <param name="numberOfOrder">The number's of order helps to getting current order.</param>
        /// <returns></returns>
        public IQueryable<ProductOrderViewModel> GetProductsInOrderAsQueryable(string numberOfOrder);


        public Task<CustomerViewModel> TakeInformationAboutLoggedInCustomerAsync(string userId);
    }
}
