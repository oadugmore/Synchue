using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public float fadeTime = 1f;
    public float waitTime = 2f;
    public CanvasGroup textCanvasGroup;
    public CanvasGroup backgroundCanvasGroup;

    void Start()
    {
        backgroundCanvasGroup.alpha = 1f;
        textCanvasGroup.alpha = 0f;
        LeanTween.alphaCanvas(textCanvasGroup, 1, fadeTime)
            .setOnComplete(() => Invoke(nameof(FadeOut), waitTime));
    }

    void FadeOut()
    {
        LeanTween.alphaCanvas(backgroundCanvasGroup, 0, fadeTime)
            .setDestroyOnComplete(true);
    }
}
