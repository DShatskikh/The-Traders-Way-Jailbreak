using UnityEngine;

namespace Game
{
    public class ReflectionPlayer : MonoBehaviour
    {
        private Player _player;
        private SpriteRenderer _spriteRenderer;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _spriteRenderer.flipY = true;
        }

        private void Update()
        {
            transform.position = _player.transform.position;
            _spriteRenderer.flipX = _player.GetFlipX;
            _spriteRenderer.sprite = _player.GetSprite;
        }
    }
}