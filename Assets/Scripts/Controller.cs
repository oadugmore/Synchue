using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controller : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private static Controller current = null;
    private bool pressed = false;
    private int pointerId = -1;


    // Use this for initialization
    void Start()
    {
        if (current == null)
            current = this;
        else
        {
            Debug.LogError("Only one instance of Controller is allowed.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static bool screenIsPressed()
    {
        return current.pressed;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (pressed) return;
        pointerId = eventData.pointerId;
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == pointerId)
            pressed = false;
    }
}
