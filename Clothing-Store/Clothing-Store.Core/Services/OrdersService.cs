namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Core.ViewModels.Customers;
    using Clothing_Store.Core.ViewModels.Orders;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Globalization;

    public class OrdersService : IOrdersService
    {
        private readonly ICustomersService customersService;
        private readonly IRepository<ProductBag> productBagRepository;
        private readonly IRepository<Customer> customersRepository;
        private readonly IRepository<Order> ordersRepository;
        private readonly IRepository<OrderProduct> orderProductsRepository;

        public OrdersService(
            ICustomersService customersService,
            IRepository<ProductBag> productBagRepository,
            IRepository<Order> ordersRepository,
            IRepository<Customer> customersRepository,
            IRepository<OrderProduct> orderProductsRepository)
        {
            this.customersService = customersService;
            this.productBagRepository = productBagRepository;
            this.ordersRepository = ordersRepository;
            this.customersRepository = customersRepository;
            this.orderProductsRepository = orderProductsRepository;

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
                    ProductOrderModel = x.OrderProducts
                    .Where(x => x.Quantity != 0)
                    .Select(x => new ProductOrderViewModel()
                    {
                        CategoryName = x.CategoryName,
                        ImageUrl = x.ImageUrl,
                        Price = x.Price,
                        TotalPrice = x.TotalPrice,
                        Quantity = x.Quantity,
                        SizeName = x.SizeName
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return userOrder;
        }

        public IQueryable<OrderViewModel> GetCustomerOrdersAsQueryable(string userId)
        {
            var orders = this.ordersRepository
                .AllAsNoTracking()
                .Where(x => x.CustomerId == userId && x.StripePaymentStatus != "refunded")
                .OrderByDescending(x => x.OrderDate)
                .Select(x => new OrderViewModel()
                {
                    NumberOfOrder = x.OrderNumber,
                    OrderDate = x.OrderDate.ToString("MM/dd/yyyy. HH:mm", CultureInfo.InvariantCulture),
                    TotalPrice = x.OrderProducts.Sum(x => (x.Price * x.Quantity) + 5),
                    StripePaymentStatus =  x.StripePaymentStatus 
                })
                .AsQueryable();


            if (!orders.Any())
            {
                throw new NullReferenceException("Вие все още нямате поръчки.");
            }

            var mineOrderModel = new MineOrdersViewModel()
            {
                OrdersModel = orders,
            };

            return orders;
        }

        public IQueryable<ProductOrderViewModel> GetProductsInOrderAsQueryable(string numberOfOrder)
        {
            var products = this.orderProductsRepository
                .AllAsNoTracking()
                .Where(x => x.Order.OrderNumber == numberOfOrder && x.Quantity != 0)
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

        public async Task CreateOrderAsync
            (CustomerViewModel newCustomer,
            string userId,
            string stripePaymentStatus,
            string stripeSessionId,
            string stripePaymentSessionIntendId)
        {
            var productOrders = await GetAllOrdersWithTheirProductsAsync(userId);

            var customer = await this.customersService.GetOrCreateCustomerAsync(newCustomer, userId);

            this.customersService.UpdateCustomerInformation(newCustomer, customer);

            var orderNumber = GenerateOrderNumber();

            var order = new Order()
            {
                CustomerId = customer.CustomerId,
                Customer = customer,
                OrderDate = DateTime.Now,
                OrderNumber = orderNumber,
                StripePaymentStatus = stripePaymentStatus,
                StripeSessionId = stripeSessionId,
                StripePaymentIntendId = stripePaymentSessionIntendId,
                OrderProducts = productOrders
            };

            customer.Orders.Add(order);

            await customersRepository.SaveChangesAsync();
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
        private async Task<List<OrderProduct>> GetAllOrdersWithTheirProductsAsync(string userId)
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
    }
}
