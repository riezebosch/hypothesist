using System;
using Hypothesist.Observers;

namespace Hypothesist
{
    public static class Hypothesize
    {
        public static IHypothesis<T> Any<T>(Predicate<T> match) =>
            new Hypothesis<T>().Any(match);
        public static IHypothesis<T> Any<T>(this IHypothesis<T> hypothesis, Predicate<T> match) =>
            hypothesis.Add(new AtLeast<T>(match, 1));

        public static IHypothesis<T> Each<T>(Predicate<T> match) =>
            new Hypothesis<T>().Each(match);
        public static IHypothesis<T> Each<T>(this IHypothesis<T> hypothesis, Predicate<T> match) =>
            hypothesis.Add(new Each<T>(match));
        
        public static IHypothesis<T> First<T>(Predicate<T> match) => 
            new Hypothesis<T>().First(match);
        public static IHypothesis<T> First<T>(this IHypothesis<T> hypothesis, Predicate<T> match) =>
            hypothesis.Add(new First<T>(match));
        public static IHypothesis<T> Single<T>(Predicate<T> match) => 
            new Hypothesis<T>().Single(match);

        public static IHypothesis<T> Single<T>(this IHypothesis<T> hypothesis, Predicate<T> match) =>
            hypothesis.Add(new Exactly<T>(match, 1));
        public static IHypothesis<T> Exactly<T>(Predicate<T> match, int occurrences) =>
            new Hypothesis<T>().Exactly(match, occurrences);

        public static IHypothesis<T> Exactly<T>(this IHypothesis<T> hypothesis, Predicate<T> match, int occurrences) =>
            hypothesis.Add(new Exactly<T>(match, occurrences));

        public static IHypothesis<T> AtLeast<T>(Predicate<T> match, int occurrences) =>
            new Hypothesis<T>().AtLeast(match, occurrences);

        public static IHypothesis<T> AtLeast<T>(this IHypothesis<T> hypothesis, Predicate<T> match, int occurrences) =>
            hypothesis.Add(new AtLeast<T>(match, occurrences));

        public static IHypothesis<T> Any<T>() =>
            new Hypothesis<T>().Any();

        private static IHypothesis<T> Any<T>(this IHypothesis<T> hypothesis) =>
            hypothesis.Any(_ => true);
    }
}