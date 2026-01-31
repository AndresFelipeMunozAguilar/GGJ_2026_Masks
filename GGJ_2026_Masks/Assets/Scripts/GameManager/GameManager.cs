using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum SceneIndex
    {
        SampleScene = 0,
        TriggerThiefWhenGameOver = 1,
        GameOver = 2
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


    public void LoadScene(SceneIndex sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)sceneIndex);
    }

}
