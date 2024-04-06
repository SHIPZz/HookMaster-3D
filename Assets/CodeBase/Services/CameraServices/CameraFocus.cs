using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using CodeBase.Services.Providers.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.CameraServices
{
    public class CameraFocus : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _firstCamera;
        [SerializeField] private CinemachineVirtualCamera _secondCamera;
        [SerializeField] private CinemachineBrain _cinemachineBrain;

        private readonly Queue<FocusInfo> _targets = new();

        private int _currentCameraId = 2;
        private PlayerProvider _playerProvider;
        
        public bool HasFollow { get; private set; }

        public event Action<FocusInfo> TargetReached;
        public event Action Moved;

        [Inject]
        private void Construct(PlayerProvider playerProvider) =>
            _playerProvider = playerProvider;

        private void Start() => 
            StartCoroutine(CheckForTargets());

        public void AddFocusTarget(FocusInfo focusInfo) => 
            _targets.Enqueue(focusInfo);

        private IEnumerator CheckForTargets()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                if (_targets.Count > 0)
                {
                    FocusInfo target = _targets.Dequeue();
                    
                    CinemachineVirtualCamera targetCamera = _currentCameraId == 1 ? _firstCamera : _secondCamera;
                    CinemachineVirtualCamera inactiveCamera = _currentCameraId == 1 ? _secondCamera : _firstCamera;

                    ConfigureCamera(inactiveCamera, null, 0);
                    ConfigureCamera(targetCamera, target.Target, 15);

                    Moved?.Invoke();
                    HasFollow = true;
                    
                    yield return new WaitForSeconds(0.2f);

                    yield return new WaitUntil(() => _cinemachineBrain.IsBlending == false);

                    TargetReached?.Invoke(target);

                    _currentCameraId = _currentCameraId == 1 ? 2 : 1;

                    if (target.CanRelease != null)
                        yield return new WaitUntil(target.CanRelease);

                    if (target.CanReleaseAsync != null)
                        yield return target.CanReleaseAsync?.Invoke().ToCoroutine();

                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    ConfigureCamera(_firstCamera, _playerProvider.Player.transform, 15);
                    ConfigureCamera(_secondCamera, null, 0);
                    _currentCameraId = 2;

                    yield return new WaitForSeconds(0.2f);
                    
                    yield return new WaitUntil(() => _cinemachineBrain.IsBlending == false);
                    
                    HasFollow = false;
                }
            }
        }

        private void ConfigureCamera(CinemachineVirtualCamera camera, Transform target, int priority)
        {
            camera.Follow = target;
            camera.LookAt = target;
            camera.Priority = priority;
        }
    }
}