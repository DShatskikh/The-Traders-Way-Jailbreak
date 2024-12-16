using System;
using System.Collections.Generic;

namespace Game
{
    public static class DIContainer
    {
        private static Dictionary<Type, object> _container = new();

        public static void Register<T>(T item) => 
            _container.Add(typeof(T), item);

        public static T Get<T>() => 
            (T)_container[typeof(T)];
    }
}