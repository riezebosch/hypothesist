using System;
using System.Threading.Tasks;

namespace Hypothesist.Tests.Helpers;

internal static class Test
{
    public static async Task TestSlowly<T>(this IHypothesis<T> future, params T[] items)
    {
        foreach (var item in items)
        {
            await future.Test(item);
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}