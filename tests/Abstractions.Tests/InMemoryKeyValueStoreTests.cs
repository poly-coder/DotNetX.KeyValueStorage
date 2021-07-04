using System;
using System.Collections.Immutable;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using DotNetX.KeyValueStorage;
using FluentAssertions;
using Xunit;

namespace Abstractions.Tests
{
    public class InMemoryKeyValueStoreTests
    {
        [Fact]
        public async Task WhenAnEntryIsStoredItCanBeLoadedBack()
        {
            var keyArray = new byte[] { 1, 2, 3 };
            var valueArray = new byte[] { 1, 2, 4 };

            var entry = new KeyValueEntry(keyArray.AsMemory(), valueArray.AsMemory());

            var store = new InMemoryKeyValueStore();

            await store.StoreAsync(entry, default);

            var newEntry = await store.LoadEntryAsync(keyArray, default);

            newEntry.Key.ToArray().Should().BeEquivalentTo(keyArray);
            newEntry.Value.ToArray().Should().BeEquivalentTo(valueArray);
            newEntry.Properties.Should().BeEquivalentTo(entry.Properties);
            newEntry.Metadata.Should().BeEquivalentTo(entry.Metadata);
        }

        [Fact]
        public async Task WhenAnEntryIsStoredItsPropertiesCanBeLoadedBack()
        {
            var keyArray = new byte[] { 1, 2, 3 };
            var valueArray = new byte[] { 1, 2, 4 };

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

            var store = new InMemoryKeyValueStore();

            await store.StoreAsync(entry, default);

            var newProperties = await store.LoadPropertiesAsync(keyArray, default);

            newProperties.Should().NotBeNull();
            newProperties.Should().BeEquivalentTo(properties);
        }

        [Fact]
        public async Task WhenAnEntryIsStoredItsMetadataCanBeLoadedBack()
        {
            var keyArray = new byte[] { 1, 2, 3 };
            var valueArray = new byte[] { 1, 2, 4 };

            var metadata = ImmutableDictionary<string, string>.Empty
                .Add("key1", "value1")
                .Add("key2", "value2")
                .Add("key3", "value3");

            var entry = new KeyValueEntry(keyArray.AsMemory(), valueArray.AsMemory(), metadata: metadata);

            var store = new InMemoryKeyValueStore();

            await store.StoreAsync(entry, default);

            var newMetadata = await store.LoadMetadataAsync(keyArray, default);

            newMetadata.Should().NotBeNull();
            newMetadata.Should().BeEquivalentTo(metadata);
        }

        [Fact]
        public async Task WhenAnEntryIsStoredItsValueCanBeLoadedBack()
        {
            var keyArray = new byte[] { 1, 2, 3 };
            var valueArray = new byte[] { 1, 2, 4 };

            var entry = new KeyValueEntry(keyArray.AsMemory(), valueArray.AsMemory());

            var store = new InMemoryKeyValueStore();

            await store.StoreAsync(entry, default);

            var newValue = await store.LoadValueAsync(keyArray, default);

            newValue.ToArray().Should().BeEquivalentTo(valueArray);
        }

        [Fact]
        public async Task WhenAnEntryIsNotFoundLoadEntryShouldReturnNull()
        {
            var keyArray = new byte[] { 1, 2, 3 };

            var store = new InMemoryKeyValueStore();

            var entry = await store.LoadEntryAsync(keyArray, default);

            entry.Should().BeNull();
        }

        [Fact]
        public async Task WhenAnEntryIsNotFoundLoadPropertiesShouldReturnNull()
        {
            var keyArray = new byte[] { 1, 2, 3 };

            var store = new InMemoryKeyValueStore();

            var properties = await store.LoadPropertiesAsync(keyArray, default);

            properties.Should().BeNull();
        }

        [Fact]
        public async Task WhenAnEntryIsNotFoundLoadMetadataShouldReturnNull()
        {
            var keyArray = new byte[] { 1, 2, 3 };

            var store = new InMemoryKeyValueStore();

            var metadata = await store.LoadMetadataAsync(keyArray, default);

            metadata.Should().BeNull();
        }

        [Fact]
        public async Task WhenAnEntryIsNotFoundLoadValueShouldReturnNull()
        {
            var keyArray = new byte[] { 1, 2, 3 };

            var store = new InMemoryKeyValueStore();

            var value = await store.LoadValueAsync(keyArray, default);

            value.Should().BeEquivalentTo(Memory<byte>.Empty);
        }

        [Fact]
        public async Task WhenAnEntryIsRemovedItShouldNotBeAvailableAfterwards()
        {
            var keyArray = new byte[] { 1, 2, 3 };
            var valueArray = new byte[] { 1, 2, 4 };

            var entry = new KeyValueEntry(keyArray.AsMemory(), valueArray.AsMemory());

            var store = new InMemoryKeyValueStore();

            await store.StoreAsync(entry, default);
            
            await store.RemoveKeyAsync(keyArray, default);

            var newEntry = await store.LoadEntryAsync(keyArray, default);

            newEntry.Should().BeNull();
        }
    }
}