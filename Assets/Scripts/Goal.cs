using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private WinScreen winScreen;

    private RectTransform uiRoot;
    private float startTime;
    private float endTime;
    public bool finished => _finished;
    private bool _finished;

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
        if (!_finished && other.gameObject.CompareTag("Player"))
        {
            endTime = Time.time;
            Instantiate(winScreen, uiRoot);
            _finished = true;
        }
    }
}
