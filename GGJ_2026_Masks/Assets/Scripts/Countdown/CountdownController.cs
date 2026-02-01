using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{

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

        StartCoroutine(gameManager.FinalBlackoutGameOverSequence());
    }

    public void DisableOrb()
    {
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
