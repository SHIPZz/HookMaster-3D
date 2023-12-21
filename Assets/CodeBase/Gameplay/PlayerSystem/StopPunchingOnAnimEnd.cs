using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class StopPunchingOnAnimEnd : MonoBehaviour
    {
        private PlayerAnimator _playerAnimator;

        [Inject]
        private void Construct(PlayerAnimator playerAnimator)
        {
            _playerAnimator = playerAnimator;
        }

        public void OnAnimationEnd()
        {
            _playerAnimator.SetPunch(false);
        }
    }
}