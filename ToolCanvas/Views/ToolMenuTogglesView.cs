using System;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuTogglesView
  {
    private ToolMenuComponents _toolMenuComponents;

    public event Action<bool> OnHideGUIStateChanged = null;
    public event Action<bool> OnHideBackgroundStateChanged = null;
    public event Action<bool> OnHideTerrainStateChanged = null;
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


    private ToolMenuTogglesView() { }
    public ToolMenuTogglesView(ToolMenuComponents toolMenuComponents)
    {
      _toolMenuComponents = toolMenuComponents;
    }

    public void SubscribeEvents()
    {
      _toolMenuComponents.Toggles_HideGUI.onValueChanged.AddListener(HideGUIToggled);
      _toolMenuComponents.Toggles_HideBackground.onValueChanged.AddListener(HideBackgroundToggled);
      _toolMenuComponents.Toggles_HideTerrain.onValueChanged.AddListener(HideTerrainToggled);
      _toolMenuComponents.Toggles_HidePlayers.onValueChanged.AddListener(HidePlayersToggled);
      _toolMenuComponents.Toggles_HideEnemies.onValueChanged.AddListener(HideEnemiesToggled);
      _toolMenuComponents.Toggles_HideNPCS.onValueChanged.AddListener(HideNPCSToggled);

      _toolMenuComponents.Toggles_DisableFog.onValueChanged.AddListener(DisableFogToggled);
      _toolMenuComponents.Toggles_DisableParticles.onValueChanged.AddListener(DisableParticlesToggled);
      _toolMenuComponents.Toggles_DisableBloom.onValueChanged.AddListener(DisableBloomToggled);
      _toolMenuComponents.Toggles_DisableVignetting.onValueChanged.AddListener(DisableVignettingToggled);
      _toolMenuComponents.Toggles_DisableWaterEffect.onValueChanged.AddListener(DisableWaterEffectToggled);
      _toolMenuComponents.Toggles_DisableSkybox.onValueChanged.AddListener(DisableSkyboxToggled);

      _toolMenuComponents.Toggles_PointFiltering.onValueChanged.AddListener(EnablePointFilteringToggled);
      _toolMenuComponents.Toggles_ShowRealModels.onValueChanged.AddListener(EnableShowRealModelsToggled);
    }
    public void UnsubscribeEvents()
    {
      _toolMenuComponents.Toggles_HideGUI.onValueChanged.RemoveListener(HideGUIToggled);
      _toolMenuComponents.Toggles_HideBackground.onValueChanged.RemoveListener(HideBackgroundToggled);
      _toolMenuComponents.Toggles_HideTerrain.onValueChanged.RemoveListener(HideTerrainToggled);
      _toolMenuComponents.Toggles_HidePlayers.onValueChanged.RemoveListener(HidePlayersToggled);
      _toolMenuComponents.Toggles_HideEnemies.onValueChanged.RemoveListener(HideEnemiesToggled);
      _toolMenuComponents.Toggles_HideNPCS.onValueChanged.RemoveListener(HideNPCSToggled);

      _toolMenuComponents.Toggles_DisableFog.onValueChanged.RemoveListener(DisableFogToggled);
      _toolMenuComponents.Toggles_DisableParticles.onValueChanged.RemoveListener(DisableParticlesToggled);
      _toolMenuComponents.Toggles_DisableBloom.onValueChanged.RemoveListener(DisableBloomToggled);
      _toolMenuComponents.Toggles_DisableVignetting.onValueChanged.RemoveListener(DisableVignettingToggled);
      _toolMenuComponents.Toggles_DisableWaterEffect.onValueChanged.RemoveListener(DisableWaterEffectToggled);
      _toolMenuComponents.Toggles_DisableSkybox.onValueChanged.RemoveListener(DisableSkyboxToggled);

      _toolMenuComponents.Toggles_PointFiltering.onValueChanged.RemoveListener(EnablePointFilteringToggled);
      _toolMenuComponents.Toggles_ShowRealModels.onValueChanged.RemoveListener(EnableShowRealModelsToggled);
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

    private void HideBackgroundToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnHideBackgroundStateChanged);
    }
    public void SetHideBackgroundToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_HideBackground.SetIsOnWithoutNotify(isOn);
    }

    private void HideTerrainToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnHideTerrainStateChanged);
    }
    public void SetHideTerrainToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_HideTerrain.SetIsOnWithoutNotify(isOn);
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

    // FOG, PARTICLES, BLOOM, VIGNETTING, WATER EFFECT, SKYBOX
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

    private void DisableWaterEffectToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnDisableWaterEffectStateChanged);
    }
    public void SetDisableWaterEffectToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_DisableWaterEffect.SetIsOnWithoutNotify(isOn);
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

    // SET POINT FILTER MODE, SHOW REAL 3D MODELS
    #region Misc Toggles
    private void EnablePointFilteringToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnEnablePointFilteringStateChanged);
    }
    public void SetEnablePointFilteringToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_PointFiltering.SetIsOnWithoutNotify(isOn);
    }

    private void EnableShowRealModelsToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnShowRealModelsStateChanged);
    }
    public void SetEnableShowRealModelsToggleState(bool isOn)
    {
      _toolMenuComponents.Toggles_ShowRealModels.SetIsOnWithoutNotify(isOn);
    }
    #endregion
  }
}