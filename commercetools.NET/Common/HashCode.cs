using System.Collections.Generic;
using System.Linq;

namespace commercetools.Common
{
    public struct HashCode
    {
        private readonly int value;
        private HashCode(int value)
        {
            this.value = value;
        }
        public static implicit operator int(HashCode hashCode)
        {
            return hashCode.value;
        }
        public static HashCode Of<T>(T item)
        {
            return new HashCode(GetHashCode(item));
        }
        public HashCode And<T>(T item)
        {
            return new HashCode(CombineHashCodes(this.value, GetHashCode(item)));
        }
        public HashCode AndEach<T>(IEnumerable<T> items)
        {
            if (items == null)
            {
                return new HashCode(CombineHashCodes(this.value, 0));
            }
            var hashCode = items.Any() ? items.Select(GetHashCode).Aggregate(CombineHashCodes) : 0;
            return new HashCode(CombineHashCodes(this.value, hashCode));
        }
        private static int CombineHashCodes(int h1, int h2)
        {
            unchecked
            {
                return ((h1 << 5) + h1) ^ h2;
            }
        }
        private static int GetHashCode<T>(T item)
        {
            return item == null ? 0 : item.GetHashCode();
        }
    }
}
