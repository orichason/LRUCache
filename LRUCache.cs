using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LRUCache
{
    //internal enum Operations
    //{
    //    Add,
    //    Subtract,
    //    Multiply,
    //    Divide
    //};
    //internal struct Equation
    //{
    //    int a;
    //    int b;
    //    Operations operation;
    //}
    public interface ICache<TKey, TValue>
    {
        bool TryGetValue(TKey key, out TValue value);
        void Put(TKey key, TValue value);
    }

    public class LRUCache<TKey, TValue> : ICache<TKey, TValue>
    {
        //struct Container
        //{
        //    public TKey Key;
        //    public TValue Value;
        //    public Container(TKey key, TValue value)
        //    {
        //        Key = key;
        //        Value = value;
        //    }
        //}

        readonly Dictionary<TKey, LinkedListNode<KeyValuePair<TKey, TValue>>> hashMap;

        readonly LinkedList<KeyValuePair<TKey, TValue>> LRU;

        int capacity;
        public LRUCache(int capacity)
        {
            this.capacity = capacity;
            hashMap = new(capacity);
            LRU = new();
        }

        /// <summary>
        /// Moves node to front of linked list
        /// </summary>
        /// <param name="node"></param>
        void MoveNodeToFront(LinkedListNode<KeyValuePair<TKey, TValue>> node)
        {
            LRU.Remove(node);
            LRU.AddFirst(node);
        }

        /// <summary>
        /// If key exists, move it to front of list and update value.
        /// If key doesn't exist, create new node and add to front of list.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void ICache<TKey, TValue>.Put(TKey key, TValue value)
        {
            KeyValuePair<TKey, TValue> keyValuePair = new(key, value);

            if (hashMap.ContainsKey(key))
            {
                hashMap[key].Value = keyValuePair;
                MoveNodeToFront(hashMap[key]);
            }

            else
            {
                if (hashMap.Count == capacity)
                {
                    LRU.Remove(LRU.Last!); //removes last node to make space
                }
                var nodeToAdd = new LinkedListNode<KeyValuePair<TKey, TValue>>(keyValuePair);
                hashMap.Add(key, nodeToAdd);
                LRU.AddFirst(nodeToAdd);
            }
        }

        /// <summary>
        /// Given a key, if it exists inside the hash map (cache hit), move the node to the head of the list. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns> value of node and whether key exists</returns>
        bool ICache<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            if (hashMap.ContainsKey(key))
            {
                var nodeToMove = hashMap[key];
                MoveNodeToFront(nodeToMove);

                value = nodeToMove.Value.Value;
                return true;
            }

            value = default!;
            return false;
        }
    }
}
