using MyKitchen.Data;
using MyKitchen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;

namespace MyKitchen.Tests
{
    public class SharedTestContext : IDisposable
    {
        public SharedTestContext()
        {
                
                var services = new ServiceCollection();

                services.AddLogging();
                
                services.AddIdentity<ApplicationUser, Microsoft.AspNetCore.Identity.IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer( WithTestDatabase.MyConnectionString));
                services.AddTransient<EFFoodItemRepository>();
                services.AddTransient<IFoodEventRepository,EFFoodEventRepository>();


                var sp = services.BuildServiceProvider();

                TestFoodItemRepository = sp.GetService<EFFoodItemRepository>();
                TestUserManager = sp.GetService<UserManager<ApplicationUser>>();
                TestFoodEventRepository = sp.GetService<IFoodEventRepository>();

                ApDbContext = sp.GetService<ApplicationDbContext>();


        }

        public EFFoodItemRepository TestFoodItemRepository { get; private set; }
        public UserManager<ApplicationUser> TestUserManager { get; private set; }
        public IFoodEventRepository TestFoodEventRepository { get; private set; }
        public ApplicationDbContext ApDbContext { get; private set; }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

    }
}
