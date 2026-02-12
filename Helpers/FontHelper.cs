using System.Linq;
using UnityEngine;
using TMPro;

namespace JKQScreenshotsToolMod.Helpers
{
  public class FontHelper
  {
    #region Fields
    // Font Assets
    private TMP_FontAsset _bradleyDJR_Font;
    private TMP_FontAsset _berninaSans_Font;
    #endregion


    #region Initialization Methods
    public void Init()
    {
      _bradleyDJR_Font = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(f => f.name == "BradleyDJR-Display SDF");
      _berninaSans_Font = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().FirstOrDefault(f => f.name == "Bernina Sans SDF");
    }

    public void Clear()
    {
      _bradleyDJR_Font = null;
      _berninaSans_Font = null;
    }
    #endregion
    

    #region Font Methods
    public TMP_FontAsset GetBradleyDJRFont() => _bradleyDJR_Font;
    public TMP_FontAsset GetBerninaSansFont() => _berninaSans_Font;
    #endregion
  }
}
