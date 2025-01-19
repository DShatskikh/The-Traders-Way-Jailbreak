using System;
using UnityEngine;

namespace Game
{
    public class SecretLocation : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _slimeSpriteRenderer, _pigSpriteRenderer;

        private void Start()
        {
            var data = RepositoryStorage.Get<EndsData>(KeyConstants.Ending);
            
            if (data.IsDefaultEnding)
            {
                var pigPoint = _pigSpriteRenderer.transform.position;
                var slimePoint = _slimeSpriteRenderer.transform.position;
                
                _slimeSpriteRenderer.flipX = true;
                _slimeSpriteRenderer.transform.position = pigPoint;
                
                _pigSpriteRenderer.flipX = false;
                _pigSpriteRenderer.transform.position = slimePoint;
            }
        }
    }
}