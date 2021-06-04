using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Hypothesist.Tests.Hypothesis
{
    public static class Exactly
    {
        [Fact]
        public static async Task Match()
        {
            var hypothesis = Hypothesize
                .Exactly<string>(x => x == "a",2);

            await hypothesis.Test("a");
            await hypothesis.Test("a");
            
            await hypothesis
                .Validate(2.Seconds());
        }
        
        [Fact]
        public static async Task None()
        {
            var hypothesis = Hypothesize
                .Exactly<string>(x => x == "a", 2);

            Func<Task> act = () => hypothesis.Validate(1.Seconds());
            await act.Should()
                .ThrowAsync<InvalidException<string>>();
        }
        
        [Fact]
        public static async Task Less()
        {
            var hypothesis = Hypothesize
                .Exactly<string>(x => x == "a", 2);
            
            await hypothesis.Test("a");
            
            Func<Task> act = () => hypothesis.Validate(1.Seconds());
            var ex = await act.Should()
                .ThrowAsync<InvalidException<string>>();

            ex.Which
                .Matched
                .Should()
                .BeEquivalentTo("a");
        }
        
        [Fact]
        public static async Task More()
        {
            var hypothesis = Hypothesize
                .Exactly<string>(x => x == "a", 2);
            
            await hypothesis.Test("a");
            await hypothesis.Test("a");
            await hypothesis.Test("a");
            
            Func<Task> act = () => hypothesis.Validate(1.Seconds());
            var ex = await act.Should()
                .ThrowAsync<InvalidException<string>>();

            ex.Which
                .Matched
                .Should()
                .BeEquivalentTo("a", "a", "a");
        }
        
        [Fact]
        public static async Task Unmatched()
        {
            var hypothesis = Hypothesize
                .Exactly<string>(x => x == "a",2);
            
            await hypothesis.Test("b");
            
            Func<Task> act = () => hypothesis.Validate(1.Seconds());
            var ex = await act.Should()
                .ThrowAsync<InvalidException<string>>();

            ex.Which
                .Unmatched
                .Should()
                .BeEquivalentTo("b");
        }
    }
}