using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Image uiImagenMenuPrincipal;
    [SerializeField] private Sprite spriteTodoApagado, spriteIniciarEncendido, spriteScoreEncendido;

    [Header("Audio")]
    [SerializeField] private AudioSource fuenteEfectos, fuenteMusicaLobby, fuenteMusicaScore;
    [SerializeField] private AudioClip sonidoIniciar, sonidoClickComun;

    [Header("Secuencia de Score")]
    [SerializeField] private VideoPlayer videoPlayerScore;
    [SerializeField] private GameObject objetoPantallaVideo;
    [SerializeField] private GameObject objetoTelonNegro; // Tu escudo contra clics
    [SerializeField] private GameObject panelScore;
    [SerializeField] private ScoreManager sistemaScoreDatos;

    void Start()
    {
        if (uiImagenMenuPrincipal) uiImagenMenuPrincipal.sprite = spriteTodoApagado;
        if (panelScore) panelScore.SetActive(false);
        if (objetoPantallaVideo) objetoPantallaVideo.SetActive(false);
        if (objetoTelonNegro) objetoTelonNegro.SetActive(false);
    }

    public void PresionarIniciar()
    {
        if (uiImagenMenuPrincipal) uiImagenMenuPrincipal.sprite = spriteIniciarEncendido;
        ReproducirSFX(sonidoIniciar);
        if (fuenteMusicaLobby) StartCoroutine(FadeOutAudio(fuenteMusicaLobby, 0.4f));
        StartCoroutine(RetrasoCargaScene("Scene_Game", 0.4f));
    }

    public void PresionarScore() => StartCoroutine(SecuenciaVideoScore());

    IEnumerator SecuenciaVideoScore()
    {
        if (uiImagenMenuPrincipal) uiImagenMenuPrincipal.sprite = spriteScoreEncendido;
        ReproducirSFX(sonidoClickComun);

        // Activamos el telón inmediatamente para bloquear botones del menú
        if (objetoTelonNegro) objetoTelonNegro.SetActive(true);

        if (fuenteMusicaLobby) StartCoroutine(FadeOutAudio(fuenteMusicaLobby, 0.5f));
        yield return new WaitForSecondsRealtime(0.2f);

        if (objetoPantallaVideo && videoPlayerScore)
        {
            objetoPantallaVideo.SetActive(true);
            videoPlayerScore.Play();
        }

        yield return new WaitForSecondsRealtime(8.0f); // Duración de tu video

        if (objetoPantallaVideo) objetoPantallaVideo.SetActive(false);
        if (sistemaScoreDatos) sistemaScoreDatos.ActualizarPanelVisual();
        if (panelScore) panelScore.SetActive(true);

        if (uiImagenMenuPrincipal) uiImagenMenuPrincipal.sprite = spriteTodoApagado;
        if (fuenteMusicaScore) fuenteMusicaScore.Play();
    }

    public void CerrarPanelScore()
    {
        ReproducirSFX(sonidoClickComun);
        if (panelScore) panelScore.SetActive(false);
        if (objetoTelonNegro) objetoTelonNegro.SetActive(false); // Apagamos el escudo

        if (fuenteMusicaScore) StartCoroutine(FadeOutAudio(fuenteMusicaScore, 0.5f));
        if (fuenteMusicaLobby) { fuenteMusicaLobby.volume = 1f; fuenteMusicaLobby.Play(); }
    }

    public void PresionarSalir()
    {
        ReproducirSFX(sonidoClickComun);
        if (fuenteMusicaLobby) StartCoroutine(FadeOutAudio(fuenteMusicaLobby, 0.5f));
        StartCoroutine(RetrasoSalida(0.5f));
    }

    private void ReproducirSFX(AudioClip clip) { if (fuenteEfectos && clip) fuenteEfectos.PlayOneShot(clip); }

    IEnumerator FadeOutAudio(AudioSource a, float d)
    {
        float vol = a.volume;
        for (float t = 0; t < d; t += Time.deltaTime) { a.volume = Mathf.Lerp(vol, 0f, t / d); yield return null; }
        a.Stop();
    }

    IEnumerator RetrasoCargaScene(string s, float t) { yield return new WaitForSecondsRealtime(t); SceneManager.LoadScene(s); }
    IEnumerator RetrasoSalida(float t) { yield return new WaitForSecondsRealtime(t); Application.Quit(); }
}