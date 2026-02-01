using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    public float crossfadeDuration = 0.6f;

    int currentMaskIndex = -1;
    Color[] originalColors;

    AudioSource sourceA;
    AudioSource sourceB;
    bool aIsActive = true;
    float[] clipPositions = new float[4]; 
    Coroutine fadeCoroutine;

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

        if (audioSource != null)
        {
            sourceA = audioSource;
            sourceB = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            sourceA = gameObject.AddComponent<AudioSource>();
            sourceB = gameObject.AddComponent<AudioSource>();
        }

        sourceA.loop = true;
        sourceB.loop = true;
        sourceA.playOnAwake = false;
        sourceB.playOnAwake = false;
        sourceA.volume = 1f;
        sourceB.volume = 0f;

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
        StartCoroutine(EnsureInitialClipPlaying());
    }

    IEnumerator EnsureInitialClipPlaying()
    {
        yield return null;
        PlayClipForIndex(-1, immediate: true);
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
        //if (overlayAnimator != null && overlayAnimator.IsAnimating()) return;

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

    AudioClip GetClipByMaskIndex(int maskIndex)
    {
        switch (maskIndex)
        {
            case -1: return clipNone;
            case 0: return clipTipo1;
            case 1: return clipTipo2;
            case 2: return clipTipo3;
            default: return null;
        }
    }

    void PlayClipForIndex(int maskIndex, bool immediate = false)
    {
        AudioClip targetClip = GetClipByMaskIndex(maskIndex);
        int clipArrayIndex = maskIndex + 1; 

        if (targetClip == null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            sourceA.Stop();
            sourceB.Stop();
            return;
        }

        AudioSource active = aIsActive ? sourceA : sourceB;
        AudioSource inactive = aIsActive ? sourceB : sourceA;

        if (active.clip == targetClip && active.isPlaying)
        {
            return;
        }

        if (active.clip != null)
        {
            clipPositions[GetClipIndexFromClip(active.clip)] = active.time;
        }

        float resumeTime = clipPositions[clipArrayIndex];

        inactive.clip = targetClip;
        try { inactive.time = resumeTime; } catch { inactive.time = 0f; }
        inactive.Play();

        if (immediate || crossfadeDuration <= 0f)
        {
            active.volume = 0f;
            inactive.volume = 1f;
            active.Stop();
            aIsActive = !aIsActive;
            return;
        }

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(Crossfade(active, inactive, crossfadeDuration));
    }

    int GetClipIndexFromClip(AudioClip clip)
    {
        if (clip == clipNone) return 0;
        if (clip == clipTipo1) return 1;
        if (clip == clipTipo2) return 2;
        if (clip == clipTipo3) return 3;
        return 0;
    }

    IEnumerator Crossfade(AudioSource from, AudioSource to, float duration)
    {
        float t = 0f;
        float fromStart = from.volume;
        float toStart = to.volume;
        while (t < duration)
        {
            t += Time.deltaTime;
            float f = Mathf.Clamp01(t / duration);
            from.volume = Mathf.Lerp(fromStart, 0f, f);
            to.volume = Mathf.Lerp(toStart, 1f, f);
            yield return null;
        }
        from.volume = 0f;
        to.volume = 1f;
        from.Stop();
        aIsActive = !aIsActive;
        fadeCoroutine = null;
    }
}
