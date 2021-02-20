using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text timerText;
    public Text deathCountText;
    public Text levelText;
    public GameObject inGameUi;
    public RectTransform pauseMenu;
    public float startEndScale = 0.75f;
    public float animDuration = 0.5f;
    public LeanTweenType openScaleEaseType = LeanTweenType.linear;
    public LeanTweenType closeScaleEaseType = LeanTweenType.linear;
    public LeanTweenType openAlphaEaseType = LeanTweenType.linear;
    public LeanTweenType closeAlphaEaseType = LeanTweenType.linear;

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
        var currentDeaths = DeathCounter.GetDeathCount();
        player = FindObjectOfType<Player>();
        timerText.transform.parent.gameObject.SetActive(Settings.hudTimerEnabled);
        deathCountText.text = currentDeaths.ToString();
        var sceneName = SceneManager.GetActiveScene().name;
        var worldAndLevel = LevelLoader.ParseWorldAndLevel(sceneName);
        levelText.text = $"Level {worldAndLevel[0]}-{worldAndLevel[1]}";
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
            var scaleTo = Vector3.one;
            var scaleEaseType = openScaleEaseType;
            var alphaEaseType = openAlphaEaseType;
            var alphaTo = 1f;
            if (pausing)
            {
                pauseMenu.localScale *= startEndScale;
                Time.timeScale = 0f;
            }
            else
            {
                scaleTo *= startEndScale;
                scaleEaseType = closeScaleEaseType;
                alphaEaseType = closeAlphaEaseType;
                alphaTo = 0f;
                Time.timeScale = 1f;
            }
            LeanTween.scale(pauseMenu, scaleTo, animDuration).setEase(scaleEaseType).setIgnoreTimeScale(true);
            LeanTween.alphaCanvas(pauseMenu.GetComponent<CanvasGroup>(), alphaTo, animDuration).setEase(alphaEaseType).setIgnoreTimeScale(true);
            pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts = pausing;
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
