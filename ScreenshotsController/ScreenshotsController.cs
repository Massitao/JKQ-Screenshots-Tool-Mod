using System.Collections;
using System.IO;
using UnityEngine;
using MelonLoader;

namespace JKQScreenshotsToolMod
{
  public class ScreenshotsController : MonoBehaviour
  {
    // References
    private JKQScreenshotsTool _mainTool => Melon<JKQScreenshotsTool>.Instance;

    // Instances
    private Coroutine _singleScreenshotCoroutine = null;
    private Coroutine _mapScreenshotsCoroutine = null;

    // Caches
    private uint _takenMapScreenshotCount = 0;
    private bool _wasFreecamEnabled = false;
    private bool _wasFreecamPlayerInputEnabled = false;
    private Vector3 _originalCameraPosition = Vector3.zero;

    private WaitForEndOfFrame _waitForEndOfFrame = null;


    private void Awake()
    {
      _waitForEndOfFrame = new WaitForEndOfFrame();
    }

    private void CreateScreenshotsFolder()
    {
      if (Directory.Exists(_mainTool.ScreenshotsFolderPath)) return;

      Directory.CreateDirectory(_mainTool.ScreenshotsFolderPath);
    }
    public void OpenScreenshotsFolder()
    {
      CreateScreenshotsFolder();
      Application.OpenURL(_mainTool.ScreenshotsFolderPath);
    }
    public bool HasEnoughDiskSpace(long fileSizeInBytes)
    {
      long deviceFreeSpace = new DriveInfo(Path.GetPathRoot(Application.persistentDataPath)).AvailableFreeSpace;
      return fileSizeInBytes < deviceFreeSpace;
    }
    public long CalculateScreenshotsSizeInBytes(uint screenshotCount = 1u)
    {
      if (screenshotCount == 0) return 0;

      uint detailLevel = _mainTool.ScreenshotDetailLevelEntry.Value;

      Resolution resolution = Screen.currentResolution;
      long pixelByte = 4;        // RGBA
      long pngOverhead = 100;    // Just in case

      checked
      {
        long rawPNG = Mathf.CeilToInt(resolution.width * detailLevel) * Mathf.CeilToInt(resolution.height * detailLevel) * pixelByte;
        long overheadPNG = rawPNG + Screen.currentResolution.height + pngOverhead;

        return overheadPNG * screenshotCount;
      }
    }

    public void TakeSingularScreenshot()
    {
      if (_singleScreenshotCoroutine != null) return;
      if (!HasEnoughDiskSpace(CalculateScreenshotsSizeInBytes())) return;

      CreateScreenshotsFolder();
      _singleScreenshotCoroutine = StartCoroutine(TakeSingularScreenshotCoroutine());
    }
    private IEnumerator TakeSingularScreenshotCoroutine()
    {
      uint detailLevel = _mainTool.ScreenshotDetailLevelEntry.Value;
      string screenshotName = $"JKQ-SingularScreenshot-{_mainTool.SingularScreenshotsCountEntry.Value++}.png";
      bool wasToolOpen = _mainTool.IsToolMenuOpen();

      _mainTool.SetToolMenuOpenState(false);
      Time.timeScale = 0.01f;

      // Wait 2 frames to make sure the tool menu canvas is invisible
      yield return _waitForEndOfFrame;
      yield return _waitForEndOfFrame;

      // Take Screenshot
      ScreenCapture.CaptureScreenshot(Path.Combine(_mainTool.ScreenshotsFolderPath, screenshotName), (int)detailLevel);
      JKQScreenshotsToolLogger.Msg($"JKQ-SingularScreenshot-{_mainTool.SingularScreenshotsCountEntry.Value} taken!");

      // Wait 4 frames in case the screenshot takes a bit longer to get captured
      yield return _waitForEndOfFrame;
      yield return _waitForEndOfFrame;
      yield return _waitForEndOfFrame;
      yield return _waitForEndOfFrame;

      Time.timeScale = 1f;

      _mainTool.SetToolMenuOpenState(wasToolOpen);
      _singleScreenshotCoroutine = null;
    }

    public void StartTakeMapScreenshots(ScreenshotRangeValues screenshotRangeValues)
    {
      if (_mapScreenshotsCoroutine != null) return;
      if (!screenshotRangeValues.CanTakeScreenshots())
      {
        screenshotRangeValues.TryCalculateScreenshotPositions();
        if (!screenshotRangeValues.CanTakeScreenshots()) return;
      }

      CreateScreenshotsFolder();

      _mapScreenshotsCoroutine = StartCoroutine(TakeMapScreenshotsCoroutine(screenshotRangeValues));
    }
    private IEnumerator TakeMapScreenshotsCoroutine(ScreenshotRangeValues screenshotRangeValues)
    {
      // Cache values
      _wasFreecamEnabled = _mainTool.CameraHelper.IsManualCameraControllerEnabled;
      _originalCameraPosition = _mainTool.CameraHelper.CameraPosition;

      // Set Z value to all screenshot positions
      for (int i = 0; i < screenshotRangeValues.allScreenshotPositions.Count; i++)
      {
        Vector3 copy = screenshotRangeValues.allScreenshotPositions[i];
        screenshotRangeValues.allScreenshotPositions[i] = new Vector3(copy.x, copy.y, _originalCameraPosition.z);
      }

      // Preparing and Disabling stuff
      _mainTool.SetToolMenuOpenState(false);
      _mainTool.EnableFreecam(true);
      _mainTool.InputHelper.DisableAllInputs();
      Time.timeScale = 0.01f;

      uint detailLevel = _mainTool.ScreenshotDetailLevelEntry.Value;

      // Wait 2 frames to make sure the tool menu canvas gets transparent
      yield return _waitForEndOfFrame;
      yield return _waitForEndOfFrame;

      // Take screenshots
      for (_takenMapScreenshotCount = 0; _takenMapScreenshotCount < screenshotRangeValues.allScreenshotPositions.Count; _takenMapScreenshotCount++)
      {
        string screenshotName = $"JKQ-MapScreenshot-({_mainTool.MapScreenshotsCountEntry.Value++}).png";

        _mainTool.CameraHelper.CameraPosition = screenshotRangeValues.allScreenshotPositions[(int)_takenMapScreenshotCount];
        yield return _waitForEndOfFrame;
        ScreenCapture.CaptureScreenshot(Path.Combine(_mainTool.ScreenshotsFolderPath, screenshotName), (int)detailLevel);
      }

      // Wait 4 frames in case the last screenshot takes a bit longer to get captured
      yield return _waitForEndOfFrame;
      yield return _waitForEndOfFrame;
      yield return _waitForEndOfFrame;
      yield return _waitForEndOfFrame;

      StopTakeMapScreenshots();
    }
    public void StopTakeMapScreenshots()
    {
      if (_mapScreenshotsCoroutine == null) return;

      // Clear Coroutine
      StopCoroutine(_mapScreenshotsCoroutine);

      _mapScreenshotsCoroutine = null;
      JKQScreenshotsToolLogger.Msg($"{_takenMapScreenshotCount} map screenshots taken!");

      // Reset stuff
      _mainTool.CameraHelper.CameraPosition = _originalCameraPosition;
      _mainTool.EnableFreecam(_wasFreecamEnabled);
      _mainTool.EnableFreecamPlayerInput(_wasFreecamPlayerInputEnabled);
      _mainTool.InputHelper.EnableUIInput(true);

      Time.timeScale = 1f;

      _mainTool.SetToolMenuOpenState(true);
    }
    public bool IsTakingMapScreenshots()
    {
      return _mapScreenshotsCoroutine != null;
    }
  }
}