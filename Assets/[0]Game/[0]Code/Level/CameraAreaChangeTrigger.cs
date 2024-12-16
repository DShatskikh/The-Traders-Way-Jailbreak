using Unity.Cinemachine;
using UnityEngine;

namespace Game
{
    public sealed class CameraAreaChangeTrigger : MonoBehaviour
    {
        private CinemachineConfiner2D _cinemachineConfiner;
        
        private void Awake()
        {
            _cinemachineConfiner = DIContainer.Get<CinemachineConfiner2D>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out ICameraAreaChecker cameraAreaChecker))
            {
                var collider = GetComponent<PolygonCollider2D>();
                
                if (_cinemachineConfiner.BoundingShape2D != null && _cinemachineConfiner.BoundingShape2D != collider)
                    _cinemachineConfiner.BoundingShape2D = collider;
            }
        }
    }
}