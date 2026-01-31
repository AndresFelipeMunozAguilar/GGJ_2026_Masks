using UnityEngine;

public class CountdownSwitch : MonoBehaviour
{
    [SerializeField]
    private CountdownController countdownController;

    public void StartCountdown()
    {
        countdownController.enabled = true;
    }

    public void StopCountdown()
    {
        countdownController.enabled = false;
    }
}
