using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Hypothesist.Tests.Helpers;
using Xunit;

namespace Hypothesist.Tests.Hypothesis
{
    public class Any
    {
        [Fact]
        public async Task Success()
        {
            var hypothesis = Hypothesize
                .Any<string>()
                .Within(1.Seconds())
                .Matches(x => x.Should().Be("a"));

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate());
        }
        
        [Fact]
        public async Task None()
        {
            var hypothesis = Hypothesize
                .Any<string>()
                .Within(1.Seconds())
                .Matches(x => x == "a");

            await hypothesis
                .Test("b");

            Func<Task> act = () => hypothesis.Validate();
            var ex = await act
                .Should()
                .ThrowAsync<InvalidException<string>>();

            ex.Which
                .Unmatched
                .Should()
                .BeEquivalentTo("b");
        }
        
        [Fact]
        public async Task Sliding()
        {
            var hypothesis = Hypothesize
                .Any<string>()
                .Within(2.Seconds())
                .Matches(y => y.Should().Be("b"));
            
            await Task.WhenAll(hypothesis.TestSlowly("a", "a", "a", "a", "b"), hypothesis.Validate());
        }
        
        [Fact]
        public async Task Aggregate()
        {
            var hypothesis = Hypothesize
                .Any<string>()
                .Within(1.Seconds())
                .Matches(y => y.Should().Be("a"));

            await hypothesis.Test("b");
            await hypothesis.Test("c");
            
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
                .BeEquivalentTo("b", "c");

        }
        
        [Fact]
        public async Task Subsequent()
        {
            var hypothesis = Hypothesize
                .Any<string>()
                .Within(1.Seconds())
                .Matches(y => y.Should().Be("b"));

            await hypothesis.Test("a");
            await hypothesis.Test("b");

            await hypothesis.Validate();
        }
    }
}