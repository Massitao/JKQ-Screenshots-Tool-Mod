using UnityEngine;

namespace JKQScreenshotsToolMod.Helpers
{
  public static class JKQScreenshotsToolExtensions
  {
    public static T FindObjectInRootPath<T>(this Transform transform, string path)
    {
      Transform foundTransform = transform.root.Find(path);
      if (foundTransform == null)
      {
        JKQScreenshotsToolLogger.Error($"Path not found!! {path}");
        return default;
      }

      T foundComponent = foundTransform.GetComponent<T>();
      if (foundComponent == null)
      {
        JKQScreenshotsToolLogger.Error($"Component not found!! {path}");
        return default;
      }

      return foundTransform.GetComponent<T>();
    }
  }
}