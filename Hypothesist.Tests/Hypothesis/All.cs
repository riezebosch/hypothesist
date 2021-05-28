using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Hypothesist.Tests.Helpers;
using Xunit;

namespace Hypothesist.Tests.Hypothesis
{
    public class All
    {
        [Fact]
        public async Task Empty() => 
            await Hypothesize
                .All<string>()
                .Within(1.Seconds())
                .Should(x => x == "a")
                .Validate();
        
        [Fact]
        public async Task Success()
        {
            var hypothesis = Hypothesize
                .All<string>()
                .Within(1.Seconds())
                .Should(x => x == "a");

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate());
        }

        [Fact]
        public async Task Invalid()
        {
            var hypothesis = Hypothesize
                .All<string>()
                .Forever()
                .Should(y => y == "a");

            await hypothesis.Test("b");
            
            Func<Task> act = () => hypothesis.Validate();
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
                .All<string>()
                .Within(1.Seconds())
                .Should(y => y == "a");
            
            await hypothesis.Test("a");
            await hypothesis.Test("b");

            Func<Task> act = () => hypothesis.Validate();
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
                .All<string>()
                .Within(2.Seconds())
                .Should(y => y == "a");

            Func<Task> act = () => Task.WhenAll(hypothesis.TestSlowly("a", "a", "a", "a", "b"), hypothesis.Validate());
            await act
                .Should()
                .ThrowAsync<InvalidException<string>>();
        }
        
        [Fact]
        public async Task Cancel()
        {
            using var tcs = new CancellationTokenSource(5.Seconds());
            var hypothesis = Hypothesize
                .All<string>()
                .Forever()
                .Should(_ => { });

            await hypothesis.Validate(tcs.Token);
        }
    }
}