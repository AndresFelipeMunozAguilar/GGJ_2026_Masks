using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float blackoutDuration = 2f;

    public float animationDuration = 2f;

    [SerializeField]
    private GameObject blackoutPrefab;

    public enum SceneIndex
    {
        Menu = 0,
        Principal = 1,
        GameOver = 2,
        SampleScene = 3,
        TriggerHumanWhenGameOver = 4,
        Alejandro = 5,
        UpdateCoundown = 6,
        Tutorial = 7,
        Win = 8,
    }

    public IEnumerator BlackoutSequence(Vector3 blakcoutHolePosition)
    {
        Debug.Log("CountdownController: Starting blackout sequence.");
        GameObject blackout = Instantiate(blackoutPrefab);
        Transform blackoutHole = blackout.transform.GetChild(0);

        //Sacar por consola que se va a iniciar el blackout
        Debug.Log($"CountdownController: Blackout iniciado y va a esperar {blackoutDuration} segs.");
        yield return new WaitForSeconds(blackoutDuration);

        blackoutHole.gameObject.SetActive(true);

        blackoutHole.position = blakcoutHolePosition;

        // Sacar por consola que se activo el hueco en el blakcout
        Debug.Log($"CountdownController: Blackout hole activated. Instantiatied in position {blakcoutHolePosition}. Now waiting {animationDuration} segs for game over animation.");

        // Indicar que se va a esperar la duracion de la animacion de game over

        Debug.Log($"CountdownController: Waiting for game over animation duration of {animationDuration} seconds.");

        yield return new WaitForSeconds(animationDuration);

        Debug.Log("CountdownController: Blackout sequence completed.");

        Destroy(blackout);
    }

    public IEnumerator FinalBlackoutGameOverSequence()
    {
        FindFirstObjectByType<FadeInImagesGroup>().FadeIn();

        // Esperar la duración del backout y la animación
        yield return new WaitForSeconds(blackoutDuration + animationDuration);

        // Cambiar a la pantalla de Game Over
        ChangeScene(SceneIndex.GameOver);
    }

    public IEnumerator FinalWinSequence()
    {
        // Esta función se llama justo después del blackout y de haber esperado el fundido en negro, se espera la animación
        yield return new WaitForSeconds(animationDuration);

        // Desactivar el orbe
        CountdownController countdownController = FindFirstObjectByType<CountdownController>();
        countdownController.DisableOrb();

        // Instanciar el prefab de blackout y cambiarle el color a blanco
        GameObject blackout = Instantiate(blackoutPrefab);
        SpriteRenderer sr = blackout.GetComponent<SpriteRenderer>();
        sr.color = Color.white;

        // Esperar un segundo
        yield return new WaitForSeconds(1f);

        // Cambiar a la pantalla de victoria
        ChangeScene(SceneIndex.Win);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(SceneIndex sceneIndex)
    {
        SceneManager.LoadScene((int)sceneIndex);
    }

}
