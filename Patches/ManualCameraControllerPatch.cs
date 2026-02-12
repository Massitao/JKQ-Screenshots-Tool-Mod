using System;
using UnityEngine.InputSystem;
using Nexile.JKQuest;
using HarmonyLib;

namespace JKQScreenshotsToolMod.Patches
{
  /// <summary>
  /// This patches the OnTogglePressed method from the ManualCameraController class.
  /// I needed a way to sync the enabled state for both the InputAction (input event) and the Tool Canvas Toggle (UI event).
  /// Before, both events handled the ManualCameraController their own way.
  /// 
  /// Changes:
  /// * OnTogglePressed will always trigger the event (it only fired if there was no menu open).
  /// * OnInputToggle event will get used by JKQScreenshotsTool to sync both the internal ManualCameraState and the Tool Menu UI.
  /// </summary>
  [HarmonyPatch(typeof(ManualCameraController), "OnTogglePressed", new Type[] { typeof(InputAction.CallbackContext) })]
  public static class ManualCameraControllerPatch
  {
    public static Action<bool> OnInputToggle = null;

    private static void Prefix(ManualCameraController __instance, out bool __state)
    {
      // The code inside this method will run before 'PrivateMethod' is executed
      __state = __instance.isEnabled;
    }

    private static void Postfix(ManualCameraController __instance, bool __state)
    {
      // The code inside this method will run after 'PrivateMethod' has executed
      if (MenuGUI.Instance.CurrentMenu != MenuGUI.Menu.None)
      {
        AccessTools.Method(typeof(ManualCameraController), "HandleModeChanged")?.Invoke(__instance, new object[] { !__instance.isEnabled });
        __state = __instance.isEnabled;
      }

      if (__state != __instance.isEnabled)
      {
        OnInputToggle?.Invoke(__instance.isEnabled);
      }
    }
  }
}