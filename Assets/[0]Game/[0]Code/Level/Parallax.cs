using Unity.Cinemachine;
using UnityEngine;

namespace Game
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField]
        private float _multiplyDistance = 1f;
        
        private Transform _camera;
        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = transform.position;
            _camera = Camera.main.transform;
        }

        private void Update()
        {
            var x = (_startPosition - _camera.position).x * _multiplyDistance;
            transform.SetX(x);
        }
    }
}