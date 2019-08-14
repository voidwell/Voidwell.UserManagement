using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Voidwell.UserManagement.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("devsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<UserDbContext>();

            var connectionString = configuration.GetValue<string>("DBConnectionString");

            builder.UseNpgsql(connectionString, o =>
            {
                o.CommandTimeout(7200);
            });

            return new UserDbContext(builder.Options);
        }
    }
}
