namespace Clothing_Store.Core.Services
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using Clothing_Store.Core.Contracts;
    using Clothing_Store.Data.Data.Models;
    using Clothing_Store.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Stripe;
    using Stripe.Checkout;
    using System.Threading.Tasks;

    public class PaymentsService : IPaymentsService
    {
        private readonly IBagsService bagService;
        private readonly IRepository<Order> ordersRepository;
        public PaymentsService(IBagsService bagService,
            IRepository<Order> ordersRepository)
        {
            this.bagService = bagService;
            this.ordersRepository = ordersRepository;

        }
        public async Task<Session> CreateCheckoutSessionAsync(string userId)
        {
            string baseUrl = "https://localhost:44312/";
            string successUrl = $"{baseUrl}Orders/OrderConfirmation";
            string cancelUrl = $"{baseUrl}Orders/PaymentFailed";

            var options = new SessionCreateOptions()
            {
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            var products = this.bagService.GetAllProductsInBagAsQueryable(userId);

                foreach (var product in products)
                {
                    var sessionListItem = new SessionLineItemOptions()
                    {
                        PriceData = new SessionLineItemPriceDataOptions()
                        {
                            UnitAmountDecimal = product.Price * 100,
                            Currency = "bgn",
                            ProductData = new SessionLineItemPriceDataProductDataOptions()
                            {
                                Images = new List<string> { product.ImageUrl },
                                Name = product.CategoryName

                            }
                        },
                        Quantity = product.Quantity
                    };

                    options.LineItems.Add(sessionListItem);
                }
            

            decimal shipping = 5.00M;

            var shippingLineItem = new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmountDecimal = shipping * 100,
                    Currency = "bgn",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = "Доставка",
                    }
                },
                Quantity = 1
            };

            options.LineItems.Add(shippingLineItem);

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return session;
        }

        public async Task RefundAsync(string orderNumber)
        {
            var order = await this.ordersRepository
                .All()
                .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);


            if (order.StripePaymentStatus == "paid")
            {
                var options = new RefundCreateOptions()
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = order.StripePaymentIntendId
                };

                var service = new RefundService();
                Refund refund = await service.CreateAsync(options);
            }

            order.StripePaymentStatus = "refunded";
            await ordersRepository.SaveChangesAsync();
        }
    }
}
