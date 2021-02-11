using System;
using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public WinScreen winScreen;
    [HideInInspector]
    public bool finished;

    private AudioSource victorySound;
    private RectTransform uiRoot;
    private TimeSpan startTime;
    private string levelName;
    private string nextSceneName;

    void Start()
    {
        startTime = TimeSpan.FromSeconds(Time.time);
        uiRoot = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        victorySound = GetComponent<AudioSource>();
    }

    // Backwards compatibility
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerReachedGoal();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerReachedGoal();
        }
    }

    private void PlayerReachedGoal()
    {
        if (!finished)
        {
            var endTime = TimeSpan.FromSeconds(Time.time);
            if (Settings.goalHapticsEnabled)
            {
                StartCoroutine(VictoryHaptics());
            }
            if (Settings.goalSoundEnabled)
            {
                SFX.Play(victorySound, SFX.goalFileID);
            }
            var ws = Instantiate(winScreen, uiRoot);
            ws.SetCompletionTime(endTime - startTime);
            finished = true;
        }
    }

    private IEnumerator VictoryHaptics()
    {
        MobileUtils.Vibrate(0.05f, 1f, 1f);
        yield return new WaitForSeconds(0.2f);
        MobileUtils.Vibrate(0.05f, 0.8f, 1f);
    }
}
