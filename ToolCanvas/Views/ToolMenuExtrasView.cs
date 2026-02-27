using JKQScreenshotsToolMod.Enums;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuExtrasView
  {
    private readonly ToolMenuComponents _toolMenuComponents;

    private Color _lastValidSkyboxColor = Color.black;
    public event Action<float> OnSkyboxColorRedValueChanged;
    public event Action<float> OnSkyboxColorGreenValueChanged;
    public event Action<float> OnSkyboxColorBlueValueChanged;

    private Color _lastValidFogColor = Color.black;
    private float _lastValidFogDensity = 1f;
    public event Action<float> OnFogColorRedValueChanged;
    public event Action<float> OnFogColorGreenValueChanged;
    public event Action<float> OnFogColorBlueValueChanged;
    public event Action<float> OnFogDensityValueChanged;

    private Color _lastValidMainLightingColor = Color.black;
    private Vector3 _lastValidMainLightingRotation = Vector3.zero;
    private float _lastValidMainLightingIntensity = 1f;
    public event Action<float> OnMainLightingColorRedValueChanged;
    public event Action<float> OnMainLightingColorGreenValueChanged;
    public event Action<float> OnMainLightingColorBlueValueChanged;
    public event Action<float> OnMainLightingRotationXValueChanged;
    public event Action<float> OnMainLightingRotationYValueChanged;
    public event Action<float> OnMainLightingRotationZValueChanged;
    public event Action<float> OnMainLightingIntensityValueChanged;

    private Color _lastValidCharacterLightingColor = Color.black;
    private float _lastValidCharacterLightingIntensity = 1f;
    public event Action<float> OnCharacterLightingColorRedValueChanged;
    public event Action<float> OnCharacterLightingColorGreenValueChanged;
    public event Action<float> OnCharacterLightingColorBlueValueChanged;
    public event Action<float> OnCharacterLightingIntensityValueChanged;

    private Color _lastValidExtraLightingColor = Color.black;
    private float _lastValidExtraLightingIntensity = 1f;
    public event Action<float> OnExtraLightingColorRedValueChanged;
    public event Action<float> OnExtraLightingColorGreenValueChanged;
    public event Action<float> OnExtraLightingColorBlueValueChanged;
    public event Action<float> OnExtraLightingIntensityValueChanged;


    private ToolMenuExtrasView() { }
    public ToolMenuExtrasView(ToolMenuComponents toolMenuComponents)
    {
      _toolMenuComponents = toolMenuComponents;
    }

    public void SubscribeEvents()
    {
      _toolMenuComponents.Extras_SkyboxColor_Red.onEndEdit.AddListener(SkyboxColorRedValueChanged);
      _toolMenuComponents.Extras_SkyboxColor_Green.onEndEdit.AddListener(SkyboxColorGreenValueChanged);
      _toolMenuComponents.Extras_SkyboxColor_Blue.onEndEdit.AddListener(SkyboxColorBlueValueChanged);

      _toolMenuComponents.Extras_FogColor_Red.onEndEdit.AddListener(FogColorRedValueChanged);
      _toolMenuComponents.Extras_FogColor_Green.onEndEdit.AddListener(FogColorGreenValueChanged);
      _toolMenuComponents.Extras_FogColor_Blue.onEndEdit.AddListener(FogColorBlueValueChanged);
      _toolMenuComponents.Extras_FogDensity.onEndEdit.AddListener(FogDensityValueChanged);

      _toolMenuComponents.Extras_MainLightingColor_Red.onEndEdit.AddListener(MainLightingColorRedValueChanged);
      _toolMenuComponents.Extras_MainLightingColor_Green.onEndEdit.AddListener(MainLightingColorGreenValueChanged);
      _toolMenuComponents.Extras_MainLightingColor_Blue.onEndEdit.AddListener(MainLightingColorBlueValueChanged);
      _toolMenuComponents.Extras_MainLightingIntensity.onEndEdit.AddListener(MainLightingIntensityValueChanged);
      _toolMenuComponents.Extras_MainLightingRotation_X.onEndEdit.AddListener(MainLightingRotationXValueChanged);
      _toolMenuComponents.Extras_MainLightingRotation_Y.onEndEdit.AddListener(MainLightingRotationYValueChanged);
      _toolMenuComponents.Extras_MainLightingRotation_Z.onEndEdit.AddListener(MainLightingRotationZValueChanged);

      _toolMenuComponents.Extras_CharacterLightingColor_Red.onEndEdit.AddListener(CharacterLightingColorRedValueChanged);
      _toolMenuComponents.Extras_CharacterLightingColor_Green.onEndEdit.AddListener(CharacterLightingColorGreenValueChanged);
      _toolMenuComponents.Extras_CharacterLightingColor_Blue.onEndEdit.AddListener(CharacterLightingColorBlueValueChanged);
      _toolMenuComponents.Extras_CharacterLightingIntensity.onEndEdit.AddListener(CharacterLightingIntensityValueChanged);

      _toolMenuComponents.Extras_ExtraLightingColor_Red.onEndEdit.AddListener(ExtraLightingColorRedValueChanged);
      _toolMenuComponents.Extras_ExtraLightingColor_Green.onEndEdit.AddListener(ExtraLightingColorGreenValueChanged);
      _toolMenuComponents.Extras_ExtraLightingColor_Blue.onEndEdit.AddListener(ExtraLightingColorBlueValueChanged);
      _toolMenuComponents.Extras_ExtraLightingIntensity.onEndEdit.AddListener(ExtraLightingIntensityValueChanged);
    }
    public void UnsubscribeEvents()
    {
      _toolMenuComponents.Extras_SkyboxColor_Red.onEndEdit.RemoveListener(SkyboxColorRedValueChanged);
      _toolMenuComponents.Extras_SkyboxColor_Green.onEndEdit.RemoveListener(SkyboxColorGreenValueChanged);
      _toolMenuComponents.Extras_SkyboxColor_Blue.onEndEdit.RemoveListener(SkyboxColorBlueValueChanged);

      _toolMenuComponents.Extras_FogColor_Red.onEndEdit.RemoveListener(FogColorRedValueChanged);
      _toolMenuComponents.Extras_FogColor_Green.onEndEdit.RemoveListener(FogColorGreenValueChanged);
      _toolMenuComponents.Extras_FogColor_Blue.onEndEdit.RemoveListener(FogColorBlueValueChanged);
      _toolMenuComponents.Extras_FogDensity.onEndEdit.RemoveListener(FogDensityValueChanged);

      _toolMenuComponents.Extras_MainLightingColor_Red.onEndEdit.RemoveListener(MainLightingColorRedValueChanged);
      _toolMenuComponents.Extras_MainLightingColor_Green.onEndEdit.RemoveListener(MainLightingColorGreenValueChanged);
      _toolMenuComponents.Extras_MainLightingColor_Blue.onEndEdit.RemoveListener(MainLightingColorBlueValueChanged);
      _toolMenuComponents.Extras_MainLightingIntensity.onEndEdit.RemoveListener(MainLightingIntensityValueChanged);
      _toolMenuComponents.Extras_MainLightingRotation_X.onEndEdit.RemoveListener(MainLightingRotationXValueChanged);
      _toolMenuComponents.Extras_MainLightingRotation_Y.onEndEdit.RemoveListener(MainLightingRotationYValueChanged);
      _toolMenuComponents.Extras_MainLightingRotation_Z.onEndEdit.RemoveListener(MainLightingRotationZValueChanged);

      _toolMenuComponents.Extras_CharacterLightingColor_Red.onEndEdit.RemoveListener(CharacterLightingColorRedValueChanged);
      _toolMenuComponents.Extras_CharacterLightingColor_Green.onEndEdit.RemoveListener(CharacterLightingColorGreenValueChanged);
      _toolMenuComponents.Extras_CharacterLightingColor_Blue.onEndEdit.RemoveListener(CharacterLightingColorBlueValueChanged);
      _toolMenuComponents.Extras_CharacterLightingIntensity.onEndEdit.RemoveListener(CharacterLightingIntensityValueChanged);

      _toolMenuComponents.Extras_ExtraLightingColor_Red.onEndEdit.RemoveListener(ExtraLightingColorRedValueChanged);
      _toolMenuComponents.Extras_ExtraLightingColor_Green.onEndEdit.RemoveListener(ExtraLightingColorGreenValueChanged);
      _toolMenuComponents.Extras_ExtraLightingColor_Blue.onEndEdit.RemoveListener(ExtraLightingColorBlueValueChanged);
      _toolMenuComponents.Extras_ExtraLightingIntensity.onEndEdit.RemoveListener(ExtraLightingIntensityValueChanged);
    }



    #region Skybox
    private void SkyboxColorRedValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidSkyboxColor.r,
        onValidValuePassed: OnSkyboxColorRedValueChanged,
        onInvalidValuePassed: SetSkyboxColorRedValue
        );
    }
    private void SetSkyboxColorRedValue(float redColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Red,
        value: redColor,
        lastValidColor: ref _lastValidSkyboxColor,
        inputField: _toolMenuComponents.Extras_SkyboxColor_Red,
        preview: _toolMenuComponents.Extras_SkyboxColorPreview
        );
    }

    private void SkyboxColorGreenValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidSkyboxColor.g,
        onValidValuePassed: OnSkyboxColorGreenValueChanged,
        onInvalidValuePassed: SetSkyboxColorGreenValue
        );
    }
    private void SetSkyboxColorGreenValue(float greenColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Green,
        value: greenColor,
        lastValidColor: ref _lastValidSkyboxColor,
        inputField: _toolMenuComponents.Extras_SkyboxColor_Green,
        preview: _toolMenuComponents.Extras_SkyboxColorPreview
        );
    }

    private void SkyboxColorBlueValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidSkyboxColor.b,
        onValidValuePassed: OnSkyboxColorBlueValueChanged,
        onInvalidValuePassed: SetSkyboxColorBlueValue
        );
    }
    private void SetSkyboxColorBlueValue(float blueColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Blue,
        value: blueColor,
        lastValidColor: ref _lastValidSkyboxColor,
        inputField: _toolMenuComponents.Extras_SkyboxColor_Blue,
        preview: _toolMenuComponents.Extras_SkyboxColorPreview
        );
    }

    public void SetSkyboxColorValue(Color color)
    {
      ToolMenuHelper.UpdateColorInputFields(
        value: color,
        lastValidColor: ref _lastValidSkyboxColor,
        redIF: _toolMenuComponents.Extras_SkyboxColor_Red,
        greenIF: _toolMenuComponents.Extras_SkyboxColor_Green,
        blueIF: _toolMenuComponents.Extras_SkyboxColor_Blue,
        preview: _toolMenuComponents.Extras_SkyboxColorPreview
        );
    }
    #endregion

    #region Fog
    private void FogColorRedValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidFogColor.r,
        onValidValuePassed: OnFogColorRedValueChanged,
        onInvalidValuePassed: SetFogColorRedValue
        );
    }
    private void SetFogColorRedValue(float redColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Red,
        value: redColor,
        lastValidColor: ref _lastValidFogColor,
        inputField: _toolMenuComponents.Extras_FogColor_Red,
        preview: _toolMenuComponents.Extras_FogColorPreview
        );
    }

    private void FogColorGreenValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidFogColor.g,
        onValidValuePassed: OnFogColorGreenValueChanged,
        onInvalidValuePassed: SetFogColorGreenValue
        );
    }
    private void SetFogColorGreenValue(float greenColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Green,
        value: greenColor,
        lastValidColor: ref _lastValidFogColor,
        inputField: _toolMenuComponents.Extras_FogColor_Green,
        preview: _toolMenuComponents.Extras_FogColorPreview
        );
    }

    private void FogColorBlueValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidFogColor.b,
        onValidValuePassed: OnFogColorBlueValueChanged,
        onInvalidValuePassed: SetFogColorBlueValue
        );
    }
    private void SetFogColorBlueValue(float blueColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Blue,
        value: blueColor,
        lastValidColor: ref _lastValidFogColor,
        inputField: _toolMenuComponents.Extras_FogColor_Blue,
        preview: _toolMenuComponents.Extras_FogColorPreview
        );
    }

    public void SetFogColorValue(Color color)
    {
      ToolMenuHelper.UpdateColorInputFields(
        value: color,
        lastValidColor: ref _lastValidFogColor,
        redIF: _toolMenuComponents.Extras_FogColor_Red,
        greenIF: _toolMenuComponents.Extras_FogColor_Green,
        blueIF: _toolMenuComponents.Extras_FogColor_Blue,
        preview: _toolMenuComponents.Extras_FogColorPreview
        );
    }


    private void FogDensityValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidFogDensity,
        onValidValuePassed: OnFogDensityValueChanged,
        onInvalidValuePassed: SetFogDensityValue
        );
    }
    public void SetFogDensityValue(float intensity)
    {
      _toolMenuComponents.Extras_FogDensity.SetTextWithoutNotify(intensity.ToString("0.00"));
      _lastValidFogDensity = intensity;
    }
    #endregion

    #region Main Lighting
    private void MainLightingColorRedValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidMainLightingColor.r,
        onValidValuePassed: OnMainLightingColorRedValueChanged,
        onInvalidValuePassed: SetMainLightingColorRedValue
        );
    }
    private void SetMainLightingColorRedValue(float redColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Red,
        value: redColor,
        lastValidColor: ref _lastValidMainLightingColor,
        inputField: _toolMenuComponents.Extras_MainLightingColor_Red,
        preview: _toolMenuComponents.Extras_MainLightingColorPreview
        );
    }

    private void MainLightingColorGreenValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidMainLightingColor.g,
        onValidValuePassed: OnMainLightingColorGreenValueChanged,
        onInvalidValuePassed: SetMainLightingColorGreenValue
        );
    }
    private void SetMainLightingColorGreenValue(float greenColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Green,
        value: greenColor,
        lastValidColor: ref _lastValidMainLightingColor,
        inputField: _toolMenuComponents.Extras_MainLightingColor_Green,
        preview: _toolMenuComponents.Extras_MainLightingColorPreview
        );
    }

    private void MainLightingColorBlueValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidMainLightingColor.b,
        onValidValuePassed: OnMainLightingColorBlueValueChanged,
        onInvalidValuePassed: SetMainLightingColorBlueValue
        );
    }
    private void SetMainLightingColorBlueValue(float blueColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Blue,
        value: blueColor,
        lastValidColor: ref _lastValidMainLightingColor,
        inputField: _toolMenuComponents.Extras_MainLightingColor_Blue,
        preview: _toolMenuComponents.Extras_MainLightingColorPreview
        );
    }

    public void SetMainLightingColorValue(Color color)
    {
      ToolMenuHelper.UpdateColorInputFields(
        value: color,
        lastValidColor: ref _lastValidMainLightingColor,
        redIF: _toolMenuComponents.Extras_MainLightingColor_Red,
        greenIF: _toolMenuComponents.Extras_MainLightingColor_Green,
        blueIF: _toolMenuComponents.Extras_MainLightingColor_Blue,
        preview: _toolMenuComponents.Extras_MainLightingColorPreview
        );
    }


    private void MainLightingRotationXValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidMainLightingRotation.x,
        onValidValuePassed: OnMainLightingRotationXValueChanged,
        onInvalidValuePassed: SetMainLightingRotationXValue
        );
    }
    private void SetMainLightingRotationXValue(float x)
    {
      _toolMenuComponents.Extras_MainLightingRotation_X.SetTextWithoutNotify(x.ToString("0.00"));
      _lastValidMainLightingRotation.x = x;
    }

    private void MainLightingRotationYValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidMainLightingRotation.y,
        onValidValuePassed: OnMainLightingRotationYValueChanged,
        onInvalidValuePassed: SetMainLightingRotationYValue
        );
    }
    private void SetMainLightingRotationYValue(float y)
    {
      _toolMenuComponents.Extras_MainLightingRotation_Y.SetTextWithoutNotify(y.ToString("0.00"));
      _lastValidMainLightingRotation.y = y;
    }

    private void MainLightingRotationZValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidMainLightingRotation.z,
        onValidValuePassed: OnMainLightingRotationZValueChanged,
        onInvalidValuePassed: SetMainLightingRotationZValue
        );
    }
    private void SetMainLightingRotationZValue(float z)
    {
      _toolMenuComponents.Extras_MainLightingRotation_Z.SetTextWithoutNotify(z.ToString("0.00"));
      _lastValidMainLightingRotation.z = z;
    }

    public void SetMainLightingRotationValue(Vector3 rotationInEuler)
    {
      _toolMenuComponents.Extras_MainLightingRotation_X.SetTextWithoutNotify(rotationInEuler.x.ToString("0.00"));
      _toolMenuComponents.Extras_MainLightingRotation_Y.SetTextWithoutNotify(rotationInEuler.y.ToString("0.00"));
      _toolMenuComponents.Extras_MainLightingRotation_Z.SetTextWithoutNotify(rotationInEuler.z.ToString("0.00"));

      _lastValidMainLightingRotation = rotationInEuler;
    }


    private void MainLightingIntensityValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidMainLightingIntensity,
        onValidValuePassed: OnMainLightingIntensityValueChanged,
        onInvalidValuePassed: SetMainLightingIntensityValue
        );
    }
    public void SetMainLightingIntensityValue(float intensity)
    {
      _toolMenuComponents.Extras_MainLightingIntensity.SetTextWithoutNotify(intensity.ToString("0.00"));
      _lastValidMainLightingIntensity = intensity;
    }
    #endregion

    #region Character Lighting
    private void CharacterLightingColorRedValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidCharacterLightingColor.r,
        onValidValuePassed: OnCharacterLightingColorRedValueChanged,
        onInvalidValuePassed: SetCharacterLightingColorRedValue
        );
    }
    private void SetCharacterLightingColorRedValue(float redColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Red,
        value: redColor,
        lastValidColor: ref _lastValidCharacterLightingColor,
        inputField: _toolMenuComponents.Extras_CharacterLightingColor_Red,
        preview: _toolMenuComponents.Extras_CharacterLightingColorPreview
        );
    }

    private void CharacterLightingColorGreenValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidCharacterLightingColor.g,
        onValidValuePassed: OnCharacterLightingColorGreenValueChanged,
        onInvalidValuePassed: SetCharacterLightingColorGreenValue
        );
    }
    private void SetCharacterLightingColorGreenValue(float greenColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Green,
        value: greenColor,
        lastValidColor: ref _lastValidCharacterLightingColor,
        inputField: _toolMenuComponents.Extras_CharacterLightingColor_Green,
        preview: _toolMenuComponents.Extras_CharacterLightingColorPreview
        );
    }

    private void CharacterLightingColorBlueValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidCharacterLightingColor.b,
        onValidValuePassed: OnCharacterLightingColorBlueValueChanged,
        onInvalidValuePassed: SetCharacterLightingColorBlueValue
        );
    }
    private void SetCharacterLightingColorBlueValue(float blueColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Blue,
        value: blueColor,
        lastValidColor: ref _lastValidCharacterLightingColor,
        inputField: _toolMenuComponents.Extras_CharacterLightingColor_Blue,
        preview: _toolMenuComponents.Extras_CharacterLightingColorPreview
        );
    }

    public void SetCharacterLightingColorValue(Color color)
    {
      ToolMenuHelper.UpdateColorInputFields(
        value: color,
        lastValidColor: ref _lastValidCharacterLightingColor,
        redIF: _toolMenuComponents.Extras_CharacterLightingColor_Red,
        greenIF: _toolMenuComponents.Extras_CharacterLightingColor_Green,
        blueIF: _toolMenuComponents.Extras_CharacterLightingColor_Blue,
        preview: _toolMenuComponents.Extras_CharacterLightingColorPreview
        );
    }


    private void CharacterLightingIntensityValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidCharacterLightingIntensity,
        onValidValuePassed: OnCharacterLightingIntensityValueChanged,
        onInvalidValuePassed: SetCharacterLightingIntensityValue
        );
    }
    public void SetCharacterLightingIntensityValue(float intensity)
    {
      _toolMenuComponents.Extras_CharacterLightingIntensity.SetTextWithoutNotify(intensity.ToString("0.00"));
      _lastValidCharacterLightingIntensity = intensity;
    }
    #endregion

    #region Extra Lighting
    private void ExtraLightingColorRedValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidExtraLightingColor.r,
        onValidValuePassed: OnExtraLightingColorRedValueChanged,
        onInvalidValuePassed: SetExtraLightingColorRedValue
        );
    }
    private void SetExtraLightingColorRedValue(float redColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Red,
        value: redColor,
        lastValidColor: ref _lastValidExtraLightingColor,
        inputField: _toolMenuComponents.Extras_ExtraLightingColor_Red,
        preview: _toolMenuComponents.Extras_ExtraLightingColorPreview
        );
    }

    private void ExtraLightingColorGreenValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidExtraLightingColor.g,
        onValidValuePassed: OnExtraLightingColorGreenValueChanged,
        onInvalidValuePassed: SetExtraLightingColorGreenValue
        );
    }
    private void SetExtraLightingColorGreenValue(float greenColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Green,
        value: greenColor,
        lastValidColor: ref _lastValidExtraLightingColor,
        inputField: _toolMenuComponents.Extras_ExtraLightingColor_Green,
        preview: _toolMenuComponents.Extras_ExtraLightingColorPreview
        );
    }

    private void ExtraLightingColorBlueValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidExtraLightingColor.b,
        onValidValuePassed: OnExtraLightingColorBlueValueChanged,
        onInvalidValuePassed: SetExtraLightingColorBlueValue
        );
    }
    private void SetExtraLightingColorBlueValue(float blueColor)
    {
      ToolMenuHelper.UpdateColorChannelInputField(
        colorChannel: ColorChannels.Blue,
        value: blueColor,
        lastValidColor: ref _lastValidExtraLightingColor,
        inputField: _toolMenuComponents.Extras_ExtraLightingColor_Blue,
        preview: _toolMenuComponents.Extras_ExtraLightingColorPreview
        );
    }

    public void SetExtraLightingColorValue(Color color)
    {
      ToolMenuHelper.UpdateColorInputFields(
        value: color,
        lastValidColor: ref _lastValidExtraLightingColor,
        redIF: _toolMenuComponents.Extras_ExtraLightingColor_Red,
        greenIF: _toolMenuComponents.Extras_ExtraLightingColor_Green,
        blueIF: _toolMenuComponents.Extras_ExtraLightingColor_Blue,
        preview: _toolMenuComponents.Extras_ExtraLightingColorPreview
        );
    }


    private void ExtraLightingIntensityValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(
        value: value,
        previousValue: _lastValidExtraLightingIntensity,
        onValidValuePassed: OnExtraLightingIntensityValueChanged,
        onInvalidValuePassed: SetExtraLightingIntensityValue
        );
    }
    public void SetExtraLightingIntensityValue(float intensity)
    {
      _toolMenuComponents.Extras_ExtraLightingIntensity.SetTextWithoutNotify(intensity.ToString("0.00"));
      _lastValidExtraLightingIntensity = intensity;
    }
    #endregion
  }
}