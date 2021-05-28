using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Hypothesist.Tests.Hypothesis
{
    public class First
    {
        [Fact]
        public async Task Success()
        {
            var hypothesis = Hypothesize
                .First<string>()
                .Within(1.Seconds())
                .Should(x => x.Should().Be("a"));

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate());
        }
        
        [Fact]
        public async Task None()
        {
            var hypothesis = Hypothesize
                .First<string>()
                .Within(1.Seconds())
                .Should(_ => { });

            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<TimeoutException>();
        }
        
        [Fact]
        public async Task Invalid()
        {
            var hypothesis = Hypothesize
                .First<string>()
                .Within(1.Seconds())
                .Should(y => y.Should().Be("a"));
            
            await hypothesis.Test("b");
            await hypothesis.Test("a");
            
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
    }
}