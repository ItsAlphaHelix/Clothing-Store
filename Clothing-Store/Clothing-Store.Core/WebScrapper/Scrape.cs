using Clothing_Store.Core.ViewModels.Products;
using Clothing_Store.Data.Data.Models;
using Clothing_Store.Data.Repositories;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text;
using System.Text.RegularExpressions;

namespace Clothing_Store.Core.WebScrapper
{
    public class Scrape : IScrape
    {
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<Size> sizesRepository;
        public Scrape(IRepository<Product> productsRepository, IRepository<Size> sizesRepository)
        {
            this.productsRepository = productsRepository;
            this.sizesRepository = sizesRepository;
        }
        public async Task ScrapeProduct(ProductInputViewModel model)
        {
            IWebDriver driver = new ChromeDriver();

            var url = $"https://www.lcwaikiki.bg/bg-BG/BG/product/LC-WAIKIKI/%D0%B4%D0%B0%D0%BC%D1%81%D0%BA%D0%B8/%D0%9F%D1%83%D0%BB%D0%BE%D0%B2%D0%B5%D1%80/{model.ProductId}/{model.ProductColorId}";
            driver.Navigate().GoToUrl(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(driver.PageSource);

            Product product = new Product();

            product.IsMale = model.IsMale;
            product.LCProductId = model.ProductId;
            product.LCProductColorId = model.ProductColorId;

            StringBuilder sb = new StringBuilder();

            var h1ProductTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='product-title seo']");
            if (h1ProductTitle != null)
            {
                product.Category = h1ProductTitle.InnerText.Trim().Split(" ").LastOrDefault();
            }

            var divPrice = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='price']");
            var divDiscountPrice = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='basket-discount']");
            var divResultPrice = divDiscountPrice != null ? divDiscountPrice : divPrice;

            string priceText = divResultPrice.InnerText.Split(" ").FirstOrDefault();

            product.Price = decimal.Parse(priceText);


            var sizeOptions = htmlDocument.DocumentNode.SelectNodes("//div[@class='option-size']/a");

            var sizes = await this.sizesRepository.All().ToListAsync();

            foreach (var sizeOption in sizeOptions)
            {
                string dataStock = sizeOption.GetAttributeValue("data-stock", " ");
                string sizeName = sizeOption.InnerText;
                int count = int.Parse(dataStock);
                var size = sizes.Where(x => x.Name == sizeName).FirstOrDefault();

                if (size != null)
                {
                    var productSizes = new ProductSize()
                    {
                        Product = product,
                        ProductId = product.Id,
                        SizeId = size.Id,
                        Size = size,
                        Count = count
                    };

                    product.ProductSizes.Add(productSizes);
                }
            }

            var divRows = htmlDocument.DocumentNode.SelectNodes("//div[@class='col-xs-12']")[4];
            var ul = divRows.SelectNodes("//ul")[3];
            var lis = ul.SelectNodes(".//li");

            string description = string.Empty;

            foreach (var li in lis)
            {
                description = description + "\n" + li.InnerText.Trim();
            }

            var div = htmlDocument.DocumentNode.SelectNodes("//div[@class='col-xs-12']")[8];
            var pElement = div.SelectNodes("//p")[4].InnerText.Trim();

            var divSeoDetail = htmlDocument.DocumentNode.SelectNodes("//div[@class='seo-detail']");

            foreach (var seo in divSeoDetail)
            {
                string text = seo.InnerText.Replace("\n", " ").Replace("\r", " ").Trim();
                text = Regex.Replace(text, @"\s+", " ");
                sb.AppendLine(text);
            }

            product.Description = sb.ToString();

            var divPanels2 = htmlDocument.DocumentNode.SelectNodes("//div[@class='panel']")[1];
            var divPanelBody = divPanels2.SelectSingleNode(".//div[@class='panel-body']");

            if (divPanelBody != null)
            {
                sb.Clear();
                var trs = divPanelBody.SelectNodes(".//table[@class='table']/tbody/tr");

                foreach (var tr in trs)
                {
                    string clearInfo = tr.InnerText.Trim();
                    sb.AppendLine(clearInfo);
                }

                string pattern = "&#176;С";
                string clearInfoResult = Regex.Replace(sb.ToString(), pattern, string.Empty);
                product.ClearInfo = clearInfoResult;
            }
            else
            {
                await Console.Out.WriteLineAsync("Doesn't exist");
            }

            var divNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='col-xs-6']");
            if (divNodes != null)
            {
                foreach (var divNode in divNodes)
                {
                    var imageNodes = divNode.SelectNodes(".//img[@data-src]");
                    foreach (var imgNode in imageNodes)
                    {
                        var dataSrc = imgNode.GetAttributeValue("data-src", string.Empty);
                        product.Images.Add(new Image { Url = dataSrc });
                    }
                }
            }

            await productsRepository.AddAsync(product);
            await productsRepository.SaveChangesAsync();
        }
    }
}
