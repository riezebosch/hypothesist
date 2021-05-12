using System;
using System.Threading.Channels;
using Hypothesize.Observers;

namespace Hypothesize
{
    public static class Future 
    {
        public static IExperiment<T> Any<T>(Action<T> assert)
        {
            var channel = Channel.CreateUnbounded<T>();
            return new Experiment<T>(new Any<T>(assert, channel.Reader), channel.Writer);
        }

        public static IExperiment<T> All<T>(Action<T> assert)
        {
            var channel = Channel.CreateUnbounded<T>();
            return new Experiment<T>(new All<T>(assert, channel.Reader), channel.Writer);
        }
    }
}