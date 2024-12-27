using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> _container = new();

        public static void Register<T>(T item) => 
            _container.Add(typeof(T), item);

        public static T Get<T>() => 
            (T)_container[typeof(T)];
        
        public static object Get(Type T)
        {
            if (_container.TryGetValue(T, out var value))
                return value;
            
            throw new Exception($"Не хватает типа: {T}");
        }
    }
}