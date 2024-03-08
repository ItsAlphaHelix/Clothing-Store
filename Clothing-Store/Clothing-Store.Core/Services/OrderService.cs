namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Bags;
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Globalization;

    public class OrderService : IOrderService
    {
        private readonly IRepository<Customer> customersRepository;
        private readonly IRepository<ProductBag> productBagRepository;
        private readonly IRepository<Order> ordersRepository;
        private readonly IRepository<OrderProduct> orderProductsRepository;

        public OrderService(
            IRepository<Customer> customersRepository,
            IRepository<ProductBag> productBagRepository,
            IRepository<Order> ordersRepository,
            IRepository<OrderProduct> orderProductsRepository)
        {
            this.customersRepository = customersRepository;
            this.productBagRepository = productBagRepository;
            this.ordersRepository = ordersRepository;
            this.orderProductsRepository = orderProductsRepository;
        }

        public async Task<CompletedOrderViewModel> CompletedOrderAsync(string userId)
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

        public async Task CreateOrderAsync(CustomerViewModel orderModel, string userId)
        {
            var productOrders = await GetProductOrdersAsync(userId);

            var customer = await GetOrCreateCustomerAsync(orderModel, userId);

            UpdateCustomerInformation(orderModel, customer);

            var orderNumber = GetNumericOrderNumber();

            var order = new Order()
            {
                CustomerId = customer.CustomerId,
                Customer = customer,
                OrderDate = DateTime.Now,
                OrderNumber = orderNumber,
                OrderProducts = productOrders
            };

            customer.Orders.Add(order);

            await customersRepository.SaveChangesAsync();
        }
        private string GetNumericOrderNumber()
        {
            var guid = Guid.NewGuid();
            var bytes = guid.ToByteArray();

            long numericOrderNumber = BitConverter.ToInt64(bytes, 0);
            return numericOrderNumber.ToString()[..10];
        }
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

        private async Task<Customer> GetOrCreateCustomerAsync(CustomerViewModel orderModel, string userId)
        {
            var customer = await this.customersRepository
                .All()
                .FirstOrDefaultAsync(x => x.CustomerId == userId);

            if (customer == null)
            {
                customer = new Customer()
                {
                    CustomerId = userId,
                    FirstName = orderModel.FirstName,
                    LastName = orderModel.LastName,
                    Address = orderModel.Address,
                    City = orderModel.City,
                    CityPinCode = orderModel.CityPinCode,
                    Email = orderModel.Email,
                    Phone = orderModel.Phone,
                    Region = orderModel.Region,
                    IsInformationSaved = orderModel.IsInformationSaved
                };

                await customersRepository.AddAsync(customer);
            }

            return customer;
        }

        private void UpdateCustomerInformation(CustomerViewModel orderModel, Customer customer)
        {
            customer.FirstName = orderModel.FirstName;
            customer.LastName = orderModel.LastName;
            customer.Address = orderModel.Address;
            customer.City = orderModel.City;
            customer.CityPinCode = orderModel.CityPinCode;
            customer.Email = orderModel.Email;
            customer.Phone = orderModel.Phone;
            customer.Region = orderModel.Region;
            customer.IsInformationSaved = orderModel.IsInformationSaved;
        }

        public async Task<MineOrdersViewModel> GetCustomerWithHisOrdersAsync(string userId)
        {
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
                    CategoryName = x.CategoryName,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    SizeName = x.SizeName,
                    TotalPrice = x.Price * x.Quantity
                }).AsQueryable();

            return products;
        }
    }
}
