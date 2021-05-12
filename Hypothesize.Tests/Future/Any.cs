using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Hypothesize.Tests.Helpers;
using Xunit;
using Xunit.Sdk;

namespace Hypothesize.Tests.Future
{
    public class Any
    {
        [Fact]
        public async Task Success()
        {
            var hypothesis = Hypothesize.Future
                .Any<string>(x => x.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate());
        }
        
        [Fact]
        public async Task None()
        {
            var hypothesis = Hypothesize.Future
                .Any<string>(_ => { })
                .Within(TimeSpan.FromSeconds(1));

            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<TimeoutException>();
        }
        
        [Fact]
        public async Task Sliding()
        {
            var hypothesis = Hypothesize.Future
                .Any<string>(y => y.Should().Be("b"))
                .Within(TimeSpan.FromSeconds(2));
            
            await Task.WhenAll(hypothesis.Slowly("a", "a", "a", "a", "b"), hypothesis.Validate());
        }
        
        [Fact]
        public async Task Throws()
        {
            var hypothesis = Hypothesize.Future
                .Any<string>(y => y.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));
            
            await hypothesis.Test("b");
            
            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<XunitException>();
        }
        
        [Fact]
        public async Task Aggregate()
        {
            var hypothesis = Hypothesize.Future
                .Any<string>(y => y.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));

            await hypothesis.Test("b");
            await hypothesis.Test("c");
            
            Func<Task> act = () => hypothesis.Validate();
            var ex = await act
                .Should()
                .ThrowAsync<AggregateException>();
            
            ex.Which
                .InnerExceptions
                .Should()
                .HaveCount(2);
        }
        
        [Fact]
        public async Task Subsequent()
        {
            var hypothesis = Hypothesize.Future
                .Any<string>(y => y.Should().Be("b"))
                .Within(TimeSpan.FromSeconds(1));

            await hypothesis.Test("a");
            await hypothesis.Test("b");

            await hypothesis.Validate();
        }
        
        [Fact]
        public async Task Forever()
        {
            var hypothesis = Hypothesize.Future
                .Any<string>(_ => { })
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
                .Any<string>(_ => { })
                .Forever();
            
            Func<Task> act = () => hypothesis.Validate(tcs.Token);
            await act
                .Should()
                .ThrowAsync<TimeoutException>();
        }
    }
}