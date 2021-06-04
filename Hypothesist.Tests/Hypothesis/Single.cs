using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Hypothesist.Tests.Helpers;
using Xunit;

namespace Hypothesist.Tests.Hypothesis
{
    public class Single
    {
        [Fact]
        public async Task Success()
        {
            var hypothesis = Hypothesize
                .Single<string>(x => x == "a");

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate(1.Seconds()));
        }
        
        [Fact]
        public async Task Others()
        {
            var hypothesis = Hypothesize
                .Single<string>(x => x == "a");

            await Task.WhenAll(hypothesis.Test("b"),
                hypothesis.Test("d"),
                hypothesis.Test("a"),
                hypothesis.Test("e"),
                hypothesis.Test("f"),
                hypothesis.Validate(1.Seconds()));
        }
        
        [Fact]
        public async Task None()
        {
            var hypothesis = Hypothesize
                .Single<string>(x => x == "a");

            Func<Task> act = () => hypothesis.Validate(1.Seconds());
            var ex = await act
                .Should()
                .ThrowAsync<InvalidException<string>>();

            ex.Which
                .Unmatched
                .Should()
                .BeEmpty();
        }
        
        [Fact]
        public async Task Wrong()
        {
            var hypothesis = Hypothesize
                .Single<string>(x => x == "a");
            
            await hypothesis.Test("b");
            
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
        public async Task Multiple()
        {
            var hypothesis = Hypothesize
                .Single<string>(x => x == "a");
            
            
            await hypothesis.Test("a");
            await hypothesis.Test("b");
            await hypothesis.Test("a");
            
            Func<Task> act = () => hypothesis.Validate(1.Seconds());
            var ex = await act
                .Should()
                .ThrowAsync<InvalidException<string>>();

            ex.Which
                .Matched
                .Should()
                .BeEquivalentTo("a", "a");

            ex.Which
                .Unmatched
                .Should()
                .BeEquivalentTo("b");
        }
        
        [Fact]
        public async Task FailFast()
        {
            var hypothesis = Hypothesize
                .Single<string>(x => x == "a");
                
            var validate = hypothesis.Validate(1.Seconds());
            var first = await Task.WhenAny(hypothesis.TestSlowly("a", "a", "a", "a"), validate);
            
            first.Should().Be(validate);
        }
    }
}