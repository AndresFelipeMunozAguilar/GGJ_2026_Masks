using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float blackoutDuration = 2f;
    public float animationDuration = 2f;
    public Boolean isAnimationPlaying = false;

    [SerializeField]
    private GameObject blackoutPrefab;

    public enum SceneIndex
    {
        Menu = 0,
        Principal = 1,
        GameOver = 2,
        SampleScene = 3,
        TriggerHumanWhenGameOver = 4,
        Alejandro = 5,
        UpdateCoundown = 6,
        Tutorial = 7,
        Win = 8,
        Nivel2 = 9  
    }

    public IEnumerator BlackoutSequence(Vector3 blakcoutHolePosition)
    {
        Debug.Log("CountdownController: Starting blackout sequence.");
        GameObject blackout = Instantiate(blackoutPrefab);
        Transform blackoutHole = blackout.transform.GetChild(0);
        isAnimationPlaying = true;

        Debug.Log($"CountdownController: Blackout iniciado y va a esperar {blackoutDuration} segs.");
        yield return new WaitForSeconds(blackoutDuration);

        blackoutHole.gameObject.SetActive(true);
        blackoutHole.position = blakcoutHolePosition;

        Debug.Log($"CountdownController: Blackout hole activated. Instantiated in position {blakcoutHolePosition}. Now waiting {animationDuration} segs for game over animation.");
        Debug.Log($"CountdownController: Waiting for game over animation duration of {animationDuration} seconds.");

        yield return new WaitForSeconds(animationDuration);

        Debug.Log("CountdownController: Blackout sequence completed.");

        Destroy(blackout);
        isAnimationPlaying = false;
    }

    public IEnumerator FinalBlackoutGameOverSequence()
    {
        while (isAnimationPlaying)
        {
            yield return new WaitForSeconds(0.75f);
        }

        CountdownController countdownController = FindFirstObjectByType<CountdownController>();
        countdownController.DisableOrb();

        FindFirstObjectByType<FadeInImagesGroup>().FadeIn();

        yield return new WaitForSeconds(blackoutDuration + animationDuration);

        ChangeScene(SceneIndex.GameOver);
    }

    public IEnumerator FinalWinSequence()
    {
        yield return new WaitForSeconds(animationDuration);

        isAnimationPlaying = true;

        CountdownController countdownController = FindFirstObjectByType<CountdownController>();
        countdownController.DisableOrb();

        GameObject blackout = Instantiate(blackoutPrefab);
        SpriteRenderer sr = blackout.GetComponent<SpriteRenderer>();
        sr.color = Color.white;

        yield return new WaitForSeconds(1f);

        // 👇 lógica nueva: decidir si ir a Nivel2 o a Win
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Principal")
        {
            ChangeScene(SceneIndex.Nivel2);
        }
        else if (currentSceneName == "Nivel2")
        {
            ChangeScene(SceneIndex.Win);
        }
        else
        {
            // fallback: por defecto ir a Win
            ChangeScene(SceneIndex.Win);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeScene(SceneIndex sceneIndex)
    {
        SceneManager.LoadScene((int)sceneIndex);
    }
}
