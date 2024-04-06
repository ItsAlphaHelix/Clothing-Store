namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Customers;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Identity.Client;
    using System.Threading.Tasks;

    public class CustomersService : ICustomersService
    {
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly IRepository<Customer> customersRepository;

        public CustomersService(
            UserManager<ApplicationUser> usersManager,
            IRepository<Customer> customersRepository)
        {
            this.usersManager = usersManager;
            this.customersRepository = customersRepository;
        }
        public async Task<CustomerViewModel> TakeInformationAboutLoggedInCustomerAsync(string userId)
        {
            var user = await usersManager.FindByIdAsync(userId);

            string userFirstName = user.FirstName;
            string userLastName = user.LastName;

            var customer = new CustomerViewModel()
            {
                Email = user.Email,
                Phone = user.PhoneNumber,
                FirstName = userFirstName,
                LastName = userLastName,
            };

            var result = await this.SaveInformationAboutCustomerForNextTimeAsync(customer, userId);

            result ??= customer;

            return result;
        }

        public async Task<bool> IsCustomerHasOrdersAsync(string userId)
        {
            var isCustomerHasOrders = await this.customersRepository
                .AllAsNoTracking()
                .Where(x => x.CustomerId == userId)
                .AnyAsync(x => x.Orders.Any(x => x.StripePaymentStatus != "refunded"));

            if (isCustomerHasOrders)
            {
                return true;
            }

            return false;
        }

        public void UpdateCustomerInformation(CustomerViewModel newCustomer, Customer oldCustomer)
        {
            oldCustomer.FirstName = newCustomer.FirstName;
            oldCustomer.LastName = newCustomer.LastName;
            oldCustomer.Address = newCustomer.Address;
            oldCustomer.City = newCustomer.City;
            oldCustomer.CityPinCode = newCustomer.CityPinCode;
            oldCustomer.Email = newCustomer.Email;
            oldCustomer.Phone = newCustomer.Phone;
            oldCustomer.Region = newCustomer.Region;
            oldCustomer.IsInformationSaved = newCustomer.IsInformationSaved;
            oldCustomer.IsCustomerWantsToPayOnline = newCustomer.IsCustomerWantsToPayOnline;
        }

        public async Task<CustomerViewModel> SaveInformationAboutCustomerForNextTimeAsync(CustomerViewModel customerModel, string userId)
        {
            var currentCustomer = await this.customersRepository
                .All()
                .Where(x => x.CustomerId == userId)
                .FirstOrDefaultAsync();

            var user = await usersManager.FindByIdAsync(userId);

            if (currentCustomer != null && currentCustomer.IsInformationSaved == true)
            {

                customerModel.IsInformationSaved = currentCustomer.IsInformationSaved;
                customerModel.IsCustomerWantsToPayOnline = currentCustomer.IsCustomerWantsToPayOnline;
                customerModel.FirstName = currentCustomer.FirstName;
                customerModel.LastName = currentCustomer.LastName;
                customerModel.City = currentCustomer.City;
                customerModel.Region = currentCustomer.Region;
                customerModel.CityPinCode = currentCustomer.CityPinCode;
                customerModel.Phone = currentCustomer.Phone;
                customerModel.Address = currentCustomer.Address;
                customerModel.Email = currentCustomer.Email;

                return customerModel;
            }

            return new CustomerViewModel();
        }

        public async Task<Customer> GetOrCreateCustomerAsync(CustomerViewModel customerModel, string userId)
        {
            var user = await usersManager.FindByIdAsync(userId);
            var customer = await this.customersRepository
                .All()
                .FirstOrDefaultAsync(x => x.CustomerId == userId);
            if (customer == null)
            {
                customer = new Customer()
                {
                    CustomerId = userId,
                    FirstName = customerModel.FirstName,
                    LastName = customerModel.LastName,
                    Address = customerModel.Address,
                    City = customerModel.City,
                    CityPinCode = customerModel.CityPinCode,
                    Email = customerModel.Email,
                    Phone = customerModel.Phone,
                    Region = customerModel.Region,
                    IsInformationSaved = customerModel.IsInformationSaved,
                    IsCustomerWantsToPayOnline = customerModel.IsCustomerWantsToPayOnline,                   

                };
                await customersRepository.AddAsync(customer);
            }

            return customer;
        }
    }
}
