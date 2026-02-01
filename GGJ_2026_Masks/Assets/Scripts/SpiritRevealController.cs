using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ObjectByType))]
public class SpiritRevealController : MonoBehaviour
{
    public SpriteRenderer mainRenderer;      // renderer que usa la animación completa de baile
    public Animator mainAnimator;            

    public SpriteRenderer bodyRenderer;      // hijo: cuerpo (grayscale)
    public SpriteRenderer headRenderer;      // hijo: cabeza (invisible hasta reveal)
    public SpriteRenderer maskRenderer;      // hijo: máscara (grayscale)

    public Sprite[] bodyFrames;              // frames de reveal (mismo length)
    public Sprite[] headFrames;
    public Sprite[] maskFrames;

    public float revealFps = 12f;
    bool isRevealing = false;
    bool revealed = false;

    void Start()
    {
        if (mainRenderer == null) mainRenderer = GetComponent<SpriteRenderer>();
        // al inicio, mainRenderer visible, partes ocultas
        SetPartsActive(false);
    }

    void SetPartsActive(bool active)
    {
        if (bodyRenderer != null) bodyRenderer.gameObject.SetActive(active);
        if (headRenderer != null) headRenderer.gameObject.SetActive(active);
        if (maskRenderer != null) maskRenderer.gameObject.SetActive(active);
    }

    bool ValidateFrames()
    {
        if (bodyFrames == null || headFrames == null || maskFrames == null) return false;
        if (bodyFrames.Length == 0) return false;
        return bodyFrames.Length == headFrames.Length && bodyFrames.Length == maskFrames.Length;
    }

    public void Reveal(Color impostorColor)
    {
        if (isRevealing || revealed) return;
        if (!ValidateFrames())
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

        
        if (bodyRenderer != null) bodyRenderer.sprite = bodyFrames[len - 1];
        if (headRenderer != null) headRenderer.sprite = headFrames[len - 1];
        if (maskRenderer != null) maskRenderer.sprite = maskFrames[len - 1];

        isRevealing = false;
        revealed = true;
    }

    public void ForceSetImpostorColor(Color impostorColor)
    {
        if (bodyRenderer != null) bodyRenderer.color = impostorColor;
        if (maskRenderer != null) maskRenderer.color = impostorColor;
    }

    public bool IsRevealed() => revealed;
}
