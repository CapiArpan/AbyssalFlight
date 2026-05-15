using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para el retraso (IEnumerator)

public class MenuManager : MonoBehaviour
{
    [Header("Configuración de Audio")]
    [SerializeField] private AudioSource fuenteEfectos;
    [SerializeField] private AudioClip sonidoClick;

    public void EmpezarJuego()
    {
        // 1. Sonido inmediato al tocar
        if (fuenteEfectos != null && sonidoClick != null)
        {
            fuenteEfectos.PlayOneShot(sonidoClick);
        }

        // 2. Iniciamos la espera para que se vea el efecto de "hundir" el botón
        StartCoroutine(RetrasoCarga());
    }

    IEnumerator RetrasoCarga()
    {
        // Esperamos 0.3 segundos (puedes ajustarlo según tu animación)
        yield return new WaitForSecondsRealtime(0.3f);

        // 3. Cargamos la escena (asegúrate de que el nombre sea idéntico)
        SceneManager.LoadScene("Scene_Game");
    }
}