using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this component to the button that closes the window.
/// </summary>
public class PopDownWindow : MonoBehaviour
{
    public LeanTweenType openAnimation = LeanTweenType.linear;
    public float openTime;
    public LeanTweenType closeAnimation = LeanTweenType.linear;
    public float closeTime;

    private RectTransform creditsPanel;

    private void Awake()
    {
        creditsPanel = transform.parent as RectTransform;
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(CloseButtonClicked);
        creditsPanel.anchoredPosition = new Vector2(0, Screen.height);
        LeanTween.move(creditsPanel, Vector3.zero, openTime).setEase(openAnimation);
    }

    void CloseButtonClicked()
    {
        LeanTween.move(creditsPanel, new Vector2(0, Screen.height), closeTime)
            .setEase(closeAnimation)
            .setDestroyOnComplete(true);
    }
}