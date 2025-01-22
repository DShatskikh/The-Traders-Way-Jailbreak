using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game
{
    public interface ISecretSlotPresenter : IDisposable
    {
        void Select();
        void Deselect();
        ISecretSlotData GetConfig { get; }
    }

    public class SecretSlotPresenter : ISecretSlotPresenter
    {
        private readonly SecretSlotView _view;
        private readonly ISecretPresenter _secretPresenter;

        public ISecretSlotData GetConfig { get; }

        public SecretSlotPresenter(ISecretSlotData config, SecretSlotView prefab, Transform container, ISecretPresenter secretPresenter)
        {
            GetConfig = config;
            _view = Object.Instantiate(prefab,container);
            _secretPresenter = secretPresenter;
            _view.SetIcon(config.GetIcon);
            LocalizedTextUtility.Load(config.GetName, text => _view.SetLabel(text));
            _view.PointerDown += ViewOnPointerDown;
            _view.PointerUp += ViewOnPointerUp;
        }

        private void ViewOnPointerDown()
        {
            _secretPresenter.Select(this);
        }

        private void ViewOnPointerUp()
        {
            
        }

        public void Dispose()
        {
            _view.PointerDown -= ViewOnPointerDown;
            _view.PointerUp -= ViewOnPointerUp;
            Object.Destroy(_view.gameObject);
        }

        public void Select()
        {
            _view.SetLabelColor(Color.yellow);
            SoundPlayer.Play(AssetProvider.Instance.ClickSound);
        }

        public void Deselect()
        {
            _view.SetLabelColor(Color.white);
        }
    }
}