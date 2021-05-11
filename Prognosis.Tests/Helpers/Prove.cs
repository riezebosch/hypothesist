using System;
using System.Threading.Tasks;

namespace Prognosis.Tests.Helpers
{
    internal static class Prove
    {
        public static async Task Slowly<T>(this ITheorem<T> future, params T[] items)
        {
            foreach (var item in items)
            {
                await future.Prove(item);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
}