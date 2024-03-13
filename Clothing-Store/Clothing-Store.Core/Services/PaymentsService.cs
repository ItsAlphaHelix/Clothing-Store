namespace Clothing_Store.Core.Services
{
    using Clothing_Store.Core.Contracts;
    using Stripe.Checkout;
    using System.Threading.Tasks;

    public class PaymentsService : IPaymentsService
    {
        private readonly IOrdersService ordersService;

        public PaymentsService(IOrdersService ordersService)
        {
            this.ordersService = ordersService;
        }
        public async Task<string> CreateCheckoutSessionAsync(string userId)
        {
            string baseUrl = "https://localhost:44312/";
            string successUrl = $"{baseUrl}Orders/CompletedOrder";
            string cancelUrl = $"{baseUrl}Orders/PaymentFailed";

            var options = new SessionCreateOptions()
            {
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            var completedOrder = await this.ordersService.GetCurrentUserOrderAsync(userId);

            foreach (var product in completedOrder.ProductOrderModel)
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
                        Name = "Shipping",
                    }
                },
                Quantity = 1
            };
            options.LineItems.Add(shippingLineItem);

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return session.Url;
        }
    }
}
