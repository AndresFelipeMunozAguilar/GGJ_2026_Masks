using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour

{
    public AudioClip clickSound;
    public AudioSource audioSource;

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayClickSound);
    }

    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
            audioSource.PlayOneShot(clickSound);
    }
}
