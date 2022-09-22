using Kvitto.Core.Entities;
using Kvitto.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kvitto.Data
{
    public class KvittoApiContext : IdentityDbContext<ApplicationUser, IdentityRole,
string>

    {
        public KvittoApiContext(DbContextOptions<KvittoApiContext> options)
            : base(options)
        {
        }

        public DbSet<Receipt> Receipt { get; set; } = default!;
        public DbSet<UploadedFile> UploadedFile { get; set; } = default!;

        //TODO There is already inherited DbSet for ApplicationUser called Users
        //public DbSet<ApplicationUser> ApplicationUser => Set<ApplicationUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .Property<DateTime>("TimeOfRegistration")
                .HasDefaultValueSql("GetDate()"); ;
        }
    }
}