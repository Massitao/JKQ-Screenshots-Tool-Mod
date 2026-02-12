using System;
using Nexile.JKQuest.Rollback.Entities.Player;
using Nexile.JKQuest.UI.Inventory;
using HarmonyLib;

namespace JKQScreenshotsToolMod.Patches
{
  /// <summary>
  /// This patches the EquipAsset method from the PlayerMeshManager class.
  /// Specifically, this patch was made to receive the manager events for the Inventory Item Preview Mannequin.
  /// Whenever you preview / select a piece of armor, the respective mannequin part gets destroyed and gets replaced by an instance of the armor piece.
  /// Because of this, I needed a way to check if the mannequin pieces had changed, and set it to the Inventory Helper.
  /// 
  /// Changes:
  /// * OnItemEquipTriggered event will get used by InventoryHelper to obtain new references to the mannequin parts (whether they changed or not).
  /// </summary>
  [HarmonyPatch(typeof(PlayerMeshManager), "EquipAsset", new Type[] { typeof(ItemAsset), typeof(bool) })]
  public static class PlayerMeshManagerPatch
  {
    public static event Action<PlayerMeshManager> OnItemEquipTriggered = null;

    private static void Postfix(PlayerMeshManager __instance)
    {
      // The code inside this method will run after 'PrivateMethod' has executed
      OnItemEquipTriggered?.Invoke(__instance);
    }
  }
}
