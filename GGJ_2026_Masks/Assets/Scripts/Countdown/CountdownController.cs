using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    private GameObject blackoutPrefab;

    [SerializeField]
    private float blackoutDuration = 2f;

    [SerializeField]
    private float gameOverAnimationDuration = 2f;

    [SerializeField]
    private GameManager gameManager;

    public IEnumerator BlackoutSequence()
    {
        Debug.Log("CountdownController: Starting blackout sequence.");
        GameObject blackout = Instantiate(blackoutPrefab);
        Transform blackoutHole = blackout.transform.GetChild(0);

        yield return new WaitForSeconds(blackoutDuration);

        blackoutHole.gameObject.SetActive(true);
        blackoutHole.position = humanGameOver.gameObject.transform.position;

        yield return new WaitForSeconds(gameOverAnimationDuration);

        Debug.Log("CountdownController: Blackout sequence completed.");
        gameManager.ChangeScene(GameManager.SceneIndex.GameOver);
    }

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
        StartCoroutine(BlackoutSequence());
        humanGameOver.TimeIsOver();
        enabled = false;
    }

}
