using UnityEngine;

namespace Inputs
{
    // Note: added mouse input for testing
    // Though touch input debugging can be enabled in Window > Analysis > Input Debugger
    public enum PlayerInputType
    {
        Mouse,
        Touch
    }

    public class InputManager : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputTransferSO _inputTransferSO;
        [SerializeField] private MobileInputSO _mobileInputSO;
        [SerializeField] private MouseInputSO _mouseInputSO;

        [Header("Input Type")]
        [SerializeField] private PlayerInputType _playerInputType;

        [Header("Events")]
        [SerializeField] private GlobalEventsBusSO _globalEventsBus;
 
        private void Awake()
        {
            switch (_playerInputType)
            {
                case PlayerInputType.Mouse:
                    _inputTransferSO.SetInputTransfer(_mouseInputSO);
                    break;
                case PlayerInputType.Touch:
                    _inputTransferSO.SetInputTransfer(_mobileInputSO);
                    break;
            }

            _inputTransferSO.Input.EnablePlayerInput();
        }

        private void Start()
        {
            _globalEventsBus.OnRestartGame += _inputTransferSO.Input.EnablePlayerInput;
            _globalEventsBus.OnGameEnd += _inputTransferSO.Input.DisablePlayerInput;
        }

        private void OnDisable()
        {
            _inputTransferSO.Input.DisablePlayerInput();

            _globalEventsBus.OnRestartGame -= _inputTransferSO.Input.EnablePlayerInput;
            _globalEventsBus.OnGameEnd -= _inputTransferSO.Input.DisablePlayerInput;
        }
    }
}
