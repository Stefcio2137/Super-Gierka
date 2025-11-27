using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SreanFadder : MonoBehaviour
{
    public Image fadeImage; // obraz pe³noekranowy (UI) czarny z alpha = 0
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0); // start przezroczysty
    }

    public void FadeOut(System.Action onComplete = null)
    {
        if (fadeImage != null)
            StartCoroutine(FadeRoutine(0f, 1f, onComplete));
    }

    public void FadeIn(System.Action onComplete = null)
    {
        if (fadeImage != null)
            StartCoroutine(FadeRoutine(1f, 0f, onComplete));
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, System.Action onComplete)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, endAlpha);

        onComplete?.Invoke();
    }
}
