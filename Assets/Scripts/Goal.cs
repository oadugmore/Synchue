using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private WinScreen winScreen;

    private RectTransform uiRoot;
    private bool finished;

    void Start()
    {
        uiRoot = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
    }

    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other) {
        if (!finished && other.gameObject.CompareTag("Player"))
        {
            Instantiate(winScreen, uiRoot);
            finished = true;
        }
    }
}
