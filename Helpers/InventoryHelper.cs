using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Nexile.JKQuest;
using Nexile.JKQuest.Rollback.Entities.Player;
using JKQScreenshotsToolMod.Patches;
using HarmonyLib;

namespace JKQScreenshotsToolMod.Helpers
{
  public class InventoryHelper
  {
    #region Fields
    // Components
    private GameObject _fullBodyPreviewGO = null;
    private GameObject _itemPreviewGO = null;
    private PlayerMeshManager _mannequinMeshManager = null;
    private List<GameObject> _mannequin = new List<GameObject>();

    // Inventory Camera Data
    private InventoryCameraParams _fullBodyCameraParams;
    private InventoryCameraParams _hatCameraParams;
    private InventoryCameraParams _headCameraParams;
    private InventoryCameraParams _torsoCameraParams;
    private InventoryCameraParams _handsCameraParams;
    private InventoryCameraParams _legsCameraParams;
    private InventoryCameraParams _baseCameraParams;
    private InventoryCameraParams _weaponCameraParams;
    private InventoryCameraParams _shieldCameraParams;

    // States
    private bool _isMannequinHidden = false;

    // Reflection References
    private static readonly Type PlayerMeshManagerTypeRef = typeof(PlayerMeshManager);
    private static readonly FieldInfo MannequinTorsoFieldInfo = AccessTools.Field(PlayerMeshManagerTypeRef, "torso");
    private static readonly FieldInfo MannequinHandsFieldInfo = AccessTools.Field(PlayerMeshManagerTypeRef, "hands");
    private static readonly FieldInfo MannequinLegsFieldInfo = AccessTools.Field(PlayerMeshManagerTypeRef, "legs");

    #endregion


    #region Initialization Methods
    public void Init()
    {
      // Find InventoryDressingRoom component in CoreScene
      InventoryDressingRoom inventoryDressingRoom = null;

      List<GameObject> sceneGOList = new List<GameObject>();
      SceneManager.GetSceneByName(GameHelper.CoreScene).GetRootGameObjects(sceneGOList);
      foreach (GameObject sceneGO in sceneGOList)
      {
        inventoryDressingRoom = sceneGO.GetComponentInChildren<InventoryDressingRoom>(true);
        if (inventoryDressingRoom != null) break;
      }
     
      // Get parent GameObjects for Full Body, and Mannequin previews 
      _fullBodyPreviewGO = inventoryDressingRoom.transform.Find("Full Player Preview")?.gameObject;
      _itemPreviewGO = inventoryDressingRoom.transform.Find("Item Preview")?.gameObject;

      // Get Mannequin parts and its Mesh Manager
      _mannequinMeshManager = _itemPreviewGO.GetComponentInChildren<PlayerMeshManager>(true);
      UpdateMannequinParts(_mannequinMeshManager);

      // Get Camera parameters for each item type
      fullBodyCameraParams = new InventoryCameraParams(_fullBodyPreviewGO.transform.Find("Inventory Camera")?.GetComponent<Camera>());
      hatCameraParams = new InventoryCameraParams(inventoryDressingRoom.transform.Find("Cameras/Camera_Hat")?.GetComponent<Camera>());
      headCameraParams = new InventoryCameraParams(inventoryDressingRoom.transform.Find("Cameras/Camera_Head")?.GetComponent<Camera>());
      torsoCameraParams = new InventoryCameraParams(inventoryDressingRoom.transform.Find("Cameras/Camera_Torso")?.GetComponent<Camera>());
      handsCameraParams = new InventoryCameraParams(inventoryDressingRoom.transform.Find("Cameras/Camera_Hands")?.GetComponent<Camera>());
      legsCameraParams = new InventoryCameraParams(inventoryDressingRoom.transform.Find("Cameras/Camera_Legs")?.GetComponent<Camera>());
      baseCameraParams = new InventoryCameraParams(inventoryDressingRoom.transform.Find("Cameras/Camera_Base")?.GetComponent<Camera>());
      weaponCameraParams = new InventoryCameraParams(inventoryDressingRoom.transform.Find("Cameras/Camera_Weapon")?.GetComponent<Camera>());
      shieldCameraParams = new InventoryCameraParams(inventoryDressingRoom.transform.Find("Cameras/Camera_Shield")?.GetComponent<Camera>());

      // Update Mannequin parts whenever the item preview changes
      PlayerMeshManagerPatch.OnItemEquipTriggered += UpdateMannequinParts;
    }

    public void Clear()
    {
      _fullBodyPreviewGO = null;
      _itemPreviewGO = null;
      _mannequinMeshManager = null;
      _mannequin.Clear();

      PlayerMeshManagerPatch.OnItemEquipTriggered -= UpdateMannequinParts;
    }
    #endregion


    #region Inventory Camera Views
    public InventoryCameraParams ViewFullBodyPreview()
    {
      EnableFullBodyPreview(true);
      EnableItemPreview(false);

      return fullBodyCameraParams;
    }
    public InventoryCameraParams ViewHatPreview()
    {
      EnableFullBodyPreview(false);
      EnableItemPreview(true);

      return hatCameraParams;
    }
    public InventoryCameraParams ViewHeadPreview()
    {
      EnableFullBodyPreview(false);
      EnableItemPreview(true);

      return headCameraParams;
    }
    public InventoryCameraParams ViewTorsoPreview()
    {
      EnableFullBodyPreview(false);
      EnableItemPreview(true);

      return torsoCameraParams;
    }
    public InventoryCameraParams ViewHandsPreview()
    {
      EnableFullBodyPreview(false);
      EnableItemPreview(true);

      return handsCameraParams;
    }
    public InventoryCameraParams ViewLegsPreview()
    {
      EnableFullBodyPreview(false);
      EnableItemPreview(true);

      return legsCameraParams;
    }
    public InventoryCameraParams ViewBasePreview()
    {
      EnableFullBodyPreview(false);
      EnableItemPreview(true);

      return baseCameraParams;
    }
    public InventoryCameraParams ViewWeaponPreview()
    {
      EnableFullBodyPreview(false);
      EnableItemPreview(true);

      return weaponCameraParams;
    }
    public InventoryCameraParams ViewShieldPreview()
    {
      EnableFullBodyPreview(false);
      EnableItemPreview(true);

      return shieldCameraParams;
    }

    private void EnableFullBodyPreview(bool enable)
    {
      _fullBodyPreviewGO.SetActive(enable);
    }
    private void EnableItemPreview(bool enable)
    {
      _itemPreviewGO.SetActive(enable);
    }
    #endregion

    #region Mannequin
    private void UpdateMannequinParts(PlayerMeshManager playerMeshManager)
    {
      // If event owner isn't the Mannequin Mesh Manager, skip
      if (playerMeshManager != _mannequinMeshManager) return;

      // Clear previous mannequin components
      _mannequin.Clear();

      // Get Torso
      GameObject mannequinTorso = (GameObject)MannequinTorsoFieldInfo.GetValue(_mannequinMeshManager);
      _mannequin.Add(mannequinTorso);
      if (mannequinTorso.name == "torso_mannequin") mannequinTorso.SetActive(!_isMannequinHidden);

      // Get Hands
      GameObject mannequinHands = (GameObject)MannequinHandsFieldInfo.GetValue(_mannequinMeshManager);
      _mannequin.Add(mannequinHands);
      if (mannequinHands.name == "hands_mannequin") mannequinHands.SetActive(!_isMannequinHidden);

      // Get Legs
      GameObject mannequinLegs = (GameObject)MannequinLegsFieldInfo.GetValue(_mannequinMeshManager);
      _mannequin.Add(mannequinLegs);
      if (mannequinLegs.name == "legs_mannequin") mannequinLegs.SetActive(!_isMannequinHidden);
    }
    public void HideInventoryMannequin(bool hide)
    {
      _isMannequinHidden = hide;

      foreach (GameObject mannequinPart in _mannequin)
      {
        if (mannequinPart.name == "torso_mannequin" || mannequinPart.name == "hands_mannequin" || mannequinPart.name == "legs_mannequin")
        {
          mannequinPart.SetActive(!_isMannequinHidden);
        }
      }
    }
    #endregion
  }
}