using System;
using System.Collections.Generic;

namespace DC.Animator.Utils
{
    public class ObjectPool<T> where T : class
    {
        private readonly Stack<T> _pool = new Stack<T>();
        private readonly Func<T> _createFunc;
        
        public ObjectPool(Func<T> createFunc) => _createFunc = createFunc;

        public T Get() => _pool.Count > 0 ? _pool.Pop() : _createFunc();

        public void Return(T obj) => _pool.Push(obj);

        public void Clear() => _pool.Clear();

        public int Count => _pool.Count;
    }
}