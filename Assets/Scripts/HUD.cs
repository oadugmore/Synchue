using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text deathCountText;
    public Text timerText;

    private Animator menuAnim;
    private const string menuSceneName = "Menu";
    private Player player;

    private static HUD _instance;
    public static HUD instance
    {
        get
        {
            if (!_instance)
            {
                throw new NullReferenceException("This level does not have a HUD, or it has not been initialized yet!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance)
        {
            Debug.LogWarning("This level has multiple HUDs; destroying all but the first one");
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

    private void Start()
    {
        menuAnim = GetComponentInChildren<Animator>();
        var currentDeaths = DeathCounter.GetDeathCount();
        deathCountText.text = currentDeaths.ToString();
        player = FindObjectOfType<Player>();
        timerText.transform.parent.gameObject.SetActive(Settings.hudTimerEnabled);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartButtonPressed();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButtonPressed();
        }
        if (Goal.instance.timerStarted && !Goal.instance.wasReached && !player.dead)
        {
            UpdateElapsedTime(TimeSpan.FromSeconds(Time.time) - Goal.instance.startTime);
        }
    }

    public void UpdateElapsedTime(TimeSpan elapsedTime)
    {
        timerText.text = elapsedTime.ToString("mm':'ss'.'ff");
    }

    public void PauseButtonPressed()
    {
        if (!player.dead && !Goal.instance.wasReached)
        {
            var pausing = Time.timeScale > 0f;
            menuAnim.SetBool("Paused", pausing);
            if (pausing)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void RestartButtonPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuButtonPressed()
    {
        Time.timeScale = 1f;
        AudioLooper.Shutdown();
        SceneManager.LoadScene(menuSceneName);
    }
}
