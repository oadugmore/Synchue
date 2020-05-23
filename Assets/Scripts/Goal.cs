using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private WinScreen winScreen;
    public bool finished;

    private RectTransform uiRoot;
    private float startTime;
    private float endTime;

    void Start()
    {
        uiRoot = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        startTime = Time.time;
    }

    public float GetCompletionTime()
    {
        return endTime - startTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (!finished && other.gameObject.CompareTag("Player"))
        {
            endTime = Time.time;
            Instantiate(winScreen, uiRoot);
            finished = true;
        }
    }
}
