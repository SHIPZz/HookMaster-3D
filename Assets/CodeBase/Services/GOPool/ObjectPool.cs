using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Services.GOPool
{
    public class ObjectPool<T, TPath, TParent> where T : Component where TPath : class where TParent : Transform
    {
        protected readonly int AdditionalSize = 2;
        private readonly Queue<T> _objects = new Queue<T>();
        protected Func<TPath, TParent, T> ObjectFactory;
        private int _count;

        public ObjectPool(Func<TPath, TParent, T> objectFactory, int count)
        {
            ObjectFactory = objectFactory ?? throw new ArgumentNullException(nameof(objectFactory));
            _count = count;
        }

        public T Pop(TPath path, TParent parent)
        {
            if (_objects.Count <= 0)
            {
                CreateObjects(_count * (AdditionalSize - 1), path, parent);
            }

            T obj = _objects.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Push(T obj)
        {
            obj.gameObject.SetActive(false);
            _objects.Enqueue(obj);
        }

        private void CreateObjects(int count, TPath path, TParent parent)
        {
            for (var i = 0; i < count; i++)
            {
                T obj = ObjectFactory.Invoke(path, parent);
                obj.gameObject.SetActive(false);
                _count++;
                _objects.Enqueue(obj);
            }
        }
    }
}