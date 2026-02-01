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
        SampleScene = 0,
        TriggerHumanWhenGameOver = 1,
        GameOver = 2,
        Menu = 3,
    }



    public IEnumerator BlackoutSequence(Vector3 blakcoutHolePosition)
    {
        Debug.Log("CountdownController: Starting blackout sequence.");
        GameObject blackout = Instantiate(blackoutPrefab);
        Transform blackoutHole = blackout.transform.GetChild(0);

        yield return new WaitForSeconds(blackoutDuration);

        blackoutHole.gameObject.SetActive(true);
        // convertir la posición world del humano a local respecto al padre del agujero
        Vector3 localPos = (blackoutHole.parent != null)
            ? blackoutHole.parent.InverseTransformPoint(blakcoutHolePosition)
            : blackout.transform.InverseTransformPoint(blakcoutHolePosition);

        blackoutHole.localPosition = localPos;


        Debug.Log("La posicion del global del agujero es: " + blackoutHole.position);
        Debug.Log("La posicion del local del agujero  es: " + blackoutHole.localPosition);

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

    public void PlayTutorial()
    {
        ChangeScene(SceneIndex.SampleScene);
    }

    public void PlayGame()
    {
        ChangeScene(SceneIndex.TriggerHumanWhenGameOver);
    }

    public void ChangeScene(SceneIndex sceneIndex)
    {
        SceneManager.LoadScene((int)sceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

}
