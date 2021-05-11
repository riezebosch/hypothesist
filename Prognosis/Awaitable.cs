using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Prognosis
{
    public static class Awaitable
    {
        public static TaskAwaiter GetAwaiter(this ITheorem theorem) =>
            theorem.Within(TimeSpan.FromSeconds(60), CancellationToken.None).GetAwaiter();
        
        public static Task Forever(this ITheorem theorem, CancellationToken token = default) =>
            theorem.Within(TimeSpan.FromMilliseconds(-1), token);
    }
}