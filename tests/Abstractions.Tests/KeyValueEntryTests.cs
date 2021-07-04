using System;
using System.Collections.Immutable;
using System.Net.Mime;
using DotNetX.KeyValueStorage;
using FluentAssertions;
using Xunit;

namespace Abstractions.Tests
{
    public class KeyValueEntryTests
    {
        [Fact]
        public void KeyIsAssigned()
        {
            var keyArray = new byte[] {1, 2, 3};
            var valueArray = new byte[] {1, 2, 4};

            var entry = new KeyValueEntry(keyArray.AsMemory(), valueArray.AsMemory());

            entry.Key.ToArray().Should().BeEquivalentTo(keyArray);
        }

        [Fact]
        public void ValueIsAssigned()
        {
            var keyArray = new byte[] {1, 2, 3};
            var valueArray = new byte[] {1, 2, 4};

            var entry = new KeyValueEntry(keyArray.AsMemory(), valueArray.AsMemory());

            entry.Value.ToArray().Should().BeEquivalentTo(valueArray);
        }

        [Fact]
        public void NullPropertiesIsAssigned()
        {
            var keyArray = new byte[] {1, 2, 3};
            var valueArray = new byte[] {1, 2, 4};

            var entry = new KeyValueEntry(keyArray.AsMemory(), valueArray.AsMemory(), properties: null);

            entry.Properties.Should().NotBeNull();
            entry.Properties.ContentType.Should().Be(MediaTypeNames.Application.Octet);
        }

        [Fact]
        public void NonNullPropertiesIsAssigned()
        {
            var keyArray = new byte[] {1, 2, 3};
            var valueArray = new byte[] {1, 2, 4};

            var properties = new KeyValueProperties()
            {
                ContentType = MediaTypeNames.Application.Json,
                ContentEncoding = "gzip",
                ContentLanguage = "en-US",
                ContentLength = 3,
                ContentDisposition = "attachment",
                ContentMD5 = "12345",
                ContentCRC64 = "ABCDE",
            };

            var entry = new KeyValueEntry(keyArray.AsMemory(), valueArray.AsMemory(), properties);

            entry.Properties.Should().NotBeNull();
            entry.Properties.Should().NotBe(properties);
            entry.Properties.Should().BeEquivalentTo(properties);
        }

        [Fact]
        public void NullMetadataIsAssigned()
        {
            var keyArray = new byte[] { 1, 2, 3 };
            var valueArray = new byte[] { 1, 2, 4 };

            var entry = new KeyValueEntry(keyArray.AsMemory(), valueArray.AsMemory(), metadata: null);

            entry.Metadata.Should().NotBeNull();
            entry.Metadata.Should().BeEquivalentTo(ImmutableDictionary<string, string>.Empty);
        }

        [Fact]
        public void NonNullMetadataIsAssigned()
        {
            var keyArray = new byte[] { 1, 2, 3 };
            var valueArray = new byte[] { 1, 2, 4 };

            var metadata = ImmutableDictionary<string, string>.Empty
                .Add("key1", "value1")
                .Add("key2", "value2")
                .Add("key3", "value3")
                ;
                
            var entry = new KeyValueEntry(keyArray.AsMemory(), valueArray.AsMemory(), metadata: metadata);

            entry.Metadata.Should().NotBeNull();
            entry.Metadata.Should().BeEquivalentTo(metadata);
        }
    }
}