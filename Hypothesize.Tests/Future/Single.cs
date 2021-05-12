using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Hypothesize.Tests.Helpers;
using Xunit;
using Xunit.Sdk;

namespace Hypothesize.Tests.Future
{
    public class Single
    {
        [Fact]
        public async Task Success()
        {
            var hypothesis = Hypothesize.Future
                .Single<string>(x => x.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));

            await Task.WhenAll(hypothesis.Test("a"), hypothesis.Validate());
        }
        
        [Fact]
        public async Task AllowOthers()
        {
            var hypothesis = Hypothesize.Future
                .Single<string>(x => x.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));

            hypothesis.Test("b");
            hypothesis.Test("d");
            hypothesis.Test("a");
            hypothesis.Test("e");
            hypothesis.Test("f");
            
            await hypothesis.Validate();
        }
        
        [Fact]
        public async Task None()
        {
            var hypothesis = Hypothesize.Future
                .Single<string>(_ => { })
                .Within(TimeSpan.FromSeconds(1));

            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<TimeoutException>();
        }
        
        [Fact]
        public async Task Wrong()
        {
            var hypothesis = Hypothesize.Future
                .Single<string>(y => y.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));
            
            await hypothesis.Test("b");
            
            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<TimeoutException>();
        }
        
        [Fact]
        public async Task Multiple()
        {
            var hypothesis = Hypothesize.Future
                .Single<string>(y => y.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));
            
            await hypothesis.Test("a");
            await hypothesis.Test("a");
            
            Func<Task> act = () => hypothesis.Validate();
            await act
                .Should()
                .ThrowAsync<InvalidOperationException>();
        }
        
        [Fact]
        public async Task FailFast()
        {
            var hypothesis = Hypothesize.Future
                .Single<string>(y => y.Should().Be("a"))
                .Within(TimeSpan.FromSeconds(1));
                
            var validate = hypothesis.Validate();
            var first = await Task.WhenAny(hypothesis.Slowly("a", "a", "a", "a"), validate);
            
            first.Should().Be(validate);
        }
        
        [Fact]
        public async Task Forever()
        {
            var hypothesis = Hypothesize.Future
                .Single<string>(_ => { })
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
                .Single<string>(_ => { })
                .Forever();
            
            Func<Task> act = () => hypothesis.Validate(tcs.Token);
            await act
                .Should()
                .ThrowAsync<TimeoutException>();
        }
    }
}