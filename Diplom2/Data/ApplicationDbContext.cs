using Diplom2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Diplom2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Collection>()
                .HasOne(p => p.ApplicationUser)
                .WithMany(t => t.Collections)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasOne(p => p.Collection)
                .WithMany(t => t.Items)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Number>()
                .HasOne(p => p.Collection)
                .WithMany(t => t.Numbers)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Line>()
                .HasOne(p => p.Collection)
                .WithMany(t => t.Lines)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Text>()
                .HasOne(p => p.Collection)
                .WithMany(t => t.Texts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Date>()
                .HasOne(p => p.Collection)
                .WithMany(t => t.Dates)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Logical>()
                .HasOne(p => p.Collection)
                .WithMany(t => t.Logicals)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ValueNumber>()
                .HasOne(p => p.Item)
                .WithMany(t => t.ValueNumbers)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ValueLine>()
                .HasOne(p => p.Item)
                .WithMany(t => t.ValueLines)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ValueText>()
                .HasOne(p => p.Item)
                .WithMany(t => t.ValueTexts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ValueDate>()
                .HasOne(p => p.Item)
                .WithMany(t => t.ValueDates)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ValueLogical>()
                .HasOne(p => p.Item)
                .WithMany(t => t.ValueLogicals)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TagItem>()
                .HasOne(p => p.Item)
                .WithMany(t => t.Tags)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Item)
                .WithMany(t => t.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasOne(p => p.Item)
                .WithMany(t => t.Likes)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasKey(x => x.ItemId);
            modelBuilder.Entity<Tag>()
                .HasKey(x => x.TagId);
            modelBuilder.Entity<TagItem>()
                .HasKey(x => new { x.ItemId, x.TagId });
            modelBuilder.Entity<TagItem>()
                .HasOne(x => x.Item)
                .WithMany(m => m.Tags)
                .HasForeignKey(x => x.ItemId);
            modelBuilder.Entity<TagItem>()
                .HasOne(x => x.Tag)
                .WithMany(e => e.Items)
                .HasForeignKey(x => x.TagId);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Number> Numbers { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Logical> Logicals { get; set; }
        public DbSet<ValueNumber> ValueNumbers { get; set; }
        public DbSet<ValueLine> ValueLines { get; set; }
        public DbSet<ValueText> ValueTexts { get; set; }
        public DbSet<ValueDate> ValueDates { get; set; }
        public DbSet<ValueLogical> ValueLogicals { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        
    }
}
