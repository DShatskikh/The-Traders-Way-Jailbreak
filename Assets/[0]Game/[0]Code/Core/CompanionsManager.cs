using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class CompanionsManager : IGameUpdateListener
    {
        [SerializeField]
        private Companion[] _companions;
        
        private List<Companion> _activeCompanions = new();
        private Player _player;
        public List<Companion> GetAllCompanions => _activeCompanions;
        public float Speed = 1;
        public float RunSpeed = 2;
        public float FlipInterval = 0.5f;
        public float ResetTargetPositionDuration => _player.IsRun ? 0.5f / 2 : 0.5f;
        public float GetSpeed => _player.IsRun ? RunSpeed : Speed;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        public void Init()
        {
            _player.StopMove += OnStop;
        }
        
        public bool TryActivateCompanion(string id, Vector2 position, out Companion companion, bool isFlip)
        {
            companion = GetCompanion(id);

            if (companion)
            {
                var target = _activeCompanions.Count > 0
                    ? _activeCompanions[_activeCompanions.Count - 1].transform
                    : ServiceLocator.Get<Player>().transform;

                companion.Activate(target, position, isFlip);
                _activeCompanions.Add(companion);
                return true;
            }

            return false;
        }

        public bool TryDeactivateCompanion(string id)
        {
            var companion = GetCompanion(id);
            var isHave = false;
            
            foreach (var activeCompanion in _activeCompanions)
            {
                if (activeCompanion == companion)
                {
                    isHave = true;
                    break;
                }
            }

            if (!isHave)
                return false;
            
            if (companion)
            {
                _activeCompanions.Remove(companion);
                companion.Deactivate();
                return true;
            }

            return false;
        }
        
        public Companion GetCompanion(string companionName)
        {
            foreach (var companion in _companions)
            {
                if (companionName == companion.GetId)
                    return companion;
            }

            throw new Exception($"Not Companion: {companionName}");
        }

        public void OnUpdate()
        {
            if (_player.IsMove)
            {
                foreach (var companion in _activeCompanions) 
                    companion.OnMove();
            }
        }

        private void OnStop()
        {
            foreach (var companion in _activeCompanions) 
                companion.OnStop();
        }
    }
}