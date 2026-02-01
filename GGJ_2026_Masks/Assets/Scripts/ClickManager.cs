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
                var movement = hit.collider.GetComponent<Movement>();
                if (movement == null) movement = hit.collider.GetComponentInParent<Movement>();

                ObjectByType obj = hit.collider.GetComponent<ObjectByType>();
                if (obj != null)
                {
                    var reveal = hit.collider.GetComponent<SpiritRevealController>();
                    if (reveal == null)
                        reveal = hit.collider.GetComponentInParent<SpiritRevealController>();

                    if (obj.isImpostor)
                    {
                        // Impostor → pausar movimiento y NO reanudar
                        if (movement != null) movement.PauseMovement();
                        Debug.Log("¡Humano impostor encontrado!");

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
                        // Espíritu normal → pausar movimiento mientras dura la animación y luego reanudar
                        Debug.Log("Espíritu normal tocado");
                        if (movement != null) movement.PauseMovement();

                        if (reveal != null && !reveal.IsFailing())
                        {
                            StartCoroutine(HandleFail(reveal, movement));
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
        while (!reveal.IsRevealed())
            yield return null;

        // 👇 No reanudar movimiento del impostor
        isBlocked = false;

        // TODO Hacer fundido en blanco 

        // Navegar a la escena de victoria
        GameManager.Instance.ChangeScene(GameManager.SceneIndex.Win);
    }

    IEnumerator HandleFail(SpiritRevealController reveal, Movement movement)
    {
        // TODO: LLamar a la pantalla negra con el hueco para enfocar al personaje

        isBlocked = true;
        reveal.FailReveal();

        // esperar hasta que termine la animación de fallo
        while (reveal.IsFailing())
            yield return null;

        // 👇 Reanudar movimiento del espíritu normal
        if (movement != null) movement.ResumeMovement();

        isBlocked = false;
    }
}
