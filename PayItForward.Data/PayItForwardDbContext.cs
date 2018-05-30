namespace PayItForward.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using PayItForward.Data.Models;

    public class PayItForwardDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public PayItForwardDbContext(DbContextOptions<PayItForwardDbContext> options)

        : base(options)
        {
        }

        public PayItForwardDbContext()
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Donation> Donations { get; set; }

        public virtual DbSet<Story> Stories { get; set; }
    }
}
