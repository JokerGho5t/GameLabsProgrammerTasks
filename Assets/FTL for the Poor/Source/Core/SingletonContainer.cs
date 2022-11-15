using System;
using System.Collections.Generic;

namespace Ships
{
    public class SingletonContainer
    {
        public static SingletonContainer Instance { get; } = new SingletonContainer();
        private readonly Dictionary<Type, object> m_Container = new Dictionary<Type, object>();

        public void Add<T>(object obj)
        {
            var type = typeof(T);

            if (m_Container.ContainsKey(type))
                throw new Exception($"{type} is already in the container!");
            
            m_Container.Add(type, obj);
        }

        public T Get<T>()
        {
            var type = typeof(T);

            if (!m_Container.ContainsKey(type))
                throw new Exception($"Before you get the {type}, you need to add it to the container!");

            return (T)m_Container[type];
        }

        public void Remove<T>()
        {
            var type = typeof(T);

            if (!m_Container.ContainsKey(type))
                throw new Exception($"To remove the {type}, the object must be in the container!");

            m_Container.Remove(type);
        }
    }
}