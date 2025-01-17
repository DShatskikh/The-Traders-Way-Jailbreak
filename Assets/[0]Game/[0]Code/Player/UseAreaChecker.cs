using System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class UseAreaChecker
    {
        [SerializeField]
        private float _radius;

        [SerializeField]
        private Transform _point;
        
        public ReactiveProperty<MonoBehaviour> NearestUseObject = new();

        public void Search()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_point.position, _radius);

            float minDistance = float.MaxValue;
            MonoBehaviour nearestUseObject = null;

            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out IUseObject useObject))
                {
                    var currentDistance = Vector2.Distance(_point.position, ((MonoBehaviour)useObject).transform.position);
                    
                    if (minDistance > currentDistance)
                    {
                        minDistance = currentDistance;
                        nearestUseObject = (MonoBehaviour)useObject;
                    }
                }
                else if (collider.TryGetComponent(out Usable usable))
                {
                    var currentDistance = Vector2.Distance(_point.position, usable.transform.position);
                    
                    if (minDistance > currentDistance)
                    {
                        minDistance = currentDistance;
                        nearestUseObject = usable;
                    }
                }
            }

            if (nearestUseObject != null)
            {
                if (nearestUseObject != NearestUseObject.Value)
                {
                    Found(nearestUseObject);
                }
            }
            else if (NearestUseObject.Value)
                Lost();
        }

        public void Found(MonoBehaviour nearestUseObject)
        {
            NearestUseObject.Value = nearestUseObject;
        }
        
        public void Lost()
        {
            NearestUseObject.Value = null;
        }

        public void Use()
        {
            if (!NearestUseObject.Value)
                return;

            switch (NearestUseObject.Value)
            {
                case IUseObject useObject:
                    useObject.Use();
                    break;
                case Usable currentUsable:
                {
                    currentUsable.OnUseUsable();
                    if (currentUsable != null)
                        currentUsable.gameObject.BroadcastMessage("OnUse", _point,
                            SendMessageOptions.DontRequireReceiver);
                    break;
                }
            }
        }
    }
}