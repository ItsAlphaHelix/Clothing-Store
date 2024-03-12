namespace Clothing_Store.Core.Services.HelperServices
{
    using Clothing_Store.Core.ViewModels.Products;
    using Clothing_Store.Core.ViewModels.Shared;
    public class FilterHelperService
    {
        protected static IQueryable<ProductViewModel> FilterProducts(PaginatedViewModel<ProductViewModel> model, IQueryable<ProductViewModel> products)
        {
            if (!string.IsNullOrWhiteSpace(model.SelectedProducts))
            {
                products = products.Where(x => model.SelectedProducts.Contains(x.Category));
            }


            if (!string.IsNullOrWhiteSpace(model.SelectedSizes))
            {
                string[] splitSelectedSizes = model.SelectedSizes.Split(",");
                products = products.Where(x => x.ProductSizes.Any(x => splitSelectedSizes.Contains(x.SizeName)));
            }

            if (!string.IsNullOrEmpty(model.SelectedPrice))
            {
                switch (model.SelectedPrice)
                {
                    case "5-15": products = products.Where(x => x.Price >= 5 && x.Price <= 15); break;
                    case "15-30": products = products.Where(x => x.Price >= 15 && x.Price <= 30); break;
                    case "30-50": products = products.Where(x => x.Price >= 30 && x.Price <= 50); break;
                    case "50-100": products = products.Where(x => x.Price >= 50 && x.Price <= 100); break;
                }
            }

            switch (model.Sorting)
            {
                case SortEnum.Default: products = products.AsQueryable(); break;
                case SortEnum.AverageRating: products = products.OrderByDescending(x => x.AverageRating); break;
                case SortEnum.PriceAsc: products = products.OrderBy(x => x.Price); break;
                case SortEnum.PriceDesc: products = products.OrderByDescending(x => x.Price); break;
            }

            return products;
        }
    }
}
