using System.Collections.Generic;
using PixelCrushers;

namespace Game
{
    public sealed class RepositoryStorage
    {
        private static RepositoryStorage _instance;
        private readonly Dictionary<string, string> _dictionary = new();

        public static void Init()
        {
            _instance = new RepositoryStorage();
        }

        public static void Set<T>(string id, T saveData)
        {
            if (!_instance._dictionary.TryAdd(id, SaveSystem.Serialize(saveData)))
            {
                _instance._dictionary[id] = SaveSystem.Serialize(saveData);
            }
        }

        public static T Get<T>(string id) where T : new()
        {
            if (_instance._dictionary.TryGetValue(id, out var result))
                return SaveSystem.Deserialize<T>(result);

            return new T();
        }
    }
}