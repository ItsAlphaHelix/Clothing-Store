namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Globalization;

    public class OrdersService : IOrdersService
    {
        private readonly IRepository<Customer> customersRepository;
        private readonly IRepository<ProductBag> productBagRepository;
        private readonly IRepository<Order> ordersRepository;
        private readonly IRepository<OrderProduct> orderProductsRepository;
        private readonly UserManager<ApplicationUser> usersManager;

        public OrdersService(
            IRepository<Customer> customersRepository,
            IRepository<ProductBag> productBagRepository,
            IRepository<Order> ordersRepository,
            IRepository<OrderProduct> orderProductsRepository,
            UserManager<ApplicationUser> usersManager)
        {
            this.customersRepository = customersRepository;
            this.productBagRepository = productBagRepository;
            this.ordersRepository = ordersRepository;
            this.orderProductsRepository = orderProductsRepository;
            this.usersManager = usersManager;

        }

        public async Task<CompletedOrderViewModel> GetCurrentUserOrderAsync(string userId)
        {
            var userOrder = await this.ordersRepository
                .AllAsNoTracking()
                .Where(x => x.CustomerId == userId)
                .OrderByDescending(x => x.OrderDate)
                .Select(x => new CompletedOrderViewModel()
                {
                    FirstName = x.Customer.FirstName,
                    LastName = x.Customer.LastName,
                    Address = x.Customer.Address,
                    City = x.Customer.City,
                    CityPinCode = x.Customer.CityPinCode,
                    Email = x.Customer.Email,
                    Phone = x.Customer.Phone,
                    Region = x.Customer.Region,
                    OrderDate = x.OrderDate.ToString("MM/dd/yyyy. HH:mm", CultureInfo.InvariantCulture),
                    OrderNumber = x.OrderNumber,
                    ProductOrderModel = x.OrderProducts.Select(x => new ProductOrderViewModel()
                    {
                        CategoryName = x.CategoryName,
                        ImageUrl = x.ImageUrl,
                        Price = x.Price,
                        TotalPrice = x.TotalPrice,
                        Quantity = x.Quantity,
                        SizeName = x.SizeName
                    })
                })
                .FirstOrDefaultAsync();

            return userOrder;
        }

        public async Task<CustomerViewModel> SaveInformationAboutCustomerForNextTime(CustomerViewModel customer, string userId)
        {
                var currentCustomer = await this.customersRepository
                    .All()
                    .Where(x => x.CustomerId == userId)
                    .FirstOrDefaultAsync();

                var user = await usersManager.FindByIdAsync(userId);

            if (currentCustomer != null && currentCustomer.IsInformationSaved == true)
            {

                customer.IsInformationSaved = currentCustomer.IsInformationSaved;
                customer.FirstName = currentCustomer.FirstName;
                customer.LastName = currentCustomer.LastName;
                customer.City = currentCustomer.City;
                customer.Region = currentCustomer.Region;
                customer.CityPinCode = currentCustomer.CityPinCode;
                customer.Phone = currentCustomer.Phone;
                customer.Address = currentCustomer.Address;
                customer.Email = currentCustomer.Email;

               return customer;
            }

            return null;
        }


        public async Task<MineOrdersViewModel> GetCustomerWithHisOrdersAsync(string userId)
        {

            var customer = await this
                .customersRepository
                .AllAsNoTracking()
                .Where(x => x.CustomerId == userId)
                .Select(x => new CustomerViewModel()
                {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email,
                        Phone = x.Phone,
                })
                .FirstOrDefaultAsync();

            var orders = this.ordersRepository
                .AllAsNoTracking()
                .Where(x => x.CustomerId == userId)
                .Select(x => new OrderViewModel()
                {
                    NumberOfOrder = x.OrderNumber,
                    OrderDate = x.OrderDate.ToString("MM/dd/yyyy. HH:mm", CultureInfo.InvariantCulture),
                    TotalPrice = x.OrderProducts.Sum(x => (x.Price * x.Quantity) + 5)
                })
                .AsQueryable();


            if (customer == null || !orders.Any())
            {
                throw new NullReferenceException("Вие все още нямате поръчки.");
            }

            var mineOrderModel = new MineOrdersViewModel()
            {
                OrdersModel = orders,
                CustomerModel = customer
            };

            return mineOrderModel;
        }

        public IQueryable<ProductOrderViewModel> GetProductsInOrderAsQueryable(string numberOfOrder)
        {
            var products = this.orderProductsRepository
                .AllAsNoTracking()
                .Where(x => x.Order.OrderNumber == numberOfOrder)
                .Select(x => new ProductOrderViewModel()
                {
                    Id = x.Id,
                    CategoryName = x.CategoryName,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    SizeName = x.SizeName,
                    TotalPrice = x.Price * x.Quantity
                }).AsQueryable();

            return products;
        }

        public async Task CreateOrderAsync(CustomerViewModel newCustomer, string userId)
        {
            var productOrders = await GetProductOrdersAsync(userId);

            var customer = await GetOrCreateCustomerAsync(newCustomer, userId);

            UpdateCustomerInformation(newCustomer, customer);

            var orderNumber = GenerateOrderNumber();

            var order = new Order()
            {
                CustomerId = customer.CustomerId,
                Customer = customer,
                OrderDate = DateTime.Now,
                OrderNumber = orderNumber,
                OrderProducts = productOrders
            };

            customer.Orders.Add(order);

            await DeleteBagAsync(userId);

            await customersRepository.SaveChangesAsync();
        }

        /// <summary>
        /// When user's create successfully order, his bag should be deleted.
        /// </summary>
        /// <param name="userId">getting user id to to find his products in bag</param>
        /// <returns></returns>
        private async Task DeleteBagAsync(string userId)
        {
            var productsInBag = await this.productBagRepository
                            .All()
                            .Where(x => x.Bag.UserId == userId)
                            .ToListAsync();

            foreach (var productInBag in productsInBag)
            {
                productBagRepository.Delete(productInBag);
            }
        }

        /// <summary>
        /// Generating order number.
        /// </summary>
        /// <returns></returns>
        private static string GenerateOrderNumber()
        {
            var guid = Guid.NewGuid();
            var bytes = guid.ToByteArray();

            long numericOrderNumber = BitConverter.ToInt64(bytes, 0);
            return numericOrderNumber.ToString()[..10];
        }

        /// <summary>
        /// Getting product orders as async method.
        /// </summary>
        /// <param name="userId">user's id helps to find the products</param>
        /// <returns>Method returns collection of order products.</returns>
        private async Task<List<OrderProduct>> GetProductOrdersAsync(string userId)
        {
            return await this.productBagRepository
                .All()
                .Where(x => x.Bag.UserId == userId)
                .Select(x => new OrderProduct()
                {
                    CategoryName = x.Product.Category,
                    ImageUrl = x.Product.Images.Select(img => img.Url).FirstOrDefault(),
                    Quantity = x.Quantity,
                    Price = x.Product.Price,
                    TotalPrice = x.Product.Price * x.Quantity,
                    SizeName = x.SizeName
                })
                .ToListAsync();
        }

        /// <summary>
        /// If the customer doesn't exists, he should be created. If he exists, he should be only returned.
        /// </summary>
        /// <param name="customerModel">The order model comes from model binding to bind the customer data into customer object.</param>
        /// <param name="userId">User's id helps to find the customer.</param>
        /// <returns>Method returns current customer</returns>
        private async Task<Customer> GetOrCreateCustomerAsync(CustomerViewModel customerModel, string userId)
        {
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
                        IsInformationSaved = customerModel.IsInformationSaved
                    };
                      await customersRepository.AddAsync(customer);
                }
            

            return customer;
        }

        /// <summary>
        /// The method helps to update customer's information.
        /// </summary>
        /// <param name="newCustomer">The new customer comes from model binding and bind new data to the old data.</param>
        /// <param name="oldCustomer">Old customer accepts new data.</param>
        private static void UpdateCustomerInformation(CustomerViewModel newCustomer, Customer oldCustomer)
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
                LastName = userLastName
            };

            var result = await this.SaveInformationAboutCustomerForNextTime(customer, userId);

            result ??= customer;

            return result;
        }

        public async Task<bool> IsCustomerHasOrdersAsync(string userId)
        {
            var customer = await this.customersRepository.AllAsNoTracking().AnyAsync(x => x.CustomerId == userId);

            if (customer)
            {
                return true;
            }

            return false;
        }
    }
}
