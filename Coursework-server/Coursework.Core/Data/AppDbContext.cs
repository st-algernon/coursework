using Coursework.Core.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Coursework.Core.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserItem> UsersItems { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldType> FieldTypes { get; set; }
        public DbSet<FieldItem> FieldsItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FieldItem>()
                .HasKey(t => new { t.ItemId, t.FieldId });

            modelBuilder.Entity<FieldItem>()
                .HasOne(fi => fi.Field)
                .WithMany(f => f.FieldItems)
                .HasForeignKey(fi => fi.FieldId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FieldItem>()
                .HasOne(fi => fi.Item)
                .WithMany(i => i.ItemFields)
                .HasForeignKey(fi => fi.ItemId);


            modelBuilder.Entity<UserItem>()
                .HasKey(t => new { t.UserId, t.ItemId });

            modelBuilder.Entity<UserItem>()
                .HasOne(ui => ui.User)
                .WithMany(u => u.UserItems)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserItem>()
                .HasOne(ui => ui.Item)
                .WithMany(i => i.ItemUsers)
                .HasForeignKey(ui => ui.ItemId);
        }
    }
}
