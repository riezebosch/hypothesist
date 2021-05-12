using System;
using System.Threading.Tasks;

namespace Hypothesize.Tests.Helpers
{
    internal static class Test
    {
        public static async Task Slowly<T>(this IHypothesis<T> future, params T[] items)
        {
            foreach (var item in items)
            {
                await future.Test(item);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}