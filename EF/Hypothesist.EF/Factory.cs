using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hypothesist.EF;

public static class Factory
{
    public static IAsyncEnumerable<T> Use<T>(this Observer<T> observer, DbContext context) =>
        observer.Use(context, _ => true);
    
    public static IAsyncEnumerable<T> Use<T>(this Observer<T> observer, DbContext context, EntityState state) => 
        Use(observer, context, args => args.OldState == state);

    public static IAsyncEnumerable<T> Use<T>(this Observer<T> observer, DbContext context, Predicate<EntityStateChangedEventArgs> filter)
    {
        context.ChangeTracker.StateChanged += async (_, args) =>
        {
            if (args.Entry.Entity is T entity && filter(args)) await observer.Add(entity);
        };

        return observer;
    }

    public static IAsyncEnumerable<T> ObserverFor<T>(this DbContext context) =>
        new Observer<T>().Use(context, _ => true);
    
    public static IAsyncEnumerable<T> ObserverFor<T>(this DbContext context, EntityState state) =>
        new Observer<T>().Use(context, state);

    public static IAsyncEnumerable<T> ObserverFor<T>(this DbContext context, Predicate<EntityStateChangedEventArgs> filter) =>
        new Observer<T>().Use(context, filter);
}