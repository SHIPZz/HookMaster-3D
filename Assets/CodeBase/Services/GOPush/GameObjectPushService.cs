using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Services.GOPush
{
    public class GameObjectPushService
    {
        public async UniTask PushRigidBodyAwayAsync(Rigidbody rigidbody, Vector3 targetPosition, float targetDistance, float speed)
        {
            while (Vector3.Distance(rigidbody.position, targetPosition) > targetDistance)
            {
                rigidbody.position =
                    Vector3.Lerp(rigidbody.position, targetPosition, speed * UnityEngine.Time.fixedDeltaTime);
                await UniTask.WaitForFixedUpdate();
            }
        }
    }
}