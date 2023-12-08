using Microsoft.EntityFrameworkCore;

namespace Hypothesist.EF.Tests.DB;

public class TestContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Other> Others { get; set; }
}