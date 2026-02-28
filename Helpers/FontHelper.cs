using System.Linq;
using UnityEngine;
using TMPro;

namespace JKQScreenshotsToolMod.Helpers
{
  public class FontHelper
  {
    #region Fields
    // Font Assets
    private TMP_FontAsset _bradleyDJRFont;
    private TMP_FontAsset _berninaSansRegularFont;
    private TMP_FontAsset _berninaSansExtraBoldFont;
    #endregion


    #region Initialization Methods
    public void Init()
    {
      _bradleyDJRFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(f => f.name == "BradleyDJR-Display SDF");
      _berninaSansRegularFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(f => f.name == "Bernina Sans SDF");
      _berninaSansExtraBoldFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(f => f.name == "Bernina Sans Extrabold SDF");
    }

    public void Clear()
    {
      _bradleyDJRFont = null;
      _berninaSansRegularFont = null;
    }
    #endregion
    

    #region Font Methods
    public TMP_FontAsset GetBradleyDJRFont() => _bradleyDJRFont;
    public TMP_FontAsset GetBerninaSansRegularFont() => _berninaSansRegularFont;
    public TMP_FontAsset GetBerninaSansExtraBoldFont() => _berninaSansExtraBoldFont;
    #endregion
  }
}
