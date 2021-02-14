using UnityEngine;
using UnityEngine.EventSystems;

public class ColorButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed { get; private set; }
    private int pointerId = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPressed)
        {
            return;
        }

        pointerId = eventData.pointerId;
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == pointerId)
        {
            isPressed = false;
        }
    }
}
