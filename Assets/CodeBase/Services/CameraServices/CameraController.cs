using Cinemachine;
using CodeBase.Services.Providers.Player;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.CameraServices
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        
        private PlayerProvider _playerProvider;

        public CinemachineBrain Camera => _cinemachineBrain;

        [Inject]
        private void Construct(PlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
        }

        private void Start()
        {
            _cinemachineVirtualCamera.Follow = _playerProvider.Player.transform;
            _cinemachineVirtualCamera.LookAt = _playerProvider.Player.transform;
        }
    }
}