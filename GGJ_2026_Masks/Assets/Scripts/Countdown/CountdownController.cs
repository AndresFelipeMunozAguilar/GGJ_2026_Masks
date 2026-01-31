using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    [SerializeField]
    private HumanGameOver humanGameOver;

    [SerializeField]
    private Image countdownBar;

    private float timeRemaining;

    // El tiempo está en segundos
    [SerializeField]
    private float maxTime = 10f;

    [SerializeField]
    private GameObject blackoutPrefab;

    [SerializeField]
    private float blackoutDuration = 2f;
    public IEnumerator BlackoutSequence()
    {
        GameObject blackout = Instantiate(blackoutPrefab);
        Transform blackoutHole = blackout.transform.GetChild(0);

        yield return new WaitForSeconds(blackoutDuration);

        blackoutHole.gameObject.SetActive(true);
        blackoutHole.position = humanGameOver.gameObject.transform.position;

        yield return new WaitForSeconds(gameOverAnimationDuration);

        Debug.Log("CountdownController: Blackout sequence completed.");

    }
    public void Start()
    {
        timeRemaining = maxTime;
    }

    public void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            countdownBar.fillAmount = timeRemaining / maxTime;
        }
        else
        {
            CountdownIsOver();
        }
    }

    public void CountdownIsOver()
    {
        Debug.Log("COUNTDOWN SAYS: TIME IS OVER");
        timeRemaining = 0;
        StartCoroutine(BlackoutSequence());
        humanGameOver.TimeIsOver();
        enabled = false;
    }
}
