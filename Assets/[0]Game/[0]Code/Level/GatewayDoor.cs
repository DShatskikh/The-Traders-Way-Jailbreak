using UnityEngine;

namespace Game
{
    public class GatewayDoor : MonoBehaviour
    {
        private const float MaxY = 2f;
        private const float Speed = 2f;
        private const float ActivateDistance = 2f;
        
        [SerializeField]
        private Transform _door;
        
        private Player _player;
        private float _startY;
        private bool _isOpen;
        
        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        private void Start()
        {
            _startY = _door.localPosition.y;
        }

        private void Update()
        {
            var y = _door.localPosition.y;

            if (Vector2.Distance(transform.position, _player.transform.position) > ActivateDistance)
            {
                y -= Time.deltaTime * Speed;
                
                if (_isOpen)
                    SoundPlayer.Play(AssetProvider.Instance.SpaceDoorCloseSound);
                
                _isOpen = false;
            }
            else
            {
                y += Time.deltaTime * Speed * 2;
                
                if (!_isOpen)
                    SoundPlayer.Play(AssetProvider.Instance.SpaceDoorOpenSound);
                
                _isOpen = true;
            }
            
            if (y < _startY)
                y = _startY;

            if (y > MaxY)
                y = MaxY;

            _door.localPosition = _door.localPosition.SetY(y);
        }
    }
}