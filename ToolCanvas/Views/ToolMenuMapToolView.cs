using System;
using UnityEngine;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuMapToolView
  {
    private ToolMenuComponents _toolMenuComponents;

    private float _lastValidNegativeLimitXValue = 0f;
    private float _lastValidNegativeLimitYValue = 0f;
    private float _lastValidPositiveLimitXValue = 0f;
    private float _lastValidPositiveLimitYValue = 0f;
    private float _lastValidIncrementXValue = 0f;
    private float _lastValidIncrementYValue = 0f;

    public event Action<float> OnNegativeLimitXValueChanged;
    public event Action<float> OnNegativeLimitYValueChanged;
    public event Action<float> OnPositiveLimitXValueChanged;
    public event Action<float> OnPositiveLimitYValueChanged;
    public event Action<float> OnIncrementXValueChanged;
    public event Action<float> OnIncrementYValueChanged;

    public event Action OnOpenScreenshotFolderButtonPressed = null;
    public event Action OnTakeScreenshotsButtonPressed = null;


    private ToolMenuMapToolView() { }
    public ToolMenuMapToolView(ToolMenuComponents toolMenuComponents)
    {
      _toolMenuComponents = toolMenuComponents;
    }

    public void SubscribeEvents()
    {
      _toolMenuComponents.MapTool_NegativeLimitX.onEndEdit.AddListener(NegativeLimitXValueChanged);
      _toolMenuComponents.MapTool_NegativeLimitY.onEndEdit.AddListener(NegativeLimitYValueChanged);
      _toolMenuComponents.MapTool_PositiveLimitX.onEndEdit.AddListener(PositiveLimitXValueChanged);
      _toolMenuComponents.MapTool_PositiveLimitY.onEndEdit.AddListener(PositiveLimitYValueChanged);
      _toolMenuComponents.MapTool_IncrementX.onEndEdit.AddListener(IncrementXValueChanged);
      _toolMenuComponents.MapTool_IncrementY.onEndEdit.AddListener(IncrementYValueChanged);

      _toolMenuComponents.MapTool_OpenScreenshotsFolder.onClick.AddListener(OpenScreenshotFolderButtonPressed);
      _toolMenuComponents.MapTool_TakeScreenshots.onClick.AddListener(TakeScreenshotsButtonPressed);
    }
    public void UnsubscribeEvents()
    {
      _toolMenuComponents.MapTool_NegativeLimitX.onEndEdit.RemoveListener(NegativeLimitXValueChanged);
      _toolMenuComponents.MapTool_NegativeLimitY.onEndEdit.RemoveListener(NegativeLimitYValueChanged);
      _toolMenuComponents.MapTool_PositiveLimitX.onEndEdit.RemoveListener(PositiveLimitXValueChanged);
      _toolMenuComponents.MapTool_PositiveLimitY.onEndEdit.RemoveListener(PositiveLimitYValueChanged);
      _toolMenuComponents.MapTool_IncrementX.onEndEdit.RemoveListener(IncrementXValueChanged);
      _toolMenuComponents.MapTool_IncrementY.onEndEdit.RemoveListener(IncrementYValueChanged);

      _toolMenuComponents.MapTool_OpenScreenshotsFolder.onClick.RemoveListener(OpenScreenshotFolderButtonPressed);
      _toolMenuComponents.MapTool_TakeScreenshots.onClick.RemoveListener(TakeScreenshotsButtonPressed);
    }


    #region Texts
    public void ShowEstimatedScreenshotSizeText(bool show)
    {
      _toolMenuComponents.MapTool_EstimatedScreenshotSize.alpha = (show) ? 1f : 0f;
    }
    public void SetEstimatedScreenshotSize(long screenshotsTotalSizeInBytes, uint detailLevel, Vector2 screenshotsRange)
    {
      string currentResolutionText = $"{Screen.currentResolution.width * detailLevel}x{Screen.currentResolution.height * detailLevel}";
      string amountOfScreenshots = $"Total Screenshots: {screenshotsRange.x * screenshotsRange.y} ({screenshotsRange.x.ToString("0")} * {screenshotsRange.y.ToString("0")})";
      string estimatedSizeText = $"{ToolMenuHelper.FormatBytes(screenshotsTotalSizeInBytes)}";

      _toolMenuComponents.MapTool_EstimatedScreenshotSize.text = $"{currentResolutionText} | {amountOfScreenshots} | {estimatedSizeText}";
    }

    public void ShowStorageWarning(bool show)
    {
      _toolMenuComponents.MapTool_StorageWarning.alpha = (show) ? 1f : 0f;
    }
    #endregion

    #region Input Fields
    public void InitializeValues(Vector3 position)
    {
      SetNegativeLimitXValue(position.x);
      SetNegativeLimitYValue(position.y);

      SetPositiveLimitXValue(position.x);
      SetPositiveLimitYValue(position.y);

      SetIncrementXValue(0f);
      SetIncrementYValue(0f);
    }

    private void NegativeLimitXValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidNegativeLimitXValue, OnNegativeLimitXValueChanged, SetNegativeLimitXValue);
    }
    public void SetNegativeLimitXValue(float negativeLimitX)
    {
      _toolMenuComponents.MapTool_NegativeLimitX.SetTextWithoutNotify(negativeLimitX.ToString());
      _lastValidNegativeLimitXValue = negativeLimitX;
    }

    private void NegativeLimitYValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidNegativeLimitYValue, OnNegativeLimitYValueChanged, SetNegativeLimitYValue);
    }
    public void SetNegativeLimitYValue(float negativeLimitY)
    {
      _toolMenuComponents.MapTool_NegativeLimitY.SetTextWithoutNotify(negativeLimitY.ToString());
      _lastValidNegativeLimitYValue = negativeLimitY;
    }

    private void PositiveLimitXValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidPositiveLimitXValue, OnPositiveLimitXValueChanged, SetPositiveLimitXValue);
    }
    public void SetPositiveLimitXValue(float positiveLimitX)
    {
      _toolMenuComponents.MapTool_PositiveLimitX.SetTextWithoutNotify(positiveLimitX.ToString());
      _lastValidPositiveLimitXValue = positiveLimitX;
    }

    private void PositiveLimitYValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidPositiveLimitYValue, OnPositiveLimitYValueChanged, SetPositiveLimitYValue);
    }
    public void SetPositiveLimitYValue(float positiveLimitY)
    {
      _toolMenuComponents.MapTool_PositiveLimitY.SetTextWithoutNotify(positiveLimitY.ToString());
      _lastValidPositiveLimitYValue = positiveLimitY;
    }

    private void IncrementXValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidIncrementXValue, OnIncrementXValueChanged, SetIncrementXValue);
    }
    public void SetIncrementXValue(float incrementX)
    {
      _toolMenuComponents.MapTool_IncrementX.SetTextWithoutNotify(incrementX.ToString());
      _lastValidIncrementXValue = incrementX;
    }

    private void IncrementYValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidIncrementYValue, OnIncrementYValueChanged, SetIncrementYValue);
    }
    public void SetIncrementYValue(float incrementY)
    {
      _toolMenuComponents.MapTool_IncrementY.SetTextWithoutNotify(incrementY.ToString());
      _lastValidIncrementYValue = incrementY;
    }
    #endregion

    #region Buttons
    private void OpenScreenshotFolderButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnOpenScreenshotFolderButtonPressed);
    }

    public void EnableTakeScreenshotsButton(bool enable)
    {
      _toolMenuComponents.MapTool_TakeScreenshots.interactable = enable;
    }
    private void TakeScreenshotsButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnTakeScreenshotsButtonPressed);
    }
    #endregion
  }
}