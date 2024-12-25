using Microsoft.EntityFrameworkCore;

namespace RealTimeChatAPI.Data.Seeders;

internal class Seeder(RealTimeChatDbContext dbContext)
{
    public async Task Seed()
    {
        if(dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}
