namespace RssReader.Tests.Extensions
{
    using System;
    using RssReader.Extensions;
    using Xunit;

    public sealed class GuidExtensionsTests
    {
        public sealed class ToShortString
        {
            [Theory]
            [InlineData("00000000-0000-0000-0000-000000000000", "AAAAAAAAAAAAAAAAAAAAAA")]
            [InlineData("11111111-1111-1111-1111-111111111111", "EREREREREREREREREREREQ")]
            [InlineData("22222222-2222-2222-2222-222222222222", "IiIiIiIiIiIiIiIiIiIiIg")]
            [InlineData("33333333-3333-3333-3333-333333333333", "MzMzMzMzMzMzMzMzMzMzMw")]
            [InlineData("44444444-4444-4444-4444-444444444444", "RERERERERERERERERERERA")]
            [InlineData("11111111-2222-3333-4444-555555555555", "ERERESIiMzNERFVVVVVVVQ")]
            public void Success(string guid, string expected)
            {
                Assert.Equal(expected, Guid.Parse(guid).ToShortString());
            }
        }
    }
}
