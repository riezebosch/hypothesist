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
                .First<string>(x => x == "a");

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
        }
        
        [Fact]
        public async Task None()
        {
            var hypothesis = Hypothesize
                .First<string>(_ => true);

            Func<Task> act = () => hypothesis.Validate(1.Seconds());
            await act
                .Should()
                .ThrowAsync<InvalidException<string>>();
        }
        
        [Fact]
        public async Task Invalid()
        {
            var hypothesis = Hypothesize
                .First<string>(y => y == "a");
            
            await hypothesis.Test("b");
            await hypothesis.Test("a");
            
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
    }
}