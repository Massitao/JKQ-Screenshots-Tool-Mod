using JKQScreenshotsToolMod.Enums;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JKQScreenshotsToolMod.UI
{
  public static class ToolMenuHelper
  {
    #region Input Components
    public static void UpdateToggleState(bool isOn, Action<bool> onToggleStateChanged)
    {
      if (onToggleStateChanged == null) return;

      onToggleStateChanged.Invoke(isOn);
      DeselectCurrentGameObject();
    }

    public static void UpdateInputField(string value, uint previousValue, Action<uint> onValidValuePassed, Action<uint> onInvalidValuePassed)
    {
      if (onValidValuePassed == null || onInvalidValuePassed == null) return;

      if (!uint.TryParse(value, out uint parsedValue))
      {
        onInvalidValuePassed.Invoke(previousValue);
        return;
      }

      onValidValuePassed.Invoke(parsedValue);
      DeselectCurrentGameObject();
    }
    public static void UpdateInputField(string value, float previousValue, Action<float> onValidValuePassed, Action<float> onInvalidValuePassed)
    {
      if (onValidValuePassed == null || onInvalidValuePassed == null) return;

      if (!float.TryParse(value, out float parsedValue))
      {
        onInvalidValuePassed.Invoke(previousValue);
        return;
      }

      onValidValuePassed.Invoke(parsedValue);
      DeselectCurrentGameObject();
    }


    public static void ButtonPressed(Action onButtonPressed)
    {
      if (onButtonPressed == null) return;

      onButtonPressed.Invoke();
      DeselectCurrentGameObject();
    }

    public static void UpdateColorChannelInputField(ColorChannels colorChannel, float value, ref Color lastValidColor, TMP_InputField inputField, Image preview)
    {
      if (inputField == null || preview == null) return;

      inputField.SetTextWithoutNotify(value.ToString("0.00"));

      switch (colorChannel)
      {
        case ColorChannels.Red:
          lastValidColor.r = value;
          break;
        case ColorChannels.Green:
          lastValidColor.g = value;
          break;
        case ColorChannels.Blue:
          lastValidColor.b = value;
          break;
      }
      lastValidColor.a = 1f;

      UpdateColorPreview(preview, lastValidColor);
    }
    public static void UpdateColorInputFields(Color value, ref Color lastValidColor, TMP_InputField redIF, TMP_InputField greenIF, TMP_InputField blueIF, Image preview)
    {
      if (redIF == null || greenIF == null || blueIF == null || preview == null) return;

      redIF.SetTextWithoutNotify(value.r.ToString("0.00"));
      greenIF.SetTextWithoutNotify(value.g.ToString("0.00"));
      blueIF.SetTextWithoutNotify(value.b.ToString("0.00"));

      lastValidColor = value;
      lastValidColor.a = 1f;

      UpdateColorPreview(preview, lastValidColor);
    }
    public static void UpdateColorPreview(Image preview, Color newColorValue)
    {
      preview.color = newColorValue;
    }
    #endregion

    #region Event System
    public static void DeselectCurrentGameObject()
    {
      // Pain in the ass
      EventSystem.current.SetSelectedGameObject(null);
    }
    #endregion

    #region Formatting
    public static string FormatBytes(long totalSizeInBytes)
    {
      // Shamelessly stolen from Stack Overflow cuz lazy
      string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
      if (totalSizeInBytes == 0) return "0" + suf[0];

      long bytes = Math.Abs(totalSizeInBytes);
      int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
      double num = Math.Round(bytes / Math.Pow(1024, place), 1);
      return (Math.Sign(totalSizeInBytes) * num).ToString() + suf[place];
    }
    #endregion
  }
}
