using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Nexile.JKQuest;
using Nexile.JKQuest.Rollback.Entities.Player;
using JKQScreenshotsToolMod.Patches;

namespace JKQScreenshotsToolMod.Helpers
{
  public class InventoryHelper
  {
    #region Fields
    // Components
    private GameObject fullBodyPreviewGO = null;
    private GameObject itemPreviewGO = null;
    private PlayerMeshManager mannequinMeshManager = null;
    private List<GameObject> mannequin = new List<GameObject>();

    // Inventory Camera Data
    private InventoryCameraParams fullBodyCameraParams;
    private InventoryCameraParams hatCameraParams;
    private InventoryCameraParams headCameraParams;
    private InventoryCameraParams torsoCameraParams;
    private InventoryCameraParams handsCameraParams;
    private InventoryCameraParams legsCameraParams;
    private InventoryCameraParams baseCameraParams;
    private InventoryCameraParams weaponCameraParams;
    private InventoryCameraParams shieldCameraParams;

    // States
    private bool isMannequinHidden = false;
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
      fullBodyPreviewGO = inventoryDressingRoom.transform.Find("Full Player Preview")?.gameObject;
      itemPreviewGO = inventoryDressingRoom.transform.Find("Item Preview")?.gameObject;

      // Get Mannequin parts and its Mesh Manager
      mannequinMeshManager = itemPreviewGO.GetComponentInChildren<PlayerMeshManager>(true);
      UpdateMannequinParts(mannequinMeshManager);

      // Get Camera parameters for each item type
      fullBodyCameraParams = new InventoryCameraParams(fullBodyPreviewGO.transform.Find("Inventory Camera")?.GetComponent<Camera>());
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
      fullBodyPreviewGO = null;
      itemPreviewGO = null;
      mannequinMeshManager = null;
      mannequin.Clear();

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
      fullBodyPreviewGO.SetActive(enable);
    }
    private void EnableItemPreview(bool enable)
    {
      itemPreviewGO.SetActive(enable);
    }
    #endregion

    #region Mannequin
    private void UpdateMannequinParts(PlayerMeshManager playerMeshManager)
    {
      // If event owner isn't the Mannequin Mesh Manager, skip
      if (playerMeshManager != mannequinMeshManager) return;

      // Clear previous mannequin components
      mannequin.Clear();

      // Get Torso
      GameObject mannequinTorso = (GameObject)typeof(PlayerMeshManager)
        .GetField("torso", BindingFlags.Instance | BindingFlags.NonPublic)
        .GetValue(mannequinMeshManager);
      if (mannequinTorso.name == "torso_mannequin") mannequinTorso.SetActive(!isMannequinHidden);
      mannequin.Add(mannequinTorso);

      // Get Hands
      GameObject mannequinHands = (GameObject)typeof(PlayerMeshManager)
        .GetField("hands", BindingFlags.Instance | BindingFlags.NonPublic)
        .GetValue(mannequinMeshManager);
      if (mannequinHands.name == "hands_mannequin") mannequinHands.SetActive(!isMannequinHidden);
      mannequin.Add(mannequinHands);

      // Get Legs
      GameObject mannequinLegs = (GameObject)typeof(PlayerMeshManager)
        .GetField("legs", BindingFlags.Instance | BindingFlags.NonPublic)
        .GetValue(mannequinMeshManager);
      if (mannequinLegs.name == "legs_mannequin") mannequinLegs.SetActive(!isMannequinHidden);
      mannequin.Add(mannequinLegs);
    }
    public void HideInventoryMannequin(bool hide)
    {
      isMannequinHidden = hide;

      foreach (GameObject mannequinPart in mannequin)
      {
        if (mannequinPart.name == "torso_mannequin" || mannequinPart.name == "hands_mannequin" || mannequinPart.name == "legs_mannequin")
        {
          mannequinPart.SetActive(!isMannequinHidden);
        }
      }
    }
    #endregion
  }
}