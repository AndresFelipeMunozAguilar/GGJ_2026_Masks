using UnityEngine;

public class MovementClamp : MonoBehaviour
{

    [Header("Límites de Movimiento Externos")]
    public Vector2 minBounds = new Vector2(-10f, -10f);
    public Vector2 maxBounds = new Vector2(10f, 10f);

    [Header("Límites de Movimiento Internos Excluidos")]
    public Vector2 innerExcludedBoundsMin = new Vector2(-3f, -4f);
    public Vector2 innerExcludedBoundsMax = new Vector2(3f, 2f);

    void LateUpdate()
    {
        // Obtenemos la posición actual después de que otros scripts actuaran
        Vector3 currentPos = transform.position;

        // "Clampeamos" o restringimos los valores
        float clampedX = Mathf.Clamp(currentPos.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(currentPos.y, minBounds.y, maxBounds.y);

        // Aplicamos la posición final (manteniendo la Z original)
        transform.position = new Vector3(clampedX, clampedY, currentPos.z);

        // Funcion para clampear la posicion y que excluya los valores entre (-1, 1) en X
        // y (-1, 1) en Y
        if ((currentPos.x > innerExcludedBoundsMin.x && currentPos.x < innerExcludedBoundsMax.x)
            && (currentPos.y > innerExcludedBoundsMin.y && currentPos.y < innerExcludedBoundsMax.y))
        {
            // Mover la posición del objeto fuera del área exluida, mediante
            // moverlo lentamente hacia afuera, en la misma direccion
            // que el vector que apunta desde este objeto hasta el centro del área excluida (0,0)
            Vector3 direction = new Vector3(
                currentPos.x > 0 ? 1 : -1,
                currentPos.y > 0 ? 1 : -1,
                0).normalized;
            float moveSpeed = 5f * Time.deltaTime;
            currentPos += direction * moveSpeed;
            clampedX = Mathf.Clamp(currentPos.x, minBounds.x, maxBounds.x);
            clampedY = Mathf.Clamp(currentPos.y, minBounds.y, maxBounds.y);

        }

        transform.position = new Vector3(clampedX, clampedY, currentPos.z);
    }

}
