using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Db
{
    public class CustomersDBContext:DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public CustomersDBContext(DbContextOptions options):base(options)
        {

        }
    }
}
