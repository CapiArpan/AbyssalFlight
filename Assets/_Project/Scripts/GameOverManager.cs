using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameOverManager : MonoBehaviour
{
    [Header("UI de Game Over")]
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject hudEnJuego;
    [SerializeField] private TextMeshProUGUI textoPuntajeFinal;
    [SerializeField] private TMP_InputField inputIniciales;

    [Header("Multimedia de Derrota")]
    [SerializeField] private VideoPlayer videoFondo;
    [SerializeField] private AudioSource[] fuentesAudioJuego;
    [SerializeField] private AudioSource audioSourceBGM_GameOver;

    private int puntajeLogrado = 0;

    void Start()
    {
        if (panelGameOver != null) panelGameOver.SetActive(false);
    }

    public void DispararGameOver(int puntajeFinal)
    {
        puntajeLogrado = puntajeFinal;

        if (textoPuntajeFinal != null)
        {
            textoPuntajeFinal.text = "SCORE: " + puntajeFinal.ToString();
        }

        // 1. Congelamos el tiempo
        Time.timeScale = 0f;

        // 2. Apagamos TODOS los audios del juego (Música y Ambiente)
        if (fuentesAudioJuego != null)
        {
            foreach (AudioSource fuente in fuentesAudioJuego)
            {
                if (fuente != null) fuente.Stop();
            }
        }

        // Ponemos la música de derrota
        if (audioSourceBGM_GameOver != null) audioSourceBGM_GameOver.Play();

        // 3. Encendemos el panel, APAGAMOS EL HUD y encendemos el video
        if (panelGameOver != null) panelGameOver.SetActive(true);
        if (hudEnJuego != null) hudEnJuego.SetActive(false);
        if (videoFondo != null) videoFondo.Play();

        // 4. Truco de UX
        if (inputIniciales != null)
        {
            inputIniciales.Select();
            inputIniciales.ActivateInputField();
        }
    }

    // --- BOTÓN 1: EL QUE YA TENÍAS (Guarda el récord y va al menú) ---
    public void BotonGuardarYSalir()
    {
        string iniciales = inputIniciales.text.Length >= 3 ? inputIniciales.text.Substring(0, 3) : "AAA";
        iniciales = iniciales.ToUpper();

        EvaluarYGuardar(puntajeLogrado, iniciales);

        Time.timeScale = 1f;
        SceneManager.LoadScene("Scene_Menu");
    }

    // --- BOTÓN 2: EL NUEVO (Reintenta rápido sin guardar nada) ---
    public void BotonReintentar()
    {
        // Vital: Descongelamos el tiempo para que el juego no empiece pausado
        Time.timeScale = 1f;

        // Recargamos la escena de juego. (Asegúrate de que tu escena se llame "Scene_Game")
        SceneManager.LoadScene("Scene_Game");
    }

    private void EvaluarYGuardar(int nuevoScore, string nombre)
    {
        for (int i = 0; i < 5; i++)
        {
            if (nuevoScore > PlayerPrefs.GetInt("HighScoreValue_" + i, 0))
            {
                // Desplazamos hacia abajo para no perder a los que estaban antes
                for (int j = 4; j > i; j--)
                {
                    PlayerPrefs.SetInt("HighScoreValue_" + j, PlayerPrefs.GetInt("HighScoreValue_" + (j - 1), 0));
                    PlayerPrefs.SetString("HighScoreName_" + j, PlayerPrefs.GetString("HighScoreName_" + (j - 1), "---"));
                }

                // Guardamos el nuevo en su lugar correspondiente
                PlayerPrefs.SetInt("HighScoreValue_" + i, nuevoScore);
                PlayerPrefs.SetString("HighScoreName_" + i, nombre);
                break;
            }
        }
        PlayerPrefs.Save();
    }
}