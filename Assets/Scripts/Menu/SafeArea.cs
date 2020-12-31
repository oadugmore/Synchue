using UnityEngine;

public class SafeArea : MonoBehaviour
{
    public bool ignoreVerticalSafeArea;
    public bool ignoreHorizontalSafeArea;

    private RectTransform panel;
    private Rect lastSafeArea = new Rect(0, 0, 0, 0);

    void Start()
    {
        panel = GetComponent<RectTransform>();
        RefreshSafeArea();
    }

    void Update()
    {
        RefreshSafeArea();
    }

    void RefreshSafeArea()
    {
        var safeArea = Screen.safeArea;
        if (safeArea != lastSafeArea)
        {
            ApplySafeArea(safeArea);
        }
    }

    void ApplySafeArea(Rect newSafeArea)
    {
        lastSafeArea = newSafeArea;

        // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
        Vector2 anchorMin = newSafeArea.position;
        Vector2 anchorMax = newSafeArea.position + newSafeArea.size;
        if (ignoreHorizontalSafeArea)
        {
            anchorMin.x = panel.anchorMin.x;
            anchorMax.x = panel.anchorMax.x;
        }
        else
        {
            anchorMin.x /= Screen.width;
            anchorMax.x /= Screen.width;
        }
        if (ignoreVerticalSafeArea)
        {
            anchorMin.y = panel.anchorMin.y;
            anchorMax.y = panel.anchorMax.y;
        }
        else
        {
            anchorMin.y /= Screen.height;
            anchorMax.y /= Screen.height;
        }

        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;

        Debug.LogFormat("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
            name, newSafeArea.x, newSafeArea.y, newSafeArea.width, newSafeArea.height, Screen.width, Screen.height);
    }
}
