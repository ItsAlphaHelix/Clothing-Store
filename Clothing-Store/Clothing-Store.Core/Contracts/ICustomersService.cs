﻿namespace Clothing_Store.Core.Contracts
{
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Data.Data.Models;
    public interface ICustomersService
    {
        Task<CustomerViewModel> TakeInformationAboutLoggedInCustomerAsync(string userId);

        Task<bool> IsCustomerHasOrdersAsync(string userId);

        Task ChangeCustomerPaymentMethodAsync(CustomerViewModel customerModel, string userId);

        void UpdateCustomerInformation(CustomerViewModel newCustomer, Customer oldCustomer);

        Task<CustomerViewModel> SaveInformationAboutCustomerForNextTimeAsync(CustomerViewModel customer, string userId);

        Task<Customer> GetOrCreateCustomerAsync(CustomerViewModel customerModel, string userId);

    }
}
