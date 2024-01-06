using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Services.GOPool
{
    public class ObjectPool<T, TPath, TParent> where T : Component where TPath : class where TParent : Transform
    {
        private readonly int _additionalSize = 2;
        private readonly Queue<T> _objects = new Queue<T>();
        private readonly Func<TPath, TParent, T> _objectFactory;
        private int _count;

        public ObjectPool(Func<TPath, TParent, T> objectFactory, int count)
        {
            _objectFactory = objectFactory ?? throw new ArgumentNullException(nameof(objectFactory));
            _count = count;
        }

        public List<T> GetAll() => _objects.ToList();

        public T Pop(TPath path, TParent parent)
        {
            if (_objects.Count <= 0)
            {
                CreateObjects(_count * (_additionalSize - 1), path, parent);
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
                CreateObject(path, parent);
            }
        }

        private void CreateObject(TPath path, TParent parent)
        {
            T obj = _objectFactory.Invoke(path, parent);
            obj.gameObject.SetActive(false);
            _count++;
            _objects.Enqueue(obj);
        }
    }
}