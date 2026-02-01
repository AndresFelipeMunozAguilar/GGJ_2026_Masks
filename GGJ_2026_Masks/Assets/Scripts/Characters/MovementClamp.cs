using UnityEngine;

public class MovementClamp : MonoBehaviour
{

    [Header("Límites de Movimiento")]
    public Vector2 minBounds = new Vector2(-10f, -10f);
    public Vector2 maxBounds = new Vector2(10f, 10f);

    void LateUpdate()
    {
        // Obtenemos la posición actual después de que otros scripts actuaran
        Vector3 currentPos = transform.position;

        // "Clampeamos" o restringimos los valores
        float clampedX = Mathf.Clamp(currentPos.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(currentPos.y, minBounds.y, maxBounds.y);

        // Aplicamos la posición final (manteniendo la Z original)
        transform.position = new Vector3(clampedX, clampedY, currentPos.z);
    }

}
