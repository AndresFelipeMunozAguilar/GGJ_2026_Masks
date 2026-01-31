using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    [SerializeField]
    private HumanGameOver humanGameOver;

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
        Vector3 blackoutHolePosition = humanGameOver.gameObject.transform.position;
        StartCoroutine(gameManager.BlackoutSequence(blackoutHolePosition));
        humanGameOver.TimeIsOver();
        enabled = false;
    }

}
