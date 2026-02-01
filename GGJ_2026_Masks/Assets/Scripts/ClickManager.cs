using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ClickManager : MonoBehaviour
{
    private Camera mainCam;
    public MaskOverlayAnimator overlayAnimator; // para bloquear inputs globales

    private bool isBlocked = false;

    private CountdownController countdownController;

    private Movement[] allMovements;
    [Header("Sonido de click en personajes")] public AudioClip clickSound; 
    private AudioSource audioSource; 

    void Awake() { 
    mainCam = Camera.main; // Crear o recuperar 
    AudioSource audioSource = GetComponent<AudioSource>(); 
    if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>(); 
    audioSource.playOnAwake = false; }


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
                PlayClickSound();

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

    private void PlayClickSound() { 

        if (clickSound != null && audioSource != null) { 
            audioSource.PlayOneShot(clickSound); 
            } 
        }

    IEnumerator HandleReveal(SpiritRevealController reveal, Color impostorColor)
    {
        isBlocked = true;
        // Desactivar otros personajes mientras se revela este
        DisableOtherCharacters(reveal.gameObject);
        // Iniciar el enfoque del personaje
        yield return FocusCharacter(reveal.transform.position);

        reveal.Reveal(impostorColor);

        // Lanzar la secuencia de victoria
        StartCoroutine(GameManager.Instance.FinalWinSequence());
    }

    IEnumerator HandleFail(SpiritRevealController reveal, Movement movement)
    {
        isBlocked = true;
        // Desactivar otros personajes mientras se revela este
        DisableOtherCharacters(reveal.gameObject);
        
        // Iniciar el enfoque del personaje
        yield return FocusCharacter(reveal.transform.position);

        reveal.FailReveal();

        // esperar hasta que termine la animación de fallo
        while (reveal.IsFailing())
            yield return null;

        // 👇 Reanudar movimiento del espíritu normal
        if (movement != null) movement.ResumeMovement();

        isBlocked = false;
        // Reactivar otros personajes
        EnableOtherCharacters();
    }

    private IEnumerator FocusCharacter(Vector3 position)
    {
        // Iniciar la secuencia de blackout desde el GameManager
        GameManager gameManager = GameManager.Instance;
        StartCoroutine(gameManager.BlackoutSequence(position));
        // Esperar el timeout antes de que se active el hueco
        yield return new WaitForSeconds(gameManager.blackoutDuration);
    }

    private void DisableOtherCharacters(GameObject characterToFocus)
    {
        // Desactivar todos los personajes excepto el que se está revelando
        Movement[] allMovements = FindObjectsByType<Movement>(FindObjectsSortMode.None);
        this.allMovements = allMovements;
        foreach (var movement in allMovements)
        {
            GameObject characterObj = movement.gameObject;
            // Verificar que no sea el movimiento del personaje que se está revelando
            if (characterObj == characterToFocus) continue;
            characterObj.SetActive(false);
        }
    }

    private void EnableOtherCharacters()
    {
        // Reactivar todos los personajes
        foreach (var movement in allMovements)
        {
            movement.gameObject.SetActive(true);
        }
    }
}
