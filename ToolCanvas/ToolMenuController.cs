using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuController : MonoBehaviour
  {
    // References and Components
    private ToolMenuComponents _toolMenuComponents;
    public ToolMenuHeaderView HeaderView { get; private set; }
    public ToolMenuFreecamView FreecamView { get; private set; }
    public ToolMenuMapToolView MapToolView { get; private set; }
    public ToolMenuInventoryView InventoryView { get; private set; }
    public ToolMenuTogglesView TogglesView { get; private set; }
    public ToolMenuExtrasView ExtrasView { get; private set; }


    private void Awake()
    {
      // Main Components
      _toolMenuComponents = gameObject.AddComponent<ToolMenuComponents>();
      _toolMenuComponents.ToolCanvasGroup.alpha = 0f;
      _toolMenuComponents.ToolCanvasGroup.blocksRaycasts = false;

      HeaderView = new ToolMenuHeaderView(_toolMenuComponents);
      FreecamView = new ToolMenuFreecamView(_toolMenuComponents);
      MapToolView = new ToolMenuMapToolView(_toolMenuComponents);
      InventoryView = new ToolMenuInventoryView(_toolMenuComponents);
      TogglesView = new ToolMenuTogglesView(_toolMenuComponents);
      ExtrasView = new ToolMenuExtrasView(_toolMenuComponents);
    }

    private void OnEnable()
    {
      HeaderView.SubscribeEvents();
      FreecamView.SubscribeEvents();
      MapToolView.SubscribeEvents();
      InventoryView.SubscribeEvents();
      TogglesView.SubscribeEvents();
      ExtrasView.SubscribeEvents();
    }
    private void OnDisable()
    {
      HeaderView.UnsubscribeEvents();
      FreecamView.UnsubscribeEvents();
      MapToolView.UnsubscribeEvents();
      InventoryView.UnsubscribeEvents();
      TogglesView.UnsubscribeEvents();
      ExtrasView.UnsubscribeEvents();
    }


    public bool IsScreenshotToolMenuOpen() => _toolMenuComponents.ToolCanvasGroup.alpha == 1f;
    public void SetScreenshotToolMenuOpen(bool open)
    {
      _toolMenuComponents.ToolCanvasGroup.alpha = (open) ? 1f : 0f;
      _toolMenuComponents.ToolCanvasGroup.blocksRaycasts = open;
    }

    public void Tabs_SetView(ToolMenuTabs tab)
    {
      _toolMenuComponents.SelectToolCanvasTabButton(_toolMenuComponents.GetToolCanvasTabButton(tab));
    }

    public void SetScreenshotToolFonts(TMP_FontAsset titleFont, TMP_FontAsset contentFont)
    {
      _toolMenuComponents.Header_Title.font = titleFont;

      foreach (TMP_Text text in _toolMenuComponents.ToolCanvas_Texts)
      {
        if (text == _toolMenuComponents.Header_Title) continue;
        text.font = contentFont;
      }
    }
  }
}