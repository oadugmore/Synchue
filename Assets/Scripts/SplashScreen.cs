using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public float fadeTime = 1f;
    public float waitTime = 2f;
    public CanvasGroup textCanvasGroup;
    public CanvasGroup backgroundCanvasGroup;

    static bool gameLoading = true;

    void Start()
    {
        if (!gameLoading)
        {
            Destroy(gameObject);
            return;
        }
        backgroundCanvasGroup.alpha = 1f;
        textCanvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(textCanvasGroup, 1, fadeTime)
            .setOnComplete(() => Invoke(nameof(FadeOut), waitTime));
        gameLoading = false;
    }

    void FadeOut()
    {
        LeanTween.alphaCanvas(backgroundCanvasGroup, 0, fadeTime)
            .setDestroyOnComplete(true);
    }
}
