using System;
using Xunit;

namespace HomeCoach.Tests
{
    using FluentAssertions;

    public class NetAtmoDataBusinessShould
    {
        [Fact]
        public void Works()
        {
            var result = true;

            result.Should().BeTrue();
        }
    }
}