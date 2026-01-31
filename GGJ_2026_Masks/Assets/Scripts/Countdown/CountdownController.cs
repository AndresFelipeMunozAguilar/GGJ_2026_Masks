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
        humanGameOver.TimeIsOver();
        enabled = false;
    }
}
