namespace Clothing_Store.Core.ViewModels.Orders
{
    public class OrderViewModel
    {
        public string NumberOfOrder { get; set; }

        public string OrderDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string StripePaymentStatus { get; set; }
    }
}
