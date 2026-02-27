using System;
using UnityEngine;
using Nexile.JKQuest;
using JKQScreenshotsToolMod.Patches;
using UnityEngine.UI;

namespace JKQScreenshotsToolMod.Helpers
{
  public class GUIHelper
  {
    #region Fields
    // Components
    private CanvasGroup _gameGUICanvasGroup;
    private MenuGUI _menuGUI;

    // References
    private Tuple<Texture, Material> _backdropMaterial;
    private Tuple<Texture, Texture> _toggleTextures;

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


    #region UI Element Getters
    public Tuple<Texture, Material> GetUIBackdropMaterial()
    {
      if (_backdropMaterial != null) return _backdropMaterial;

      Image backdrop = _menuGUI.transform.Find("Content/Canvas Options/Margin/Options/Backdrop").GetComponent<Image>();
      if (backdrop == null) return null;

      return _backdropMaterial = Tuple.Create<Texture, Material>(backdrop.mainTexture, backdrop.material);
    }

    public Tuple<Texture, Texture> GetUIToggleTextures()
    {
      string togglePath = "Content/Canvas Options/Margin/Options/Margin/Options List/Scroll View/Viewport/Options/Gameplay/Gamepad Rumble/ToggleConsoleUGUI (Controller Vibration)/Toggle";
      Image toggle = _menuGUI.transform.Find(togglePath).GetComponent<Image>();
      if (toggle == null) return null;

      Image toggleCheckmark = toggle.transform.Find("Checkmark").GetComponent<Image>();


      return _toggleTextures = Tuple.Create<Texture, Texture>(toggle.mainTexture, toggleCheckmark.mainTexture);
    }


    #endregion


    #region GUI State Methods
    public void SetGameGUIInvisibility(bool invisible)
    {
      _gameGUICanvasGroup.alpha = (invisible) ? 0f : 1f;
      UIToggle.UIDisabld = invisible;
    }

    public void OpenInventoryMenu()
    {
      if (_menuGUI.CurrentMenu == MenuGUI.Menu.Inventory) return;
      _menuGUI.TrySetMenu(MenuGUI.Menu.Inventory);
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