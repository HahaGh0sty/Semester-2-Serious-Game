using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SplashScreenLoader : MonoBehaviour
{
    [SerializeField] float waitBeforeAllowingInput = 9f;
    [SerializeField] GameObject pressAnyKeyText;
    [SerializeField] Image fadeOverlay;
    [SerializeField] float fadeDuration = 1f;

    bool canPressKey = false;
    bool hasPressedKey = false;

    void Start()
    {
        if (pressAnyKeyText != null)
            pressAnyKeyText.SetActive(false);

        if (fadeOverlay != null)
            fadeOverlay.color = new Color(0, 0, 0, 1);

        StartCoroutine(FadeInThenWait());
    }

    void Update()
    {
        if (canPressKey && !hasPressedKey && Input.anyKeyDown)
        {
            hasPressedKey = true;
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    IEnumerator FadeInThenWait()
    {
        if (fadeOverlay != null)
        {
            float timer = 3;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = 1 - Mathf.Clamp01(timer / fadeDuration);
                fadeOverlay.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        yield return new WaitForSeconds(waitBeforeAllowingInput);
        canPressKey = true;

        if (pressAnyKeyText != null)
            pressAnyKeyText.SetActive(true);
    }

    IEnumerator FadeOutAndLoadScene()
    {
        if (fadeOverlay != null)
        {
            float timer = 0;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Clamp01(timer / fadeDuration);
                fadeOverlay.color = new Color(0, 0, 0, alpha);
                yield return null;
            }
        }

        SceneManager.LoadScene(1);
    }
}
