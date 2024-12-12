using Microsoft.EntityFrameworkCore;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data;

internal class RealTimeChatDbContext
    (DbContextOptions<RealTimeChatDbContext> options) : DbContext(options)
{
    internal DbSet<User> Users { get; set; }
    internal DbSet<Message> Messages { get; set; }
    internal DbSet<MessageStatus> MessageStatus { get; set; }
    internal DbSet<Chat> Chats { get; set; }
    internal DbSet<ChatUser> ChatUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Message
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Restrict);

        // MessageStatus
        modelBuilder.Entity<MessageStatus>()
            .HasOne(ms => ms.Message)
            .WithMany(m => m.MessageStatuses)
            .HasForeignKey(ms => ms.MessageId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MessageStatus>()
            .HasOne(ms => ms.User)
            .WithMany(u => u.MessageStatuses)
            .HasForeignKey(ms => ms.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ChatUsers
        modelBuilder.Entity<ChatUser>()
            .HasKey(cu => new { cu.UserId, cu.ChatId });

        modelBuilder.Entity<ChatUser>()
            .HasOne(cu => cu.User)
            .WithMany(u => u.ChatUsers)
            .HasForeignKey(cu => cu.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatUser>()
            .HasOne(cu => cu.Chat)
            .WithMany(c => c.ChatUsers)
            .HasForeignKey(cu => cu.ChatId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

