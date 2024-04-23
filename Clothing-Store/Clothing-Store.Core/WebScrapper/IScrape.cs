using Clothing_Store.Core.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.WebScrapper
{
    public interface IScrape
    {
        Task ScrapeProduct(ProductInputViewModel model);
    }
}
