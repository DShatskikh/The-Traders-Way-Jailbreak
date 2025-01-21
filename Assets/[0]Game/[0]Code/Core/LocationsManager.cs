using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Analytics;
using Object = UnityEngine.Object;

namespace Game
{
    [Serializable]
    public sealed class LocationsManager
    {
        [SerializeField]
        private Transform _container;

        private Location[] _locations;
        private Location _currentLocation;
        private Player _player;
        private CinemachineConfiner2D _camera;
        private GameStateController _gameStateController;
        private ISaveLoadService _saveLoadService;
        private IAnalyticsService _analyticsService;

        [Serializable]
        public class Data
        {
            public string LocationName = "World";
            public int PointIndex;
            public float CharacterPositionX;
            public float CharacterPositionY;
        }

        [Inject]
        private void Construct(ISaveLoadService saveLoadService, IAnalyticsService analyticsService)
        {
            _saveLoadService = saveLoadService;
            _analyticsService = analyticsService;
        }
        
        public void Init()
        {
            _player = ServiceLocator.Get<Player>();
            _camera = ServiceLocator.Get<CinemachineConfiner2D>();
            _gameStateController = ServiceLocator.Get<GameStateController>();

            _locations = Resources.LoadAll<Location>("Locations\\");
        }
        
        public void SwitchLocation(string nextLocationName, int pointIndex)
        {
            DestroyCurrentLocation();

            _currentLocation = CreateLocation(GetLocation(nextLocationName));
                    
            if (_currentLocation.Points.Length <= pointIndex)
            {
                Debug.LogWarning($"Такого индекса нет Всего точек: ({_currentLocation.Points.Length}) Текущий индекс: ({pointIndex}) Локация: ({nextLocationName})");
                pointIndex = 0;
            }

            var targetPosition = _currentLocation.Points[pointIndex].position;
            _player.transform.position = targetPosition;
            _camera.ForceCameraPosition(targetPosition, Quaternion.identity);
            
            RepositoryStorage.Set(KeyConstants.Location, new Data { LocationName = nextLocationName, PointIndex = pointIndex });
            _saveLoadService.Save();
            _analyticsService.Send("Location ", _currentLocation.gameObject.name);
        }

        public void DestroyCurrentLocation()
        {
            if (!_currentLocation)
                return;
            
            foreach(var gameListener in _currentLocation.GetComponentsInChildren<IGameListener>(true)) 
                _gameStateController.RemoveListener(gameListener);
                
            Object.Destroy(_currentLocation.gameObject);
        }

        private Location CreateLocation(Location location)
        {
            var createdLocation = Object.Instantiate(location, _container);
            
            foreach(var mb in createdLocation.GetComponentsInChildren<MonoBehaviour>(true)) 
                Injector.Inject(mb);
            
            foreach(var gameListener in createdLocation.GetComponentsInChildren<IGameListener>(true)) 
                _gameStateController.AddListener(gameListener);

            return createdLocation;
        }

        private Location GetLocation(string id)
        {
            foreach (var location in _locations)
            {
                if (location.GetName == id)
                    return location;
            }

            throw new Exception($"Нет такой локации: ({id})");
        }
    }
}