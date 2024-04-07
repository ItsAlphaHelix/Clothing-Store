namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Customers;
    using Clothing_Store.Core.ViewModels.Orders;

    public interface IOrdersService
    {
        /// <summary>
        /// The method helps to create order async.
        /// </summary>
        /// <param name="customerModel">Customer model comes from model binder, with his help you can create the customer.</param>
        /// <param name="userId">The user's id helps to find current customer in the app.</param>
        /// <returns></returns>
            Task CreateOrderAsync(
            CustomerViewModel newCustomer,
            string userId,
            string stripePaymentStatus = null,
            string stripeSessionId = null,
            string stripePaymentSessionIntendId = null);

        /// <summary>
        /// The method helps to getting current user's order.
        /// </summary>
        /// <param name="userId">The user's id helps to find the user's order.</param>
        /// <returns>Returns </returns>
        Task<CompletedOrderViewModel> GetCurrentUserOrderAsync(string userId);


        /// <summary>
        /// The method helps to getting all products in order as queryable.
        /// </summary>
        /// <param name="numberOfOrder">The number's of order helps to getting current order.</param>
        /// <returns></returns>
        IQueryable<ProductOrderViewModel> GetProductsInOrderAsQueryable(string numberOfOrder);

        IQueryable<OrderViewModel> GetCustomerOrdersAsQueryable(string userId);
    }
}
