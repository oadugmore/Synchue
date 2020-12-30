using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public float transitionTime = 0.5f;
    public LeanTweenType transitionType = LeanTweenType.linear;
    public Toggle vibrationToggle;
    public Toggle deathSoundToggle;
    public Toggle goalSoundToggle;

    private RectTransform settingsPanel;
    private Vector3 originalSize;
    private CanvasGroup canvasGroup;

    private void Start()
    {
        settingsPanel = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalSize = settingsPanel.localScale;
        settingsPanel.localScale /= 2;
        canvasGroup.alpha = 0f;
        vibrationToggle.isOn = Settings.hapticsEnabled;
        deathSoundToggle.isOn = Settings.deathSoundEnabled;
        goalSoundToggle.isOn = Settings.goalSoundEnabled;
        vibrationToggle.onValueChanged.AddListener(enabled => Settings.hapticsEnabled = enabled);
        deathSoundToggle.onValueChanged.AddListener(enabled => Settings.deathSoundEnabled = enabled);
        goalSoundToggle.onValueChanged.AddListener(enabled => Settings.goalSoundEnabled = enabled);
        gameObject.SetActive(false);
    }

    public void OpenSettings()
    {
        gameObject.SetActive(true);
        LeanTween.scale(settingsPanel, originalSize, transitionTime).setEase(transitionType);
        LeanTween.alphaCanvas(canvasGroup, 1f, transitionTime).setEase(transitionType);
    }

    public void CloseSettings()
    {
        LeanTween.scale(settingsPanel, originalSize / 2, transitionTime).setOnComplete(() => gameObject.SetActive(false)).setEase(transitionType);
        LeanTween.alphaCanvas(canvasGroup, 0f, transitionTime).setEase(transitionType);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameObject.activeInHierarchy)
        {
            CloseSettings();
        }
    }
}