using ECommerce.Api.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider productsProvider;

        public ProductsController(IProductsProvider productsProvider)
        {
            this.productsProvider = productsProvider;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            var Result = await productsProvider.GetProductsAsync();
            if (Result.IsSuccess)
            {
                return Ok(Result.products);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var Result = await productsProvider.GetProductAsync(id);
            if (Result.IsSuccess)
            {
                return Ok(Result.product);
            }
            return NotFound();
        }
    }
}
