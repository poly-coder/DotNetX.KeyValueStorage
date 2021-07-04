using System;
using System.Collections.Immutable;
using System.Net.Mime;

namespace DotNetX.KeyValueStorage
{
    public class KeyValueEntry
    {
        public KeyValueEntry(
            ReadOnlyMemory<byte> key,
            ReadOnlyMemory<byte> value,
            KeyValueProperties properties = null,
            ImmutableDictionary<string, string> metadata = null)
        {
            Key = key;
            
            Value = value;
            
            Properties = properties?.Clone() ?? new KeyValueProperties();
            Properties.ContentType = Properties.ContentType ?? MediaTypeNames.Application.Octet;
            
            Metadata = metadata ?? ImmutableDictionary<string, string>.Empty;
        }

        public ReadOnlyMemory<byte> Key { get; }
        public ReadOnlyMemory<byte> Value { get; }
        public KeyValueProperties Properties { get; }
        public ImmutableDictionary<string, string> Metadata { get; }
    }
}
