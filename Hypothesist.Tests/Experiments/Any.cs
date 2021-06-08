using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Hypothesist.Tests.Helpers;
using Xunit;

namespace Hypothesist.Tests.Experiments
{
    public class Any
    {
        [Fact]
        public async Task Valid()
        {
            var hypothesis = Hypothesis
                .For<string>()
                .Any(x => x == "a");

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
        }
        
        [Fact]
        public async Task Anything()
        {
            var hypothesis = Hypothesis
                .For<string>()
                .Any();

            await Task.WhenAll(
                hypothesis.Test("a"),
                hypothesis.Validate(10.Minutes()));
        }
        
        [Fact]
        public async Task Invalid()
        {
            var hypothesis = Hypothesis
                .For<string>()
                .Any(x => x == "a");

            await hypothesis
                .Test("b");

            Func<Task> act = () => hypothesis.Validate(1.Seconds());
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
            var hypothesis = Hypothesis
                .For<string>()
                .Any(y => y == "b");
            
            await Task.WhenAll(hypothesis.TestSlowly("a", "a", "a", "a", "b"), hypothesis.Validate(2.Seconds()));
        }
        
        [Fact]
        public async Task Next()
        {
            var hypothesis = Hypothesis
                .For<string>()
                .Any(y => y == "b");

            await hypothesis.Test("a");
            await hypothesis.Test("b");

            await hypothesis.Validate(1.Seconds());
        }
    }
}