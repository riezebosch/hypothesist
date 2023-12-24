namespace Hypothesist.Tests.Helpers;

internal static class Test
{
    public static async Task AddSlowly<T>(this Observer<T> observer, params T[] items)
    {
        foreach (var item in items)
        {
            await observer.Add(item);
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}