using UnityEngine;

namespace Game
{
    public abstract class BaseScreen : MonoBehaviour
    {
        public virtual void Show() => 
            gameObject.SetActive(true);

        public virtual void Hide() => 
            gameObject.SetActive(false);

        public virtual void Bind(object args) { }
    }
}