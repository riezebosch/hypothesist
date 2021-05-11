using System;
using Prognosis.Theorems;

namespace Prognosis
{
    public static class Future 
    {
        public static ITheorem<T> Any<T>(Action<T> theorem) => 
            new Any<T>(theorem);

        public static ITheorem<T> All<T>(Action<T> theorem) =>
            new All<T>(theorem);
    }
}