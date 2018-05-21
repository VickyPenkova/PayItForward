﻿namespace PayItForward.Data
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using PayItForward.Data.Models;

    public class PayItForwardDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public PayItForwardDbContext(DbContextOptions<PayItForwardDbContext> options)

        : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Donation> Donations { get; set; }

        public virtual DbSet<Story> Stories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();

           optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
