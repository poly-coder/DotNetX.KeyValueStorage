using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetX.KeyValueStorage
{
    public interface IKeyValueStore
    {
        Task<KeyValueProperties> StoreAsync(
            KeyValueEntry entry,
            CancellationToken cancellationToken);

        Task<KeyValueProperties> LoadPropertiesAsync(
            ReadOnlyMemory<byte> key,
            CancellationToken cancellationToken);

        Task<ImmutableDictionary<string, string>> LoadMetadataAsync(
            ReadOnlyMemory<byte> key,
            CancellationToken cancellationToken);

        Task<ReadOnlyMemory<byte>> LoadValueAsync(
            ReadOnlyMemory<byte> key,
            CancellationToken cancellationToken);

        Task<KeyValueEntry> LoadEntryAsync(
            ReadOnlyMemory<byte> key,
            CancellationToken cancellationToken);

        Task RemoveKeyAsync(
            ReadOnlyMemory<byte> key,
            CancellationToken cancellationToken);
    }
}
