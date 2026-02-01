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

                        // Obtener el controlador de revelado
                        var reveal = hit.collider.GetComponent<SpiritRevealController>();
                        if (reveal == null)
                        {
                            // intentar en los hijos por si el script está en el root o en otro GameObject
                            reveal = hit.collider.GetComponentInParent<SpiritRevealController>();
                        }

                        // Determinar color: si ya existe uno guardado, usarlo; si no, generar y guardar
                        Color colorToUse = obj.impostorColor;
                        if (obj.isImpostor && colorToUse != Color.white)
                        {
                            // ya está definido, usarlo
                        }
                        else
                        {
                            // generar color aleatorio entre los 3 hex (amarillo, azul, rojo)
                            int r = Random.Range(1, 4);
                            switch (r)
                            {
                                case 1: colorToUse = HexToColor("FFF700"); break;
                                case 2: colorToUse = HexToColor("00FFFC"); break;
                                case 3: colorToUse = HexToColor("FF0020"); break;
                            }
                            obj.impostorColor = colorToUse;
                            obj.isImpostor = true;
                            obj.tipo = "Impostor";
                        }

                        if (reveal != null && !reveal.IsRevealed())
                        {
                            // prevenir nuevos clicks
                            var col = hit.collider.GetComponent<Collider2D>();
                            if (col != null) col.enabled = false;

                            // aplicar color inmediatamente (opcional) y reproducir la animación de revelado
                            reveal.ForceSetImpostorColor(colorToUse);
                            reveal.Reveal(colorToUse);
                        }
                    }
                    else
                    {
                        Debug.Log("Has hecho click en un espíritu de tipo: " + obj.tipo);
                    }
                }
            }
        }
    }

    Color HexToColor(string hex)
    {
        if (!hex.StartsWith("#")) hex = "#" + hex;
        if (ColorUtility.TryParseHtmlString(hex, out Color c)) return c;
        return Color.white;
    }
}
