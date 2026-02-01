using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    [SerializeField]
    private HumanGameOver humanGameOver;

    // Referencia del objeto Spawner para desactivarlo
    [SerializeField]
    private Spawner spawner;

    [SerializeField]
    private Image countdownOrb;

    private float currentTime = 0f;

    // El tiempo está en segundos
    [SerializeField]
    private float maxTime = 10f;

    [SerializeField]
    private GameManager gameManager;

    public void Start()
    {
        // Debug.Log("Encontrando GameManager en CountdownController OnEnable");
        currentTime = 0f;
        gameManager = GameManager.Instance;
        humanGameOver = FindFirstObjectByType<HumanGameOver>();
    }

    public void Update()
    {
        if (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            countdownOrb.fillAmount = currentTime / maxTime;
        }
        else
        {
            CountdownIsOver();
        }
    }

    public void CountdownIsOver()
    {
        Debug.Log("COUNTDOWN SAYS: TIME IS OVER");
        currentTime = maxTime;

        humanGameOver.TimeIsOver();

        // Un vector con una posicion cualquiera
        Vector3 blackoutHolePosition = new Vector3(0f, 0f, 0f);

        StartCoroutine(gameManager.BlackoutSequence(blackoutHolePosition));

        gameManager.ChangeScene(GameManager.SceneIndex.GameOver);

        // Desactivar el spawner y por tanto todos los demás personajes
        spawner.gameObject.SetActive(false);        

        // Desactivar a los hijos del countdown para que no se vea mas
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // Desactivar este script para evitar múltiples llamadas
        this.enabled = false;
    }

}
