using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NRDCL.Models;
using NRDCL.Models.Acc;

namespace NRDCL.Data
{
    public class NRDCL_DB_Context : IdentityDbContext<ApplicationUser>
    {
        public NRDCL_DB_Context(DbContextOptions<NRDCL_DB_Context> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customer_Table { get; set; }
        public DbSet<Site> Site_Table { get; set; }

        public DbSet<Product> Product_Table { get; set; }
        public DbSet<Deposit> Deposit_Table { get; set; }

        public DbSet<Order> Order_Table { get; set; }
    }
}
