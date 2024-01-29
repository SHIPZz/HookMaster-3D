using CodeBase.Services.UI;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerInputService
    {
        public PlayerInput PlayerInput;

        private readonly UIService _uiService;

        public PlayerInputService(UIService uiService)
        {
            _uiService = uiService;
        }

        public void SetActive(bool isActive)
        {
            if (!isActive)
            {
                PlayerInput.SetBlocked(true);
                _uiService.SetActiveJoystickUI(false);
                return;
            }
            
            PlayerInput.SetBlocked(false);
            _uiService.SetActiveJoystickUI(true);
        }

        public void SetPlayerInput(PlayerInput playerInput)
        {
            PlayerInput = playerInput;
        }
    }
}