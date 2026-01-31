using UnityEngine;

public class ObjectByType : MonoBehaviour
{
    public string tipo; 
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        EventManager.OnFilterChanged.AddListener(HandleFilter);
    }

    void OnDisable()
    {
        EventManager.OnFilterChanged.RemoveListener(HandleFilter);
    }

    void HandleFilter(string filter)
    {
        if (filter == "ALL")
        {
            SetAlpha(1f); // visible
        }
        else
        {
            SetAlpha(filter == tipo ? 1f : 0f);
        }
    }

    void SetAlpha(float a)
    {
        if (sr != null)
        {
            Color c = sr.color;
            c.a = a;
            sr.color = c;
        }
    }
}
