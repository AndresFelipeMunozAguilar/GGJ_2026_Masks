using UnityEngine;

public class HumanGameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject spiritGem;

    [SerializeField]
    private float radius = 5f; // Adjust this value as needed

    [SerializeField]
    private float angle = 45f; // Adjust this value as needed (in degrees)

    public void TimeIsOver()
    {
        // Encontrar la posición del Spirit Gem y
        // calcular la nueva posición del humano
        // pasando los grados a radianes
        Vector3 spiritGemPosition = spiritGem.transform.position;
        float radianAngle = angle * Mathf.Deg2Rad;

        // Calcular la nueva posición usando la coordenadas polares
        Vector3 offset = new Vector3(Mathf.Cos(radianAngle) * radius, Mathf.Sin(radianAngle) * radius, 0f);
        Vector3 positionHuman = spiritGemPosition + offset;

        // Spawnear el humano en la nueva posición
        transform.position = positionHuman;
    }
}