using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Hypothesist.Tests.Hypothesis
{
    public class Forever
    {
        [Fact]
        public async Task Run()
        {
            var hypothesis = Hypothesize
                .Single<string>()
                .Forever()
                .Should(_ => { });

            var delay = Task.Delay(5.Seconds());
            var first = await Task.WhenAny(hypothesis.Validate(), delay);
            
            first.Should().Be(delay);
        }
        
        [Fact]
        public async Task Cancel()
        {
            var hypothesis = Hypothesize
                .Single<string>()
                .Forever()
                .Should(_ => { });
            
            Func<Task> act = async () =>
            {
                using var tcs = new CancellationTokenSource(5.Seconds());
                await hypothesis.Validate(tcs.Token);
            };
            
            await act
                .Should()
                .ThrowAsync<InvalidException<string>>();
        }   
    }
}