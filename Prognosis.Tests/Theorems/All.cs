using System;
using System.Threading.Tasks;
using FluentAssertions;
using Prognosis.Tests.Helpers;
using Xunit;
using Xunit.Sdk;

namespace Prognosis.Tests.Theorems
{
    public class All
    {
        [Fact]
        public async Task Empty() => 
            await Future
                .All<string>(x => x.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));

        [Fact]
        public async Task Throws()
        {
            var future = Future.All<string>(y => y.Should().Be("a"));
            await future.Prove("b");
            
            await future.Invoking(async x => await x)
                .Should()
                .ThrowAsync<XunitException>();
        }
        
        [Fact]
        public async Task Subsequent()
        {
            var future = Future.All<string>(y => y.Should().Be("a"));
            await future.Prove("a");
            await future.Prove("b");

            await future.Invoking(async x => await x.Within(TimeSpan.FromSeconds(1)))
                .Should()
                .ThrowAsync<XunitException>();
        }
        
        [Fact]
        public async Task Sliding()
        {
            var future = Future.All<string>(y => y.Should().Be("a"));
            await Task.WhenAll(future.Slowly("a", "a", "a", "a", "b"), future.Invoking(x => x.Within(TimeSpan.FromSeconds(2)))
                .Should()
                .ThrowAsync<XunitException>());
        }
    }
}