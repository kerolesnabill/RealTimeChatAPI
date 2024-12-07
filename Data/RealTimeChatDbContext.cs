using Microsoft.EntityFrameworkCore;
using RealTimeChatAPI.Models;

namespace RealTimeChatAPI.Data;

internal class RealTimeChatDbContext
    (DbContextOptions<RealTimeChatDbContext> options) : DbContext(options)
{
    internal DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

