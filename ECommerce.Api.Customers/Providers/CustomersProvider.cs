using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDBContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDBContext dbContext,ILogger<CustomersProvider> logger,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            seeding();
        }

        private void seeding()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer { Id = 1, Name = "Sarah", Address = "152,custerd street,Washington" });
                dbContext.Customers.Add(new Db.Customer { Id = 2, Name = "George", Address = "356,custerd street,Washington" });
                dbContext.Customers.Add(new Db.Customer { Id = 3, Name = "Rajesh", Address = "982,Downtown,NY,Ny" });
                dbContext.Customers.Add(new Db.Customer { Id = 4, Name = "David", Address = "448,clove Ally,Musssori" });
                dbContext.Customers.Add(new Db.Customer { Id = 5, Name = "Fathima", Address = "8201,Chapman Femme,Paris" });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string Errormessage)> GetCustomerAsync(int CustomerId)
        {
            try
            {
                var Customer = await dbContext.Customers.FirstOrDefaultAsync(c => c.Id == CustomerId);
                if (Customer != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(Customer);
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

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string Errormessage)> GetCustomersAsync()
        {
            try
            {
                var Customers =await dbContext.Customers.ToListAsync();
               
                if (Customers!=null && Customers.Any())
                {
                  
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(Customers);
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
