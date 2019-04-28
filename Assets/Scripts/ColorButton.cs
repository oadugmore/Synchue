using UnityEngine;
using UnityEngine.EventSystems;

public class ColorButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private bool pressed = false;
    private int pointerId = -1;

    // Use this for initialization

    public bool IsPressed()
    {
        return pressed;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pressed)
        {
            return;
        }

        pointerId = eventData.pointerId;
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == pointerId)
        {
            pressed = false;
        }
    }

}
