using MelonLoader;

namespace JKQScreenshotsToolMod
{
  public static class JKQScreenshotsToolLogger
  {
    private static MelonLogger.Instance _logger => Melon<JKQScreenshotsTool>.Instance.LoggerInstance;

    public static void Msg(string message)
    {
      _logger.Msg(message);
    }
    public static void Warning(string message)
    {
      _logger.Warning(message);
    }
    public static void Error(string message)
    {
      _logger.Error(message);
    }
    public static void BigError(string message)
    {
      _logger.BigError(message);
    }
  }
}
