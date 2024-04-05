using CodeBase.Services.CameraServices;
using UnityEngine;
using Zenject;

namespace CodeBase.Services.TransformCameraFace
{
    public class TransformCameraFacing : MonoBehaviour
    {
        private CameraController _cameraController;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        private void Start()
        {
            transform.rotation = Quaternion.LookRotation(_cameraController.Camera.transform.forward);
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(_cameraController.Camera.transform.forward);
        }
    }
}