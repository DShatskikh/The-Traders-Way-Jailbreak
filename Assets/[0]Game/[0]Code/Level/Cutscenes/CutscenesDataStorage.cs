using System;
using System.Collections.Generic;
using PixelCrushers;

namespace Game
{
    public class CutscenesDataStorage
    {
        private static CutscenesDataStorage _instance;
        private readonly Dictionary<Type, string> _dictionary = new();

        public static void Init()
        {
            _instance = new CutscenesDataStorage();
        }

        public static void SetData<T>(T saveData) => 
            _instance._dictionary.Add(typeof(T), SaveSystem.Serialize(saveData));

        public static T GetData<T>() where T : new()
        {
            if (_instance._dictionary.TryGetValue(typeof(T), out var result))
                return SaveSystem.Deserialize<T>(result);

            return new T();
        }
    }
}