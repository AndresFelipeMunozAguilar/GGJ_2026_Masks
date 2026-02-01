using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelIntroImage : MonoBehaviour
{
    public Image introImage;          // la imagen a mostrar
    public float displayTime = 3f;    // tiempo visible
    public float fadeDuration = 1f;   // tiempo de desvanecimiento

    void Start()
    {
        if (introImage != null)
        {
            introImage.gameObject.SetActive(true);
            StartCoroutine(HideImageAfterDelay());
        }
    }

    IEnumerator HideImageAfterDelay()
    {
        // esperar el tiempo visible
        yield return new WaitForSeconds(displayTime);

        // desvanecer poco a poco
        float elapsed = 0f;
        Color c = introImage.color;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            introImage.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        // ocultar completamente
        introImage.gameObject.SetActive(false);
    }
}
