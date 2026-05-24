using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Componente Visual Base")]
    [Tooltip("La UI Image principal que tiene el bloque completo de botones")]
    [SerializeField] private Image uiImagenMenuPrincipal;

    [Header("Sprites de los Estados")]
    [Tooltip("Imagen 1: Todos los botones apagados")]
    [SerializeField] private Sprite spriteTodoApagado;
    [Tooltip("Imagen 2: Botón INICIAR iluminado en magma")]
    [SerializeField] private Sprite spriteIniciarEncendido;
    [Tooltip("Imagen 3: Botón SCORE iluminado en magma")]
    [SerializeField] private Sprite spriteScoreEncendido;

    [Header("Configuración de Audio (SFX/BGM)")]
    [SerializeField] private AudioSource fuenteEfectos;
    [SerializeField] private AudioSource fuenteMusicaLobby;
    [SerializeField] private AudioClip sonidoIniciar;
    [SerializeField] private AudioClip sonidoClickComun;

    [Header("Paneles de Interfaz")]
    [SerializeField] private GameObject panelScore;

    void Start()
    {
        if (uiImagenMenuPrincipal != null && spriteTodoApagado != null)
        {
            uiImagenMenuPrincipal.sprite = spriteTodoApagado;
        }

        if (panelScore != null)
        {
            panelScore.SetActive(false);
        }
    }

    public void PresionarIniciar()
    {
        if (uiImagenMenuPrincipal != null && spriteIniciarEncendido != null)
        {
            uiImagenMenuPrincipal.sprite = spriteIniciarEncendido;
        }

        ReproducirSFX(sonidoIniciar);

        if (fuenteMusicaLobby != null)
        {
            StartCoroutine(FadeOutAudio(fuenteMusicaLobby, 0.4f));
        }

        StartCoroutine(RetrasoCargaScene("Scene_Game", 0.4f));
    }

    public void PresionarScore()
    {
        StartCoroutine(ManejarPulsoScore());
    }

    IEnumerator ManejarPulsoScore()
    {
        if (uiImagenMenuPrincipal != null && spriteScoreEncendido != null)
        {
            uiImagenMenuPrincipal.sprite = spriteScoreEncendido;
        }

        ReproducirSFX(sonidoClickComun);

        yield return new WaitForSecondsRealtime(0.2f);

        if (panelScore != null)
        {
            panelScore.SetActive(true);
        }

        uiImagenMenuPrincipal.sprite = spriteTodoApagado;
    }

    public void PresionarSalir()
    {
        ReproducirSFX(sonidoClickComun);

        if (fuenteMusicaLobby != null)
        {
            StartCoroutine(FadeOutAudio(fuenteMusicaLobby, 0.5f));
        }

        StartCoroutine(RetrasoSalida(0.5f));
    }

    public void CerrarPanelScore()
    {
        ReproducirSFX(sonidoClickComun);
        if (panelScore != null)
        {
            panelScore.SetActive(false);
        }
    }

    private void ReproducirSFX(AudioClip clip)
    {
        if (fuenteEfectos != null && clip != null)
        {
            fuenteEfectos.PlayOneShot(clip);
        }
    }

    IEnumerator FadeOutAudio(AudioSource audioSource, float duracion)
    {
        float volumenInicial = audioSource.volume;
        for (float t = 0; t < duracion; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(volumenInicial, 0f, t / duracion);
            yield return null;
        }
        audioSource.Stop();
    }

    IEnumerator RetrasoCargaScene(string nombreEscena, float tiempoEspera)
    {
        yield return new WaitForSecondsRealtime(tiempoEspera);
        SceneManager.LoadScene(nombreEscena);
    }

    IEnumerator RetrasoSalida(float tiempoEspera)
    {
        yield return new WaitForSecondsRealtime(tiempoEspera);
        Debug.Log("Cierre de juego limpio ejecutado.");
        Application.Quit();
    }
}