using System;
using UnityEngine;

namespace Game
{
    public sealed class Barrier : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Player _player;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _spriteRenderer.color = _spriteRenderer.color.SetA(0);
        }

        private void Update()
        {
            var alpha = _spriteRenderer.color.a;

            if (alpha < 0)
                alpha = 0;

            if (alpha > 1)
                alpha = 1;

            if (Vector2.Distance(transform.position, _player.transform.position) > 1)
            {
                _spriteRenderer.color = _spriteRenderer.color.SetA(alpha - Time.deltaTime * 2);
            }
            else
            {
                _spriteRenderer.color = _spriteRenderer.color.SetA(alpha + Time.deltaTime * 2);
            }
        }
    }
}