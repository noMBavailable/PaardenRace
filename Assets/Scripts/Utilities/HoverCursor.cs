using UnityEngine;
using UnityEngine.EventSystems;

public class HoverCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance?.SetHoverCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance?.SetDefaultCursor();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CursorManager.Instance?.SetDefaultCursor();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CursorManager.Instance?.SetHoverCursor();
    }

    private void OnDisable()
    {
        // Ensures cursor resets if button disappears while hovered
        if (CursorManager.Instance != null)
            CursorManager.Instance.SetDefaultCursor();
    }
}
