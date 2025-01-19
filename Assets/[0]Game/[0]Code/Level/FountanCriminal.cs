using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class FountanCriminal : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer[] _spriteRenderer;

        [SerializeField]
        private Transform[] _positions;

        [SerializeField]
        private float _upgradeSortingY = 0.2f;
        
        public float Speed = 5;

        private void Start()
        {
            foreach (var spriteRenderer in _spriteRenderer)
            {
                StartCoroutine(AwaitMove(spriteRenderer, GetPointIndex(FindNearestPoint(spriteRenderer.transform))));
            }
        }

        private IEnumerator AwaitMove(SpriteRenderer spriteRenderer, int startIndex)
        {
            int index = startIndex;
            
            while (true)
            {
                if (index >= _positions.Length)
                    index = 0;
                
                var point = _positions[index];
                
                float step = Speed * Time.deltaTime;
                while (Vector3.Distance(spriteRenderer.transform.position, point.position) > 0.01f)
                {
                    spriteRenderer.transform.position = Vector3.MoveTowards(spriteRenderer.transform.position, point.position, step);
                    spriteRenderer.sortingOrder = spriteRenderer.transform.localPosition.y >= _upgradeSortingY ? 0 : 2;
                    spriteRenderer.flipX = spriteRenderer.transform.position.x > point.position.x;
                    yield return null;
                }

                index++;
                yield return null;
            }
        }

        private int GetPointIndex(Transform nearest)
        {
            for (int i = 0; i < _positions.Length; i++)
            {
                if (_positions[i] == nearest)
                    return i;
            }

            throw new Exception();
        }
        
        private Transform FindNearestPoint(Transform entityTransform)
        {
            Transform nearest = _positions[0];
            float minDistance = Vector3.Distance(entityTransform.position, nearest.position);

            foreach (Transform point in _positions)
            {
                float distance = Vector3.Distance(entityTransform.position, point.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = point;
                }
            }

            return nearest;
        }
    }
}