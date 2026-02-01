using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private float blackoutDuration = 2f;

    [SerializeField]
    private float gameOverAnimationDuration = 2f;

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
        Debug.Log($"CountdownController: Blackout hole activated. Instantiatied in position {blakcoutHolePosition}. Now waiting {gameOverAnimationDuration} segs for game over animation.");

        // Indicar que se va a esperar la duracion de la animacion de game over

        Debug.Log($"CountdownController: Waiting for game over animation duration of {gameOverAnimationDuration} seconds.");

        yield return new WaitForSeconds(gameOverAnimationDuration);

        Debug.Log("CountdownController: Blackout sequence completed.");
        ChangeScene(GameManager.SceneIndex.GameOver);
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
