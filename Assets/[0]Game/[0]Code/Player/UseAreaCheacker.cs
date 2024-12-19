using System;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class UseAreaCheacker
    {
        [SerializeField]
        private float _radius;

        [SerializeField]
        private Transform _point;
        
        private MonoBehaviour _nearestUseObject;

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
                if (nearestUseObject != _nearestUseObject)
                {
                    Found(nearestUseObject);
                }
            }
            else if (_nearestUseObject)
                Lost();
        }

        public void Found(MonoBehaviour nearestUseObject)
        {
            /*EventBus.SubmitUp = () => Use(nearestUseObject);

            if (GameData.DeviceType == CurrentDeviceType.Mobile)
            {
                GameData.UseButton.gameObject.SetActive(true);
            
                if (nearestUseObject.TryGetComponent(out IUseName useName))
                    GameData.UseButton.SetText(useName.Name);
                else
                    GameData.UseButton.ResetText();
            }*/

            _nearestUseObject = nearestUseObject;
        }
        
        public void Lost()
        {
            /*EventBus.SubmitUp = null;

            if (GameData.DeviceType == CurrentDeviceType.Mobile)
            {
                GameData.UseButton.gameObject.SetActive(false);
            }*/

            _nearestUseObject = null;
        }

        public void Use()
        {
            //GameData.UseButton.gameObject.SetActive(false);
            
            if (!_nearestUseObject)
                return;

            switch (_nearestUseObject)
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