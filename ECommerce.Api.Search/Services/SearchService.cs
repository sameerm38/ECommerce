using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService ordersService,IProductsService productsService,ICustomersService customersService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int CustomerId)
        {
            var customerResult = await customersService.GetCustomerAsync(CustomerId);
            var ordersResult = await ordersService.GetOrdersAsync(CustomerId);
            var productsResult = await productsService.GetProductsAsync();
            if (ordersResult.IsSuccess)
            {
                foreach(var order in ordersResult.Orders)
                {
                    foreach(var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess? 
                            productsResult.Products.FirstOrDefault(p => p.ID == item.ProductId)?.Name:
                            "Product information is not available";
                    }
                }
                return (true, new
                {
                    Customer = customerResult.IsSuccess ?
                    customerResult.Customer:
                    new Models.Customer{Name="Customer Infomation is not available" },
                    Orders = ordersResult.Orders
                });
            }
            return (false, null);
        }
    }
}
