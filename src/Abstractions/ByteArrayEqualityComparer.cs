using System;
using System.Collections.Generic;

namespace DotNetX.KeyValueStorage
{
    // TODO: Copy to DotNetX
    public class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
    {
        public static readonly ByteArrayEqualityComparer Default = new ByteArrayEqualityComparer();

        public bool Equals(byte[] x, byte[] y)
        {
            if (x == null) return y == null;
            if (y == null) return false;
            if (x.Length != y.Length) return false;

            int length = x.Length;
            for (int i = 0; i < length; i++)
            {
                if (x[i] != y[i]) return false;
            }

            return true;
        }

        public int GetHashCode(byte[] array)
        {
            var hash = 0;

            int length = Math.Min(array.Length, 1024);
            for (int i = 0; i < length; i++)
            {
                hash = (hash * 37) ^ (int) array[i];
            }

            return hash;
        }
    }
}