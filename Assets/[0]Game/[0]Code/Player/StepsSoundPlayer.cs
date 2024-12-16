using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    [Serializable]
    public class StepsSoundPlayer
    {
        [SerializeField] 
        private AudioSource _stepSource1, _stepSource2;

        [SerializeField] 
        private float _intervalStep = 0.7f;

        [SerializeField]
        private LayerMask _layerMask;
        
        private bool _isStepRight;
        private float _currentStepTime;
        
        private readonly Transform _transform;
        private readonly AssetProvider _assetProvider;
        private bool _isRun;

        public StepsSoundPlayer(Transform transform, AssetProvider assetProvider)
        {
            _transform = transform;
            _assetProvider = assetProvider;
        }

        public void OnSpeedChange(float value)
        {
            if (value == 0)
                return;
            
            _currentStepTime += Time.deltaTime;
                
            if (_currentStepTime >= (_isRun ? _intervalStep / 2 : _intervalStep))
            {
                _currentStepTime = 0;
                RaycastHit2D hit = Physics2D.Raycast(_transform.position, Vector2.down, 0.1f, _layerMask);
                TileBase tile = null;
                
                if (hit.collider != null)
                {
                    var tilemap = hit.collider.GetComponent<Tilemap>();
                    Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
                    tile = tilemap.GetTile(cellPosition);
                }

                PlayFootstepSound(tile);
                _isStepRight = !_isStepRight;
            }
        }

        public void OnIsRunChange(bool value) => 
            _isRun = value;

        private void PlayFootstepSound(TileBase tile)
        {
            var config = _assetProvider.StepSoundPairsConfig;
            var pair = _assetProvider.TileTagConfig.GetPair(tile, config);
            
            if (_isStepRight)
            {
                AudioClip clipToPlay = pair.Right;
                _stepSource1.clip = clipToPlay;
                _stepSource1.Play();
            }
            else
            {
                AudioClip clipToPlay = pair.Left;
                _stepSource2.clip = clipToPlay;
                _stepSource2.Play();   
            }
        }
    }
}