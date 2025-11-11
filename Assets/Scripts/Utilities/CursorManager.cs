using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [Header("Cursor Textures")]
    public Texture2D defaultCursor;
    public Texture2D hoverCursor;

    [Header("Cursor Hotspot")]
    public Vector2 defaultHotspot = Vector2.zero;
    public Vector2 hoverHotspot = Vector2.zero;

    private void Start()
    {
        SetDefaultCursor();
    }

    private void Update()
    {
        // Check if the pointer is over a UI element that is interactable
        if (IsPointerOverInteractableUI())
            SetHoverCursor();
        else
            SetDefaultCursor();
    }

    private bool IsPointerOverInteractableUI()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            return false;

        // Raycast to see if pointer is over an interactable UI element
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            // Only return true if the element is interactable
            var selectable = result.gameObject.GetComponent<UnityEngine.UI.Selectable>();
            if (selectable != null && selectable.interactable)
                return true;

            // Include TMP_InputField as interactable
            if (result.gameObject.GetComponent<TMPro.TMP_InputField>() != null)
                return true;
        }

        return false;
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, defaultHotspot, CursorMode.Auto);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetHoverCursor()
    {
        Cursor.SetCursor(hoverCursor, hoverHotspot, CursorMode.Auto);
    }
}
