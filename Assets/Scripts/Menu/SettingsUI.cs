using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public GameObject creditsPrefab;
    public float transitionTime = 0.5f;
    public LeanTweenType transitionType = LeanTweenType.linear;
    public Toggle hudTimerToggle;
    public Toggle musicToggle;
    public Toggle deathHapticsToggle;
    public Toggle goalHapticsToggle;
    public Toggle deathSoundToggle;
    public Toggle goalSoundToggle;

    private RectTransform settingsBackground;
    private RectTransform settingsPanel;
    private Vector3 originalSize;
    private CanvasGroup canvasGroup;
    private LevelLoader levelLoader;

    private void Start()
    {
        settingsBackground = GetComponent<RectTransform>();
        settingsPanel = GetComponentsInChildren<RectTransform>()[1];
        canvasGroup = GetComponent<CanvasGroup>();
        levelLoader = FindObjectOfType<LevelLoader>();
        originalSize = settingsBackground.localScale;
        settingsBackground.localScale /= 2;
        canvasGroup.alpha = 0f;
        hudTimerToggle.isOn = Settings.hudTimerEnabled;
        musicToggle.isOn = Settings.musicEnabled;
        deathHapticsToggle.isOn = Settings.deathHapticsEnabled;
        goalHapticsToggle.isOn = Settings.goalHapticsEnabled;
        deathSoundToggle.isOn = Settings.deathSoundEnabled;
        goalSoundToggle.isOn = Settings.goalSoundEnabled;
        hudTimerToggle.onValueChanged.AddListener(enabled => Settings.hudTimerEnabled = enabled);
        musicToggle.onValueChanged.AddListener(enabled => Settings.musicEnabled = enabled);
        deathHapticsToggle.onValueChanged.AddListener(enabled => Settings.deathHapticsEnabled = enabled);
        goalHapticsToggle.onValueChanged.AddListener(enabled => Settings.goalHapticsEnabled = enabled);
        deathSoundToggle.onValueChanged.AddListener(enabled => Settings.deathSoundEnabled = enabled);
        goalSoundToggle.onValueChanged.AddListener(enabled => Settings.goalSoundEnabled = enabled);
        gameObject.SetActive(false);
    }

    public void OpenSettings()
    {
        gameObject.SetActive(true);
        LeanTween.scale(settingsBackground, originalSize, transitionTime).setEase(transitionType);
        LeanTween.alphaCanvas(canvasGroup, 1f, transitionTime).setEase(transitionType);
        levelLoader.playButton.interactable = false;
    }

    public void CloseSettings()
    {
        LeanTween.scale(settingsBackground, originalSize / 2, transitionTime).setOnComplete(() => gameObject.SetActive(false)).setEase(transitionType);
        LeanTween.alphaCanvas(canvasGroup, 0f, transitionTime).setEase(transitionType);
    }

    public void ShowCredits()
    {
        Instantiate(creditsPrefab, settingsBackground);
    }

    // TODO: Find a better way to handle multiple levels of "dismissable" views
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape) && gameObject.activeInHierarchy)
    //     {
    //         CloseSettings();
    //     }
    // }
}
