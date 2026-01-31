using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MaskButtonData
{
    public Button button;
    public Sprite closedSprite;
    public Sprite openSprite;
    public string hexColor;
}

public class MaskUIManager : MonoBehaviour
{
    public MaskButtonData[] masks = new MaskButtonData[3];
    public AudioSource audioSource;
    public AudioClip clipNone;
    public AudioClip clipTipo1;
    public AudioClip clipTipo2;
    public AudioClip clipTipo3;

    int currentMaskIndex = -1;
    Color[] originalColors;

    void OnEnable()
    {
        EventManager.OnFilterChanged.AddListener(HandleFilterChanged);
    }

    void OnDisable()
    {
        EventManager.OnFilterChanged.RemoveListener(HandleFilterChanged);
    }

    void Start()
    {
        originalColors = new Color[masks.Length];

        for (int i = 0; i < masks.Length; i++)
        {
            int idx = i;
            var data = masks[i];
            if (data.button != null)
            {
                data.button.onClick.AddListener(() => OnMaskButtonPressed(idx));
                Image icon = GetButtonIcon(data.button);
                if (icon != null)
                {
                    originalColors[i] = icon.color;
                    icon.sprite = data.closedSprite;
                }
            }
        }

        UpdateAllButtonIcons();
        PlayClipForIndex(-1);
    }

    void HandleFilterChanged(string filter)
    {
        if (filter == "ALL")
        {
            currentMaskIndex = -1;
        }
        else
        {
            currentMaskIndex = TipoToMaskIndex(filter);
        }
        UpdateAllButtonIcons();
        PlayClipForIndex(currentMaskIndex);
    }

    void OnMaskButtonPressed(int index)
    {
        if (currentMaskIndex == index)
        {
            currentMaskIndex = -1;
            EventManager.OnFilterChanged.Invoke("ALL");
        }
        else
        {
            currentMaskIndex = index;
            EventManager.OnFilterChanged.Invoke(MaskIndexToTipo(index));
        }

        UpdateAllButtonIcons();
        PlayClipForIndex(currentMaskIndex);
    }

    void UpdateAllButtonIcons()
    {
        for (int i = 0; i < masks.Length; i++)
        {
            var data = masks[i];
            if (data.button == null) continue;
            Image icon = GetButtonIcon(data.button);
            if (icon == null) continue;

            bool active = (i == currentMaskIndex);
            icon.sprite = (active && data.openSprite != null) ? data.openSprite : data.closedSprite;
            icon.color = active ? HexToColorSafe(data.hexColor) : originalColors[i];
        }
    }

    Image GetButtonIcon(Button b)
    {
        if (b == null) return null;
        Image img = b.GetComponent<Image>();
        if (img != null) return img;
        return b.GetComponentInChildren<Image>();
    }

    string MaskIndexToTipo(int index)
    {
        switch (index)
        {
            default:
            case 0: return "Tipo1";
            case 1: return "Tipo2";
            case 2: return "Tipo3";
        }
    }

    int TipoToMaskIndex(string tipo)
    {
        switch (tipo)
        {
            case "Tipo1": return 0;
            case "Tipo2": return 1;
            case "Tipo3": return 2;
            default: return -1;
        }
    }

    Color HexToColorSafe(string hex)
    {
        if (string.IsNullOrEmpty(hex)) return Color.white;
        if (!hex.StartsWith("#")) hex = "#" + hex;
        if (ColorUtility.TryParseHtmlString(hex, out Color c)) return c;
        return Color.white;
    }

    void PlayClipForIndex(int maskIndex)
    {
        if (audioSource == null) return;

        AudioClip clipToPlay = null;
        switch (maskIndex)
        {
            case -1: clipToPlay = clipNone; break;
            case 0: clipToPlay = clipTipo1; break;
            case 1: clipToPlay = clipTipo2; break;
            case 2: clipToPlay = clipTipo3; break;
        }

        if (clipToPlay == null)
        {
            audioSource.Stop();
            return;
        }

        if (audioSource.clip == clipToPlay && audioSource.isPlaying) return;

        audioSource.clip = clipToPlay;
        audioSource.Play();
    }
}
