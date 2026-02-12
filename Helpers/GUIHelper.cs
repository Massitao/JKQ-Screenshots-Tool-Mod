using System;
using UnityEngine;
using Nexile.JKQuest;
using JKQScreenshotsToolMod.Patches;

namespace JKQScreenshotsToolMod.Helpers
{
  public class GUIHelper
  {
    #region Fields
    // Components
    private CanvasGroup _gameGUICanvasGroup;
    private MenuGUI _menuGUI;


    // Events
    public event Action OnMenuGUIMenuChanged;
    public event Action<bool> OnUIInputPatchInputToggled; // Player has toggled the GUI view state using inputs
    #endregion


    #region Initialization Methods
    public void Init()
    {
      _gameGUICanvasGroup = GameObject.Find("GUI").GetComponent<CanvasGroup>();
      _menuGUI = MenuGUI.Instance;

      _menuGUI.OnMenuChanged += MenuGUIOnMenuChanged;
      UITogglePatch.OnInputToggle += UIInputTogglePatchInputToggled;
    }


    public void Clear()
    {
      _menuGUI.OnMenuChanged -= MenuGUIOnMenuChanged;
      UITogglePatch.OnInputToggle -= UIInputTogglePatchInputToggled;

      _gameGUICanvasGroup = null;
      _menuGUI = null;
    }
    #endregion


    #region GUI State Methods
    public void SetGameGUIInvisibility(bool invisible)
    {
      _gameGUICanvasGroup.alpha = (invisible) ? 0f : 1f;
      UIToggle.UIDisabld = invisible;
    }

    private void MenuGUIOnMenuChanged(MenuGUI.Menu old, MenuGUI.Menu current)
    {
      OnMenuGUIMenuChanged?.Invoke();
    }

    private void UIInputTogglePatchInputToggled(bool isOn)
    {
      OnUIInputPatchInputToggled?.Invoke(isOn);
    }
    #endregion
  }
}