using UnityEngine;

namespace JKQScreenshotsToolMod
{
  public struct InventoryCameraParams
  {
    public InventoryCameraParams(Vector3 position, Quaternion rotation, float fov)
    {
      CameraPosition = position;
      CameraRotation = rotation;
      CameraFOV = fov;
    }
    public InventoryCameraParams(Camera camera)
    {
      CameraPosition = camera.transform.position;
      CameraRotation = camera.transform.rotation;
      CameraFOV = camera.fieldOfView;
    }

    public Vector3 CameraPosition { get; private set; }
    public Quaternion CameraRotation { get; private set; }
    public float CameraFOV { get; private set; }
  }
}