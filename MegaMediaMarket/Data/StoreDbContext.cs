using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SiteASPCOm.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SiteASPCOm.Data
{
    public class StoreDbContext : IdentityDbContext<ApplicationUser>
    {
        public StoreDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
