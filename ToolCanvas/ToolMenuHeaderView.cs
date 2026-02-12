using System;
using UnityEngine;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuHeaderView
  {
    private ToolMenuComponents _toolMenuComponents;

    private uint _lastValidDetailLevelValue = 0u;

    public event Action<uint> OnDetailLevelValueChanged;
    public event Action OnTakeScreenshotButtonPressed = null;
    public event Action OnCloseToolMenuButtonPressed = null;


    private ToolMenuHeaderView() { }
    public ToolMenuHeaderView(ToolMenuComponents toolMenuComponents)
    {
      _toolMenuComponents = toolMenuComponents;
    }

    public void SubscribeEvents()
    {
      _toolMenuComponents.Header_TakeScreenshot.onClick.AddListener(TakeScreenshotButtonPressed);
      _toolMenuComponents.Header_CloseToolMenu.onClick.AddListener(CloseToolMenuButtonPressed);

      _toolMenuComponents.Header_DetailLevel.onEndEdit.AddListener(DetailLevelValueChanged);
    }
    public void UnsubscribeEvents()
    {
      _toolMenuComponents.Header_TakeScreenshot.onClick.RemoveListener(TakeScreenshotButtonPressed);
      _toolMenuComponents.Header_CloseToolMenu.onClick.RemoveListener(CloseToolMenuButtonPressed);

      _toolMenuComponents.Header_DetailLevel.onEndEdit.RemoveListener(DetailLevelValueChanged);
    }


    #region Texts
    public void UpdateCurrentCameraCoordsAndFOV(Vector2 cameraPos, float fov)
    {

      _toolMenuComponents.Header_XCoord.text = $"X: {cameraPos.x.ToString("0.00")}";
      _toolMenuComponents.Header_YCoord.text = $"Y: {cameraPos.y.ToString("0.00")}";
      _toolMenuComponents.Header_FOV.text = $"FOV: {fov.ToString("0.00")}";
    }
    #endregion

    #region Input Fields
    private void DetailLevelValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidDetailLevelValue, OnDetailLevelValueChanged, SetDetailLevelValue);
    }
    public void SetDetailLevelValue(uint detailLevel)
    {
      _toolMenuComponents.Header_DetailLevel.SetTextWithoutNotify(detailLevel.ToString());
      _lastValidDetailLevelValue = detailLevel;
    }
    #endregion

    #region Buttons
    private void TakeScreenshotButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnTakeScreenshotButtonPressed);
    }
    private void CloseToolMenuButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnCloseToolMenuButtonPressed);
    }
    #endregion
  }
}