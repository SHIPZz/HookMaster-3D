using CodeBase.Gameplay.PlayerSystem;

namespace CodeBase.Services.Player
{
    public class PlayerAnimationService
    {
        private PlayerAnimator _playerAnimator;

        public void PlayPunchAnimation()
        {
            _playerAnimator.SetPunch(true);
        }

        public void SetPlayerAnimator(PlayerAnimator playerAnimator)
        {
            _playerAnimator = playerAnimator;
        }
    }
}