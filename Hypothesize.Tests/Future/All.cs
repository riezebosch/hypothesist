using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Hypothesize.Tests.Helpers;
using Xunit;
using Xunit.Sdk;

namespace Hypothesize.Tests.Future
{
    public class All
    {
        [Fact]
        public async Task Empty() => 
            await Hypothesize.Future
                .All<string>(x => x.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1))
                .Validate();
        
        [Fact]
        public async Task Success()
        {
            var hypothesis = Hypothesize.Future
                .All<string>(x => x.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate());
        }

        [Fact]
        public async Task Throws()
        {
            var hypothesis = Hypothesize.Future
                .All<string>(y => y.Should().Be("a"))
                .Forever();

            await hypothesis.Test("b");
            
            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<XunitException>();
        }
        
        [Fact]
        public async Task Subsequent()
        {
            var hypothesis = Hypothesize.Future
                .All<string>(y => y.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));
            
            await hypothesis.Test("a");
            await hypothesis.Test("b");

            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<XunitException>();
        }
        
        [Fact]
        public async Task Sliding()
        {
            var hypothesis = Hypothesize.Future
                .All<string>(y => y.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(2));

            Func<Task> act = () => Task.WhenAll(hypothesis.Slowly("a", "a", "a", "a", "b"), hypothesis.Validate());
            await act
                .Should()
                .ThrowAsync<XunitException>();
        }
        
        [Fact]
        public async Task Cancelled()
        {
            using var tcs = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var hypothesis = Hypothesize.Future
                .All<string>(_ => { })
                .Forever();

            await hypothesis.Validate(tcs.Token);
        }
    }
}