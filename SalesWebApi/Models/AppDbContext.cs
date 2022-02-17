using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebApi.Models {
    public class AppDbContext : DbContext { // inherits our DbContext to use its class and data within

        public virtual DbSet<Customer> Customers { get; set; } // creates an instance of our Customer class,
                                                               // and identifying it as Customers for the DbContext to use
        public virtual DbSet<Order> Orders { get; set; }
        //default constructor not needed
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } // creates an instance and gets the options from DbContext parent,
                                                                                        // and sets it up for us to modify
        
        protected override void OnModelCreating(ModelBuilder builder) {

        }

    }
}
