using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class CameraAreaChecker
    {
        [SerializeField]
        private Transform _point;

        [SerializeField]
        private LayerMask _mask;

        private CinemachineConfiner2D _cinemachineConfiner2D;

        public void Init()
        {
            _cinemachineConfiner2D = ServiceLocator.Get<CinemachineConfiner2D>();
        }
        
        public void Check()
        {
            RaycastHit2D hit = Physics2D.Raycast(_point.position, Vector2.down, 0.1f, _mask);
            CameraArea nearestUseObject = null;
            
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out CameraArea cameraArea))
                {
                    nearestUseObject = cameraArea;
                }
            }
            
            _cinemachineConfiner2D.BoundingShape2D = nearestUseObject 
                ? nearestUseObject.GetComponent<PolygonCollider2D>() : null;
        }
    }
}