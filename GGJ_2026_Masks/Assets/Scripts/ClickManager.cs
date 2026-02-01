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
            Debug.Log("Click detectado");
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

                    if (obj.isImpostor)
                    {
                        Debug.Log("HUMANO IMPOSTOR AAAAA");

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
                        Debug.Log("Espíritu normal");
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
        // TODO: LLamar a la pantalla negra con el hueco para enfocar al personaje

        reveal.Reveal(impostorColor);

        // esperar hasta que termine la animación de revelado
        while (reveal.IsRevealed() == false)
            yield return null;

        isBlocked = false;

        // TODO Hacer fundido en blanco 

        // Navegar a la escena de victoria
        GameManager.Instance.ChangeScene(GameManager.SceneIndex.Win);
    }

    IEnumerator HandleFail(SpiritRevealController reveal)
    {
        // TODO: LLamar a la pantalla negra con el hueco para enfocar al personaje

        isBlocked = true;
        reveal.FailReveal();

        // esperar hasta que termine la animación de fallo
        while (reveal.IsFailing())
            yield return null;

        isBlocked = false;
    }
}
