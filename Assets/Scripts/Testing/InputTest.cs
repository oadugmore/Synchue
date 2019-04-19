using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTest : MonoBehaviour
{
    InputTest instance;

    public Image touchesIndicator;
    public Image pointerIndicator;

    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Debug.LogError("Multiple instances not allowed.");
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
