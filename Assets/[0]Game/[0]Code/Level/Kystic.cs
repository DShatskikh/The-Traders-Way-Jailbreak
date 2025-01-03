﻿using MoreMountains.Feedbacks;
using PixelCrushers.DialogueSystem;
using UnityEngine;

namespace Game
{
    public class Kystic : MonoBehaviour, IUseObject
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        [SerializeField]
        private MMF_Player _mmfPlayer;

        [SerializeField]
        private DialogueSystemTrigger _dialogueSystemTrigger;
        
        private StockMarketService _stockMarketService;

        [Inject]
        private void Construct(StockMarketService stockMarketService)
        {
            _stockMarketService = stockMarketService;
        }
        
        public void Use()
        {
            SoundPlayer.Play(AssetProvider.Instance.BuySound);
            _particleSystem.Play();
            _mmfPlayer.PlayFeedbacks();
            _stockMarketService.AddItem("Kystic");
            _dialogueSystemTrigger.OnUse();
        }
    }
}