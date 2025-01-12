using System;
using System.Collections;
using UnityEngine;
using YG;

namespace Game
{
    [Serializable]
    public class ADSTimer : IGameStartListener
    {
        private CoroutineRunner _coroutineRunner;
        private GameStateController _gameStateController;

        private void Construct(CoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public void OnStartGame()
        {
            CoroutineRunner.Instance.StartCoroutine(CheckTimerAd());
        }
        
        private IEnumerator CheckTimerAd()
        {
            while (true)
            {
                if (YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval
                    && Time.timeScale != 0 && _gameStateController.CurrentState == GameStateController.GameState.PLAYING)
                {
                    _coroutineRunner.StartCoroutine(TimerAdShow());
                    yield break;
                }

                yield return new WaitForSeconds(1.0f);
                yield return new WaitForSecondsRealtime(1.0f);
            }
        }

        private IEnumerator TimerAdShow()
        {
            while (true)
            {
                //Тут показать отчет
                
                YandexGame.FullscreenShow();

                while (!YandexGame.nowFullAd)
                    yield return null;

                yield break;
            }
        }
    }
}