using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Voidwell.UserManagement.Data
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=voidwell.auth;Username=localdev;Password=localdev;Pooling=true");

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}
