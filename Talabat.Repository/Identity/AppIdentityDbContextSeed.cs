using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Yusuf Cim",
                    Email = "YusufCim@gmail.com",
                    UserName = "YusufCim",
                    PhoneNumber = "01025254262"
                };
                await userManager.CreateAsync(user, "Abc@12");
            }
        }
    }
}
