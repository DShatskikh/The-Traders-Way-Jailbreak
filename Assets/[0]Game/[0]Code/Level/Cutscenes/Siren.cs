using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public sealed class Siren : MonoBehaviour
    {
        [SerializeField]
        private Image _image;

        private Sequence _sequence;
        private AllBuyCheckHandler _allBuyCheckHandler;
        private bool _isShow;

        [Inject]
        private void Construct(AllBuyCheckHandler allBuyCheckHandler)
        {
            _allBuyCheckHandler = allBuyCheckHandler;
            
            _allBuyCheckHandler.AllBuy += AllBuyCheckHandlerOnAllBuy;
        }

        private void Start()
        {
            var saveData = RepositoryStorage.Get<MyCellCutscene.SaveData>(KeyConstants.MyCellCutscene);

            if (saveData.State == MyCellCutscene.State.DontPayTax)
            {
                AllBuyCheckHandlerOnAllBuy();
            }
            else
            {
                _image.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (!_isShow)
                return;

            if (RepositoryStorage.Get<MyCellCutscene.SaveData>(KeyConstants.MyCellCutscene).State !=
                MyCellCutscene.State.DontPayTax)
            {
                _isShow = false;
                OnDestroy();
                _image.color = Color.clear;
            }
        }

        private void AllBuyCheckHandlerOnAllBuy()
        {
            _isShow = true;
            
            _image.gameObject.SetActive(true);
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence
                .Append(_image.DOColor(_image.color.SetA(0.2f), 1))
                .Append(_image.DOColor(_image.color.SetA(0f), 1))
                .SetDelay(1).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
            _allBuyCheckHandler.AllBuy -= AllBuyCheckHandlerOnAllBuy;
        }
    }
}