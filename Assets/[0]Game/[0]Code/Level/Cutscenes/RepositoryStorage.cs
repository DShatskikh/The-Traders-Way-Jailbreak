using System.Collections.Generic;
using PixelCrushers;

namespace Game
{
    public sealed class RepositoryStorage
    {
        private static RepositoryStorage _instance;
        public static IReadOnlyDictionary<string, string> Container => _instance._dictionary;
        private readonly Dictionary<string, string> _dictionary;

        public RepositoryStorage(Dictionary<string, string> dictionary)
        {
            _dictionary = dictionary;
            _instance = this;
        }

        public static void Set<T>(string id, T saveData)
        {
            if (!_instance._dictionary.TryAdd(id, SaveSystem.Serialize(saveData)))
                _instance._dictionary[id] = SaveSystem.Serialize(saveData);
        }

        public static T Get<T>(string id) where T : new()
        {
            if (_instance._dictionary.TryGetValue(id, out var result))
                return SaveSystem.Deserialize<T>(result);

            return new T();
        }
    }
}