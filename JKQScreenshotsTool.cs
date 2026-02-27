using System;
using System.IO;
using UnityEngine;
using MelonLoader;
using MelonLoader.Utils;
using JKQScreenshotsToolMod.UI;
using JKQScreenshotsToolMod.Helpers;

namespace JKQScreenshotsToolMod
{
  public class JKQScreenshotsTool : MelonMod
  {
    #region Fields
    // Melon Categories
    public MelonPreferences_Category ScreenshotToolCountCategory;
    public MelonPreferences_Entry<uint> ScreenshotDetailLevelEntry;
    public MelonPreferences_Entry<uint> SingularScreenshotsCountEntry;
    public MelonPreferences_Entry<uint> MapScreenshotsCountEntry;

    // Paths and Asset Bundles
    public readonly string ScreenshotsFolderPath = Path.Combine(Application.persistentDataPath, "JKQ Screenshots Tool");
    private readonly string ToolMenuAssetBundlePath = Path.Combine(MelonEnvironment.ModsDirectory, "JKQScreenshotsTool/jkq_screenshotstool");
    private const string ToolMenuPrefabName = "JKQScreenshotTool_Canvas";

    private GameObject _toolMenuPrefab = null;

    // Inputs
    private const KeyCode TakeScreenshotKey = KeyCode.F9;
    private const KeyCode ToggleMenuKey = KeyCode.F10;

    // Components
    private ToolMenuController _toolMenu = null;
    private ScreenshotsController _screenshotsController = null;

    public InputHelper InputHelper { get; private set; }
    public GameHelper GameHelper { get; private set; }
    public CameraHelper CameraHelper { get; private set; }
    public GUIHelper GUIHelper { get; private set; }
    public InventoryHelper InventoryHelper { get; private set; }
    public FontHelper FontHelper { get; private set; }


    // Tool States
    private bool _initializedToolMenuValues = false;
    private ScreenshotRangeValues _cachedScreenshotRangeValues;

    private bool _isToolMenuOpen = false;
    private bool _isInInventoryView = false;
    private bool _wasShowingRealModels = false;
    #endregion


    #region Melon Methods
    private void CreateModEntries()
    {
      // Create Category
      ScreenshotToolCountCategory = MelonPreferences.GetCategory("JKQScreenshotToolMod");
      if (ScreenshotToolCountCategory == null)
      {
        ScreenshotToolCountCategory = MelonPreferences.CreateCategory("JKQScreenshotToolMod");
      }

      // Create Entries
      // Screenshot Detail Level
      ScreenshotDetailLevelEntry = ScreenshotToolCountCategory.GetEntry<uint>("ScreenshotDetailLevel");
      if (ScreenshotDetailLevelEntry == null)
      {
        ScreenshotDetailLevelEntry = ScreenshotToolCountCategory.CreateEntry("ScreenshotDetailLevel", 1u);
      }

      // Singular Screenshots Count
      SingularScreenshotsCountEntry = ScreenshotToolCountCategory.GetEntry<uint>("SingularScreenshotsCount");
      if (SingularScreenshotsCountEntry == null)
      {
        SingularScreenshotsCountEntry = ScreenshotToolCountCategory.CreateEntry("SingularScreenshotsCount", 0u);
      }
      if (SingularScreenshotsCountEntry.Value > 255u)
      {
        SingularScreenshotsCountEntry.Value = 0u;
      }

      // Map Screenshots Count
      MapScreenshotsCountEntry = ScreenshotToolCountCategory.GetEntry<uint>("MapScreenshotsCount");
      if (MapScreenshotsCountEntry == null)
      {
        MapScreenshotsCountEntry = ScreenshotToolCountCategory.CreateEntry("MapScreenshotsCount", 0u);
      }
      if (MapScreenshotsCountEntry.Value > 255u)
      {
        MapScreenshotsCountEntry.Value = 0u;
      }
    }
    private void LoadToolMenuPrefab()
    {
      AssetBundle myBundle = AssetBundle.LoadFromFile(ToolMenuAssetBundlePath);
      if (myBundle == null) MelonLogger.Error("AssetBundle failed to load.");

      _toolMenuPrefab = myBundle.LoadAsset<GameObject>(ToolMenuPrefabName);
    }

    public override void OnLateInitializeMelon()
    {
      CreateModEntries();
      LoadToolMenuPrefab();

      // Helpers Instantiation
      InputHelper = new InputHelper();
      GameHelper = new GameHelper();
      CameraHelper = new CameraHelper();
      GUIHelper = new GUIHelper();
      InventoryHelper = new InventoryHelper();
      FontHelper = new FontHelper();
    }
    public override void OnDeinitializeMelon()
    {
      InputHelper = null;
      GameHelper = null;
      CameraHelper = null;
      GUIHelper = null;
      InventoryHelper = null;
      FontHelper = null;
    }

    public override void OnLateUpdate()
    {
      if (_toolMenu == null) return;

      // Input: Toggle Show Tool Menu
      if (Input.GetKeyDown(ToggleMenuKey))
      {
        SetToolMenuOpenState(!_isToolMenuOpen);

        // (One time set) Sets initial values for the entire tool
        SetDefaultToolMenuValues();
      }

      // Input: Singular screenshot
      if (Input.GetKeyDown(TakeScreenshotKey))
      {
        TryTakeFreecamScreenshot();
      }

      // Updates Coordinate texts if the Tool Menu is open
      UpdateToolMenuCoordinatesAndFOV();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
      // Ignore this scene
      if (sceneName == GameHelper.CoreScene) return;

      // Destroy Menu if the player is not playing
      if (sceneName == GameHelper.SplashScreenScene || sceneName == GameHelper.MainMenuScene)
      {
        DestroyToolMenu();
        return;
      }

      // Loaded Scene Log
      JKQScreenshotsToolLogger.Msg($"Loaded Game Scene: {sceneName}");

      // Instantiate Tool Menu if the Prefab exists and the Menu hasnt been instantiated yet 
      if (_toolMenuPrefab != null && _toolMenu == null)
      {
        InstantiateToolMenu();
        return;
      }
    }
    #endregion


    #region Tool
    #region Initialization Methods
    private void InstantiateToolMenu()
    {
      if (_toolMenu != null) return;

      // Initialize Helpers
      InputHelper.Init();
      GameHelper.Init();
      CameraHelper.Init();
      GUIHelper.Init();
      InventoryHelper.Init();
      FontHelper.Init();

      // Instantiate Menu
      _toolMenu = GameObject.Instantiate(_toolMenuPrefab).AddComponent<ToolMenuController>();
      _toolMenu.gameObject.name = ToolMenuPrefabName;
      _toolMenu.gameObject.hideFlags = (UnityEngine.HideFlags)61;
      _toolMenu.SetScreenshotToolFonts(FontHelper.GetBradleyDJRFont(), FontHelper.GetBerninaSansFont());
      GameObject.DontDestroyOnLoad(_toolMenu.gameObject);

      _screenshotsController = _toolMenu.gameObject.AddComponent<ScreenshotsController>();

      // Suscribe to Events
      SubscribeGeneralEvents();
      SubscribeHeaderEvents();
      SubscribeFreecamEvents();
      SubscribeMapToolEvents();
      SubscribeInventoryEvents();
      SubscribeTogglesEvents();
      SubscribeExtrasEvents();
    }

    private void SetDefaultToolMenuValues()
    {
      if (_initializedToolMenuValues) return;

      // Header values
      _toolMenu.HeaderView.SetDetailLevelValue(ScreenshotDetailLevelEntry.Value);

      // Freecam
      _toolMenu.FreecamView.SetSpeedValue(CameraHelper.ManualCameraMoveSpeed);
      _toolMenu.FreecamView.SetSpeedDampingValue(CameraHelper.ManualCameraSpeedDamping);
      _toolMenu.FreecamView.SetFOVValue(CameraHelper.CameraFOV);
      _toolMenu.FreecamView.SetZoomSpeedValue(CameraHelper.ManualCameraZoomSpeed);

      TryEnableTakeScreenshotFreecamButton();

      // Map Tool
      Vector2 camPos = CameraHelper.CameraPosition;
      camPos.Set((float)(Math.Truncate(camPos.x * 100) / 100), (float)(Math.Truncate(camPos.y * 100) / 100));
      _toolMenu.MapToolView.InitializeValues(camPos);

      _cachedScreenshotRangeValues = new ScreenshotRangeValues(new Vector2(camPos.x, camPos.x), new Vector2(camPos.y, camPos.y), 0f, 0f);
      _cachedScreenshotRangeValues.TryCalculateScreenshotPositions();

      TryEnableMapToolTakeScreenshotsButton();

      // Extras
      _toolMenu.ExtrasView.SetSkyboxColorValue(CameraHelper.CameraSkyboxColor);
      _toolMenu.ExtrasView.SetFogColorValue(CameraHelper.FogColor);
      _toolMenu.ExtrasView.SetFogDensityValue(CameraHelper.FogDensity);
      _toolMenu.ExtrasView.SetMainLightingColorValue(GameHelper.MainLightingColor);
      _toolMenu.ExtrasView.SetMainLightingIntensityValue(GameHelper.MainLightingIntensity);
      _toolMenu.ExtrasView.SetMainLightingRotationValue(GameHelper.MainLightingRotation.eulerAngles);
      _toolMenu.ExtrasView.SetCharacterLightingColorValue(GameHelper.CharacterLightingColor);
      _toolMenu.ExtrasView.SetCharacterLightingIntensityValue(GameHelper.CharacterLightingIntensity);
      _toolMenu.ExtrasView.SetExtraLightingColorValue(GameHelper.ExtraLightingColor);
      _toolMenu.ExtrasView.SetExtraLightingIntensityValue(GameHelper.ExtraLightingIntensity);
    }

    private void DestroyToolMenu()
    {
      if (_toolMenu == null) return;

      // Clear Helpers
      InputHelper.Clear();
      GameHelper.Clear();
      CameraHelper.Clear();
      GUIHelper.Clear();
      InventoryHelper.Clear();
      FontHelper.Clear();

      // Clear caches
      _isToolMenuOpen = false;
      _initializedToolMenuValues = false;
      _isInInventoryView = false;
      _wasShowingRealModels = false;

      // Clear Events
      UnsubscribeGeneralEvents();
      UnsubscribeHeaderEvents();
      UnsubscribeFreecamEvents();
      UnsubscribeMapToolEvents();
      UnsubscribeInventoryEvents();
      UnsubscribeTogglesEvents();
      UnsubscribeExtrasEvents();

      // Destroy Tool Menu
      GameObject.Destroy(_toolMenu.gameObject);
    }
    #endregion

    #region General Usage Methods
    public bool IsToolMenuOpen()
    {
      return _isToolMenuOpen;
    }
    public void SetToolMenuOpenState(bool open)
    {
      if (_isToolMenuOpen == open) return;

      _isToolMenuOpen = open;

      _toolMenu.SetScreenshotToolMenuOpen(open);
      InputHelper.SetCursorVisibility(open);

      // Stop taking Map Screenshots proccess if player opens the Tool Menu
      if (_screenshotsController.IsTakingMapScreenshots() && open)
      {
        _screenshotsController.StopTakeMapScreenshots();
      }
    }

    private void OpenScreenshotsFolder()
    {
      _screenshotsController.OpenScreenshotsFolder();
    }

    private void SubscribeGeneralEvents() { }
    private void UnsubscribeGeneralEvents() { }
    #endregion

    #region Header
    private void UpdateToolMenuCoordinatesAndFOV()
    {
      if (!_toolMenu.IsScreenshotToolMenuOpen()) return;

      _toolMenu.HeaderView.UpdateCurrentCameraCoordsAndFOV(CameraHelper.CameraPosition, CameraHelper.CameraFOV);
    }

    private void SetDetailLevel(uint detailLevel)
    {
      if (detailLevel == 0) detailLevel = 1u;
      if (detailLevel == 9) detailLevel = 9u;

      // Avoid gigantic screenshots. Why would anyone need a +10K resolution screenshot?
      int screenWidthWithDetailLevel = Screen.width * (int)detailLevel;
      int resolutionWidth10K = 10240;
      if (screenWidthWithDetailLevel > resolutionWidth10K) detailLevel = (uint)Mathf.FloorToInt(resolutionWidth10K / Screen.width);

      // Set Detail Level
      ScreenshotDetailLevelEntry.Value = detailLevel;
      _toolMenu.HeaderView.SetDetailLevelValue(detailLevel);

      // Enable Buttons if there's enough free storage
      TryEnableTakeScreenshotFreecamButton();
      TryEnableMapToolTakeScreenshotsButton();
    }

    private void TakeScreenshotHeaderButtonPressed()
    {
      TryTakeFreecamScreenshot();
    }
    private void CloseMenuHeaderButtonPressed()
    {
      SetToolMenuOpenState(false);
    }


    private void SubscribeHeaderEvents()
    {
      _toolMenu.HeaderView.OnDetailLevelValueChanged += SetDetailLevel;
      _toolMenu.HeaderView.OnTakeScreenshotButtonPressed += TakeScreenshotHeaderButtonPressed;
      _toolMenu.HeaderView.OnCloseToolMenuButtonPressed += CloseMenuHeaderButtonPressed;
    }
    private void UnsubscribeHeaderEvents()
    {
      _toolMenu.HeaderView.OnDetailLevelValueChanged -= SetDetailLevel;
      _toolMenu.HeaderView.OnTakeScreenshotButtonPressed -= TakeScreenshotHeaderButtonPressed;
      _toolMenu.HeaderView.OnCloseToolMenuButtonPressed -= CloseMenuHeaderButtonPressed;
    }
    #endregion

    #region Freecam
    public void EnableFreecam(bool enable)
    {
      CameraHelper.IsManualCameraControllerEnabled = enable;
      _toolMenu.FreecamView.SetFreecamToggleState(enable);

      if (!enable)
      {
        EnableFreecamPlayerInput(false);
        EnableFreecamAsAudioListener(false);

        if (_isInInventoryView)
        {
          ExitInventoryView();
        }
      }
    }
    private void EnableFreecamInputToggle(bool isOn)
    {
      _toolMenu.FreecamView.SetFreecamToggleState(isOn);

      if (!isOn)
      {
        EnableFreecamPlayerInput(false);
        EnableFreecamAsAudioListener(false);

        if (_isInInventoryView)
        {
          ExitInventoryView();
        }
      }
    }

    public void EnableFreecamPlayerInput(bool isOn)
    {
      if (!CameraHelper.IsManualCameraControllerEnabled)
      {
        _toolMenu.FreecamView.SetFreecamPlayerInputToggleState(false);
        return;
      }

      InputHelper.EnablePlayerInput(isOn);
      InputHelper.EnableManualCameraInput(!isOn);
      _toolMenu.FreecamView.SetFreecamPlayerInputToggleState(isOn);
    }
    private void EnableFreecamAsAudioListener(bool isOn)
    {
      CameraHelper.IsCameraAudioListener = isOn;
      _toolMenu.FreecamView.SetFreecamAudioListenerToggleState(isOn);
    }

    private void SetFreecamSpeed(float speed)
    {
      if (speed < 0f) speed = 0.1f;

      CameraHelper.ManualCameraMoveSpeed = speed;
      _toolMenu.FreecamView.SetSpeedValue(speed);
    }
    private void SetFreecamSpeedDamping(float damping)
    {
      if (damping < 0f) damping = 0f;

      CameraHelper.ManualCameraSpeedDamping = damping;
      _toolMenu.FreecamView.SetSpeedDampingValue(damping);
    }
    private void SetFreecamFOV(float fov)
    {
      if (fov < 1f) fov = 1f;
      if (fov > 180f) fov = 179.99f;

      CameraHelper.CameraFOV = fov;
      _toolMenu.FreecamView.SetFOVValue(fov);
    }
    private void SetFreecamZoomSpeed(float zoomSpeed)
    {
      if (zoomSpeed < 0f) zoomSpeed = 0.1f;

      CameraHelper.ManualCameraZoomSpeed = zoomSpeed;
      _toolMenu.FreecamView.SetZoomSpeedValue(zoomSpeed);
    }

    private bool CanTakeFreecamScrenshot()
    {
      if (_screenshotsController.IsTakingMapScreenshots()) return false;

      long estimatedSize = _screenshotsController.CalculateScreenshotsSizeInBytes(screenshotCount: 1u);
      _toolMenu.FreecamView.SetEstimatedScreenshotSize(estimatedSize, ScreenshotDetailLevelEntry.Value);

      if (!_screenshotsController.HasEnoughDiskSpace(estimatedSize))
      {
        _toolMenu.FreecamView.EnabeTakeScreenshotButton(false);
        _toolMenu.FreecamView.ShowStorageWarning(true);
        return false;
      }

      return true;
    }
    private bool TryEnableTakeScreenshotFreecamButton()
    {
      if (!CanTakeFreecamScrenshot()) return false;

      _toolMenu.FreecamView.ShowStorageWarning(false);
      _toolMenu.FreecamView.EnabeTakeScreenshotButton(true);
      return true;
    }
    private void TryTakeFreecamScreenshot()
    {
      if (!CanTakeFreecamScrenshot())
      {
        SetToolMenuOpenState(true);
        _toolMenu.Tabs_SetView(ToolMenuTabs.Freecam);
        return;
      }

      _screenshotsController.TakeSingularScreenshot();
    }

    private void SubscribeFreecamEvents()
    {
      _toolMenu.FreecamView.OnFreecamStateChanged += EnableFreecam;
      CameraHelper.OnManualCameraControllerPatchInputToggled += EnableFreecamInputToggle;
      _toolMenu.FreecamView.OnFreecamPlayerInputStateChanged += EnableFreecamPlayerInput;
      _toolMenu.FreecamView.OnFreecamAudioListenerStateChanged += EnableFreecamAsAudioListener;

      _toolMenu.FreecamView.OnSpeedChanged += SetFreecamSpeed;
      _toolMenu.FreecamView.OnSpeedDampingChanged += SetFreecamSpeedDamping;
      _toolMenu.FreecamView.OnFOVChanged += SetFreecamFOV;
      _toolMenu.FreecamView.OnZoomSpeedChanged += SetFreecamZoomSpeed;

      _toolMenu.FreecamView.OnOpenScreenshotFolderButtonPressed += OpenScreenshotsFolder;
      _toolMenu.FreecamView.OnTakeScreenshotButtonPressed += TryTakeFreecamScreenshot;
    }
    private void UnsubscribeFreecamEvents()
    {
      _toolMenu.FreecamView.OnFreecamStateChanged -= EnableFreecam;
      CameraHelper.OnManualCameraControllerPatchInputToggled -= EnableFreecamInputToggle;
      _toolMenu.FreecamView.OnFreecamPlayerInputStateChanged -= EnableFreecamPlayerInput;
      _toolMenu.FreecamView.OnFreecamAudioListenerStateChanged -= EnableFreecamAsAudioListener;

      _toolMenu.FreecamView.OnSpeedChanged -= SetFreecamSpeed;
      _toolMenu.FreecamView.OnSpeedDampingChanged -= SetFreecamSpeedDamping;
      _toolMenu.FreecamView.OnFOVChanged -= SetFreecamFOV;
      _toolMenu.FreecamView.OnZoomSpeedChanged += SetFreecamZoomSpeed;

      _toolMenu.FreecamView.OnOpenScreenshotFolderButtonPressed -= OpenScreenshotsFolder;
      _toolMenu.FreecamView.OnTakeScreenshotButtonPressed -= TryTakeFreecamScreenshot;
    }
    #endregion

    #region Map Tool
    private void SetMapToolNegativeLimitX(float negativeLimitX)
    {
      Vector2 xRange = _cachedScreenshotRangeValues.GetXRange();
      float currentNegativeLimitX = xRange.x;
      float currentPositiveLimitX = xRange.y;

      currentNegativeLimitX = Mathf.Min(negativeLimitX, currentPositiveLimitX);
      _cachedScreenshotRangeValues.SetXRange(new Vector2(currentNegativeLimitX, currentPositiveLimitX));

      MapToolValuesUpdated();
    }
    private void SetMapToolNegativeLimitY(float negativeLimitY)
    {
      Vector2 yRange = _cachedScreenshotRangeValues.GetYRange();
      float currentNegativeLimitY = yRange.x;
      float currentPositiveLimitY = yRange.y;

      currentNegativeLimitY = Mathf.Min(negativeLimitY, currentPositiveLimitY);
      _cachedScreenshotRangeValues.SetYRange(new Vector2(currentNegativeLimitY, currentPositiveLimitY));

      MapToolValuesUpdated();
    }
    private void SetMapToolPositiveLimitX(float positiveLimitX)
    {
      Vector2 xRange = _cachedScreenshotRangeValues.GetXRange();
      float currentNegativeLimitX = xRange.x;
      float currentPositiveLimitX = xRange.y;

      currentPositiveLimitX = Mathf.Max(currentNegativeLimitX, positiveLimitX);
      _cachedScreenshotRangeValues.SetXRange(new Vector2(currentNegativeLimitX, currentPositiveLimitX));

      MapToolValuesUpdated();
    }
    private void SetMapToolPositiveLimitY(float positiveLimitY)
    {
      Vector2 yRange = _cachedScreenshotRangeValues.GetYRange();
      float currentNegativeLimitY = yRange.x;
      float currentPositiveLimitY = yRange.y;

      currentPositiveLimitY = Mathf.Max(currentNegativeLimitY, positiveLimitY);
      _cachedScreenshotRangeValues.SetYRange(new Vector2(currentNegativeLimitY, currentPositiveLimitY));

      MapToolValuesUpdated();
    }
    private void SetMapToolIncrementX(float incrementX)
    {
      float currentIncrementX = Mathf.Max(0f, incrementX);
      _cachedScreenshotRangeValues.SetXIncrement(currentIncrementX);

      MapToolValuesUpdated();
    }
    private void SetMapToolIncrementY(float incrementY)
    {
      float currentIncrementY = Mathf.Max(0f, incrementY);
      _cachedScreenshotRangeValues.SetYIncrement(currentIncrementY);

      MapToolValuesUpdated();
    }

    private void MapToolValuesUpdated()
    {
      NormalizeMapToolValues();
      TryEnableMapToolTakeScreenshotsButton();
    }
    private void NormalizeMapToolValues()
    {
      // Caches
      Vector2 xRange = _cachedScreenshotRangeValues.GetXRange();
      float currentNegativeLimitX = xRange.x;
      float currentPositiveLimitX = xRange.y;

      Vector2 yRange = _cachedScreenshotRangeValues.GetYRange();
      float currentNegativeLimitY = yRange.x;
      float currentPositiveLimitY = yRange.y;

      float currentIncrementX = _cachedScreenshotRangeValues.GetXIncrement();
      float currentIncrementY = _cachedScreenshotRangeValues.GetYIncrement();


      // Negative Limits
      currentNegativeLimitX = Mathf.Min(currentNegativeLimitX, currentPositiveLimitX);
      currentNegativeLimitY = Mathf.Min(currentNegativeLimitY, currentPositiveLimitY);

      _toolMenu.MapToolView.SetNegativeLimitXValue(currentNegativeLimitX);
      _toolMenu.MapToolView.SetNegativeLimitYValue(currentNegativeLimitY);

      // Positive Limits
      currentPositiveLimitX = Mathf.Max(currentNegativeLimitX, currentPositiveLimitX);
      currentPositiveLimitY = Mathf.Max(currentNegativeLimitY, currentPositiveLimitY);

      _toolMenu.MapToolView.SetPositiveLimitXValue(currentPositiveLimitX);
      _toolMenu.MapToolView.SetPositiveLimitYValue(currentPositiveLimitY);

      // Increments
      currentIncrementX = Mathf.Clamp(currentIncrementX, 0f, Mathf.Abs(currentPositiveLimitX - currentNegativeLimitX));
      currentIncrementY = Mathf.Clamp(currentIncrementY, 0f, Mathf.Abs(currentPositiveLimitY - currentNegativeLimitY));

      _toolMenu.MapToolView.SetIncrementXValue(currentIncrementX);
      _toolMenu.MapToolView.SetIncrementYValue(currentIncrementY);


      // Set Normalized Values
      _cachedScreenshotRangeValues.SetXRange(new Vector2(currentNegativeLimitX, currentPositiveLimitX));
      _cachedScreenshotRangeValues.SetYRange(new Vector2(currentNegativeLimitY, currentPositiveLimitY));
      _cachedScreenshotRangeValues.SetXIncrement(currentIncrementX);
      _cachedScreenshotRangeValues.SetYIncrement(currentIncrementY);

      _cachedScreenshotRangeValues.TryCalculateScreenshotPositions();
    }

    private bool CanTakeMapScreenshots()
    {
      if (_screenshotsController.IsTakingMapScreenshots()) return false;

      if (!_cachedScreenshotRangeValues.CanTakeScreenshots())
      {
        _cachedScreenshotRangeValues = new ScreenshotRangeValues();
        _toolMenu.MapToolView.EnableTakeScreenshotsButton(false);
        _toolMenu.MapToolView.ShowEstimatedScreenshotSizeText(false);
        _toolMenu.MapToolView.ShowStorageWarning(false);
        return false;
      }

      long estimatedSize = _screenshotsController.CalculateScreenshotsSizeInBytes(screenshotCount: _cachedScreenshotRangeValues.amountOfScreenshots);
      _toolMenu.MapToolView.ShowEstimatedScreenshotSizeText(true);
      _toolMenu.MapToolView.SetEstimatedScreenshotSize(estimatedSize, ScreenshotDetailLevelEntry.Value, _cachedScreenshotRangeValues.amountOfScreenshotsPerAxis);

      if (!_screenshotsController.HasEnoughDiskSpace(estimatedSize))
      {
        _toolMenu.MapToolView.EnableTakeScreenshotsButton(false);
        _toolMenu.MapToolView.ShowStorageWarning(true);
        return false;
      }

      return true;
    }
    private void TryEnableMapToolTakeScreenshotsButton()
    {
      if (!CanTakeMapScreenshots()) return;

      _toolMenu.MapToolView.ShowStorageWarning(false);
      _toolMenu.MapToolView.EnableTakeScreenshotsButton(true);
    }
    private void TryTakeMapScreenshots()
    {
      if (!CanTakeMapScreenshots()) return;

      _screenshotsController.StartTakeMapScreenshots(_cachedScreenshotRangeValues);
    }

    private void SubscribeMapToolEvents()
    {
      _toolMenu.MapToolView.OnNegativeLimitXValueChanged += SetMapToolNegativeLimitX;
      _toolMenu.MapToolView.OnNegativeLimitYValueChanged += SetMapToolNegativeLimitY;
      _toolMenu.MapToolView.OnPositiveLimitXValueChanged += SetMapToolPositiveLimitX;
      _toolMenu.MapToolView.OnPositiveLimitYValueChanged += SetMapToolPositiveLimitY;
      _toolMenu.MapToolView.OnIncrementXValueChanged += SetMapToolIncrementX;
      _toolMenu.MapToolView.OnIncrementYValueChanged += SetMapToolIncrementY;

      _toolMenu.MapToolView.OnOpenScreenshotFolderButtonPressed += OpenScreenshotsFolder;
      _toolMenu.MapToolView.OnTakeScreenshotsButtonPressed += TryTakeMapScreenshots;
    }
    private void UnsubscribeMapToolEvents()
    {
      _toolMenu.MapToolView.OnNegativeLimitXValueChanged -= SetMapToolNegativeLimitX;
      _toolMenu.MapToolView.OnNegativeLimitYValueChanged -= SetMapToolNegativeLimitY;
      _toolMenu.MapToolView.OnPositiveLimitXValueChanged -= SetMapToolPositiveLimitX;
      _toolMenu.MapToolView.OnPositiveLimitYValueChanged -= SetMapToolPositiveLimitY;
      _toolMenu.MapToolView.OnIncrementXValueChanged -= SetMapToolIncrementX;
      _toolMenu.MapToolView.OnIncrementYValueChanged -= SetMapToolIncrementY;

      _toolMenu.MapToolView.OnOpenScreenshotFolderButtonPressed -= OpenScreenshotsFolder;
      _toolMenu.MapToolView.OnTakeScreenshotsButtonPressed -= TryTakeMapScreenshots;
    }
    #endregion

    #region Inventory
    private void EnterInventoryView(InventoryCameraParams inventoryCameraParams)
    {
      GUIHelper.OpenInventoryMenu();

      bool enteredAgain = _isInInventoryView;
      _isInInventoryView = true;
   
      EnableFreecam(true);
      EnableFreecamPlayerInput(false);

      CameraHelper.CameraPosition = inventoryCameraParams.CameraPosition;
      CameraHelper.CameraRotation = inventoryCameraParams.CameraRotation;
      CameraHelper.CameraFOV = inventoryCameraParams.CameraFOV;
      CameraHelper.ResetFreecamVelocity();

      if (!enteredAgain)
      {
        _wasShowingRealModels = CameraHelper.IsShowingRealModels;
      }
      ShowRealModels(true);
    }
    private void ExitInventoryView()
    {
      if (!_isInInventoryView) return;

      _isInInventoryView = false;

      CameraHelper.ResetCameraZPosition();
      CameraHelper.ResetCameraRotation();
      CameraHelper.ResetCameraFOV();
      CameraHelper.ResetFreecamVelocity();

      ShowRealModels(_wasShowingRealModels);

      EnableFreecam(false);
    }

    private void FullBodyButtonPressed()
    {
      EnterInventoryView(InventoryHelper.ViewFullBodyPreview());
    }
    private void HatButtonPressed()
    {
      EnterInventoryView(InventoryHelper.ViewHatPreview());
    }
    private void HeadButtonPressed()
    {
      EnterInventoryView(InventoryHelper.ViewHeadPreview());
    }
    private void TorsoButtonPressed()
    {
      EnterInventoryView(InventoryHelper.ViewTorsoPreview());
    }
    private void HandsButtonPressed()
    {
      EnterInventoryView(InventoryHelper.ViewHandsPreview());
    }
    private void LegsButtonPressed()
    {
      EnterInventoryView(InventoryHelper.ViewLegsPreview());
    }
    private void BaseButtonPressed()
    {
      EnterInventoryView(InventoryHelper.ViewBasePreview());
    }
    private void WeaponButtonPressed()
    {
      EnterInventoryView(InventoryHelper.ViewWeaponPreview());
    }
    private void ShieldButtonPressed()
    {
      EnterInventoryView(InventoryHelper.ViewShieldPreview());
    }

    private void HideMannequin(bool isOn)
    {
      InventoryHelper.HideInventoryMannequin(isOn);
    }

    private void SubscribeInventoryEvents()
    {
      // Revise this later to check if its possible to preserve Inventory Previews
      GUIHelper.OnMenuGUIMenuChanged += ExitInventoryView;

      _toolMenu.InventoryView.OnFullBodyButtonPressed += FullBodyButtonPressed;
      _toolMenu.InventoryView.OnHatButtonPressed += HatButtonPressed;
      _toolMenu.InventoryView.OnHeadButtonPressed += HeadButtonPressed;
      _toolMenu.InventoryView.OnTorsoButtonPressed += TorsoButtonPressed;
      _toolMenu.InventoryView.OnHandsButtonPressed += HandsButtonPressed;
      _toolMenu.InventoryView.OnLegsButtonPressed += LegsButtonPressed;
      _toolMenu.InventoryView.OnBaseButtonPressed += BaseButtonPressed;
      _toolMenu.InventoryView.OnWeaponButtonPressed += WeaponButtonPressed;
      _toolMenu.InventoryView.OnShieldButtonPressed += ShieldButtonPressed;

      _toolMenu.InventoryView.OnHideMannequinStateChanged += HideMannequin;
    }
    private void UnsubscribeInventoryEvents()
    {
      GUIHelper.OnMenuGUIMenuChanged -= ExitInventoryView;

      _toolMenu.InventoryView.OnFullBodyButtonPressed -= FullBodyButtonPressed;
      _toolMenu.InventoryView.OnHatButtonPressed -= HatButtonPressed;
      _toolMenu.InventoryView.OnHeadButtonPressed -= HeadButtonPressed;
      _toolMenu.InventoryView.OnTorsoButtonPressed -= TorsoButtonPressed;
      _toolMenu.InventoryView.OnHandsButtonPressed -= HandsButtonPressed;
      _toolMenu.InventoryView.OnLegsButtonPressed -= LegsButtonPressed;
      _toolMenu.InventoryView.OnBaseButtonPressed -= BaseButtonPressed;
      _toolMenu.InventoryView.OnWeaponButtonPressed -= WeaponButtonPressed;
      _toolMenu.InventoryView.OnShieldButtonPressed -= ShieldButtonPressed;

      _toolMenu.InventoryView.OnHideMannequinStateChanged -= HideMannequin;
    }
    #endregion

    #region Toggles
    private void HideGUI(bool hide)
    {
      GUIHelper.SetGameGUIInvisibility(hide);
      _toolMenu.TogglesView.SetHideGUIToggleState(hide);
    }
    private void HideGUIInputToggle(bool hide)
    {
      _toolMenu.TogglesView.SetHideGUIToggleState(hide);
    }
    private void HidePlayers(bool hide)
    {
      GameHelper.HidePlayers(hide);
      _toolMenu.TogglesView.SetHidePlayersToggleState(hide);
    }
    private void HideEnemies(bool hide)
    {
      GameHelper.HideEnemies(hide);
      _toolMenu.TogglesView.SetHideEnemiesToggleState(hide);
    }
    private void HideNPCS(bool hide)
    {
      GameHelper.HideNPCs(hide);
      _toolMenu.TogglesView.SetHideNPCSToggleState(hide);
    }

    private void DisableFog(bool disable)
    {
      CameraHelper.IsFogDisabled = disable;
      _toolMenu.TogglesView.SetDisableFogToggleState(disable);
    }
    private void DisableParticles(bool disable)
    {
      GameHelper.AreParticlesDisabled = disable;
      _toolMenu.TogglesView.SetDisableParticlesToggleState(disable);
    }
    private void DisableBloom(bool disable)
    {
      CameraHelper.IsBloomDisabled = disable;
      _toolMenu.TogglesView.SetDisableBloomToggleState(disable);
    }
    private void DisableVignetting(bool disable)
    {
      CameraHelper.IsVignettingDisabled = disable;
      _toolMenu.TogglesView.SetDisableVignettingToggleState(disable);
    }
    private void DisableSkybox(bool disable)
    {
      CameraHelper.IsSkyboxDisabled = disable;
      _toolMenu.TogglesView.SetDisableSkyboxToggleState(disable);
    }

    private void EnablePointFiltering(bool enable)
    {
      CameraHelper.IsPointFilterModeOn = enable;
      _toolMenu.TogglesView.SetEnablePointFilteringToggleState(enable);
    }
    private void ShowRealModels(bool show)
    {
      CameraHelper.IsShowingRealModels = show;
      _toolMenu.TogglesView.SetFixFogArtifactsToggleState(show);
    }
    private void FixFogArtifacts(bool fix)
    {
      CameraHelper.AreFogArtifactsFixed = fix;
      _toolMenu.TogglesView.SetFixFogArtifactsToggleState(fix);
    }


    private void SubscribeTogglesEvents()
    {
      _toolMenu.TogglesView.OnHideGUIStateChanged += HideGUI;
      GUIHelper.OnUIInputPatchInputToggled += HideGUIInputToggle;
      _toolMenu.TogglesView.OnHidePlayersStateChanged += HidePlayers;
      _toolMenu.TogglesView.OnHideEnemiesStateChanged += HideEnemies;
      _toolMenu.TogglesView.OnHideNPCSStateChanged += HideNPCS;

      _toolMenu.TogglesView.OnDisableFogStateChanged += DisableFog;
      _toolMenu.TogglesView.OnDisableParticlesStateChanged += DisableParticles;
      _toolMenu.TogglesView.OnDisableBloomStateChanged += DisableBloom;
      _toolMenu.TogglesView.OnDisableVignettingStateChanged += DisableVignetting;
      _toolMenu.TogglesView.OnDisableSkyboxStateChanged += DisableSkybox;

      _toolMenu.TogglesView.OnEnablePointFilteringStateChanged += EnablePointFiltering;
      _toolMenu.TogglesView.OnShowRealModelsStateChanged += ShowRealModels;
      _toolMenu.TogglesView.OnFixFogArtifactsStateChanged += FixFogArtifacts;
    }
    private void UnsubscribeTogglesEvents()
    {
      _toolMenu.TogglesView.OnHideGUIStateChanged -= HideGUI;
      GUIHelper.OnUIInputPatchInputToggled -= HideGUIInputToggle;
      _toolMenu.TogglesView.OnHidePlayersStateChanged -= HidePlayers;
      _toolMenu.TogglesView.OnHideEnemiesStateChanged -= HideEnemies;
      _toolMenu.TogglesView.OnHideNPCSStateChanged -= HideNPCS;

      _toolMenu.TogglesView.OnDisableFogStateChanged -= DisableFog;
      _toolMenu.TogglesView.OnDisableParticlesStateChanged -= DisableParticles;
      _toolMenu.TogglesView.OnDisableBloomStateChanged -= DisableBloom;
      _toolMenu.TogglesView.OnDisableVignettingStateChanged -= DisableVignetting;
      _toolMenu.TogglesView.OnDisableSkyboxStateChanged -= DisableSkybox;

      _toolMenu.TogglesView.OnEnablePointFilteringStateChanged -= EnablePointFiltering;
      _toolMenu.TogglesView.OnShowRealModelsStateChanged -= ShowRealModels;
      _toolMenu.TogglesView.OnFixFogArtifactsStateChanged -= FixFogArtifacts;
    }
    #endregion

    #region Extras
    private void SetSkyboxColorRed(float colorRed)
    {
      colorRed = Mathf.Clamp01(colorRed);
      Color newColor = CameraHelper.CameraSkyboxColor;
      
      newColor.r = colorRed;
      newColor.a = 1f;

      SetSkyboxColor(newColor);
    }
    private void SetSkyboxColorGreen(float colorGreen)
    {
      colorGreen = Mathf.Clamp01(colorGreen);
      Color newColor = CameraHelper.CameraSkyboxColor;

      newColor.g = colorGreen;
      newColor.a = 1f;

      SetSkyboxColor(newColor);
    }
    private void SetSkyboxColorBlue(float colorBlue)
    {
      colorBlue = Mathf.Clamp01(colorBlue);
      Color newColor = CameraHelper.CameraSkyboxColor;

      newColor.b = colorBlue;
      newColor.a = 1f;

      SetSkyboxColor(newColor);
    }
    private void SetSkyboxColor(Color newColor)
    {
      CameraHelper.CameraSkyboxColor = newColor;
      _toolMenu.ExtrasView.SetSkyboxColorValue(newColor);
    }

    private void SetFogColorRed(float colorRed)
    {
      colorRed = Mathf.Clamp01(colorRed);
      Color newColor = CameraHelper.FogColor;

      newColor.r = colorRed;
      newColor.a = 1f;

      SetFogColor(newColor);
    }
    private void SetFogColorGreen(float colorGreen)
    {
      colorGreen = Mathf.Clamp01(colorGreen);
      Color newColor = CameraHelper.FogColor;

      newColor.g = colorGreen;
      newColor.a = 1f;

      SetFogColor(newColor);
    }
    private void SetFogColorBlue(float colorBlue)
    {
      colorBlue = Mathf.Clamp01(colorBlue);
      Color newColor = CameraHelper.FogColor;

      newColor.b = colorBlue;
      newColor.a = 1f;

      SetFogColor(newColor);
    }
    private void SetFogColor(Color newColor)
    {
      CameraHelper.FogColor = newColor;
      _toolMenu.ExtrasView.SetFogColorValue(newColor);
    }

    private void SetFogDensity(float density)
    {
      density = Mathf.Max(0, density);
      CameraHelper.FogDensity = density;
      _toolMenu.ExtrasView.SetFogDensityValue(density);
    }


    private void SetMainLightingColorRed(float colorRed)
    {
      colorRed = Mathf.Clamp01(colorRed);
      Color newColor = GameHelper.MainLightingColor;

      newColor.r = colorRed;
      newColor.a = 1f;

      SetMainLightingColor(newColor);
    }
    private void SetMainLightingColorGreen(float colorGreen)
    {
      colorGreen = Mathf.Clamp01(colorGreen);
      Color newColor = GameHelper.MainLightingColor;

      newColor.g = colorGreen;
      newColor.a = 1f;

      SetMainLightingColor(newColor);
    }
    private void SetMainLightingColorBlue(float colorBlue)
    {
      colorBlue = Mathf.Clamp01(colorBlue);
      Color newColor = GameHelper.MainLightingColor;

      newColor.b = colorBlue;
      newColor.a = 1f;

      SetMainLightingColor(newColor);
    }
    private void SetMainLightingColor(Color newColor)
    {
      GameHelper.MainLightingColor = newColor;
      _toolMenu.ExtrasView.SetMainLightingColorValue(newColor);
    }

    private void SetMainLightingIntensity(float intensity)
    {
      intensity = Mathf.Max(0, intensity);
      GameHelper.MainLightingIntensity = intensity;
      _toolMenu.ExtrasView.SetMainLightingIntensityValue(intensity);
    }

    private void SetMainLightingRotationX(float x)
    {
      x = (x > 359.99f) ? 0f : Mathf.Clamp(x, 0f, 359.99f);

      Vector3 newRotationInEuler = GameHelper.MainLightingRotation.eulerAngles;
      newRotationInEuler.x = x;
      SetMainLightingRotation(newRotationInEuler);
    }
    private void SetMainLightingRotationY(float y)
    {
      y = (y > 359.99f) ? 0f : Mathf.Clamp(y, 0f, 359.99f);

      Vector3 newRotationInEuler = GameHelper.MainLightingRotation.eulerAngles;
      newRotationInEuler.y = y;
      SetMainLightingRotation(newRotationInEuler);
    }
    private void SetMainLightingRotationZ(float z)
    {
      z = (z > 359.99f) ? 0f : Mathf.Clamp(z, 0f, 359.99f);

      Vector3 newRotationInEuler = GameHelper.MainLightingRotation.eulerAngles;
      newRotationInEuler.z = z;
      SetMainLightingRotation(newRotationInEuler);
    }
    private void SetMainLightingRotation(Vector3 newRotationInEuler)
    {
      GameHelper.MainLightingRotation = Quaternion.Euler(newRotationInEuler);
      _toolMenu.ExtrasView.SetMainLightingRotationValue(newRotationInEuler);
    }


    private void SetCharacterLightingColorRed(float colorRed)
    {
      colorRed = Mathf.Clamp01(colorRed);
      Color newColor = GameHelper.CharacterLightingColor;

      newColor.r = colorRed;
      newColor.a = 1f;

      SetCharacterLightingColor(newColor);
    }
    private void SetCharacterLightingColorGreen(float colorGreen)
    {
      colorGreen = Mathf.Clamp01(colorGreen);
      Color newColor = GameHelper.CharacterLightingColor;

      newColor.g = colorGreen;
      newColor.a = 1f;

      SetCharacterLightingColor(newColor);
    }
    private void SetCharacterLightingColorBlue(float colorBlue)
    {
      colorBlue = Mathf.Clamp01(colorBlue);
      Color newColor = GameHelper.CharacterLightingColor;

      newColor.b = colorBlue;
      newColor.a = 1f;

      SetCharacterLightingColor(newColor);
    }
    private void SetCharacterLightingColor(Color newColor)
    {
      GameHelper.CharacterLightingColor = newColor;
      _toolMenu.ExtrasView.SetCharacterLightingColorValue(newColor);
    }

    private void SetCharacterLightingIntensity(float intensity)
    {
      intensity = Mathf.Max(0, intensity);
      GameHelper.CharacterLightingIntensity = intensity;
      _toolMenu.ExtrasView.SetCharacterLightingIntensityValue(intensity);
    }


    private void SetExtraLightingColorRed(float colorRed)
    {
      colorRed = Mathf.Clamp01(colorRed);
      Color newColor = GameHelper.ExtraLightingColor;

      newColor.r = colorRed;
      newColor.a = 1f;

      SetExtraLightingColor(newColor);
    }
    private void SetExtraLightingColorGreen(float colorGreen)
    {
      colorGreen = Mathf.Clamp01(colorGreen);
      Color newColor = GameHelper.ExtraLightingColor;

      newColor.g = colorGreen;
      newColor.a = 1f;

      SetExtraLightingColor(newColor);
    }
    private void SetExtraLightingColorBlue(float colorBlue)
    {
      colorBlue = Mathf.Clamp01(colorBlue);
      Color newColor = GameHelper.ExtraLightingColor;

      newColor.b = colorBlue;
      newColor.a = 1f;

      SetExtraLightingColor(newColor);
    }
    private void SetExtraLightingColor(Color newColor)
    {
      GameHelper.ExtraLightingColor = newColor;
      _toolMenu.ExtrasView.SetExtraLightingColorValue(newColor);
    }

    private void SetExtraLightingIntensity(float intensity)
    {
      intensity = Mathf.Max(0, intensity);
      GameHelper.ExtraLightingIntensity = intensity;
      _toolMenu.ExtrasView.SetExtraLightingIntensityValue(intensity);
    }


    private void SubscribeExtrasEvents()
    {
      // Skybox
      _toolMenu.ExtrasView.OnSkyboxColorRedValueChanged += SetSkyboxColorRed;
      _toolMenu.ExtrasView.OnSkyboxColorGreenValueChanged += SetSkyboxColorGreen;
      _toolMenu.ExtrasView.OnSkyboxColorBlueValueChanged += SetSkyboxColorBlue;


      // Fog
      _toolMenu.ExtrasView.OnFogColorRedValueChanged += SetFogColorRed;
      _toolMenu.ExtrasView.OnFogColorGreenValueChanged += SetFogColorGreen;
      _toolMenu.ExtrasView.OnFogColorBlueValueChanged += SetFogColorBlue;

      _toolMenu.ExtrasView.OnFogDensityValueChanged += SetFogDensity;


      // Main Lighting
      _toolMenu.ExtrasView.OnMainLightingColorRedValueChanged += SetMainLightingColorRed;
      _toolMenu.ExtrasView.OnMainLightingColorGreenValueChanged += SetMainLightingColorGreen;
      _toolMenu.ExtrasView.OnMainLightingColorBlueValueChanged += SetMainLightingColorBlue;

      _toolMenu.ExtrasView.OnMainLightingIntensityValueChanged += SetMainLightingIntensity;

      _toolMenu.ExtrasView.OnMainLightingRotationXValueChanged += SetMainLightingRotationX;
      _toolMenu.ExtrasView.OnMainLightingRotationYValueChanged += SetMainLightingRotationY;
      _toolMenu.ExtrasView.OnMainLightingRotationZValueChanged += SetMainLightingRotationZ;


      // Character Lighting
      _toolMenu.ExtrasView.OnCharacterLightingColorRedValueChanged += SetCharacterLightingColorRed;
      _toolMenu.ExtrasView.OnCharacterLightingColorGreenValueChanged += SetCharacterLightingColorGreen;
      _toolMenu.ExtrasView.OnCharacterLightingColorBlueValueChanged += SetCharacterLightingColorBlue;

      _toolMenu.ExtrasView.OnCharacterLightingIntensityValueChanged += SetCharacterLightingIntensity;


      // Extra Lighting
      _toolMenu.ExtrasView.OnExtraLightingColorRedValueChanged += SetExtraLightingColorRed;
      _toolMenu.ExtrasView.OnExtraLightingColorGreenValueChanged += SetExtraLightingColorGreen;
      _toolMenu.ExtrasView.OnExtraLightingColorBlueValueChanged += SetExtraLightingColorBlue;

      _toolMenu.ExtrasView.OnExtraLightingIntensityValueChanged += SetExtraLightingIntensity;
    }
    private void UnsubscribeExtrasEvents()
    {
      // Skybox
      _toolMenu.ExtrasView.OnSkyboxColorRedValueChanged -= SetSkyboxColorRed;
      _toolMenu.ExtrasView.OnSkyboxColorGreenValueChanged -= SetSkyboxColorGreen;
      _toolMenu.ExtrasView.OnSkyboxColorBlueValueChanged -= SetSkyboxColorBlue;


      // Fog
      _toolMenu.ExtrasView.OnFogColorRedValueChanged -= SetFogColorRed;
      _toolMenu.ExtrasView.OnFogColorGreenValueChanged -= SetFogColorGreen;
      _toolMenu.ExtrasView.OnFogColorBlueValueChanged -= SetFogColorBlue;

      _toolMenu.ExtrasView.OnFogDensityValueChanged -= SetFogDensity;


      // Main Lighting
      _toolMenu.ExtrasView.OnMainLightingColorRedValueChanged -= SetMainLightingColorRed;
      _toolMenu.ExtrasView.OnMainLightingColorGreenValueChanged -= SetMainLightingColorGreen;
      _toolMenu.ExtrasView.OnMainLightingColorBlueValueChanged -= SetMainLightingColorBlue;

      _toolMenu.ExtrasView.OnMainLightingIntensityValueChanged -= SetMainLightingIntensity;

      _toolMenu.ExtrasView.OnMainLightingRotationXValueChanged -= SetMainLightingRotationX;
      _toolMenu.ExtrasView.OnMainLightingRotationYValueChanged -= SetMainLightingRotationY;
      _toolMenu.ExtrasView.OnMainLightingRotationZValueChanged -= SetMainLightingRotationZ;


      // Character Lighting
      _toolMenu.ExtrasView.OnCharacterLightingColorRedValueChanged -= SetCharacterLightingColorRed;
      _toolMenu.ExtrasView.OnCharacterLightingColorGreenValueChanged -= SetCharacterLightingColorGreen;
      _toolMenu.ExtrasView.OnCharacterLightingColorBlueValueChanged -= SetCharacterLightingColorBlue;

      _toolMenu.ExtrasView.OnCharacterLightingIntensityValueChanged -= SetCharacterLightingIntensity;


      // Extra Lighting
      _toolMenu.ExtrasView.OnExtraLightingColorRedValueChanged -= SetExtraLightingColorRed;
      _toolMenu.ExtrasView.OnExtraLightingColorGreenValueChanged -= SetExtraLightingColorGreen;
      _toolMenu.ExtrasView.OnExtraLightingColorBlueValueChanged -= SetExtraLightingColorBlue;

      _toolMenu.ExtrasView.OnExtraLightingIntensityValueChanged -= SetExtraLightingIntensity;
    }
    #endregion
    #endregion
  }
}