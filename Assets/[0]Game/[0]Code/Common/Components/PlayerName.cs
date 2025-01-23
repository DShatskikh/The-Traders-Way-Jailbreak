using System;

namespace Game
{
    [Serializable]
    public struct PlayerName
    {
        public string Name;

        public PlayerName(string playerName)
        {
            Name = playerName;
        }
    }
}