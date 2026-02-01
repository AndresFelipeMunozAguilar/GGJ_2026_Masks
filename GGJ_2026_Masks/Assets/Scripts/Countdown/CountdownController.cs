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

        // debug para identificar instancia y posiciones
        var t = humanGameOver.transform;
        Debug.Log($"Human name={humanGameOver.name} scene={humanGameOver.gameObject.scene.name} id={humanGameOver.GetInstanceID()} parent={(t.parent != null ? t.parent.name : "null")}");
        Debug.Log($"local={t.localPosition} world={t.position}");

        Vector3 blackoutHolePosition = t.position;

        StartCoroutine(gameManager.BlackoutSequence(blackoutHolePosition));

        enabled = false;
    }

}
