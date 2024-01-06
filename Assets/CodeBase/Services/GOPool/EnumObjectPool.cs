using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Services.GOPool
{
    public class EnumObjectPool<T, TParent, TEnum>
        where T : Component
        where TParent : Transform
        where TEnum : Enum
    {
        private const int AdditionalSize = 2;
        private readonly Func<TParent, TEnum, T> _objectFactory;
        private Dictionary<TEnum, Queue<T>> _objectQueues = new();
        private int _count;

        public EnumObjectPool(Func<TParent, TEnum, T> objectFactory, int count)
        {
            _objectFactory = objectFactory;
            _count = count;
        }

        public T Pop(TParent parent, TEnum id)
        {
            if (!_objectQueues.ContainsKey(id))
            {
                CreateObjects(_count * (AdditionalSize - 1), parent, id);
            }

            Queue<T> objectQueue = _objectQueues[id];

            if (objectQueue.Count <= 0)
            {
                CreateObjects(AdditionalSize, parent, id);
            }

            T obj = objectQueue.Dequeue();

            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Push(T obj, TEnum id)
        {
            obj.gameObject.SetActive(false);
            _objectQueues[id].Enqueue(obj);
        }

        private void CreateObjects(int count, TParent parent, TEnum id)
        {
            Queue<T> objectQueue = !_objectQueues.ContainsKey(id) ? new Queue<T>() : _objectQueues[id];
            
            for (var i = 0; i < count; i++)
            {
                T obj = _objectFactory.Invoke(parent, id);
                obj.gameObject.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            _count += count;
            _objectQueues[id] = objectQueue;
        }
    }
}