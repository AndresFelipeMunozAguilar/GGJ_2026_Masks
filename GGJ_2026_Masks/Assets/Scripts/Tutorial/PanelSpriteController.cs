using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class PanelSpriteController : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField]
    private Image imagenPanel;

    [SerializeField]
    private Button botonCambio;

    [Header("Configuración de Sprites")]
    [SerializeField]
    private List<Sprite> listaSprites;

    private int indiceActual = 0;

    [Header("Configuración de Tiempo")]
    [SerializeField]
    private float tiempoEntreCambios = 5f;

    [Header("Cambio de escena")]
    [SerializeField]
    private ButtonsManager buttonsManager;

    IEnumerator ActivarBotonTrasTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        botonCambio.interactable = true;
    }

    public void Start()
    {
        // Aseguramos que el botón empiece desactivado
        botonCambio.interactable = false;

        // Iniciamos la cuenta regresiva
        StartCoroutine(ActivarBotonTrasTiempo(tiempoEntreCambios));
    }


    public void ChangeNextSprite()
    {
        // Se verifica si hay elementos en la lista
        if (listaSprites.Count == 0) return;

        // Cambiamos el sprite y pasamos al siguiente índice (en bucle)
        indiceActual++;

        Debug.Log($"Vamos a comprobar si {indiceActual} es mayor que {listaSprites.Count}");
        if (indiceActual >= listaSprites.Count)
        {
            // Mostrar si se activó la condición
            Debug.Log("Se ha alcanzado el final de la lista de sprites.");

            indiceActual = 0;

            // Llamar a la función PlayGame ya que 
            // se han mostrado todos los sprites
            PlayGame();

            return;
        }

        // Mostrar el valor del índice actual
        Debug.Log("Índice actual: " + indiceActual);

        imagenPanel.sprite = listaSprites[indiceActual];
    }

    public void DeactivateButton()
    {
        botonCambio.interactable = false;

        StartCoroutine(ActivarBotonTrasTiempo(tiempoEntreCambios));
    }

    public void PlayGame()
    {
        Debug.Log("PanelSpriteController: Iniciando el juego desde el tutorial.");
        buttonsManager.PlayGame();

    }
}