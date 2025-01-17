using System.Collections;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class ADSScreen : ScreenBase
    {
        [SerializeField]
        private MMF_Player _mmfPlayer;

        [SerializeField]
        private Image _timerIcon;
        
        private GameStateController _gameStateController;

        [Inject]
        private void Construct(GameStateController gameStateController)
        {
            _gameStateController = gameStateController;
        }

        public IEnumerator AwaitShowTimer()
        {
            yield return _mmfPlayer.PlayFeedbacksCoroutine(Vector3.zero);
            yield return _timerIcon.DOFillAmount(0, 3).WaitForCompletion();
        }
    }
}