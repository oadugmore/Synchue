using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private WinScreen winScreen;

    private RectTransform uiRoot;
    private float startTime;
    private float endTime;
    public bool finished => _finished;
    private bool _finished;
    private string levelName;
    private string nextLevelName;

    void Start()
    {
        // SceneManager.GetActiveScene().GetRootGameObjects();
        startTime = Time.time;
        uiRoot = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        string worldNumber = SceneManager.GetActiveScene().name.Split('_')[1];
        int levelNumber = int.Parse(SceneManager.GetActiveScene().name.Split('_')[3]);
        levelName = string.Format("World {0} Level {1}", worldNumber, levelNumber);
        nextLevelName = string.Format("World_{0}_Level_{1}", worldNumber, levelNumber + 1);
        if (!Application.CanStreamedLevelBeLoaded(nextLevelName))
        {
            nextLevelName = null;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!_finished && other.gameObject.CompareTag("Player"))
        {
            endTime = Time.time;
            var ws = Instantiate(winScreen, uiRoot);
            ws.SetCompletionTime(endTime - startTime);
            ws.SetLevelNames(levelName, nextLevelName);
            _finished = true;
        }
    }
}
