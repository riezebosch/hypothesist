using Hypothesist.Builders;
using Hypothesist.Observers;

namespace Hypothesist
{
    public static class Hypothesize 
    {
        public static IStatement<T> Any<T>() => 
            new Statement<T>(new Any<T>());

        public static IStatement<T> All<T>() => 
            new Statement<T>(new All<T>());
        
        public static IStatement<T> First<T>() => 
            new Statement<T>(new First<T>());

        public static IStatement<T> Single<T>() => 
            new Statement<T>(new Single<T>());

        public static IStatement<T> Exactly<T>(int occurrences) =>
            new Statement<T>(new Exactly<T>(occurrences));

        public static IStatement<T> AtLeast<T>(int occurrences) =>
            new Statement<T>(new AtLeast<T>(occurrences));
    }
}