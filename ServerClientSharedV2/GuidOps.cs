using Google.Protobuf;
using System;

namespace ServerClientSharedV2
{
    public static class GuidOps
    {
        /// <summary>
        /// Instead of <see cref="Guid"/> byte constructor since it will throw if the bytes are wrong.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(byte[] bytes, out Guid result)
        {
            if (!Validate(bytes))
                return false;
            result = new Guid(bytes);
            return true;
        }

        public static bool TryParse(ByteString bytes, out Guid result)
        {
            if (!Validate(bytes))
                return false;
            result = new Guid(bytes.ToByteArray()); //It should be possible to remove this allocation
            return true;
        }

        public static bool Validate(ByteString bytes)
        {
            return bytes != null && bytes.Length == 16;
        }

        public static bool Validate(byte[] bytes)
        {
            return bytes != null && bytes.Length == 16;
        }

        public static ByteString ToByteString(Guid guid)
        {
            return ByteString.CopyFrom(guid.ToByteArray());
        }
    }
}
