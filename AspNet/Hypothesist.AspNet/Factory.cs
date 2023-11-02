namespace Hypothesist.AspNet;

public static class Factory
{
    public static Test<T> Test<T>(this IHypothesis<T> hypothesis) => new(hypothesis);
}