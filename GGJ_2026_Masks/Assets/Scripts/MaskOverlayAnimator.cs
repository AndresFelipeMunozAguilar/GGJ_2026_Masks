using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaskOverlayAnimator : MonoBehaviour
{
    public Image overlayImage;       
    public Animator overlayAnimator; 

    private bool isAnimating = false;
    private bool maskActive = false;
    private Color currentColor = Color.black;
    private string currentMaskTipo = ""; // nuevo: guardar tipo actual

    public void ToggleMask(Color maskColor, string tipo)
    {
        if (isAnimating) return;

        if (!maskActive)
        {
            // No hay máscara → poner nueva
            currentColor = maskColor;
            currentMaskTipo = tipo;

            overlayImage.color = maskColor;
            overlayImage.gameObject.SetActive(true);

            StartCoroutine(PlayAnimation("MaskIn", true));
        }
        else
        {
            if (currentMaskTipo == tipo)
            {
                // Misma máscara → quitar
                overlayImage.color = currentColor;
                overlayImage.gameObject.SetActive(true);

                StartCoroutine(PlayAnimation("MaskOut", false));
            }
            else
            {
                // Otra máscara distinta → cambiar directamente con MaskIn
                currentColor = maskColor;
                currentMaskTipo = tipo;

                overlayImage.color = maskColor;
                overlayImage.gameObject.SetActive(true);

                StartCoroutine(PlayAnimation("MaskIn", true));
            }
        }
    }

    IEnumerator PlayAnimation(string triggerName, bool activating)
    {
        isAnimating = true;

        overlayAnimator.ResetTrigger("MaskIn");
        overlayAnimator.ResetTrigger("MaskOut");
        overlayAnimator.SetTrigger(triggerName);

        float length = GetAnimationClipLength(triggerName);
        yield return new WaitForSeconds(length);

        if (activating)
        {
            maskActive = true;
            overlayImage.gameObject.SetActive(false);
        }
        else
        {
            maskActive = false;
            currentMaskTipo = "";
            overlayImage.gameObject.SetActive(false);
        }

        isAnimating = false;
    }

    public float GetAnimationClipLength(string stateName)
    {
        foreach (var clip in overlayAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == stateName)
                return clip.length;
        }
        return 0.5f;
    }

    public bool IsAnimating() => isAnimating;
    public bool IsMaskActive() => maskActive;
    public string CurrentMaskTipo() => currentMaskTipo;
}
