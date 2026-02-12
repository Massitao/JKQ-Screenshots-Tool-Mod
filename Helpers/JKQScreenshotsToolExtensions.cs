using UnityEngine;

namespace JKQScreenshotsToolMod.Helpers
{
  public static class JKQScreenshotsToolExtensions
  {
    public static T FindObjectInRootPath<T>(this Transform transform, string path)
    {
      return transform.root.Find(path).GetComponent<T>();
    }
  }
}