using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JKQScreenshotsToolMod.UI
{
  [RequireComponent(typeof(Image))]
  public class ToolMenuTabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
  {
    private ToolMenuTabGroup toolCanvasTabGroup;
    private Image imageBackground;

    private void Awake()
    {
      imageBackground = GetComponent<Image>();
    }

    public void SetTabGroup(ToolMenuTabGroup tabGroup)
    {
      toolCanvasTabGroup = tabGroup;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      toolCanvasTabGroup?.OnTabSelected(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
      toolCanvasTabGroup?.OnTabEnter(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
      toolCanvasTabGroup?.OnTabExit(this);
    }

    public void SetButtonColor(Color color)
    {
      imageBackground.color = color;
    }
  }
}