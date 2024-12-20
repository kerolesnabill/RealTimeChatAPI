using Microsoft.EntityFrameworkCore;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data;

internal class RealTimeChatDbContext
    (DbContextOptions<RealTimeChatDbContext> options) : DbContext(options)
{
    internal DbSet<User> Users { get; set; }
    internal DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

