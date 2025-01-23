using System;
using System.Collections;
using UnityEngine;
using YG;

namespace Game
{
    [Serializable]
    public class ADSTimer : IGameStartListener
    {
        [SerializeField]
        private ADSScreen _adsScreen;
        
        private CoroutineRunner _coroutineRunner;
        private GameStateController _gameStateController;

        [Inject]
        private void Construct(CoroutineRunner coroutineRunner, GameStateController gameStateController)
        {
            _coroutineRunner = coroutineRunner;
            _gameStateController = gameStateController;
        }
        
        public void OnStartGame()
        {
            CoroutineRunner.Instance.StartCoroutine(CheckTimerAd());
            //CoroutineRunner.Instance.StartCoroutine(TimerAdShow());
        }
        
        private IEnumerator CheckTimerAd()
        {
            while (true)
            {
                if (YG2.timerInterAdv >= YG2.infoYG.InterstitialAdv.interAdvInterval
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

                _gameStateController.OpenADS();
                _adsScreen.gameObject.SetActive(true);
                yield return _adsScreen.AwaitShowTimer();
                _adsScreen.gameObject.SetActive(false);
                
                YG2.InterstitialAdvShow();

                while (!YG2.nowInterAdv)
                    yield return null;

                _gameStateController.CloseADS();
                yield break;
            }
        }
    }
}