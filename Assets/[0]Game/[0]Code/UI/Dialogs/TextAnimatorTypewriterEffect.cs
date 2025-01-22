using Febucci.UI;
using MoreMountains.Feedbacks;
using PixelCrushers.DialogueSystem;
using RimuruDev;
using UnityEngine;
using UnityEngine.InputSystem;
using DeviceType = RimuruDev.DeviceType;

namespace Game
{
    public sealed class TextAnimatorTypewriterEffect : AbstractTypewriterEffect
    {
        [Header("TextAnimator")]
        [SerializeField]
        private TextAnimatorPlayer _textAnimatorPlayer;
       
        [SerializeField]
        private AudioSource _audioSource, _audioSource_2;

        [SerializeField]
        private GameObject _button;
        
        [SerializeField]
        private MMF_Player _iconMmfPlayer;

        [SerializeField]
        private GameObject _hint;

        [SerializeField]
        private GameObject _namePanel;

        private bool _isPlaying;
        private PlayerInput _playerInput;
        private DeviceTypeDetector _deviceTypeDetector;
        private bool _isFirstAudioSource;

        public override bool isPlaying => _isPlaying;

        public override void Awake()
        {
            base.Awake();
            _playerInput = ServiceLocator.Get<PlayerInput>();
            _deviceTypeDetector = ServiceLocator.Get<DeviceTypeDetector>();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            gameObject.SetActive(true);
            _textAnimatorPlayer.onCharacterVisible.AddListener((c) => OnWrite());
            _textAnimatorPlayer.onTextShowed.AddListener(Stop);
            _textAnimatorPlayer.onTypewriterStart.AddListener(OnTypewriterStart);
            _button.SetActive(false);
            
            _playerInput.actions["Submit"].canceled += ShowAllText;
            _playerInput.actions["Cancel"].canceled += ShowAllText;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _textAnimatorPlayer.onCharacterVisible.RemoveListener((c) => OnWrite());
            _textAnimatorPlayer.onTextShowed.RemoveListener(Stop);
            _textAnimatorPlayer.onTypewriterStart.RemoveListener(OnTypewriterStart);
            gameObject.SetActive(false);

            if (_playerInput)
            {
                _playerInput.actions["Submit"].canceled -= ShowAllText;
                _playerInput.actions["Cancel"].canceled -= ShowAllText;
            }
        }

        public override void Start() { }

        public override void Stop()
        {
            _isPlaying = false;
            _button.SetActive(true);
            _hint.SetActive(false);
            StopTyping();
        }

        public override void StartTyping(string text, int fromIndex = 0)
        {
            var actorName = DialogueManager.currentConversationState.subtitle.speakerInfo.nameInDatabase;
            var clipName = DialogueManager.masterDatabase.GetActor(actorName).LookupValue("AudioClip");
            var clipPath = "AudioClips/" + (clipName != "" ? clipName : "snd_txtlan_ch1");
            var clip = Resources.Load<AudioClip>(clipPath);
            _audioSource.clip = clip;
            _audioSource_2.clip = clip;

            var useDisplayName = DialogueManager.masterDatabase.GetActor(actorName).LookupValue("Display Name");
            _namePanel.SetActive(useDisplayName != string.Empty);
                
            _textAnimatorPlayer.StartShowingText();
        }

        public override void StopTyping()
        {
            _button.SetActive(true);
        }

        private void OnWrite()
        {
            _isFirstAudioSource = !_isFirstAudioSource;
            
            if (_isFirstAudioSource)
                _audioSource.Play();
            else
                _audioSource_2.Play();
        }

        private void OnTypewriterStart()
        {
            _isPlaying = true;
            _button.SetActive(false);
            
            if (_deviceTypeDetector.DeviceType == DeviceType.WebPC)
                _hint.SetActive(true);
            
            _iconMmfPlayer.PlayFeedbacks();
        }

        private void ShowAllText(InputAction.CallbackContext obj)
        {
            _textAnimatorPlayer.SkipTypewriter();
        }
    }
}