using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ClickManager : MonoBehaviour
{
    private Camera mainCam;
    public MaskOverlayAnimator overlayAnimator; // para bloquear inputs globales

    private bool isBlocked = false;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        // Bloquear inputs si hay animación de overlay o penalización activa
        if ((overlayAnimator != null && overlayAnimator.IsAnimating()) || isBlocked)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = mainCam.ScreenPointToRay(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                ObjectByType obj = hit.collider.GetComponent<ObjectByType>();
                if (obj != null)
                {
                    var reveal = hit.collider.GetComponent<SpiritRevealController>();
                    if (reveal == null)
                        reveal = hit.collider.GetComponentInParent<SpiritRevealController>();

                    if (obj.tipo == "")
                    {
                        Debug.Log("¡Encontraste un humano impostor!");

                        if (reveal != null && !reveal.IsRevealed())
                        {
                            var col = hit.collider.GetComponent<Collider2D>();
                            if (col != null) col.enabled = false;

                            reveal.ForceSetImpostorColor(obj.impostorColor);
                            StartCoroutine(HandleReveal(reveal, obj.impostorColor));
                        }
                    }
                    else
                    {
                        Debug.Log("¡Error! Tocaste un espíritu normal, pierdes tiempo...");
                        if (reveal != null && !reveal.IsFailing())
                        {
                            StartCoroutine(HandleFail(reveal));
                        }
                    }
                }
            }
        }
    }

    IEnumerator HandleReveal(SpiritRevealController reveal, Color impostorColor)
    {
        isBlocked = true;
        reveal.Reveal(impostorColor);

        // esperar hasta que termine la animación de revelado
        while (reveal.IsRevealed() == false)
            yield return null;

        isBlocked = false;
    }

    IEnumerator HandleFail(SpiritRevealController reveal)
    {
        isBlocked = true;
        reveal.FailReveal();

        // esperar hasta que termine la animación de fallo
        while (reveal.IsFailing())
            yield return null;

        isBlocked = false;
    }
}
