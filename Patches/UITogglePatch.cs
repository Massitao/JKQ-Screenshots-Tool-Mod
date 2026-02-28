using System;
using UnityEngine;
using Nexile.JKQuest;
using HarmonyLib;

namespace JKQScreenshotsToolMod.Patches
{
  /// <summary>
  /// This patches the Update method from the UITogglePatch class.
  /// This class seems like something made really fast just to toggle the GUI's alpha on and off. No InputActions. Just a hardcoded F11.
  /// Anyway, this patch will help syncing the state of the GUI with the Tool Menu UI.
  /// 
  /// Changes:
  /// * Because we're patching an Update method, I "save" the Input Press, then trigger the event if it has been toggled and if the input was pressed.
  /// * OnInputToggle event will get used by JKQScreenshotsTool to sync the Tool Menu UI.
  /// </summary>
  [HarmonyPatch(typeof(UIToggle), "Update")]
  public static class UITogglePatch
  {
    public static event Action<bool> OnInputToggle = null;

    private static void Prefix(out (bool wasDisabled, bool keyPressed) __state)
    {
      // The code inside this method will run before 'PrivateMethod' is executed
      __state = (UIToggle.UIDisabld, Input.GetKeyDown(KeyCode.F11));
    }

    private static void Postfix((bool wasDisabled, bool keyPressed) __state)
    {
      // The code inside this method will run after 'PrivateMethod' has executed
      if (__state.wasDisabled != UIToggle.UIDisabld && __state.keyPressed)
      {
        OnInputToggle?.Invoke(UIToggle.UIDisabld);
      }
    }
  }
}