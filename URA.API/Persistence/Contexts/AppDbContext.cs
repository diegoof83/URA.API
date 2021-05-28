using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URA.API.Domain.Models;

namespace URA.API.Persistence.Contexts
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users
        {
            get { return Set<User>(); }
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    //Users
        //    new UserMap(modelBuilder.Entity<User>());
        //}

        //public class UserMap
        //{
        //    public UserMap(EntityTypeBuilder<User> entityBuilder)
        //    {
        //        entityBuilder.ToTable("User");
        //        entityBuilder.HasKey(p => p.Id);
        //        entityBuilder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        //        entityBuilder.Property(p => p.Name).IsRequired().HasMaxLength(30);
        //        entityBuilder.Property(p => p.Email).IsRequired().HasMaxLength(50);
        //        entityBuilder.Property(p => p.Password).IsRequired().HasMaxLength(12);
        //    }
        //}
    }
}
