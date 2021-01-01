using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public WinScreen winScreen;

    private AudioSource victorySound;
    private RectTransform uiRoot;
    private float startTime;
    private float endTime;
    public bool finished => _finished;
    private bool _finished;
    private string levelName;
    private string nextSceneName;

    void Start()
    {
        startTime = Time.time;
        uiRoot = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        victorySound = GetComponent<AudioSource>();
        var worldNumber = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);
        var levelNumber = int.Parse(SceneManager.GetActiveScene().name.Split('_')[3]);
        levelName = string.Format("Phase {0}: Level {1}", worldNumber, levelNumber);
        nextSceneName = LevelLoader.GetSceneNameFromLevel(worldNumber, levelNumber + 1);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!_finished && other.gameObject.CompareTag("Player"))
        {
            endTime = Time.time;
            var ws = Instantiate(winScreen, uiRoot);
            ws.SetCompletionTime(endTime - startTime);
            ws.SetLevelNames(levelName, nextSceneName);
            var deaths = DeathCounter.GetDeathCount();
            ws.SetDeathCount(deaths);
            if (Settings.hapticsEnabled)
            {
                StartCoroutine(VictoryHaptics());
            }
            if (Settings.goalSoundEnabled)
            {
                SFX.Play(victorySound, SFX.goalFileID);
            }
            _finished = true;
        }
    }

    private IEnumerator VictoryHaptics()
    {
        MobileUtils.Vibrate(0.05f, 1f, 1f);
        yield return new WaitForSeconds(0.2f);
        MobileUtils.Vibrate(0.05f, 0.8f, 1f);
    }
}
