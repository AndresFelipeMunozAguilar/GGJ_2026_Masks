using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObjectByType))]
public class SpiritRevealController : MonoBehaviour
{
    [Header("Renderers")]
    public SpriteRenderer mainRenderer;      // para espíritus normales
    public Animator mainAnimator;            // animación de baile

    public SpriteRenderer bodyRenderer;      // impostor: cuerpo
    public SpriteRenderer headRenderer;      // impostor: cabeza
    public SpriteRenderer maskRenderer;      // impostor: máscara

    [Header("Frames de impostor")]
    public Sprite[] bodyFrames;
    public Sprite[] headFrames;
    public Sprite[] maskFrames;

    [Header("Frames de fallo (espíritu normal)")]
    public Sprite[] failFrames;

    [Header("Velocidad de animaciones")]
    public float revealFps = 12f;
    public float failFps = 12f;

    private bool isRevealing = false;
    private bool revealed = false;
    private bool isFailing = false;

    void Start()
    {
        if (mainRenderer == null) mainRenderer = GetComponent<SpriteRenderer>();
        SetPartsActive(false); // ocultar partes de impostor al inicio
    }

    void SetPartsActive(bool active)
    {
        if (bodyRenderer != null) bodyRenderer.gameObject.SetActive(active);
        if (headRenderer != null) headRenderer.gameObject.SetActive(active);
        if (maskRenderer != null) maskRenderer.gameObject.SetActive(active);
    }

    bool ValidateImpostorFrames()
    {
        if (bodyFrames == null || headFrames == null || maskFrames == null) return false;
        if (bodyFrames.Length == 0) return false;
        return bodyFrames.Length == headFrames.Length && bodyFrames.Length == maskFrames.Length;
    }

    // Revelar impostor
    public void Reveal(Color impostorColor)
    {
        if (isRevealing || revealed) return;
        if (!ValidateImpostorFrames())
        {
            Debug.LogError($"[{name}] Arrays de frames inválidos o desalineados.");
            return;
        }
        StartCoroutine(DoReveal(impostorColor));
    }

    IEnumerator DoReveal(Color impostorColor)
    {
        isRevealing = true;

        if (mainAnimator != null) mainAnimator.enabled = false;
        if (mainRenderer != null) mainRenderer.enabled = false;

        SetPartsActive(true);

        if (bodyRenderer != null) bodyRenderer.color = impostorColor;
        if (maskRenderer != null) maskRenderer.color = impostorColor;

        int len = bodyFrames.Length;
        float wait = 1f / Mathf.Max(1f, revealFps);

        for (int i = 0; i < len; i++)
        {
            if (bodyRenderer != null) bodyRenderer.sprite = bodyFrames[i];
            if (headRenderer != null) headRenderer.sprite = headFrames[i];
            if (maskRenderer != null) maskRenderer.sprite = maskFrames[i];
            yield return new WaitForSeconds(wait);
        }

        isRevealing = false;
        revealed = true;
    }

    // Fallo al intentar revelar un espíritu normal
    public void FailReveal()
    {
        if (isFailing || revealed) return;
        if (failFrames == null || failFrames.Length == 0)
        {
            Debug.LogError($"[{name}] No hay frames de fallo asignados.");
            return;
        }
        StartCoroutine(DoFailReveal());
    }

    IEnumerator DoFailReveal()
    {
        isFailing = true;

        // Desactivar baile mientras dura el fallo
        if (mainAnimator != null) mainAnimator.enabled = false;

        int len = failFrames.Length;
        float wait = 1f / Mathf.Max(1f, failFps);

        for (int i = 0; i < len; i++)
        {
            if (mainRenderer != null) mainRenderer.sprite = failFrames[i];
            yield return new WaitForSeconds(wait);
        }

        // Al terminar, volver al baile
        if (mainAnimator != null) mainAnimator.enabled = true;

        isFailing = false;
    }

    public void ForceSetImpostorColor(Color impostorColor)
    {
        if (bodyRenderer != null) bodyRenderer.color = impostorColor;
        if (maskRenderer != null) maskRenderer.color = impostorColor;
    }

    public bool IsRevealed() => revealed;
    public bool IsFailing() => isFailing;
}
