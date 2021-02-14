﻿using System;
using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public WinScreen winScreen;
    [HideInInspector]
    public bool wasReached;
    public bool timerStarted;

    private AudioSource victorySound;
    private RectTransform uiRoot;
    private string levelName;
    private string nextSceneName;
    private Player player;

    public TimeSpan startTime { get; private set; }

    private static Goal _instance;
    public static Goal instance
    {
        get
        {
            if (!_instance)
            {
                throw new NullReferenceException("This level does not have a goal, or it has not been initialized yet!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance)
        {
            Debug.LogWarning("This level has multiple goals; destroying all but the first one");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    void Start()
    {
        uiRoot = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        victorySound = GetComponent<AudioSource>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (!timerStarted && (Controller.GetAxis(InteractColor.Blue) > 0 || Controller.GetAxis(InteractColor.Orange) > 0))
        {
            timerStarted = true;
            startTime = TimeSpan.FromSeconds(Time.time);
        }
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
        if (!wasReached && !player.dead)
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
            var totalElapsedTime = endTime - startTime;
            ws.SetCompletionTime(totalElapsedTime);
            HUD.instance.UpdateElapsedTime(totalElapsedTime);
            player.Freeze(0.5f);
            wasReached = true;
        }
    }

    private IEnumerator VictoryHaptics()
    {
        MobileUtils.Vibrate(0.05f, 1f, 1f);
        yield return new WaitForSeconds(0.2f);
        MobileUtils.Vibrate(0.05f, 0.8f, 1f);
    }
}
