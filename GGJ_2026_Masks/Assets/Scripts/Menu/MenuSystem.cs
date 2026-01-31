using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Play()
    {
        Debug.Log("Iniciando el juego...");
        // TODO: Implementar la carga de la escena del juego
    }
    public void Quit()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
