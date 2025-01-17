using UnityEngine;

namespace Game
{
    public sealed class Ladder : MonoBehaviour
    {
        [SerializeField]
        private Transform _target, _start;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                bool isRight = Vector2.Distance(player.transform.position, _start.position) >
                               Vector2.Distance(player.transform.position, _target.position);
                player.SetMover(new CharacterLadderMover(player.transform, _start.position, _target.position, isRight));
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.ResetMover();
            }
        }
    }
}