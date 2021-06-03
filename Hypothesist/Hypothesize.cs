using Hypothesist.Builders;
using Hypothesist.Observers;

namespace Hypothesist
{
    public static class Hypothesize 
    {
        public static IStatement<T> Any<T>() => 
            new Statement<T>(new AtLeast<T>(1));

        public static IStatement<T> Each<T>() => 
            new Statement<T>(new Each<T>());
        
        public static IStatement<T> First<T>() => 
            new Statement<T>(new First<T>());

        public static IStatement<T> Single<T>() => 
            new Statement<T>(new Exactly<T>(1));

        public static IStatement<T> Exactly<T>(int occurrences) =>
            new Statement<T>(new Exactly<T>(occurrences));

        public static IStatement<T> AtLeast<T>(int occurrences) =>
            new Statement<T>(new AtLeast<T>(occurrences));
    }
}