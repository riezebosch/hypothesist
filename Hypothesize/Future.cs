using System;
using System.Threading.Channels;
using Hypothesize.Observers;

namespace Hypothesize
{
    public static class Future 
    {
        public static IExperiment<T> Any<T>(Action<T> assert) => 
            new Experiment<T>(new Any<T>(assert), Channel.CreateUnbounded<T>());

        public static IExperiment<T> All<T>(Action<T> assert) => 
            new Experiment<T>(new All<T>(assert), Channel.CreateUnbounded<T>());

        public static IExperiment<T> First<T>(Action<T> assert) => 
            new Experiment<T>(new First<T>(assert), Channel.CreateUnbounded<T>());

        public static IExperiment<T> Single<T>(Action<T> assert) => 
            new Experiment<T>(new Single<T>(assert), Channel.CreateUnbounded<T>());
    }
}