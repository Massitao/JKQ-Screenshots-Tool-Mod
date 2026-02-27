using System;
using System.Reflection;
using UnityEngine;
using FIMSpace;
using HarmonyLib;
using Nexile.JKQuest;
using JKQScreenshotsToolMod.Patches;

namespace JKQScreenshotsToolMod.Helpers
{
  public class CameraHelper
  {
    #region Fields
    // Components
    private Camera _gameCamera;
    private ManualCameraController _manualCameraController;
    private GameObject _quad2D;
    private RenderTexture _quad2DRenderTexture;
    private AttenuationTarget _attenuationTarget;
    private BeautifyEffect.Beautify _beautifyComponent;
    private VolumetricFogAndMist.VolumetricFog _fogComponent;


    // Default Camera Values
    private const float DefaultCameraZPosition = -150F;
    private const float DefaultCameraFOV = 17f;

    // Default Fog Values
    private int _defaultFogDownsampling = 0;

    // Culling Masks
    private const Int32 DefaultCullingMask = 1345207;
    private const Int32 VisibleRealModelsCullingMask = 67108863;


    // Reflection References
    private static readonly Type ManualCameraControllerTypeRef = typeof(ManualCameraController);
    private static readonly FieldInfo SpeedFieldInfo = AccessTools.Field(ManualCameraControllerTypeRef, "speed");
    private static readonly FieldInfo SmoothDampFieldInfo = AccessTools.Field(ManualCameraControllerTypeRef, "smoothDamp");
    private static readonly FieldInfo ZoomSpeedFieldInfo = AccessTools.Field(ManualCameraControllerTypeRef, "zoomSpeed");
    private static readonly FieldInfo MinFOVFieldInfo = AccessTools.Field(ManualCameraControllerTypeRef, "minFov");
    private static readonly FieldInfo MaxFOVFieldInfo = AccessTools.Field(ManualCameraControllerTypeRef, "maxFov");
    private static readonly FieldInfo CameraVelocityFieldInfo = AccessTools.Field(ManualCameraControllerTypeRef, "cameraVelocity");

    private static readonly Type RTResolutionTypeRef = typeof(RTResolution);
    private static readonly FieldInfo RenderTextureInfo = AccessTools.Field(RTResolutionTypeRef, "renderTexture");

    private static readonly Type AttenuationTargetTypeRef = typeof(AttenuationTarget);
    private static readonly FieldInfo AttenuationTargetPercentageFieldInfo = AccessTools.Field(AttenuationTargetTypeRef, "targetPercentage");

    private static readonly MethodInfo HandleModeChangedMethod = AccessTools.Method(typeof(ManualCameraController), "HandleModeChanged");


    // Events
    public event Action<bool> OnManualCameraControllerPatchInputToggled = null; // Player has activated the Manual Camera using inputs
    #endregion


    #region Initialization Methods
    public void Init()
    {
      _gameCamera = Camera.main;
      _manualCameraController = _gameCamera.GetComponentInChildren<Nexile.JKQuest.ManualCameraController>();
      _quad2D = _gameCamera.transform.Find("2dQuad").gameObject;
      _quad2DRenderTexture = (RenderTexture)RenderTextureInfo.GetValue(_gameCamera.GetComponentInChildren<RTResolution>());
      _attenuationTarget = _gameCamera.GetComponentInChildren<AttenuationTarget>();
      _beautifyComponent = _gameCamera.GetComponent<BeautifyEffect.Beautify>();
      _fogComponent = _gameCamera.GetComponent<VolumetricFogAndMist.VolumetricFog>();

      _manualCameraController.enabled = true;
      ManualCameraMinFOV = 1f;
      ManualCameraMaxFOV = 120f;
      ManualCameraZoomSpeed = 8f;

      ManualCameraControllerPatch.OnInputToggle += ManualCameraInputToggled;
    }

    public void Clear()
    {
      _gameCamera = null;
      _manualCameraController = null;
      _quad2D = null;
      _quad2DRenderTexture = null;
      _attenuationTarget = null;
      _beautifyComponent = null;
      _fogComponent = null;

      ManualCameraControllerPatch.OnInputToggle -= ManualCameraInputToggled;
    }
    #endregion


    #region Game Camera Methods
    public Vector3 CameraPosition
    {
      get => _gameCamera.transform.position;
      set => _gameCamera.transform.position = value;
    }
    public void ResetCameraZPosition()
    {
      Vector3 newPos = CameraPosition;
      newPos.z = DefaultCameraZPosition;
      CameraPosition = newPos;
    }

    public Quaternion CameraRotation
    {
      get => _gameCamera.transform.rotation;
      set => _gameCamera.transform.rotation = value;
    }
    public Vector3 CameraRotationInEuler
    {
      get => _gameCamera.transform.rotation.eulerAngles;
      set => _gameCamera.transform.rotation = Quaternion.Euler(value);
    }
    public void ResetCameraRotation()
    {
      CameraRotation = Quaternion.identity;
    }

    public float CameraFOV
    {
      get => _gameCamera.fieldOfView;
      set => _gameCamera.fieldOfView = value;
    }
    public void ResetCameraFOV()
    {
      CameraFOV = DefaultCameraFOV;
    }

    public bool IsSkyboxDisabled
    {
      get => _gameCamera.clearFlags == CameraClearFlags.SolidColor;
      set => _gameCamera.clearFlags = (value) ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
    }
    public Color CameraSkyboxColor
    {
      get => _gameCamera.backgroundColor;
      set => _gameCamera.backgroundColor = value;
    }
    #endregion

    #region Manual Camera Controller Methods
    public bool IsManualCameraControllerEnabled
    {
      get => _manualCameraController.isEnabled;
      set
      {
        if (_manualCameraController.isEnabled == value) return;

        ResetFreecamVelocity();
        HandleModeChangedMethod?.Invoke(_manualCameraController, new object[] { value });
      }
    }

    public float ManualCameraMoveSpeed
    {
      get => (float)SpeedFieldInfo.GetValue(_manualCameraController);
      set => SpeedFieldInfo.SetValue(_manualCameraController, value);
    }
    public float ManualCameraSpeedDamping
    {
      get => (float)SmoothDampFieldInfo.GetValue(_manualCameraController);
      set => SmoothDampFieldInfo.SetValue(_manualCameraController, value);
    }
    public float ManualCameraZoomSpeed
    {
      get => (float)ZoomSpeedFieldInfo.GetValue(_manualCameraController);
      set => ZoomSpeedFieldInfo.SetValue(_manualCameraController, value);
    }
    public float ManualCameraMinFOV
    {
      get => (float)MinFOVFieldInfo.GetValue(_manualCameraController);
      set => MinFOVFieldInfo.SetValue(_manualCameraController, value);
    }
    public float ManualCameraMaxFOV
    {
      get => (float)MaxFOVFieldInfo.GetValue(_manualCameraController);
      set => MaxFOVFieldInfo.SetValue(_manualCameraController, value);
    }

    public void ResetFreecamVelocity()
    {
      CameraVelocityFieldInfo.SetValue(_manualCameraController, Vector2.zero);
    }

    private void ManualCameraInputToggled(bool isOn)
    {
      ResetFreecamVelocity();
      OnManualCameraControllerPatchInputToggled?.Invoke(isOn);
    }
    #endregion

    #region 2DQuad Methods
    public bool IsPointFilterModeOn
    {
      get => _quad2DRenderTexture.filterMode == FilterMode.Point;
      set => _quad2DRenderTexture.filterMode = (value) ? FilterMode.Point : FilterMode.Bilinear;
    }

    public bool IsShowingRealModels
    {
      get => _gameCamera.cullingMask == VisibleRealModelsCullingMask;
      set
      {
        _gameCamera.cullingMask = (value) ? VisibleRealModelsCullingMask : DefaultCullingMask;
        _quad2D.SetActive(!value);
      }
    }
    #endregion

    #region Attenuation Target Methods
    public bool IsCameraAudioListener
    {
      get => (float)AttenuationTargetPercentageFieldInfo.GetValue(_attenuationTarget) == 0f;
      set => AttenuationTargetPercentageFieldInfo.SetValue(_attenuationTarget, (value) ? 0f : 0.66f);
    }
    #endregion

    #region Fog Component Methods
    public bool IsFogDisabled
    {
      get => !_fogComponent.enabled;
      set => _fogComponent.enabled = !value;
    }

    public bool AreFogArtifactsFixed
    {
      get => _fogComponent.downsampling == 0;
      set
      {
        if (_fogComponent.downsampling != 0)
        {
          _defaultFogDownsampling = _fogComponent.downsampling;
        }

        _fogComponent.downsampling = (value) ? 0 : _defaultFogDownsampling;
      }
    }

    public Color FogColor
    {
      get => _fogComponent.color;
      set => _fogComponent.color = value;
    }
    public float FogDensity
    {
      get => _fogComponent.density;
      set => _fogComponent.density = value;
    }

    #endregion

    #region Post-Processing Methods
    public bool IsBloomDisabled
    {
      get => !_beautifyComponent.bloom;
      set => _beautifyComponent.bloom = !value;
    }
    public bool IsVignettingDisabled
    {
      get => !_beautifyComponent.vignetting;
      set => _beautifyComponent.vignetting = !value;
    }
    #endregion
  }
}