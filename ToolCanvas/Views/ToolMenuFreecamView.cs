using System;
using UnityEngine;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuFreecamView
  {
    private readonly ToolMenuComponents _toolMenuComponents;

    private float _lastValidSpeedValue = 0f;
    private float _lastValidSpeedDampingValue = 0f;
    private float _lastValidFOVValue = 0f;
    private float _lastValidZoomSpeedValue = 0f;

    public event Action<bool> OnFreecamStateChanged;
    public event Action<bool> OnFreecamPlayerInputStateChanged;
    public event Action<bool> OnFreecamAudioListenerStateChanged;
    public event Action<float> OnSpeedChanged;
    public event Action<float> OnSpeedDampingChanged;
    public event Action<float> OnFOVChanged;
    public event Action<float> OnZoomSpeedChanged;

    public event Action OnOpenScreenshotFolderButtonPressed;
    public event Action OnTakeScreenshotButtonPressed;


    private ToolMenuFreecamView() { }
    public ToolMenuFreecamView(ToolMenuComponents toolMenuComponents)
    {
      _toolMenuComponents = toolMenuComponents;
    }

    public void SubscribeEvents()
    {
      _toolMenuComponents.Freecam_EnableManualCamera.onValueChanged.AddListener(FreecamToggled);
      _toolMenuComponents.Freecam_AllowPlayerMovement.onValueChanged.AddListener(FreecamPlayerInputToggled);
      _toolMenuComponents.Freecam_SetCameraAsAudioListener.onValueChanged.AddListener(FreecamAudioListenerToggled);

      _toolMenuComponents.Freecam_CameraSpeed.onEndEdit.AddListener(SpeedValueChanged);
      _toolMenuComponents.Freecam_CameraSpeedDamping.onEndEdit.AddListener(SpeedDampingValueChanged);
      _toolMenuComponents.Freecam_CameraFOV.onEndEdit.AddListener(FOVValueChanged);
      _toolMenuComponents.Freecam_CameraZoomSpeed.onEndEdit.AddListener(ZoomSpeedValueChanged);

      _toolMenuComponents.Freecam_OpenScreenshotsFolder.onClick.AddListener(OpenScreenshotFolderButtonPressed);
      _toolMenuComponents.Freecam_TakeScreenshot.onClick.AddListener(TakeScreenshotButtonPressed);
    }
    public void UnsubscribeEvents()
    {
      _toolMenuComponents.Freecam_EnableManualCamera.onValueChanged.RemoveListener(FreecamToggled);
      _toolMenuComponents.Freecam_AllowPlayerMovement.onValueChanged.RemoveListener(FreecamPlayerInputToggled);
      _toolMenuComponents.Freecam_SetCameraAsAudioListener.onValueChanged.RemoveListener(FreecamAudioListenerToggled);

      _toolMenuComponents.Freecam_CameraSpeed.onEndEdit.RemoveListener(SpeedValueChanged);
      _toolMenuComponents.Freecam_CameraSpeedDamping.onEndEdit.RemoveListener(SpeedDampingValueChanged);
      _toolMenuComponents.Freecam_CameraFOV.onEndEdit.RemoveListener(FOVValueChanged);
      _toolMenuComponents.Freecam_CameraZoomSpeed.onEndEdit.RemoveListener(ZoomSpeedValueChanged);

      _toolMenuComponents.Freecam_OpenScreenshotsFolder.onClick.RemoveListener(OpenScreenshotFolderButtonPressed);
      _toolMenuComponents.Freecam_TakeScreenshot.onClick.RemoveListener(TakeScreenshotButtonPressed);
    }


    #region Texts
    public void SetEstimatedScreenshotSize(long screenshotSizeInBytes, uint detailLevel)
    {
      string currentResolutionText = $"{Screen.currentResolution.width * detailLevel}x{Screen.currentResolution.height * detailLevel}";
      string estimatedSizeText = $"{ToolMenuHelper.FormatBytes(screenshotSizeInBytes)}";

      _toolMenuComponents.Freecam_EstimatedScreenshotSize.text = $"{currentResolutionText} | {estimatedSizeText}";
    }

    public void ShowStorageWarning(bool show)
    {
      _toolMenuComponents.Freecam_StorageWarning.alpha = (show) ? 1f : 0f;
    }
    #endregion

    #region Toggles
    private void FreecamToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnFreecamStateChanged);
    }
    public void SetFreecamToggleState(bool isOn)
    {
      _toolMenuComponents.Freecam_EnableManualCamera.SetIsOnWithoutNotify(isOn);

      EnableFreecamPlayerInputToggle(isOn);
      EnableFreecamAudioListenerToggle(isOn);
    }

    private void EnableFreecamPlayerInputToggle(bool enable)
    {
      _toolMenuComponents.Freecam_AllowPlayerMovement.interactable = enable;

      if (!enable && _toolMenuComponents.Freecam_AllowPlayerMovement.isOn)
      {
        SetFreecamPlayerInputToggleState(false);
      }
    }
    private void FreecamPlayerInputToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnFreecamPlayerInputStateChanged);
    }
    public void SetFreecamPlayerInputToggleState(bool isOn)
    {
      _toolMenuComponents.Freecam_AllowPlayerMovement.SetIsOnWithoutNotify(isOn);
    }

    private void EnableFreecamAudioListenerToggle(bool enable)
    {
      _toolMenuComponents.Freecam_SetCameraAsAudioListener.interactable = enable;

      if (!enable && _toolMenuComponents.Freecam_SetCameraAsAudioListener.isOn)
      {
        SetFreecamAudioListenerToggleState(false);
      }
    }
    private void FreecamAudioListenerToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnFreecamAudioListenerStateChanged);
    }
    public void SetFreecamAudioListenerToggleState(bool isOn)
    {
      _toolMenuComponents.Freecam_SetCameraAsAudioListener.SetIsOnWithoutNotify(isOn);
    }
    #endregion

    #region Input Fields
    private void SpeedValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidSpeedValue,
        onValidValuePassed: OnSpeedChanged,
        onInvalidValuePassed: SetSpeedValue
        );
    }
    public void SetSpeedValue(float speed)
    {
      _toolMenuComponents.Freecam_CameraSpeed.SetTextWithoutNotify(speed.ToString());
      _lastValidSpeedValue = speed;
    }

    private void SpeedDampingValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidSpeedDampingValue,
        onValidValuePassed: OnSpeedDampingChanged,
        onInvalidValuePassed: SetSpeedDampingValue
        );
    }
    public void SetSpeedDampingValue(float dampingSpeed)
    {
      _toolMenuComponents.Freecam_CameraSpeedDamping.SetTextWithoutNotify(dampingSpeed.ToString());
      _lastValidSpeedDampingValue = dampingSpeed;
    }

    private void FOVValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidFOVValue,
        onValidValuePassed: OnFOVChanged,
        onInvalidValuePassed: SetFOVValue
        );
    }
    public void SetFOVValue(float fov)
    {
      _toolMenuComponents.Freecam_CameraFOV.SetTextWithoutNotify(fov.ToString());
      _lastValidFOVValue = fov;
    }

    private void ZoomSpeedValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidZoomSpeedValue,
        onValidValuePassed: OnZoomSpeedChanged,
        onInvalidValuePassed: SetZoomSpeedValue
        );
    }
    public void SetZoomSpeedValue(float zoomSpeed)
    {
      _toolMenuComponents.Freecam_CameraZoomSpeed.SetTextWithoutNotify(zoomSpeed.ToString());
      _lastValidZoomSpeedValue = zoomSpeed;
    }
    #endregion

    #region Buttons
    private void OpenScreenshotFolderButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnOpenScreenshotFolderButtonPressed);
    }

    public void EnabeTakeScreenshotButton(bool enable)
    {
      _toolMenuComponents.Freecam_TakeScreenshot.interactable = enable;
    }
    private void TakeScreenshotButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnTakeScreenshotButtonPressed);
    }
    #endregion
  }
}