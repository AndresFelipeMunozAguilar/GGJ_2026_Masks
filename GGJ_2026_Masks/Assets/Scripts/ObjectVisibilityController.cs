using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectVisibilityController : MonoBehaviour
{
    private string currentFilter = "";
    private float lastChangeTime = -1f;
    public float cooldown = 0.5f; 

    void Update()
    {
        if (Keyboard.current == null) return;

       
        if (Time.time - lastChangeTime < cooldown) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            ToggleFilter("Tipo1");
            lastChangeTime = Time.time;
        }

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            ToggleFilter("Tipo2");
            lastChangeTime = Time.time;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleFilter("Tipo3");
            lastChangeTime = Time.time;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ShowAll();
            lastChangeTime = Time.time;
        }
    }

    void ToggleFilter(string tipo)
    {
        if (currentFilter == tipo)
        {
            ShowAll();
        }
        else
        {
            EventManager.OnFilterChanged.Invoke(tipo);
            currentFilter = tipo;
        }
    }

    void ShowAll()
    {
        EventManager.OnFilterChanged.Invoke("ALL");
        currentFilter = "";
    }
}
