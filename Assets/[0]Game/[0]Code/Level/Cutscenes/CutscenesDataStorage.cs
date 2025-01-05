using System;
using System.Collections.Generic;
using PixelCrushers;

namespace Game
{
    public class CutscenesDataStorage
    {
        private static CutscenesDataStorage _instance;
        private readonly Dictionary<string, string> _dictionary = new();

        public static void Init()
        {
            _instance = new CutscenesDataStorage();
        }

        public static void SetData<T>(string id, T saveData)
        {
            if (!_instance._dictionary.TryAdd(id, SaveSystem.Serialize(saveData)))
            {
                _instance._dictionary[id] = SaveSystem.Serialize(saveData);
            }
        }

        public static T GetData<T>(string id) where T : new()
        {
            if (_instance._dictionary.TryGetValue(id, out var result))
                return SaveSystem.Deserialize<T>(result);

            return new T();
        }
    }
}