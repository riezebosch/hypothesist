using System;
using Hypothesist.Observers;

namespace Hypothesist
{
    public static class Hypothesize 
    {
        public static IHypothesis<T> Any<T>(Predicate<T> match) => 
            new Hypothesis<T>(new AtLeast<T>(match, 1));

        public static IHypothesis<T> Each<T>(Predicate<T> match) => 
            new Hypothesis<T>(new Each<T>(match));
        
        public static IHypothesis<T> First<T>(Predicate<T> match) => 
            new Hypothesis<T>(new First<T>(match));

        public static IHypothesis<T> Single<T>(Predicate<T> match) => 
            new Hypothesis<T>(new Exactly<T>(match, 1));

        public static IHypothesis<T> Exactly<T>(Predicate<T> match, int occurrences) =>
            new Hypothesis<T>(new Exactly<T>(match, occurrences));

        public static IHypothesis<T> AtLeast<T>(Predicate<T> match, int occurrences) =>
            new Hypothesis<T>(new AtLeast<T>(match, occurrences));
    }
}