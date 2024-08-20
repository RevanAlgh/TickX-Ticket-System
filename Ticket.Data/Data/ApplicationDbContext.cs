using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Data.Models.Enums;

namespace Ticket.Data.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User 
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.Password).IsRequired().HasMaxLength(70);

                entity.HasMany(u => u.Tickets)
                      .WithOne(t => t.User)
                      .HasForeignKey(t => t.CreatedBy);

                entity.HasMany(u => u.Comments)
                      .WithOne(c => c.User)
                      .HasForeignKey(c => c.UserId);

                entity.HasIndex(u => u.Email)
                       .IsUnique();

                entity.HasIndex(u => u.UserName)
                       .IsUnique();

                entity.HasIndex(u => u.MobileNumber)
                       .IsUnique();

                entity.HasData(
                new User
                {
                    UserId = 1,
                    FullName = "Manager",
                    UserName ="Manager",
                    MobileNumber = "123-456-7890",
                    Email = "Manager@gmail.com",
                    DOB = new DateOnly(2000, 1, 11),
                    Address = "123 St, Riyadh, KSA",
                    UserImage = "",
                    Password = BCrypt.Net.BCrypt.HashPassword("Password123"),
                    IsActive = true,
                    Token = "",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Role = Roles.SupportManager
                });

            });

            // Product 
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.ProductId);

                entity.HasMany(p => p.Tickets)
                      .WithOne(t => t.Product)
                      .HasForeignKey(t => t.ProductId);

                entity.HasIndex(u => u.Name)
                      .IsUnique();

                entity.HasData(
                   new Product
                   { ProductId = 1, Name = "Mobile Phone" });

                entity.HasData(
                   new Product
                   { ProductId = 2, Name = "Computer Screen" });
            });

            // Ticket 
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.TicketId);
                entity.Property(t => t.TicketDescription).IsRequired().HasMaxLength(5000);
                entity.Property(t => t.Title).IsRequired();

                entity.HasMany(t => t.Comments)
                      .WithOne(c => c.Ticket)
                      .HasForeignKey(c => c.TicketId);

                entity.HasMany(t => t.Attachments)
                      .WithOne(a => a.Ticket)
                      .HasForeignKey(a => a.TicketId);
            });

            // Comment 
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.CommentId);
                entity.Property(c => c.Replie).IsRequired().HasMaxLength(5000);

            });

            // Attachment 
            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.HasKey(a => a.AttachmentId);

                entity.Property(a => a.FilePath).IsRequired();

                entity.HasOne(a => a.Ticket)
                      .WithMany(t => t.Attachments)
                      .HasForeignKey(a => a.TicketId);
            });

        }
    }
}