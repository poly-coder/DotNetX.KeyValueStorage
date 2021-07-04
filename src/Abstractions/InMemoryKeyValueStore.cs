using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.KeyValueStorage
{
    public class InMemoryKeyValueStore : IKeyValueStore
    {
        private readonly ReaderWriterLockSlim lockObj =
            new ReaderWriterLockSlim();

        private readonly Dictionary<byte[], StoreEntry> entries =
            new Dictionary<byte[], StoreEntry>(ByteArrayEqualityComparer.Default);

        public async Task<KeyValueProperties> StoreAsync(
            KeyValueEntry entry,
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            lockObj.TryEnterWriteLock(TimeSpan.FromSeconds(15));

            try
            {
                var storeEntry = new StoreEntry(entry);

                entries[storeEntry.Key] = storeEntry;

                return storeEntry.Properties.Clone();
            }
            finally
            {
                lockObj.ExitWriteLock();
            }
        }

        public async Task<KeyValueProperties> LoadPropertiesAsync(
            ReadOnlyMemory<byte> key,
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            lockObj.TryEnterReadLock(TimeSpan.FromSeconds(15));

            try
            {
                var keyBytes = key.ToArray();

                if (entries.TryGetValue(keyBytes, out var entry))
                {
                    return entry.Properties.Clone();
                }

                return null;
            }
            finally
            {
                lockObj.ExitReadLock();
            }
        }

        public async Task<ImmutableDictionary<string, string>> LoadMetadataAsync(
            ReadOnlyMemory<byte> key,
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            lockObj.TryEnterReadLock(TimeSpan.FromSeconds(15));

            try
            {
                var keyBytes = key.ToArray();

                if (entries.TryGetValue(keyBytes, out var entry))
                {
                    return entry.Metadata;
                }

                return null;
            }
            finally
            {
                lockObj.ExitReadLock();
            }
        }

        public async Task<ReadOnlyMemory<byte>> LoadValueAsync(
            ReadOnlyMemory<byte> key,
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            lockObj.TryEnterReadLock(TimeSpan.FromSeconds(15));

            try
            {
                var keyBytes = key.ToArray();

                if (entries.TryGetValue(keyBytes, out var entry))
                {
                    return entry.Value;
                }

                return null;
            }
            finally
            {
                lockObj.ExitReadLock();
            }
        }

        public async Task<KeyValueEntry> LoadEntryAsync(
            ReadOnlyMemory<byte> key,
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            lockObj.TryEnterReadLock(TimeSpan.FromSeconds(15));

            try
            {
                var keyBytes = key.ToArray();

                if (entries.TryGetValue(keyBytes, out var entry))
                {
                    return entry.ToKeyValueEntry();
                }

                return null;
            }
            finally
            {
                lockObj.ExitReadLock();
            }
        }

        public async Task RemoveKeyAsync(
            ReadOnlyMemory<byte> key,
            CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            lockObj.TryEnterWriteLock(TimeSpan.FromSeconds(15));

            try
            {
                entries.Remove(key.ToArray());
            }
            finally
            {
                lockObj.ExitWriteLock();
            }
        }

        private class StoreEntry
        {
            public StoreEntry(KeyValueEntry entry)
            {
                Key = entry.Key.ToArray();
                Value = entry.Value.ToArray();
                Properties = entry.Properties.Clone();
                Metadata = entry.Metadata.ToImmutableDictionary();
            }

            public byte[] Key { get; }
            public byte[] Value { get; }
            public KeyValueProperties Properties { get; }
            public ImmutableDictionary<string, string> Metadata { get; }

            public KeyValueEntry ToKeyValueEntry()
            {
                return new KeyValueEntry(
                    Key,
                    Value,
                    Properties.Clone(),
                    Metadata);
            }
        }
    }
}
