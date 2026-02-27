using System;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuInventoryView
  {
    private readonly ToolMenuComponents _toolMenuComponents;

    public event Action OnFullBodyButtonPressed;
    public event Action OnHatButtonPressed;
    public event Action OnHeadButtonPressed;
    public event Action OnTorsoButtonPressed;
    public event Action OnHandsButtonPressed;
    public event Action OnLegsButtonPressed;
    public event Action OnBaseButtonPressed;
    public event Action OnWeaponButtonPressed;
    public event Action OnShieldButtonPressed;
    public event Action<bool> OnHideMannequinStateChanged;


    private ToolMenuInventoryView() { }
    public ToolMenuInventoryView(ToolMenuComponents toolMenuComponents)
    {
      _toolMenuComponents = toolMenuComponents;
    }

    public void SubscribeEvents()
    {
      _toolMenuComponents.Inventory_FullBody.onClick.AddListener(FullBodyButtonPressed);
      _toolMenuComponents.Inventory_Hat.onClick.AddListener(HatButtonPressed);
      _toolMenuComponents.Inventory_Head.onClick.AddListener(HeadButtonPressed);
      _toolMenuComponents.Inventory_Torso.onClick.AddListener(TorsoButtonPressed);
      _toolMenuComponents.Inventory_Hands.onClick.AddListener(HandsButtonPressed);
      _toolMenuComponents.Inventory_Legs.onClick.AddListener(LegsButtonPressed);
      _toolMenuComponents.Inventory_Base.onClick.AddListener(BaseButtonPressed);
      _toolMenuComponents.Inventory_Weapon.onClick.AddListener(WeaponButtonPressed);
      _toolMenuComponents.Inventory_Shield.onClick.AddListener(ShieldButtonPressed);

      _toolMenuComponents.Inventory_HideMannequin.onValueChanged.AddListener(HideMannequinToggled);
    }
    public void UnsubscribeEvents()
    {
      _toolMenuComponents.Inventory_FullBody.onClick.RemoveListener(FullBodyButtonPressed);
      _toolMenuComponents.Inventory_Hat.onClick.RemoveListener(HatButtonPressed);
      _toolMenuComponents.Inventory_Head.onClick.RemoveListener(HeadButtonPressed);
      _toolMenuComponents.Inventory_Torso.onClick.RemoveListener(TorsoButtonPressed);
      _toolMenuComponents.Inventory_Hands.onClick.RemoveListener(HandsButtonPressed);
      _toolMenuComponents.Inventory_Legs.onClick.RemoveListener(LegsButtonPressed);
      _toolMenuComponents.Inventory_Base.onClick.RemoveListener(BaseButtonPressed);
      _toolMenuComponents.Inventory_Weapon.onClick.RemoveListener(WeaponButtonPressed);
      _toolMenuComponents.Inventory_Shield.onClick.RemoveListener(ShieldButtonPressed);

      _toolMenuComponents.Inventory_HideMannequin.onValueChanged.RemoveListener(HideMannequinToggled);
    }


    #region Toggles
    private void HideMannequinToggled(bool isOn)
    {
      ToolMenuHelper.UpdateToggleState(isOn, OnHideMannequinStateChanged);
    }
    public void Inventory_SetHideMannequinToggleState(bool isOn)
    {
      _toolMenuComponents.Inventory_HideMannequin.SetIsOnWithoutNotify(isOn);
    }
    #endregion

    #region Buttons
    private void FullBodyButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnFullBodyButtonPressed);
    }
    private void HatButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnHatButtonPressed);
    }
    private void HeadButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnHeadButtonPressed);
    }
    private void TorsoButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnTorsoButtonPressed);
    }
    private void HandsButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnHandsButtonPressed);
    }
    private void LegsButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnLegsButtonPressed);
    }
    private void BaseButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnBaseButtonPressed);
    }
    private void WeaponButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnWeaponButtonPressed);
    }
    private void ShieldButtonPressed()
    {
      ToolMenuHelper.ButtonPressed(OnShieldButtonPressed);
    }
    #endregion
  }
}