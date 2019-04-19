using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTouchesTest : MonoBehaviour
{
    public Image touchesIndicator;

    // Start is called before the first frame update
    void Start()
    {
        //touchesIndicator = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {
                touchesIndicator.color = Color.red;
                return;
            }
        }
        touchesIndicator.color = Color.white;
    }
}
