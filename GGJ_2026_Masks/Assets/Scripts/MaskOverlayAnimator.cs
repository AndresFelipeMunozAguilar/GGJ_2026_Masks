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

    public void ToggleMask(Color maskColor)
    {
        if (isAnimating) return;

        if (!maskActive)
        {
            // Poner máscara
            currentColor = maskColor;

            // Importante: no mostrar el overlay sólido de inmediato
            overlayImage.color = maskColor;
            overlayImage.gameObject.SetActive(true);

            StartCoroutine(PlayAnimation("MaskIn", true));
        }
        else
        {
            // Quitar máscara
            overlayImage.color = currentColor;
            overlayImage.gameObject.SetActive(true);

            StartCoroutine(PlayAnimation("MaskOut", false));
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
            // Al terminar MaskIn → marcar que hay máscara activa
            maskActive = true;
            // Desactivar el overlay para que no quede pintando la pantalla
            overlayImage.gameObject.SetActive(false);
        }
        else
        {
            // Al terminar MaskOut → máscara desactivada
            maskActive = false;
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
}
