using Game;
using UnityEngine;

namespace RimuruDev
{
    public class AndroidObjectDisabler : MonoBehaviour
    {
        private DeviceTypeDetector _deviceTypeDetector;

        [Inject]
        private void Construct(DeviceTypeDetector deviceTypeDetector)
        {
            _deviceTypeDetector = deviceTypeDetector;
        }

        private void Start()
        {
            if (_deviceTypeDetector.DeviceType == DeviceType.WebMobile)
                gameObject.SetActive(false);
        }
    }
}