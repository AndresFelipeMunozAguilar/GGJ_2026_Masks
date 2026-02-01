using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ObjectVisibilityController : MonoBehaviour
{
    private string currentFilter = "";
    private float lastChangeTime = -1f;
    public float cooldown = 0.5f; 
    
    public MaskOverlayAnimator overlayAnimator;

    [Header("Tiempo de adelanto para aplicar filtro (segundos)")]
    public float filterAdvanceTime = 0.05f; 

    void Update()
    {
        if (Keyboard.current == null) return;
        if (Time.time - lastChangeTime < cooldown) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            lastChangeTime = Time.time;
            StartCoroutine(ApplyFilterWithAnimation("Tipo1", HexToColor("FFF700")));
        }

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            lastChangeTime = Time.time;
            StartCoroutine(ApplyFilterWithAnimation("Tipo2", HexToColor("00FFFC")));
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            lastChangeTime = Time.time;
            StartCoroutine(ApplyFilterWithAnimation("Tipo3", HexToColor("FF0020")));
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame) 
        { 
            if (!string.IsNullOrEmpty(currentFilter)) 
            { 
                lastChangeTime = Time.time;
                StartCoroutine(RemoveMaskWithAnimation());
            } 
        }
    }

    IEnumerator ApplyFilterWithAnimation(string tipo, Color maskColor)
    {
        overlayAnimator.PlayMask(maskColor);

        float length = overlayAnimator.GetAnimationClipLength("MaskInState");
        yield return new WaitForSeconds(Mathf.Max(0, length - filterAdvanceTime));

        ToggleFilter(tipo);
    }

    IEnumerator RemoveMaskWithAnimation()
    {
        overlayAnimator.PlayMaskReverse();

        float length = overlayAnimator.GetAnimationClipLength("MaskOutState");
        yield return new WaitForSeconds(Mathf.Max(0, length - filterAdvanceTime));

        ShowAll();
    }

    void ToggleFilter(string tipo)
    {
        if (currentFilter == tipo)
        {
            ShowAll();
        }
        else
        {
            EventManager.OnFilterChanged.Invoke(tipo);
            currentFilter = tipo;
        }
    }

    void ShowAll()
    {
        EventManager.OnFilterChanged.Invoke("ALL");
        currentFilter = "";
    }

    Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
            return color;
        return Color.white;
    }
}
