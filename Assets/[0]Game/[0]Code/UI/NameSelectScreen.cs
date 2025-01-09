using System.Collections;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game
{
    public class NameSelectScreen : ScreenBase
    {
        private const string DefaultName = "Денис";
        
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField]
        private Button _button;

        [SerializeField]
        private TMP_InputField _inputField;
        
        [SerializeField]
        private Transform _rocket;

        [SerializeField]
        private Transform[] _rocketStartPoints;

        [SerializeField]
        private Transform _luckyBlock;
        
        private Player _player;
        private ScreenManager _screenManager;
        private LocationsManager _locationsManager;
        private GameStateController _gameStateController;

        [Inject]
        private void Construct(Player player, ScreenManager screenManager, LocationsManager locationsManager, GameStateController gameStateController)
        {
            _player = player;
            _screenManager = screenManager;
            _locationsManager = locationsManager;
            _gameStateController = gameStateController;
        }
        
        private void Start()
        {
            _screenManager.Hide(ScreenType.Main);
            _button.onClick.AddListener(OnClick);
            _player.gameObject.SetActive(false);

            StartCoroutine(AwaitPlayerAnimation());
            StartCoroutine(AwaitRocketAnimation());
            StartCoroutine(AwaitLuckyBlockAnimation());
        }

        private IEnumerator AwaitPlayerAnimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                var state = Random.Range(0, 5);
                _animator.SetFloat("State", state);
                _spriteRenderer.flipX = Random.Range(0, 2) == 0;
            }
        }

        private IEnumerator AwaitRocketAnimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(3);
                _rocket.gameObject.SetActive(true);

                var rocketStartPoint = _rocketStartPoints[Random.Range(0, _rocketStartPoints.Length)];
                _rocket.SetParent(rocketStartPoint);
                
                _rocket.position = rocketStartPoint.position;
                _rocket.rotation = rocketStartPoint.rotation;

                var sequrnce = DOTween.Sequence();
                yield return sequrnce.Append(_rocket.DOLocalMoveY(_rocket.localPosition.y + 30, 6)).WaitForCompletion();
                _rocket.gameObject.SetActive(false);
            }
        }

        private IEnumerator AwaitLuckyBlockAnimation()
        {
            while (true)
            {
                var sequrnce = DOTween.Sequence();
                yield return sequrnce
                    .Append(_luckyBlock.DOLocalMoveY(_luckyBlock.localPosition.y + 10, 20))
                    .Insert(0, _luckyBlock.DORotate(new Vector3(0, 0, _luckyBlock.localRotation.z + 90), 20))
                    .WaitForCompletion();

                if (_luckyBlock.transform.position.y > 10)
                    _luckyBlock.transform.position = _luckyBlock.transform.position.SetY(-10);
            }
        }

        private void OnClick()
        {
            _button.onClick.RemoveAllListeners();
            Hide();
            var playerName = _inputField.text;

            if (playerName == string.Empty)
                playerName = DefaultName;
            
            CutscenesDataStorage.SetData("Name", playerName);
            Lua.Run($"Variable[\"PlayerName\"] = \"{playerName}\"");
            _player.gameObject.SetActive(true);
            _locationsManager.SwitchLocation("World", 0);
            _gameStateController.StartGame();
            
            _screenManager.Show(ScreenType.Main);
            Destroy(gameObject);
        }
    }
}