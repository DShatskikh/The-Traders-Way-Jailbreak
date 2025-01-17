using UnityEngine;

namespace Game
{
    public sealed class HatView : MonoBehaviour
    {
        [SerializeField]
        private SerializablePair<HatBaseConfig, GameObject>[] _hats;

        [SerializeField]
        private Transform _hatPoint;

        public Transform HatPoint => _hatPoint;
        
        private HatManager _hatManager;

        [Inject]
        private void Construct(HatManager hatManager)
        {
            _hatManager = hatManager;
        }

        private void OnEnable()
        {
            UpgradeHat(_hatManager.GetCurrentHat);
            _hatManager.OnHatUpgrade += UpgradeHat;
        }

        private void OnDisable()
        {
            _hatManager.OnHatUpgrade -= UpgradeHat;
        }

        private void UpgradeHat(HatData data)
        {
            foreach (var hat in _hats)
            {
                if (data != null)
                    hat.Value.SetActive(hat.Key == data.Config);
                else
                    hat.Value.SetActive(false);
            }
        }
    }
}