using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;
using TAuth.ResourceAPI.Entities;

namespace TAuth.ResourceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            InitAsync(host).Wait();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task InitAsync(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var dbContext = provider.GetRequiredService<ResourceContext>();
                var userManager = provider.GetRequiredService<UserManager<AppUser>>();

                var isInit = !(await dbContext.Database.GetAppliedMigrationsAsync()).Any();

                await dbContext.Database.MigrateAsync();

                if (isInit)
                {
                    var user1 = new AppUser
                    {
                        UserName = "trung.tran",
                        FirstName = "Trung",
                        LastName = "Tran"
                    };
                    var user2 = new AppUser
                    {
                        UserName = "bob",
                        FirstName = "Bob",
                        LastName = "Corn"
                    };

                    await userManager.CreateAsync(user1, "123123");
                    await userManager.CreateAsync(user2, "bob");

                    await dbContext.Resources.AddRangeAsync(
                        new ResourceEntity
                        {
                            Name = "Sample Resource 1",
                            UserId = user1.Id,
                        },
                        new ResourceEntity
                        {
                            Name = "Sample Resource 2",
                            UserId = user2.Id
                        });

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
