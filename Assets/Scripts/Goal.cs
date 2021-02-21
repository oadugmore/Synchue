using System;
using System.Collections;
using CloudOnce;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class Goal : MonoBehaviour
{
    public WinScreen winScreen;
    [HideInInspector]
    public bool wasReached;
    public bool timerStarted;

    private AudioSource victorySound;
    private RectTransform uiRoot;
    private Player player;
    private TimeSpan currentWr;
    private TimeSpan currentPb;

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
        if (Cloud.IsSignedIn)
        {
            LoadScores();
        }
        else
        {
            Cloud.SignIn(true, success =>
            {
                if (success)
                {
                    LoadScores();
                }
                else
                {
                    Debug.LogError("Failed to sign in!");
                }
            });
        }
    }

    private void LoadScores()
    {
        var leaderboard = Social.CreateLeaderboard();
        var leaderboardId = MobileUtils.GetNativeLeaderboardId(SceneManager.GetActiveScene().name);
        leaderboard.id = leaderboardId;
        leaderboard.timeScope = TimeScope.AllTime;
        leaderboard.LoadScores(success =>
        {
            if (success && leaderboard.scores.Length > 0)
            {
                var millisecondsMultiplier = Application.platform == RuntimePlatform.Android ? 1 : 10;
                currentWr = TimeSpan.FromMilliseconds(leaderboard.scores[0].value * millisecondsMultiplier);
                currentPb = TimeSpan.FromMilliseconds(leaderboard.localUserScore.value * millisecondsMultiplier);
                var scoreMessage = "Scores:\n";
                foreach (var score in leaderboard.scores)
                {
                    scoreMessage += $"Score for player {score.userID}: {score.formattedValue} ({score.value}, rank {score.rank})\n";
                }
                Debug.Log(scoreMessage);
            }
        });
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
            var completionTime = endTime - startTime;
            ws.completionTime = completionTime;
            ws.currentPb = currentPb;
            ws.currentWr = currentWr;
            HUD.instance.UpdateElapsedTime(completionTime);
            HUD.instance.inGameUi.SetActive(false);
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
