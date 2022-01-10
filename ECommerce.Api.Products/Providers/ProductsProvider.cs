using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductsProvider : IProductsProvider
    {
        private readonly ProductsDBContext dbContext;
        private readonly ILogger<ProductsProvider> logger;
        private readonly IMapper mapper;

        public ProductsProvider(ProductsDBContext dbContext,ILogger<ProductsProvider> logger,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            seeding();
        }

        private void seeding()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Db.Product { ID = 1, Name = "Keyboard", Price = 10, Inventory = 2000 });
                dbContext.Products.Add(new Db.Product { ID = 2, Name = "Mouse", Price = 5, Inventory = 5000 });
                dbContext.Products.Add(new Db.Product { ID = 3, Name = "Monitor", Price = 150, Inventory = 200 });
                dbContext.Products.Add(new Db.Product { ID = 4, Name = "CPU", Price = 200, Inventory = 350 });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Product> products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var Products = await dbContext.Products.ToListAsync();
                if(Products!=null && Products.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(Products);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception Ex)
            {
                logger?.LogError(Ex.ToString());
                return (false, null, Ex.Message);
               
            }
        }

        public async Task<(bool IsSuccess, Models.Product product, string ErrorMessage)> GetProductAsync(int ProductId)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.ID == ProductId);
                if (product != null)
                {
                    var result = mapper.Map<Db.Product, Models.Product>(product);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception Ex)
            {

                logger?.LogError(Ex.ToString());
                return (false, null, Ex.Message);
            }
        }
    }
}
