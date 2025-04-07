using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrossfadeSlideshow : MonoBehaviour
{
    public Sprite[] slideshowImages;
    public float slideDuration = 3f;
    public float fadeDuration = 1f;

    public Image imageA;
    public Image imageB;

    private CanvasGroup groupA;
    private CanvasGroup groupB;

    private int currentIndex = 0;
    private bool showingA = true;

    void Start()
    {
        groupA = imageA.GetComponent<CanvasGroup>();
        groupB = imageB.GetComponent<CanvasGroup>();

        imageA.sprite = slideshowImages[0];
        groupA.alpha = 1;
        groupB.alpha = 0;

        StartCoroutine(PlaySlideshow());
    }

    IEnumerator PlaySlideshow()
    {
        while (true)
        {
            yield return new WaitForSeconds(slideDuration);

            int nextIndex = (currentIndex + 1) % slideshowImages.Length;

            if (showingA)
            {
                imageB.sprite = slideshowImages[nextIndex];
                yield return StartCoroutine(Crossfade(groupA, groupB));
            }
            else
            {
                imageA.sprite = slideshowImages[nextIndex];
                yield return StartCoroutine(Crossfade(groupB, groupA));
            }

            currentIndex = nextIndex;
            showingA = !showingA;
        }
    }

    IEnumerator Crossfade(CanvasGroup from, CanvasGroup to)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            from.alpha = Mathf.Lerp(1f, 0f, t);
            to.alpha = Mathf.Lerp(0f, 1f, t);
            timer += Time.deltaTime;
            yield return null;
        }

        from.alpha = 0f;
        to.alpha = 1f;
    }
}
