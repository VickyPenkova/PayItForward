﻿namespace PayItForward.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using PayItForward.Data;
    using PayItForward.Web.Models;
    using PayItForward.Web.Services;
    using PayItForwardDbmodels = PayItForward.Data.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PayItForwardDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<PayItForwardDbmodels.User, IdentityRole>()
                .AddEntityFrameworkStores<PayItForwardDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Migrate() method is called on each startup to take the database to the latest available migration.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Migrate and seed the database during startup. Must be synchronous.
            // try
            // {
            //    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
            //    .CreateScope())
            //    {
            //        serviceScope.ServiceProvider.GetService<PayItForwardDbContext>().Database.EnsureCreated();
            //    }
            // }
            // catch (Exception ex)
            // {
            //    ex.Source = "Failed to migrate or seed database";
            // }
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
