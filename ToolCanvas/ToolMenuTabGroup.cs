using System.Collections.Generic;
using UnityEngine;
using MelonLoader;

namespace JKQScreenshotsToolMod.UI
{
  public class ToolMenuTabGroup : MonoBehaviour
  {
    private Dictionary<ToolMenuTabButton, GameObject> tabViewDictionary = new Dictionary<ToolMenuTabButton, GameObject>();
    private ToolMenuTabButton selectedTab = null;

    private readonly Color tabIdleColor = new Color(.3867925f, .3867925f, .3867925f);
    private readonly Color tabHoverColor = new Color(.7830189f, .5569303f, 0f);
    private readonly Color tabActiveColor = new Color(.007843138f, .3254902f, .8078432f);


    public void Subscribe(ToolMenuTabButton tabButton, GameObject view)
    {
      if (tabButton == null || view == null)
      {
        Melon<JKQScreenshotsTool>.Logger.Error($"ToolCanvasTabGroup: Tried to subscribe incomplete Tab-View pair!");
        return;
      }

      if (tabViewDictionary.ContainsKey(tabButton))
      {
        Melon<JKQScreenshotsTool>.Logger.Warning($"ToolCanvasTabGroup: Tried to subscribe an already subscribed Tab!");
        return;
      }

      tabViewDictionary.Add(tabButton, view);
      tabButton.SetTabGroup(this);
    }

    public void OnTabEnter(ToolMenuTabButton tabButton)
    {
      ResetTabs();
      if (selectedTab == null || tabButton != selectedTab)
      {
        tabButton.SetButtonColor(tabHoverColor);
      }
    }
    public void OnTabSelected(ToolMenuTabButton tabButton)
    {
      selectedTab = tabButton;
      ResetTabs();
      tabButton.SetButtonColor(tabActiveColor);

      foreach (ToolMenuTabButton button in tabViewDictionary.Keys)
      {
        tabViewDictionary[button].SetActive(button == tabButton);
      }
    }
    public void OnTabExit(ToolMenuTabButton tabButton)
    {
      ResetTabs();
    }

    public void ResetTabs()
    {
      foreach (ToolMenuTabButton tabButton in tabViewDictionary.Keys)
      {
        if (selectedTab != null && tabButton == selectedTab) continue;
        tabButton.SetButtonColor(tabIdleColor);
      }
    }
  }
}