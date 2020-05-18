using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTouchesTest : MonoBehaviour
{
    public Image touchesIndicator;
    public Text touchesTime;

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
            if (touch.phase == TouchPhase.Began)
            {
                touchesIndicator.color = Color.red;
                touchesTime.text = Time.time.ToString();
                return;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                touchesIndicator.color = Color.white;
                touchesTime.text = Time.time.ToString();
            }
        }
    }
}
