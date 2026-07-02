using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI de Pausa")]
    [SerializeField] private GameObject panelPausa;

    [Header("Controles de Audio")]
    [SerializeField] private Slider sliderVolumen;
    [SerializeField] private Toggle toggleSilenciar;

    void Start()
    {
        if (panelPausa != null) panelPausa.SetActive(false);
        Time.timeScale = 1f;

        if (sliderVolumen != null)
        {
            sliderVolumen.value = AudioListener.volume;
            sliderVolumen.onValueChanged.AddListener(CambiarVolumen);
        }

        if (toggleSilenciar != null)
        {
            toggleSilenciar.isOn = (AudioListener.volume == 0f);
            toggleSilenciar.onValueChanged.AddListener(Silenciar);
        }
    }

    // --- ESTAS SON LAS FUNCIONES QUE USARÁN TUS BOTONES ---
    public void PausarJuego()
    {
        Time.timeScale = 0f; // Congela el juego
        if (panelPausa != null) panelPausa.SetActive(true);
    }

    public void ReanudarJuego()
    {
        Time.timeScale = 1f; // Descongela
        if (panelPausa != null) panelPausa.SetActive(false);
    }

    public void SalirAlMenuSinGuardar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scene_Menu");
    }

    // --- CONTROLES DE AUDIO (Sin cambios) ---
    public void CambiarVolumen(float valor) => AudioListener.volume = valor;
    public void Silenciar(bool silenciado) => AudioListener.volume = silenciado ? 0f : (sliderVolumen ? sliderVolumen.value : 1f);
}