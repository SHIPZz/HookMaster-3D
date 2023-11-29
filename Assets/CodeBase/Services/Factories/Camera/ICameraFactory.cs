using CodeBase.Gameplay.Camera;

namespace CodeBase.Services.Factories.Camera
{
    public interface ICameraFactory
    {
        CameraFollower Create();
    }
}