using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LRUCache
{
    public class LRUCache<T>
    {
        public interface ICache<TKey, TValue>
        {
            bool TryGetValue(TKey key, out TValue value);
            void Put(TKey key, TValue value);
        }

        internal enum Operations
        {
            Add,
            Subtract,
            Multiply,
            Divide
        };
        internal struct cacheMaterial
        {
            int a;
            int b;
            Operations operation;
        }

        Dictionary<cacheMaterial, T> hashMap;

        LinkedList<T> LRU;

        public LRUCache()
        {
            hashMap = new();
            LRU = new();
        }
    }
}
