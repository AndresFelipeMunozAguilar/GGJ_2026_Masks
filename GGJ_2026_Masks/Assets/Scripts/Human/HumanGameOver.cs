using UnityEngine;

public class HumanGameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject spiritGem;

    public float radius = 5f; // Adjust this value as needed

    public float angle = 45f; // Adjust this value as needed (in degrees)

    public void TimeIsOver()
    {
        Debug.Log("HumanGameOver: Time is over. Triggering game over sequence.");

        Vector3 spiritGemPosition = spiritGem.transform.position;
        float radianAngle = angle * Mathf.Deg2Rad;


        Vector3 offset = new Vector3(Mathf.Cos(radianAngle) * radius, Mathf.Sin(radianAngle) * radius, 0f);
        transform.position = spiritGemPosition + offset;

    }
}