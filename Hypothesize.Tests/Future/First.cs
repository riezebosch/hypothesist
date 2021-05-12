using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Hypothesize.Tests.Future
{
    public class First
    {
        [Fact]
        public async Task Success()
        {
            var hypothesis = Hypothesize.Future
                .First<string>(x => x.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate());
        }
        
        [Fact]
        public async Task None()
        {
            var hypothesis = Hypothesize.Future
                .First<string>(_ => { })
                .Within(TimeSpan.FromSeconds(1));

            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<TimeoutException>();
        }
        
        [Fact]
        public async Task Throws()
        {
            var hypothesis = Hypothesize.Future
                .First<string>(y => y.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));
            
            await hypothesis.Test("b");
            await hypothesis.Test("a");
            
            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<XunitException>();
        }
        
        [Fact]
        public async Task Forever()
        {
            var hypothesis = Hypothesize.Future
                .First<string>(_ => { })
                .Forever();

            var delay = Task.Delay(TimeSpan.FromSeconds(5));
            var first = await Task.WhenAny(hypothesis.Validate(), delay);
            
            first.Should().Be(delay);
        }
        
        [Fact]
        public async Task Cancelled()
        {
            using var tcs = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var hypothesis = Hypothesize.Future
                .First<string>(_ => { })
                .Forever(tcs.Token);
            
            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<TimeoutException>();
        }
    }
}