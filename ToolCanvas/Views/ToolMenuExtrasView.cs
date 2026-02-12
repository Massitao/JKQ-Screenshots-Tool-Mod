using System;
using UnityEngine;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuExtrasView
  {
    private ToolMenuComponents _toolMenuComponents;

    private Color _lastValidSkyboxColor = Color.black;
    public event Action<float> OnSkyboxColorRedValueChanged = null;
    public event Action<float> OnSkyboxColorGreenValueChanged = null;
    public event Action<float> OnSkyboxColorBlueValueChanged = null;

    private Color _lastValidMainLightingColor = Color.black;
    private Vector3 _lastValidMainLightingRotation = Vector3.zero;
    private float _lastValidMainLightingIntensity = 1f;
    public event Action<float> OnMainLightingColorRedValueChanged = null;
    public event Action<float> OnMainLightingColorGreenValueChanged = null;
    public event Action<float> OnMainLightingColorBlueValueChanged = null;
    public event Action<float> OnMainLightingRotationXValueChanged = null;
    public event Action<float> OnMainLightingRotationYValueChanged = null;
    public event Action<float> OnMainLightingRotationZValueChanged = null;
    public event Action<float> OnMainLightingIntensityValueChanged = null;

    private Color _lastValidCharacterLightingColor = Color.black;
    private float _lastValidCharacterLightingIntensity = 1f;
    public event Action<float> OnCharacterLightingColorRedValueChanged = null;
    public event Action<float> OnCharacterLightingColorGreenValueChanged = null;
    public event Action<float> OnCharacterLightingColorBlueValueChanged = null;
    public event Action<float> OnCharacterLightingIntensityValueChanged = null;

    private Color _lastValidExtraLightingColor = Color.black;
    private float _lastValidExtraLightingIntensity = 1f;
    public event Action<float> OnExtraLightingColorRedValueChanged = null;
    public event Action<float> OnExtraLightingColorGreenValueChanged = null;
    public event Action<float> OnExtraLightingColorBlueValueChanged = null;
    public event Action<float> OnExtraLightingIntensityValueChanged = null;


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
      ToolMenuHelper.UpdateInputField(value, _lastValidSkyboxColor.r, OnSkyboxColorRedValueChanged, SetSkyboxColorRedValue);
    }
    private void SetSkyboxColorRedValue(float redColor)
    {
      _toolMenuComponents.Extras_SkyboxColor_Red.SetTextWithoutNotify(redColor.ToString("0.00"));
      _lastValidSkyboxColor.r = redColor;
      _lastValidSkyboxColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_SkyboxColorPreview, _lastValidSkyboxColor);
    }

    private void SkyboxColorGreenValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidSkyboxColor.g, OnSkyboxColorGreenValueChanged, SetSkyboxColorGreenValue);
    }
    private void SetSkyboxColorGreenValue(float greenColor)
    {
      _toolMenuComponents.Extras_SkyboxColor_Green.SetTextWithoutNotify(greenColor.ToString("0.00"));
      _lastValidSkyboxColor.g = greenColor;
      _lastValidSkyboxColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_SkyboxColorPreview, _lastValidSkyboxColor);
    }

    private void SkyboxColorBlueValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidSkyboxColor.b, OnSkyboxColorBlueValueChanged, SetSkyboxColorBlueValue);
    }
    private void SetSkyboxColorBlueValue(float blueColor)
    {
      _toolMenuComponents.Extras_SkyboxColor_Blue.SetTextWithoutNotify(blueColor.ToString("0.00"));
      _lastValidSkyboxColor.b = blueColor;
      _lastValidSkyboxColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_SkyboxColorPreview, _lastValidSkyboxColor);
    }

    public void SetSkyboxColorValue(Color color)
    {
      _toolMenuComponents.Extras_SkyboxColor_Red.SetTextWithoutNotify(color.r.ToString("0.00"));
      _toolMenuComponents.Extras_SkyboxColor_Green.SetTextWithoutNotify(color.g.ToString("0.00"));
      _toolMenuComponents.Extras_SkyboxColor_Blue.SetTextWithoutNotify(color.b.ToString("0.00"));

      _lastValidSkyboxColor = color;
      _lastValidSkyboxColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_SkyboxColorPreview, _lastValidSkyboxColor);
    }
    #endregion

    #region Main Lighting
    private void MainLightingColorRedValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidMainLightingColor.r, OnMainLightingColorRedValueChanged, SetMainLightingColorRedValue);
    }
    private void SetMainLightingColorRedValue(float redColor)
    {
      _toolMenuComponents.Extras_MainLightingColor_Red.SetTextWithoutNotify(redColor.ToString("0.00"));
      _lastValidMainLightingColor.r = redColor;
      _lastValidMainLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_MainLightingColorPreview, _lastValidMainLightingColor);
    }

    private void MainLightingColorGreenValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidMainLightingColor.g, OnMainLightingColorGreenValueChanged, SetMainLightingColorGreenValue);
    }
    private void SetMainLightingColorGreenValue(float greenColor)
    {
      _toolMenuComponents.Extras_MainLightingColor_Green.SetTextWithoutNotify(greenColor.ToString("0.00"));
      _lastValidMainLightingColor.g = greenColor;
      _lastValidMainLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_MainLightingColorPreview, _lastValidMainLightingColor);
    }

    private void MainLightingColorBlueValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidMainLightingColor.b, OnMainLightingColorBlueValueChanged, SetMainLightingColorBlueValue);
    }
    private void SetMainLightingColorBlueValue(float blueColor)
    {
      _toolMenuComponents.Extras_SkyboxColor_Blue.SetTextWithoutNotify(blueColor.ToString("0.00"));
      _lastValidSkyboxColor.b = blueColor;
      _lastValidMainLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_SkyboxColorPreview, _lastValidSkyboxColor);
    }

    public void SetMainLightingColorValue(Color color)
    {
      _toolMenuComponents.Extras_MainLightingColor_Red.SetTextWithoutNotify(color.r.ToString("0.00"));
      _toolMenuComponents.Extras_MainLightingColor_Green.SetTextWithoutNotify(color.g.ToString("0.00"));
      _toolMenuComponents.Extras_MainLightingColor_Blue.SetTextWithoutNotify(color.b.ToString("0.00"));

      _lastValidMainLightingColor = color;
      _lastValidMainLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_MainLightingColorPreview, _lastValidMainLightingColor);
    }


    private void MainLightingRotationXValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidMainLightingRotation.x, OnMainLightingRotationXValueChanged, SetMainLightingRotationXValue);
    }
    private void SetMainLightingRotationXValue(float x)
    {
      _toolMenuComponents.Extras_MainLightingRotation_X.SetTextWithoutNotify(x.ToString("0.00"));
      _lastValidMainLightingRotation.x = x;
    }

    private void MainLightingRotationYValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidMainLightingRotation.y, OnMainLightingRotationYValueChanged, SetMainLightingRotationYValue);
    }
    private void SetMainLightingRotationYValue(float y)
    {
      _toolMenuComponents.Extras_MainLightingRotation_Y.SetTextWithoutNotify(y.ToString("0.00"));
      _lastValidMainLightingRotation.x = y;
    }

    private void MainLightingRotationZValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidMainLightingRotation.z, OnMainLightingRotationZValueChanged, SetMainLightingRotationZValue);
    }
    private void SetMainLightingRotationZValue(float z)
    {
      _toolMenuComponents.Extras_MainLightingRotation_Z.SetTextWithoutNotify(z.ToString("0.00"));
      _lastValidMainLightingRotation.x = z;
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
      ToolMenuHelper.UpdateInputField(value, _lastValidMainLightingIntensity, OnMainLightingIntensityValueChanged, SetMainLightingIntensityValue);
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
      ToolMenuHelper.UpdateInputField(value, _lastValidCharacterLightingColor.r, OnCharacterLightingColorRedValueChanged, SetCharacterLightingColorRedValue);
    }
    private void SetCharacterLightingColorRedValue(float redColor)
    {
      _toolMenuComponents.Extras_CharacterLightingColor_Red.SetTextWithoutNotify(redColor.ToString("0.00"));
      _lastValidCharacterLightingColor.r = redColor;
      _lastValidCharacterLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_CharacterLightingColorPreview, _lastValidCharacterLightingColor);
    }

    private void CharacterLightingColorGreenValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidCharacterLightingColor.g, OnCharacterLightingColorGreenValueChanged, SetCharacterLightingColorGreenValue);
    }
    private void SetCharacterLightingColorGreenValue(float greenColor)
    {
      _toolMenuComponents.Extras_CharacterLightingColor_Green.SetTextWithoutNotify(greenColor.ToString("0.00"));
      _lastValidCharacterLightingColor.g = greenColor;
      _lastValidCharacterLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_CharacterLightingColorPreview, _lastValidCharacterLightingColor);
    }

    private void CharacterLightingColorBlueValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidCharacterLightingColor.b, OnCharacterLightingColorBlueValueChanged, SetCharacterLightingColorBlueValue);
    }
    private void SetCharacterLightingColorBlueValue(float blueColor)
    {
      _toolMenuComponents.Extras_SkyboxColor_Blue.SetTextWithoutNotify(blueColor.ToString("0.00"));
      _lastValidSkyboxColor.b = blueColor;
      _lastValidCharacterLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_SkyboxColorPreview, _lastValidSkyboxColor);
    }
    
    public void SetCharacterLightingColorValue(Color color)
    {
      _toolMenuComponents.Extras_CharacterLightingColor_Red.SetTextWithoutNotify(color.r.ToString("0.00"));
      _toolMenuComponents.Extras_CharacterLightingColor_Green.SetTextWithoutNotify(color.g.ToString("0.00"));
      _toolMenuComponents.Extras_CharacterLightingColor_Blue.SetTextWithoutNotify(color.b.ToString("0.00"));

      _lastValidCharacterLightingColor = color;
      _lastValidCharacterLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_CharacterLightingColorPreview, _lastValidCharacterLightingColor);
    }
    

    private void CharacterLightingIntensityValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidCharacterLightingIntensity, OnCharacterLightingIntensityValueChanged, SetCharacterLightingIntensityValue);
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
      ToolMenuHelper.UpdateInputField(value, _lastValidExtraLightingColor.r, OnExtraLightingColorRedValueChanged, SetExtraLightingColorRedValue);
    }
    private void SetExtraLightingColorRedValue(float redColor)
    {
      _toolMenuComponents.Extras_ExtraLightingColor_Red.SetTextWithoutNotify(redColor.ToString("0.00"));
      _lastValidExtraLightingColor.r = redColor;
      _lastValidExtraLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_ExtraLightingColorPreview, _lastValidExtraLightingColor);
    }

    private void ExtraLightingColorGreenValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidExtraLightingColor.g, OnExtraLightingColorGreenValueChanged, SetExtraLightingColorGreenValue);
    }
    private void SetExtraLightingColorGreenValue(float greenColor)
    {
      _toolMenuComponents.Extras_ExtraLightingColor_Green.SetTextWithoutNotify(greenColor.ToString("0.00"));
      _lastValidExtraLightingColor.g = greenColor;
      _lastValidExtraLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_ExtraLightingColorPreview, _lastValidExtraLightingColor);
    }

    private void ExtraLightingColorBlueValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidExtraLightingColor.b, OnExtraLightingColorBlueValueChanged, SetExtraLightingColorBlueValue);
    }
    private void SetExtraLightingColorBlueValue(float blueColor)
    {
      _toolMenuComponents.Extras_SkyboxColor_Blue.SetTextWithoutNotify(blueColor.ToString("0.00"));
      _lastValidSkyboxColor.b = blueColor;
      _lastValidExtraLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_SkyboxColorPreview, _lastValidSkyboxColor);
    }

    public void SetExtraLightingColorValue(Color color)
    {
      _toolMenuComponents.Extras_ExtraLightingColor_Red.SetTextWithoutNotify(color.r.ToString("0.00"));
      _toolMenuComponents.Extras_ExtraLightingColor_Green.SetTextWithoutNotify(color.g.ToString("0.00"));
      _toolMenuComponents.Extras_ExtraLightingColor_Blue.SetTextWithoutNotify(color.b.ToString("0.00"));

      _lastValidExtraLightingColor = color;
      _lastValidExtraLightingColor.a = 1f;

      ToolMenuHelper.UpdateColorPreview(_toolMenuComponents.Extras_ExtraLightingColorPreview, _lastValidExtraLightingColor);
    }


    private void ExtraLightingIntensityValueChanged(string value)
    {
      ToolMenuHelper.UpdateInputField(value, _lastValidExtraLightingIntensity, OnExtraLightingIntensityValueChanged, SetExtraLightingIntensityValue);
    }
    public void SetExtraLightingIntensityValue(float intensity)
    {
      _toolMenuComponents.Extras_ExtraLightingIntensity.SetTextWithoutNotify(intensity.ToString("0.00"));
      _lastValidExtraLightingIntensity = intensity;
    }
    #endregion
  }
}