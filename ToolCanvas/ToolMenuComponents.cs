using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MelonLoader;
using JKQScreenshotsToolMod.Helpers;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuComponents : MonoBehaviour
  {
    #region Fields
    // Canvas
    public Canvas ToolCanvas;
    public CanvasGroup ToolCanvasGroup;
    public List<TMP_Text> ToolCanvas_Texts;


    // Header Components
    public TMP_Text Header_Title;
    public Button Header_TakeScreenshot;
    public Button Header_CloseToolMenu;

    public TMP_Text Header_XCoord;
    public TMP_Text Header_YCoord;
    public TMP_Text Header_FOV;
    public TMP_InputField Header_DetailLevel;


    // Tabs and Views Components
    public ToolMenuTabGroup TabGroup;

    public ToolMenuTabButton Tab_Freecam;
    public ToolMenuTabButton Tab_MapTool;
    public ToolMenuTabButton Tab_Inventory;
    public ToolMenuTabButton Tab_Toggles;
    public ToolMenuTabButton Tab_Extras;

    public GameObject View_Freecam;
    public GameObject View_MapTool;
    public GameObject View_Inventory;
    public GameObject View_Toggles;
    public GameObject View_Extras;


    // Freecam View Components
    public Toggle Freecam_EnableManualCamera;
    public Toggle Freecam_AllowPlayerMovement;
    public Toggle Freecam_SetCameraAsAudioListener;

    public TMP_InputField Freecam_CameraSpeed;
    public TMP_InputField Freecam_CameraSpeedDamping;
    public TMP_InputField Freecam_CameraFOV;
    public TMP_InputField Freecam_CameraZoomSpeed;

    public Button Freecam_OpenScreenshotsFolder;

    public TMP_Text Freecam_StorageWarning;
    public TMP_Text Freecam_EstimatedScreenshotSize;
    public Button Freecam_TakeScreenshot;


    // Map Tool View Components
    public TMP_InputField MapTool_NegativeLimitX;
    public TMP_InputField MapTool_NegativeLimitY;

    public TMP_InputField MapTool_PositiveLimitX;
    public TMP_InputField MapTool_PositiveLimitY;

    public TMP_InputField MapTool_IncrementX;
    public TMP_InputField MapTool_IncrementY;

    public Button MapTool_OpenScreenshotsFolder;

    public TMP_Text MapTool_StorageWarning;
    public TMP_Text MapTool_EstimatedScreenshotSize;
    public Button MapTool_TakeScreenshots;


    // Inventory View Components
    public Button Inventory_FullBody;
    public Button Inventory_Hat;
    public Button Inventory_Head;
    public Button Inventory_Torso;
    public Button Inventory_Hands;
    public Button Inventory_Legs;
    public Button Inventory_Base;
    public Button Inventory_Weapon;
    public Button Inventory_Shield;
    public Toggle Inventory_HideMannequin;


    // Toggles View Components
    public Toggle Toggles_HideGUI;
    public Toggle Toggles_HidePlayers;
    public Toggle Toggles_HideEnemies;
    public Toggle Toggles_HideNPCS;

    public Toggle Toggles_DisableFog;
    public Toggle Toggles_DisableParticles;
    public Toggle Toggles_DisableBloom;
    public Toggle Toggles_DisableVignetting;
    public Toggle Toggles_DisableSkybox;

    public Toggle Toggles_PointFiltering;
    public Toggle Toggles_ShowRealModels;
    public Toggle Toggles_FixFogArtifacts;


    // Extras View Components
    public Image Extras_SkyboxColorPreview;
    public TMP_InputField Extras_SkyboxColor_Red;
    public TMP_InputField Extras_SkyboxColor_Green;
    public TMP_InputField Extras_SkyboxColor_Blue;

    public TMP_InputField Extras_FogDensity;
    public Image Extras_FogColorPreview;
    public TMP_InputField Extras_FogColor_Red;
    public TMP_InputField Extras_FogColor_Green;
    public TMP_InputField Extras_FogColor_Blue;

    public TMP_InputField Extras_MainLightingIntensity;
    public Image Extras_MainLightingColorPreview;
    public TMP_InputField Extras_MainLightingColor_Red;
    public TMP_InputField Extras_MainLightingColor_Green;
    public TMP_InputField Extras_MainLightingColor_Blue;
    public TMP_InputField Extras_MainLightingRotation_X;
    public TMP_InputField Extras_MainLightingRotation_Y;
    public TMP_InputField Extras_MainLightingRotation_Z;

    public TMP_InputField Extras_CharacterLightingIntensity;
    public Image Extras_CharacterLightingColorPreview;
    public TMP_InputField Extras_CharacterLightingColor_Red;
    public TMP_InputField Extras_CharacterLightingColor_Green;
    public TMP_InputField Extras_CharacterLightingColor_Blue;

    public TMP_InputField Extras_ExtraLightingIntensity;
    public Image Extras_ExtraLightingColorPreview;
    public TMP_InputField Extras_ExtraLightingColor_Red;
    public TMP_InputField Extras_ExtraLightingColor_Green;
    public TMP_InputField Extras_ExtraLightingColor_Blue;
    #endregion


    private void Awake()
    {
      ToolCanvas = GetComponent<Canvas>();
      ToolCanvas.sortingOrder = 99;

      ToolCanvasGroup = GetComponent<CanvasGroup>();
      if (ToolCanvasGroup == null)
      {
        Melon<JKQScreenshotsTool>.Logger.BigError($"ToolCanvasComponents was set in an erroneous GameObject, or couldn't find the right components!");
        return;
      }

      // General
      ToolCanvas_Texts = GetComponentsInChildren<TMP_Text>(true).ToList();


      // Header
      const string headerPath = "Header/";

      Header_Title = transform.transform.FindObjectInRootPath<TMP_Text>(headerPath + "ToolTitle");
      Header_TakeScreenshot = transform.FindObjectInRootPath<Button>(headerPath + "ScreenshotShortcut/Button");
      Header_CloseToolMenu = transform.FindObjectInRootPath<Button>(headerPath + "CloseMenuShortcut/Button");

      Header_XCoord = transform.FindObjectInRootPath<TMP_Text>(headerPath + "XCoordinate");
      Header_YCoord = transform.FindObjectInRootPath<TMP_Text>(headerPath + "YCoordinate");
      Header_FOV = transform.FindObjectInRootPath<TMP_Text>(headerPath + "FOV");
      Header_DetailLevel = transform.FindObjectInRootPath<TMP_InputField>(headerPath + "DetailLevel/Input");


      // Tabs and Views
      const string tabsPath = "Content/Tabs/";
      const string viewsPath = "Content/Views/";

      Tab_Freecam = transform.FindObjectInRootPath<Transform>(tabsPath + "FreecamTab")?.gameObject.AddComponent<ToolMenuTabButton>();
      Tab_MapTool = transform.FindObjectInRootPath<Transform>(tabsPath + "MapToolTab")?.gameObject.AddComponent<ToolMenuTabButton>();
      Tab_Inventory = transform.FindObjectInRootPath<Transform>(tabsPath + "InventoryTab")?.gameObject.AddComponent<ToolMenuTabButton>();
      Tab_Toggles = transform.FindObjectInRootPath<Transform>(tabsPath + "TogglesTab")?.gameObject.AddComponent<ToolMenuTabButton>();
      Tab_Extras = transform.FindObjectInRootPath<Transform>(tabsPath + "ExtrasTab")?.gameObject.AddComponent<ToolMenuTabButton>();

      View_Freecam = transform.FindObjectInRootPath<Transform>(viewsPath + "FreecamView")?.gameObject;
      View_MapTool = transform.FindObjectInRootPath<Transform>(viewsPath + "MapToolView")?.gameObject;
      View_Inventory = transform.FindObjectInRootPath<Transform>(viewsPath + "InventoryView")?.gameObject;
      View_Toggles = transform.FindObjectInRootPath<Transform>(viewsPath + "TogglesView")?.gameObject;
      View_Extras = transform.FindObjectInRootPath<Transform>(viewsPath + "ExtrasView")?.gameObject;

      TabGroup = transform.FindObjectInRootPath<Transform>("Content/Tabs")?.gameObject.AddComponent<ToolMenuTabGroup>();
      TabGroup.Subscribe(Tab_Freecam, View_Freecam);
      TabGroup.Subscribe(Tab_MapTool, View_MapTool);
      TabGroup.Subscribe(Tab_Inventory, View_Inventory);
      TabGroup.Subscribe(Tab_Toggles, View_Toggles);
      TabGroup.Subscribe(Tab_Extras, View_Extras);
      TabGroup.OnTabSelected(Tab_Freecam);

      // Freecam View
      const string freecamViewPath = viewsPath + "FreecamView/";

      Freecam_EnableManualCamera = transform.FindObjectInRootPath<Toggle>(freecamViewPath + "EnableFreecam/Toggle");
      Freecam_AllowPlayerMovement = transform.FindObjectInRootPath<Toggle>(freecamViewPath + "AllowPlayerMovement/Toggle");
      Freecam_SetCameraAsAudioListener = transform.FindObjectInRootPath<Toggle>(freecamViewPath + "SetCameraAsAudioListener/Toggle");

      Freecam_CameraSpeed = transform.FindObjectInRootPath<TMP_InputField>(freecamViewPath + "CameraSpeed/Input");
      Freecam_CameraSpeedDamping = transform.FindObjectInRootPath<TMP_InputField>(freecamViewPath + "CameraSpeedDamping/Input");
      Freecam_CameraFOV = transform.FindObjectInRootPath<TMP_InputField>(freecamViewPath + "CameraFOV/Input");
      Freecam_CameraZoomSpeed = transform.FindObjectInRootPath<TMP_InputField>(freecamViewPath + "CameraZoomSpeed/Input");

      Freecam_OpenScreenshotsFolder = transform.FindObjectInRootPath<Button>(freecamViewPath + "OpenScreenshotsFolderButton");

      Freecam_StorageWarning = transform.FindObjectInRootPath<TMP_Text>(freecamViewPath + "TakeScreenshotButton/StorageWarningText");
      Freecam_EstimatedScreenshotSize = transform.FindObjectInRootPath<TMP_Text>(freecamViewPath + "TakeScreenshotButton/EstimatedSizeText");
      Freecam_TakeScreenshot = transform.FindObjectInRootPath<Button>(freecamViewPath + "TakeScreenshotButton/Button");


      // Map Tool View
      const string mapToolViewPath = viewsPath + "MapToolView/";

      MapTool_NegativeLimitX = transform.FindObjectInRootPath<TMP_InputField>(mapToolViewPath + "AxisControl/XAxis/RangeLabel/NegativeInput");
      MapTool_NegativeLimitY = transform.FindObjectInRootPath<TMP_InputField>(mapToolViewPath + "AxisControl/YAxis/RangeLabel/NegativeInput");

      MapTool_PositiveLimitX = transform.FindObjectInRootPath<TMP_InputField>(mapToolViewPath + "AxisControl/XAxis/RangeLabel/PositiveInput");
      MapTool_PositiveLimitY = transform.FindObjectInRootPath<TMP_InputField>(mapToolViewPath + "AxisControl/YAxis/RangeLabel/PositiveInput");

      MapTool_IncrementX = transform.FindObjectInRootPath<TMP_InputField>(mapToolViewPath + "AxisControl/XAxis/IncrementLabel/Input");
      MapTool_IncrementY = transform.FindObjectInRootPath<TMP_InputField>(mapToolViewPath + "AxisControl/YAxis/IncrementLabel/Input");

      MapTool_OpenScreenshotsFolder = transform.FindObjectInRootPath<Button>(mapToolViewPath + "OpenMapScreenshotsFolderButton");

      MapTool_StorageWarning = transform.FindObjectInRootPath<TMP_Text>(mapToolViewPath + "TakeMapScreenshotsButton/StorageWarningText");
      MapTool_EstimatedScreenshotSize = transform.FindObjectInRootPath<TMP_Text>(mapToolViewPath + "TakeMapScreenshotsButton/EstimatedSizeText");
      MapTool_TakeScreenshots = transform.FindObjectInRootPath<Button>(mapToolViewPath + "TakeMapScreenshotsButton/Button");


      // Inventory View
      const string inventoryViewPath = viewsPath + "InventoryView/";
      const string inventoryViewButtonGroupPath = inventoryViewPath + "ButtonGroup/";

      Inventory_FullBody = transform.FindObjectInRootPath<Button>(inventoryViewButtonGroupPath + "FullBodyButton");
      Inventory_Hat = transform.FindObjectInRootPath<Button>(inventoryViewButtonGroupPath + "HatButton");
      Inventory_Head = transform.FindObjectInRootPath<Button>(inventoryViewButtonGroupPath + "HeadButton");
      Inventory_Torso = transform.FindObjectInRootPath<Button>(inventoryViewButtonGroupPath + "TorsoButton");
      Inventory_Hands = transform.FindObjectInRootPath<Button>(inventoryViewButtonGroupPath + "HandsButton");
      Inventory_Legs = transform.FindObjectInRootPath<Button>(inventoryViewButtonGroupPath + "LegsButton");
      Inventory_Base = transform.FindObjectInRootPath<Button>(inventoryViewButtonGroupPath + "BaseButton");
      Inventory_Weapon = transform.FindObjectInRootPath<Button>(inventoryViewButtonGroupPath + "WeaponButton");
      Inventory_Shield = transform.FindObjectInRootPath<Button>(inventoryViewButtonGroupPath + "ShieldButton");
      Inventory_HideMannequin = transform.FindObjectInRootPath<Toggle>(inventoryViewPath + "HideMannequin/Toggle");


      // Toggles View
      const string togglesViewPath = viewsPath + "TogglesView/";
      const string togglesViewToggleGroupPath = togglesViewPath + "ToggleGroup/";

      Toggles_HideGUI = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "HideGUI/Toggle");
      Toggles_HidePlayers = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "HidePlayer/Toggle");
      Toggles_HideEnemies = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "HideEnemy/Toggle");
      Toggles_HideNPCS = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "HideNPC/Toggle");

      Toggles_DisableFog = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "DisableFog/Toggle");
      Toggles_DisableParticles = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "DisableParticles/Toggle");
      Toggles_DisableBloom = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "DisableBloom/Toggle");
      Toggles_DisableVignetting = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "DisableVignetting/Toggle");
      Toggles_DisableSkybox = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "DisableSkybox/Toggle");

      Toggles_PointFiltering = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "SetPointFilterMode/Toggle");
      Toggles_ShowRealModels = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "ShowRealModels/Toggle");
      Toggles_FixFogArtifacts = transform.FindObjectInRootPath<Toggle>(togglesViewToggleGroupPath + "FixFogArtifacts/Toggle");


      // Extras View
      const string extrasViewPath = viewsPath + "ExtrasView/";

      Extras_SkyboxColorPreview = transform.FindObjectInRootPath<Image>(extrasViewPath + "SkyboxGroup/Color/ColorPreview");
      Extras_SkyboxColor_Red = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "SkyboxGroup/Color/RedInput");
      Extras_SkyboxColor_Green = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "SkyboxGroup/Color/GreenInput");
      Extras_SkyboxColor_Blue = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "SkyboxGroup/Color/BlueInput");

      Extras_FogDensity = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "FogGroup/Density/Input");
      Extras_FogColorPreview = transform.FindObjectInRootPath<Image>(extrasViewPath + "FogGroup/Color/ColorPreview");
      Extras_FogColor_Red = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "FogGroup/Color/RedInput");
      Extras_FogColor_Green = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "FogGroup/Color/GreenInput");
      Extras_FogColor_Blue = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "FogGroup/Color/BlueInput");

      Extras_MainLightingIntensity = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "MainLightingGroup/Intensity/Input");
      Extras_MainLightingColorPreview = transform.FindObjectInRootPath<Image>(extrasViewPath + "MainLightingGroup/Color/ColorPreview");
      Extras_MainLightingColor_Red = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "MainLightingGroup/Color/RedInput");
      Extras_MainLightingColor_Green = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "MainLightingGroup/Color/GreenInput");
      Extras_MainLightingColor_Blue = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "MainLightingGroup/Color/BlueInput");
      Extras_MainLightingRotation_X = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "MainLightingGroup/Rotation/XInput");
      Extras_MainLightingRotation_Y = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "MainLightingGroup/Rotation/YInput");
      Extras_MainLightingRotation_Z = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "MainLightingGroup/Rotation/ZInput");

      Extras_CharacterLightingIntensity = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "CharacterLightingGroup/Intensity/Input");
      Extras_CharacterLightingColorPreview = transform.FindObjectInRootPath<Image>(extrasViewPath + "CharacterLightingGroup/Color/ColorPreview");
      Extras_CharacterLightingColor_Red = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "CharacterLightingGroup/Color/RedInput");
      Extras_CharacterLightingColor_Green = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "CharacterLightingGroup/Color/GreenInput");
      Extras_CharacterLightingColor_Blue = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "CharacterLightingGroup/Color/BlueInput");

      Extras_ExtraLightingIntensity = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "ExtraLightingGroup/Intensity/Input");
      Extras_ExtraLightingColorPreview = transform.FindObjectInRootPath<Image>(extrasViewPath + "ExtraLightingGroup/Color/ColorPreview");
      Extras_ExtraLightingColor_Red = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "ExtraLightingGroup/Color/RedInput");
      Extras_ExtraLightingColor_Green = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "ExtraLightingGroup/Color/GreenInput");
      Extras_ExtraLightingColor_Blue = transform.FindObjectInRootPath<TMP_InputField>(extrasViewPath + "ExtraLightingGroup/Color/BlueInput");
    }

    public ToolMenuTabButton GetToolCanvasTabButton(ToolMenuTabs tab)
    {
      switch (tab)
      {
        case ToolMenuTabs.Freecam: return Tab_Freecam;
        case ToolMenuTabs.MapTool: return Tab_MapTool;
        case ToolMenuTabs.Inventory: return Tab_Inventory;
        case ToolMenuTabs.Toggles: return Tab_Toggles;
        case ToolMenuTabs.Extras: return Tab_Extras;
        default: return null;
      };
    }
    public void SelectToolCanvasTabButton(ToolMenuTabButton tabButton)
    {
      TabGroup.OnTabSelected(tabButton);
    }
  }
}