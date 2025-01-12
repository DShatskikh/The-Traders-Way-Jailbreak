using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game
{
    public class ADSScreen : ScreenBase
    {
        [SerializeField]
        private MMF_Player _mmfPlayer;
        
        private GameStateController _gameStateController;

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }
    }
}