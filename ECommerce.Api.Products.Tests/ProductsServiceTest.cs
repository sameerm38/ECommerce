using System;
using Xunit;
using ECommerce.Api.Products.Providers;
using ECommerce.Api.Products.Db;
using Microsoft.EntityFrameworkCore;
using ECommerce.Api.Products.Profiles;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductsReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("GetProductsReturnsAllProducts")
                .Options;
            var dbContext = new ProductsDBContext(options);
            CreateProducts(dbContext);
            var productsProfile = new ProductProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = config.CreateMapper();
            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var product =await productsProvider.GetProductsAsync();
            Assert.True(product.IsSuccess);
            Assert.True(product.products.Any());
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async Task GetProductsReturnsProductUsingValidId()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("GetProductsReturnsProductUsingValidId")
                .Options;
            var dbContext = new ProductsDBContext(options);
            CreateProducts(dbContext);
            var productsProfile = new ProductProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = config.CreateMapper();
            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var product = await productsProvider.GetProductAsync(1);
            Assert.True(product.IsSuccess);
            Assert.NotNull(product.product);
            Assert.True(product.product.ID == 1);
            Assert.Null(product.ErrorMessage);
        }

        [Fact]
        public async Task GetProductsReturnsProductUsingInValidId()
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase("GetProductsReturnsProductUsingInValidId")
                .Options;
            var dbContext = new ProductsDBContext(options);
            CreateProducts(dbContext);
            var productsProfile = new ProductProfile();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(productsProfile));
            var mapper = config.CreateMapper();
            var productsProvider = new ProductsProvider(dbContext, null, mapper);
            var product = await productsProvider.GetProductAsync(-1);
            Assert.False(product.IsSuccess);
            Assert.Null(product.product);
            
            Assert.NotNull(product.ErrorMessage);
        }

        private void CreateProducts(ProductsDBContext dbContext)
        {
            for (int i = 1; i <= 10; i++)
            {
                dbContext.Products.Add(new Product()
                {
                    ID = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i * 10,
                    Price = (decimal)(i * 3.14)
                }) ;

            }
            dbContext.SaveChanges();
        }
    }
}
