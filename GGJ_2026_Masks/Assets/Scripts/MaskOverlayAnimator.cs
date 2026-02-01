using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaskOverlayAnimator : MonoBehaviour
{
    public Image overlayImage;       
    public Animator overlayAnimator; 
    private bool isAnimating = false;
    private Color currentColor = Color.black;

    public void PlayMask(Color maskColor)
    {
        if (isAnimating) return;
        currentColor = maskColor;
        overlayImage.gameObject.SetActive(true);
        overlayImage.color = maskColor;
        StartCoroutine(PlayAnimation("MaskIn"));
    }

    public void PlayMaskReverse()
    {
        if (isAnimating) return;
        overlayImage.gameObject.SetActive(true);
        overlayImage.color = currentColor;
        StartCoroutine(PlayAnimation("MaskOut"));
    }

    IEnumerator PlayAnimation(string triggerName)
    {
        isAnimating = true;
        overlayAnimator.SetTrigger(triggerName);

        // Esperar la duración del clip
        float length = GetAnimationClipLength(triggerName);
        yield return new WaitForSeconds(length);

        // Siempre desactivar el overlay al terminar cualquier animación
        overlayImage.gameObject.SetActive(false);

        isAnimating = false;
    }

    public float GetAnimationClipLength(string stateName)
    {
        foreach (var clip in overlayAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == stateName)
                return clip.length;
        }
        return 0.5f; // fallback
    }

    public bool IsAnimating() => isAnimating;
}
