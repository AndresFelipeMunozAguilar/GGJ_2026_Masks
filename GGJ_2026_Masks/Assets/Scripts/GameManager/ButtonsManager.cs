using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Obtener la instancia del GameManager
        gameManager = GameManager.Instance;
    }

    public void PlayTutorial()
    {
        gameManager.ChangeScene(GameManager.SceneIndex.SampleScene);
    }

    public void PlayGame()
    {
        gameManager.ChangeScene(GameManager.SceneIndex.Principal);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
