using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Prognosis.Tests.Helpers;
using Xunit;
using Xunit.Sdk;

namespace Prognosis.Tests.Theorems
{
    public class Any
    {
        [Fact]
        public async Task Success()
        {
            var future = Future.Any<string>(x => x.Should().Be("a"));
            await Task.WhenAll(future.Prove("a"), future.Within(TimeSpan.FromSeconds(1)));
        }
        
        [Fact]
        public void Timeout()
        {
            var future = Future.Any<string>(_ => { });
            future.Invoking(async x => await x.Within(TimeSpan.FromSeconds(1)))
                .Should()
                .Throw<TimeoutException>();
        }
        
        [Fact]
        public async Task Sliding()
        {
            var future = Future.Any<string>(y => y.Should().Be("b"));
            await Task.WhenAll(future.Slowly("a", "a", "a", "a", "b"), future.Within(TimeSpan.FromSeconds(2)));
        }
        
        [Fact]
        public async Task Throws()
        {
            var future = Future.Any<string>(y => y.Should().Be("a"));
            await future.Prove("b");
            
            future.Invoking(async x => await x.Within(TimeSpan.FromSeconds(1)))
                .Should()
                .Throw<XunitException>();
        }
        
        [Fact]
        public async Task Aggregate()
        {
            var future = Future.Any<string>(y => y.Should().Be("a"));
            await future.Prove("b");
            await future.Prove("c");
            
            future.Invoking(async x => await x.Within(TimeSpan.FromSeconds(1)))
                .Should()
                .Throw<AggregateException>()
                .Which
                .InnerExceptions
                .Should()
                .HaveCount(2);
        }
        
        [Fact]
        public async Task Subsequent()
        {
            var future = Future.Any<string>(y => y.Should().Be("b"));
            await future.Prove("a");
            await future.Prove("b");

            await future.Within(TimeSpan.FromSeconds(1));
        }
        
        [Fact]
        public async Task Forever()
        {
            var future = Future.Any<string>(_ => { });
            var first = await Task.WhenAny(future.Forever(), Task.Delay(TimeSpan.FromSeconds(5)));
            first.Should().NotBe(future);
        }
        
        [Fact]
        public async Task ForeverCancelled()
        {
            var future = Future.Any<string>(_ => { });
            using var tcs = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await future.Invoking(x => x.Forever(tcs.Token))
                .Should()
                .ThrowAsync<TimeoutException>();
        }
    }
}