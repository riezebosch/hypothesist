using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Hypothesist.Tests.Helpers;
using Xunit;

namespace Hypothesist.Tests.Hypothesis
{
    public class Each
    {
        [Fact]
        public async Task Empty() => 
            await Hypothesize
                .Each<string>(x => x == "a")
                .Validate(1.Seconds());
        
        [Fact]
        public async Task Match()
        {
            var hypothesis = Hypothesize
                .Each<string>(x => x == "a");

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
        }

        [Fact]
        public async Task Invalid()
        {
            var hypothesis = Hypothesize
                .Each<string>(y => y == "a");

            await hypothesis.Test("b");
            
            Func<Task> act = () => hypothesis.Validate(1.Seconds());
            var ex = await act
                .Should()
                .ThrowAsync<InvalidException<string>>();

            ex.Which
                .Matched
                .Should()
                .BeEmpty();
            
            ex.Which
                .Unmatched
                .Should()
                .BeEquivalentTo("b");
        }
        
        [Fact]
        public async Task Subsequent()
        {
            var hypothesis = Hypothesize
                .Each<string>(y => y == "a");
            
            await hypothesis.Test("a");
            await hypothesis.Test("b");

            Func<Task> act = () => hypothesis.Validate(1.Seconds());
            var ex = await act
                .Should()
                .ThrowAsync<InvalidException<string>>();

            ex.Which
                .Matched
                .Should()
                .BeEquivalentTo("a");

            ex.Which
                .Unmatched
                .Should()
                .BeEquivalentTo("b");
        }
        
        [Fact]
        public async Task Sliding()
        {
            var hypothesis = Hypothesize
                .Each<string>(y => y == "a");

            Func<Task> act = () => Task.WhenAll(
                hypothesis.TestSlowly("a", "a", "a", "a", "b"), 
                hypothesis.Validate(2.Seconds()));
            await act
                .Should()
                .ThrowAsync<InvalidException<string>>();
        }
        
        [Fact]
        public async Task Cancel()
        {
            using var tcs = new CancellationTokenSource(5.Seconds());
            var hypothesis = Hypothesize
                .Each<string>(_ => true);

            await hypothesis.Validate(20.Minutes(), tcs.Token);
        }
    }
}