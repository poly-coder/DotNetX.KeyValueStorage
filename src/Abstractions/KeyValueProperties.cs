using System;

namespace DotNetX.KeyValueStorage
{
    public class KeyValueProperties : ICloneable
    {
        public string ContentType { get; set; }
        public string ContentEncoding { get; set; }
        public string ContentLanguage { get; set; }
        public string ContentDisposition { get; set; }
        public long ContentLength { get; set; }
        public string ContentMD5 { get; set; }
        public string ContentCRC64 { get; set; }

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }

        public KeyValueProperties Clone()
        {
            return (KeyValueProperties) ((ICloneable) this).Clone();
        }
    }
}
