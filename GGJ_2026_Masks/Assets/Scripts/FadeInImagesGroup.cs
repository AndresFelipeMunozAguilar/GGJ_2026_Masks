using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInImagesGroup : MonoBehaviour
{
    public float duration = 2f;

    Image[] images;
    Coroutine fadeCoroutine;

    void Awake()
    {
        // Solo tomamos las imágenes hijas (aunque estén inactivas)
        images = GetComponentsInChildren<Image>(true);

        // Las dejamos invisibles y desactivadas
        foreach (var img in images)
        {
            img.gameObject.SetActive(false);

            Color c = img.color;
            c.a = 0f;
            img.color = c;
        }
    }

    public void FadeIn()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        // Activamos todas las imágenes
        foreach (var img in images)
            img.gameObject.SetActive(true);

        fadeCoroutine = StartCoroutine(FadeInRoutine());
    }

    IEnumerator FadeInRoutine()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, 1f, t / duration);

            foreach (var img in images)
            {
                Color c = img.color;
                c.a = a;
                img.color = c;
            }

            yield return null;
        }

        foreach (var img in images)
        {
            Color c = img.color;
            c.a = 1f;
            img.color = c;
        }
    }
}
