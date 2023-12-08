using Hypothesist.EF.Tests.DB;
using Microsoft.EntityFrameworkCore;

namespace Hypothesist.EF.Tests;

public class Tests
{
    [Fact]
    public async Task Use()
    {
        // Arrange
        await using var context = await TestContext();
        var observer = Observer
            .For<Item>()
            .Use(context);

        // Act
        context.Items.Add(new Item(0));
        await context.SaveChangesAsync();

        // Assert
        await Hypothesis
            .On(observer)
            .Timebox(2.Seconds())
            .Any()
            .Match(new Item(1))
            .Validate();
    }
    
    [Fact]
    public async Task ObserverFor()
    {
        // Arrange
        await using var context = await TestContext();
        var observer = context
            .ObserverFor<Item>();

        // Act
        context.Items.Add(new Item(0));
        await context.SaveChangesAsync();

        // Assert
        await Hypothesis
            .On(observer)
            .Timebox(2.Seconds())
            .Any()
            .Match(new Item(1))
            .Validate();
    }
    
    [Fact]
    public async Task ObserverForFilter()
    {
        // Arrange
        await using var context = await TestContext();
        var observer = context
            .ObserverFor<Item>(a => a.OldState == EntityState.Added);
        
        // Act
        context.Items.Add(new Item(0));
        await context.SaveChangesAsync();

        // Assert
        await Hypothesis
            .On(observer)
            .Timebox(2.Seconds())
            .Any()
            .Match(new Item(1))
            .Validate();
    }

    [Fact]
    public async Task Multiple()
    {
        // Arrange
        await using var context = await TestContext();
        var a = Observer
            .For<Item>()
            .Use(context);
        var b = Observer
            .For<Other>()
            .Use(context);
        
        // Act
        context.Items.Add(new Item(0));
        context.Others.Add(new Other(0));
        await context.SaveChangesAsync();
        
        // Assert
        await Hypothesis
            .On(a)
            .Timebox(2.Seconds())
            .First()
            .Match(new Item(1))
            .Validate();
        
        await Hypothesis
            .On(b)
            .Timebox(2.Seconds())
            .First()
            .Match(new Other(1))
            .Validate();
    }

    [Fact]
    public async Task ObserverForState()
    {
        // Arrange
        await using var context = await TestContext();
        var observer = context
            .ObserverFor<Item>(EntityState.Deleted);

        // Act
        var entity = new Item(0);
        context.Items.Add(entity);
        await context.SaveChangesAsync();
        
        context.Items.Remove(entity);
        await context.SaveChangesAsync();

        // Assert
        await Hypothesis
            .On(observer)
            .Timebox(2.Seconds())
            .Any()
            .Match(new Item(1))
            .Validate();
    }

    private static async Task<TestContext> TestContext()
    {
        var context = new TestContext(new DbContextOptionsBuilder().UseSqlite("Filename=test.db").Options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
        
        return context;
    }
}