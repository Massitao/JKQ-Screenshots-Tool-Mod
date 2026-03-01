using UnityEngine;
using UnityEngine.InputSystem;

namespace JKQScreenshotsToolMod.Helpers
{
  public class InputHelper
  {
    #region Fields
    // Components
    private PlayerInput _playerInput;


    // Internal initialization
    private static bool _injectedPCInputs = false;


    // Input References
    private InputActionMap _playerInputActionMap = null;
    private InputActionMap _uiInputActionMap = null;
    private InputActionMap _manualCameraInputActionMap = null;
    #endregion


    #region Initialization Methods
    public void Init()
    {
      _playerInput = Nexile.JKQuest.InputManager.Instance.PlayerInput;
      _playerInputActionMap = _playerInput.actions.FindActionMap("Player", false);
      _uiInputActionMap = _playerInput.actions.FindActionMap("UI", false);
      _manualCameraInputActionMap = _playerInput.actions.FindActionMap("ManualCamera", false);

      InjectPCKeyBindingsToManualCamera();
    }

    public void Clear()
    {
      _playerInput = null;
      _playerInputActionMap = null;
      _uiInputActionMap = null;
      _manualCameraInputActionMap = null;
    }

    // Injects Bindings once in the game's life cycle
    private void InjectPCKeyBindingsToManualCamera()
    {
      if (_injectedPCInputs) return;

      // Manual Camera - Escape Action
      System.Guid ManualCamera_EscapeActionGUID = System.Guid.Parse("2673e686-6d53-44f4-8250-d443af8e1f97");
      UnityEngine.InputSystem.InputAction ManualCamera_EscapeAction = _playerInput.actions.FindAction(ManualCamera_EscapeActionGUID);

      // Can't remove an InputAction while the ActionMaps are active...
      DisableAllInputs();

      // What is the point of this action really?
      // It just disables every single input action and softlocks you (because all action maps get disabled)
      // REMOVE IT FROM THE EXISTENCE. NOW!!!!!!!!!!!!!
      ManualCamera_EscapeAction.RemoveAction();

      // Back to normal
      EnablePlayerInput(true);
      EnableUIInput(true);
      EnableManualCameraInput(false);


      // Manual Camera - Move Action
      System.Guid ManualCamera_MoveActionGUID = System.Guid.Parse("f5e42c67-fa6a-458d-9118-ec5abff7461a");
      UnityEngine.InputSystem.InputAction ManualCamera_MoveAction = _playerInput.actions.FindAction(ManualCamera_MoveActionGUID);

      // Manual Camera - Zoom Action
      System.Guid ManualCamera_ZoomActionGUID = System.Guid.Parse("e35b940d-f477-46f0-a978-1d7a9d1cc17a");
      UnityEngine.InputSystem.InputAction ManualCamera_ZoomAction = _playerInput.actions.FindAction(ManualCamera_ZoomActionGUID);

      // Map - Move Action
      System.Guid Map_MoveActionGUID = System.Guid.Parse("aa5385b7-d9a1-4e15-8d6c-36af50600d03");
      UnityEngine.InputSystem.InputAction Map_MoveAction = _playerInput.actions.FindAction(Map_MoveActionGUID);


      /// Map_MoveAction Bindings:
      /// 
      /// [0]  Gamepad Left Stick
      /// [1]  Gamepad DPad
      /// [2]  Movemap 2D Vector (Arrow Keys)
      /// [3]  Keyboard Up Arrow
      /// [4]  Keyboard Down Arrow
      /// [5]  Keyboard Left Arrow
      /// [6]  Keyboard Right Arrow
      /// [7]  Movemap 2D Vector (WASD Keys)
      /// [8]  Keyboard W
      /// [9]  Keyboard S
      /// [10] Keyboard A
      /// [11] Keyboard D
      /// 
      /// Total Length: 12

      // Inject Arrow Keys inputs to ManualCamera_MoveAction
      // The action already has controller inputs, so it's not necessary to include them
      ManualCamera_MoveAction.AddBinding(Map_MoveAction.bindings[2]); // 2D Vector Composite (for Arrow keys)
      ManualCamera_MoveAction.AddBinding(Map_MoveAction.bindings[3]); // Up Arrow
      ManualCamera_MoveAction.AddBinding(Map_MoveAction.bindings[4]); // Down Arrow
      ManualCamera_MoveAction.AddBinding(Map_MoveAction.bindings[5]); // Left Arrow
      ManualCamera_MoveAction.AddBinding(Map_MoveAction.bindings[6]); // Right Arrow

      // Inject [W] and [S] key inputs to ManualCamera_ZoomAction
      ManualCamera_ZoomAction.AddBinding(Map_MoveAction.bindings[7]); // 2D Vector Composite (for WASD keys)
      ManualCamera_ZoomAction.AddBinding(Map_MoveAction.bindings[8]); // W key
      ManualCamera_ZoomAction.AddBinding(Map_MoveAction.bindings[9]); // S key

      _injectedPCInputs = true;
    }
    #endregion


    #region Enable Input Methods
    public bool IsPlayerInputEnabled() => _playerInputActionMap.enabled;
    public void EnablePlayerInput(bool enable)
    {
      if (enable) _playerInputActionMap.Enable();
      else _playerInputActionMap.Disable();
    }

    public bool IsUIInputEnabled() => _uiInputActionMap.enabled;
    public void EnableUIInput(bool enable)
    {
      if (enable) _uiInputActionMap.Enable();
      else _uiInputActionMap.Disable();
    }

    public bool IsManualCameraInputEnabled() => _manualCameraInputActionMap.enabled;
    public void EnableManualCameraInput(bool enable)
    {
      if (enable) _manualCameraInputActionMap.Enable();
      else _manualCameraInputActionMap.Disable();
    }

    public void DisableAllInputs()
    {
      EnablePlayerInput(false);
      EnableUIInput(false);
      EnableManualCameraInput(false);
    }
    #endregion

    #region Cursor Methods
    public void SetCursorVisibility(bool visible)
    {
      Cursor.visible = visible;
      Cursor.lockState = (visible) ? CursorLockMode.None : CursorLockMode.Confined;
    }
    #endregion
  }
}