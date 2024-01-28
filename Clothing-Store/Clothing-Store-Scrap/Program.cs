using Clothing_Store.Core.Scrapper;
using Clothing_Store.Data.Data;
using Clothing_Store.Data.Data.Models;
using Clothing_Store.Data.Repositories;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Runtime.CompilerServices;

namespace Clothing_Store_Scrap
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            DbContextOptions<ClothingStoreContext> options = new DbContextOptions<ClothingStoreContext>();
            ClothingContext context = new ClothingContext(options);
            ScrapProduct scrap = new ScrapProduct(context);
            await scrap.StartScrap();
        }
    }
}