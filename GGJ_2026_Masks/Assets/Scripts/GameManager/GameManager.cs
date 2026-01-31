using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum SceneIndex
    {
        SampleScene = 0,
        TriggerHumanWhenGameOver = 1,
        GameOver = 2,
        Menu = 3,
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
