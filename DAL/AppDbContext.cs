using System;
using System.Collections.Generic;
using Laboris.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Laboris.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Blog> Blogs { get; set; } = null!;
        public DbSet<Products> Products { get; set; } = null!;
        public DbSet<Customers> Customers { get; set; } = null!;
        public DbSet<BlogCategory> BlogCategories { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<ProductsCategory> ProductsCategories { get; set; } = null!;
        public DbSet<BasketItem> BasketItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;


       



    }

}

