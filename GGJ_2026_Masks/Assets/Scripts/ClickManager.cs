using UnityEngine;
using UnityEngine.InputSystem; 

public class ClickManager : MonoBehaviour
{
    private Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = mainCam.ScreenPointToRay(mousePos);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                ObjectByType obj = hit.collider.GetComponent<ObjectByType>();
                if (obj != null)
                {
                    if (string.IsNullOrEmpty(obj.tipo) || obj.tipo == "none")
                    {
                        Debug.Log("AAAA HUMANOOOOOO");
                    }
                    else
                    {
                        Debug.Log("Has hecho click en un espíritu de tipo: " + obj.tipo);
                    }
                }
            }
        }
    }
}
