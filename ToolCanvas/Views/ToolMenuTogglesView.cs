using System;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuTogglesView
  {
    private ToolMenuComponents _toolMenuComponents;

    public event Action<bool> OnHideGUIStateChanged = null;
    public event Action<bool> OnHidePlayersStateChanged = null;
    public event Action<bool> OnHideEnemiesStateChanged = null;
    public event Action<bool> OnHideNPCSStateChanged = null;

    public event Action<bool> OnDisableFogStateChanged = null;
    public event Action<bool> OnDisableParticlesStateChanged = null;
    public event Action<bool> OnDisableBloomStateChanged = null;
    public event Action<bool> OnDisableVignettingStateChanged = null;
    public event Action<bool> OnDisableWaterEffectStateChanged = null;
    public event Action<bool> OnDisableSkyboxStateChanged = null;

    public event Action<bool> OnEnablePointFilteringStateChanged = null;
    public event Action<bool> OnShowRealModelsStateChanged = null;
    public event Action<bool> OnFixFogArtifactsStateChanged = null;


    private ToolMenuTogglesView() { }
    public ToolMenuTogglesView(ToolMenuComponents toolMenuComponents)
    {
      _toolMenuComponents = toolMenuComponents;
    }

    public void SubscribeEvents()
    {
      _toolMenuComponents.Toggles_HideGUI.onValueChanged.AddListener(HideGUIToggled);
      _toolMenuComponents.Toggles_HidePlayers.onValueChanged.AddListener(HidePlayersToggled);
      _toolMenuComponents.Toggles_HideEnemies.onValueChanged.AddListener(HideEnemiesToggled);
      _toolMenuComponents.Toggles_HideNPCS.onValueChanged.AddListener(HideNPCSToggled);

      _toolMenuComponents.Toggles_DisableFog.onValueChanged.AddListener(DisableFogToggled);
      _toolMenuComponents.Toggles_DisableParticles.onValueChanged.AddListener(DisableParticlesToggled);
      _toolMenuComponents.Toggles_DisableBloom.onValueChanged.AddListener(DisableBloomToggled);
      _toolMenuComponents.Toggles_DisableVignetting.onValueChanged.AddListener(DisableVignettingToggled);
      _toolMenuComponents.Toggles_DisableSkybox.onValueChanged.AddListener(DisableSkyboxToggled);

      _toolMenuComponents.Toggles_PointFiltering.onValueChanged.AddListener(EnablePointFilteringToggled);
      _toolMenuComponents.Toggles_ShowRealModels.onValueChanged.AddListener(ShowRealModelsToggled);
      _toolMenuComponents.Toggles_FixFogArtifacts.onValueChanged.AddListener(FixFogArtifactsToggled);
    }
    public void UnsubscribeEvents()
    {
      _toolMenuComponents.Toggles_HideGUI.onValueChanged.RemoveListener(HideGUIToggled);
      _toolMenuComponents.Toggles_HidePlayers.onValueChanged.RemoveListener(HidePlayersToggled);
      _toolMenuComponents.Toggles_HideEnemies.onValueChanged.RemoveListener(HideEnemiesToggled);
      _toolMenuComponents.Toggles_HideNPCS.onValueChanged.RemoveListener(HideNPCSToggled);

      _toolMenuComponents.Toggles_DisableFog.onValueChanged.RemoveListener(DisableFogToggled);
      _toolMenuComponents.Toggles_DisableParticles.onValueChanged.RemoveListener(DisableParticlesToggled);
      _toolMenuComponents.Toggles_DisableBloom.onValueChanged.RemoveListener(DisableBloomToggled);
      _toolMenuComponents.Toggles_DisableVignetting.onValueChanged.RemoveListener(DisableVignettingToggled);
      _toolMenuComponents.Toggles_DisableSkybox.onValueChanged.RemoveListener(DisableSkyboxToggled);

      _toolMenuComponents.Toggles_PointFiltering.onValueChanged.RemoveListener(EnablePointFilteringToggled);
      _toolMenuComponents.Toggles_ShowRealModels.onValueChanged.RemoveListener(ShowRealModelsToggled);
      _toolMenuComponents.Toggles_FixFogArtifacts.onValueChanged.RemoveListener(FixFogArtifactsToggled);
    }


    // GUI, BACKGROUND, TERRAIN, PLAYERS, ENEMIES, NPCS
    #region Hide Toggles
    private void HideGUIToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnHideGUIStateChanged);
    }
    public void SetHideGUIToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_HideGUI.SetIsOnWithoutNotify(isOn);
    }

    private void HidePlayersToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnHidePlayersStateChanged);
    }
    public void SetHidePlayersToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_HidePlayers.SetIsOnWithoutNotify(isOn);
    }

    private void HideEnemiesToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnHideEnemiesStateChanged);
    }
    public void SetHideEnemiesToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_HideEnemies.SetIsOnWithoutNotify(isOn);
    }

    private void HideNPCSToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnHideNPCSStateChanged);
    }
    public void SetHideNPCSToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_HideNPCS.SetIsOnWithoutNotify(isOn);
    }
    #endregion

    // FOG, PARTICLES, BLOOM, VIGNETTING, SKYBOX
    #region Disable Toggles
    private void DisableFogToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnDisableFogStateChanged);
    }
    public void SetDisableFogToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_DisableFog.SetIsOnWithoutNotify(isOn);
    }

    private void DisableParticlesToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnDisableParticlesStateChanged);
    }
    public void SetDisableParticlesToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_DisableParticles.SetIsOnWithoutNotify(isOn);
    }

    private void DisableBloomToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnDisableBloomStateChanged);
    }
    public void SetDisableBloomToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_DisableBloom.SetIsOnWithoutNotify(isOn);
    }

    private void DisableVignettingToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnDisableVignettingStateChanged);
    }
    public void SetDisableVignettingToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_DisableVignetting.SetIsOnWithoutNotify(isOn);
    }

    private void DisableSkyboxToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnDisableSkyboxStateChanged);
    }
    public void SetDisableSkyboxToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_DisableSkybox.SetIsOnWithoutNotify(isOn);
    }
    #endregion

    // SET POINT FILTER MODE, SHOW REAL 3D MODELS, FIX FOG ARTIFACTS
    #region Misc Toggles
    private void EnablePointFilteringToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnEnablePointFilteringStateChanged);
    }
    public void SetEnablePointFilteringToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_PointFiltering.SetIsOnWithoutNotify(isOn);
    }

    private void ShowRealModelsToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnShowRealModelsStateChanged);
    }
    public void SetShowRealModelsToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_ShowRealModels.SetIsOnWithoutNotify(isOn);
    }

    private void FixFogArtifactsToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnFixFogArtifactsStateChanged);
    }
    public void SetFixFogArtifactsToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_FixFogArtifacts.SetIsOnWithoutNotify(isOn);
    }
    #endregion
  }
}