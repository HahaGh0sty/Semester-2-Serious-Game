using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class Slideshow : MonoBehaviour
{
    public Sprite[] slideshowImages;
    public float slideDuration = 5f;
    public float fadeDuration = 0f;

    private Image image;
    private CanvasGroup canvasGroup;
    private int currentIndex = 0;

    void Start()
    {
        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (slideshowImages.Length > 0)
        {
            StartCoroutine(PlaySlideshow());
        }
    }

    IEnumerator PlaySlideshow()
    {
        while (true)
        {
            // Set image and fade in
            image.sprite = slideshowImages[currentIndex];
            yield return StartCoroutine(Fade(0f, 1f));

            // Wait with full opacity
            yield return new WaitForSeconds(slideDuration);

            // Fade out
            yield return StartCoroutine(Fade(1f, 0f));

            // Next slide
            currentIndex = (currentIndex + 1) % slideshowImages.Length;
        }
    }

    IEnumerator Fade(float from, float to)
    {
        float timer = 0f;

        while (timer <= fadeDuration)
        {
            float t = timer / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(from, to, t);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
