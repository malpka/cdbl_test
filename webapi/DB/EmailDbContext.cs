using System;
using Microsoft.EntityFrameworkCore;
using webapi.Domain;

namespace webapi.Models
{
    public class EmailDbContext : DbContext
    {
        public EmailDbContext(DbContextOptions<EmailDbContext> options) : base(options) { }

        public DbSet<Email> Emails { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Email>(email =>
            {
                email.HasKey(e => e.Id);
                email.Property(e => e.Description).IsRequired();
                email.HasMany(e => e.Attachments).WithOne(a => a.Email);
            });

            modelBuilder.Entity<Attachment>(attachment =>
            {
                attachment.HasKey(t => t.Id);
                attachment.Property(t => t.Name).IsRequired();
                attachment.Property(t => t.Content).IsRequired();
                attachment.HasOne(t => t.Email).WithMany(s => s.Attachments);
            });

        }
    }
}