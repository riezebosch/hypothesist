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
        public async Task Prove()
        {
            var hypothesis = Hypothesize.Future
                .Any<string>(x => x.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate());
        }
        
        [Fact]
        public void Timeout()
        {
            var hypothesis = Hypothesize.Future
                .Any<string>(_ => { })
                .Within(TimeSpan.FromSeconds(1));

            hypothesis.Invoking(async x => await x.Validate())
                .Should()
                .Throw<TimeoutException>();
        }
        
        [Fact]
        public async Task Finally()
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
            
            hypothesis.Invoking(async x => await x.Validate())
                .Should()
                .Throw<XunitException>();
        }
        
        [Fact]
        public async Task Aggregate()
        {
            var hypothesis = Hypothesize.Future
                .Any<string>(y => y.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));

            await hypothesis.Test("b");
            await hypothesis.Test("c");
            
            hypothesis.Invoking(async x => await x.Validate())
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
            
            var first = await Task.WhenAny(hypothesis.Validate(), Task.Delay(TimeSpan.FromSeconds(5)));
            first.Should().NotBe(hypothesis);
        }
        
        [Fact]
        public async Task Cancelled()
        {
            using var tcs = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var hypothesis = Hypothesize.Future
                .Any<string>(_ => { })
                .Forever(tcs.Token);
            
            await hypothesis.Invoking(x => x.Validate())
                .Should()
                .ThrowAsync<TimeoutException>();
        }
    }
}