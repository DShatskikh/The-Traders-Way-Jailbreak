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

        public IEnumerator AwaitShowTimer()
        {
            yield return _mmfPlayer.PlayFeedbacksCoroutine(Vector3.zero);
            _timerIcon.fillAmount = 1;
            yield return _timerIcon.DOFillAmount(0, 3).WaitForCompletion();
        }
    }
}