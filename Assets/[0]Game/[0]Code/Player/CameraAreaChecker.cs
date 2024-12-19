using System;
using Unity.Cinemachine;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CameraAreaChecker
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
            Collider2D[] colliders = Physics2D.OverlapPointAll(_point.position, _mask);

            float minDistance = float.MaxValue;
            CameraArea nearestUseObject = null;

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out CameraArea cameraArea))
                {
                    var currentDistance = Vector2.Distance(_point.position, cameraArea.transform.position);
                    
                    if (minDistance > currentDistance)
                    {
                        minDistance = currentDistance;
                        nearestUseObject = cameraArea;
                    }
                }
            }

            _cinemachineConfiner2D.BoundingShape2D = nearestUseObject 
                ? nearestUseObject.GetComponent<PolygonCollider2D>() : null;
        }
    }
}