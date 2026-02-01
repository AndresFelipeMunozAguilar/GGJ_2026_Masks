using UnityEngine;

public class SpriteSorterGroup : MonoBehaviour
{
    private SpriteRenderer[] renderers;

    void Awake()
    {
        // Obtiene todos los SpriteRenderer del objeto y sus hijos
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Calcula el orden según la posición en Y
        int order = Mathf.RoundToInt(-transform.position.y * 100);

        // Aplica el mismo orden a todos los SpriteRenderer del grupo
        foreach (var sr in renderers)
        {
            sr.sortingOrder = order;
        }
    }
}
