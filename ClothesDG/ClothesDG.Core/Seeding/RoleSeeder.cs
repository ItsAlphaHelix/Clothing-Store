﻿using ClothesDG.Data.Data;
using ClothesDG.Data.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesDG.Core.Seeding
{
    public class RoleSeeder : ISeeder
    {
        public async Task SeedAsync(ClothesDGContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await SeedRoleAsync(roleManager, "Admin");
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
