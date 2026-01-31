using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static UnityEvent<string> OnFilterChanged = new UnityEvent<string>();
}
