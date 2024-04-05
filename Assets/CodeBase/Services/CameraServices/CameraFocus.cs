using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using CodeBase.Services.Providers.Player;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.CameraServices
{
    public class CameraFocus : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _firstCamera;
        [SerializeField] private CinemachineVirtualCamera _secondCamera;

        private readonly Queue<FocusInfo> _targets = new();

        private int _currentCameraId = 2;
        private PlayerProvider _playerProvider;

        [Inject]
        private void Construct(PlayerProvider playerProvider) => 
            _playerProvider = playerProvider;

        private void Start() => 
            StartCoroutine(CheckForTargets());

        public void AddFocusTarget(FocusInfo focusInfo)
        {
            _targets.Enqueue(focusInfo);
        }

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

                    inactiveCamera.Priority = 0;
                    targetCamera.Priority = 15;
                    targetCamera.Follow = target.Target;
                    targetCamera.LookAt = target.Target;

                    yield return new WaitUntil(() => Math.Abs(targetCamera.transform.position.x - target.Target.position.x) <= 3f);

                    _currentCameraId = _currentCameraId == 1 ? 2 : 1;

                    if (target.CanRelease != null)
                        yield return new WaitUntil(target.CanRelease);

                    if (target.CanReleaseAsync != null)
                        yield return target.CanReleaseAsync?.Invoke().ToCoroutine();

                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    _firstCamera.Follow = _playerProvider.Player.transform;
                    _firstCamera.LookAt = _playerProvider.Player.transform;
                    _firstCamera.Priority = 10;
                    _secondCamera.Priority = 0;
                    _secondCamera.Follow = null;
                    _secondCamera.LookAt = null;
                }
            }
        }
    }
}