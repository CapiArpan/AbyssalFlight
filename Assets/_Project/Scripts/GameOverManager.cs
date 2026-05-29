using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameOverManager : MonoBehaviour
{
    [Header("UI de Game Over")]
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private TextMeshProUGUI textoPuntajeFinal;
    [SerializeField] private TMP_InputField inputIniciales;

    [Header("Multimedia de Derrota")]
    [SerializeField] private VideoPlayer videoFondo;
    [SerializeField] private AudioSource audioSourceBGM_Juego; // La música normal del nivel
    [SerializeField] private AudioSource audioSourceBGM_GameOver; // Tu música épica de derrota

    private int puntajeLogrado = 0;

    void Start()
    {
        if (panelGameOver != null) panelGameOver.SetActive(false);
    }

    // Llama a esta función desde tu Player cuando las vidas lleguen a 0
    public void DispararGameOver(int puntajeFinal)
    {
        puntajeLogrado = puntajeFinal;
        textoPuntajeFinal.text = "SCORE: " + puntajeFinal.ToString();

        // 1. Congelamos el tiempo (Criterio de Rúbrica)
        Time.timeScale = 0f;

        // 2. Apagamos la música alegre y ponemos la de derrota
        if (audioSourceBGM_Juego != null) audioSourceBGM_Juego.Stop();
        if (audioSourceBGM_GameOver != null) audioSourceBGM_GameOver.Play();

        // 3. Encendemos el panel y el video en loop
        panelGameOver.SetActive(true);
        if (videoFondo != null) videoFondo.Play();
    }

    // Esta función va en el OnClick() del Botón "Guardar"
    public void BotonGuardarYSalir()
    {
        // Forzamos a mayúsculas y tomamos máximo 3 letras
        string iniciales = inputIniciales.text.Length >= 3 ? inputIniciales.text.Substring(0, 3) : "AAA";
        iniciales = iniciales.ToUpper();

        // Usamos la misma lógica del ScoreManager para guardar directo en la memoria
        EvaluarYGuardar(puntajeLogrado, iniciales);

        // Descongelamos el tiempo y volvemos al menú
        Time.timeScale = 1f;
        SceneManager.LoadScene("Scene_Menu"); // PON EL NOMBRE EXACTO DE TU ESCENA DE MENÚ AQUÍ
    }

    // Lógica interna para guardar sin necesitar el ScoreManager en esta escena
    private void EvaluarYGuardar(int nuevoScore, string nombre)
    {
        int[] topScores = new int[5];
        string[] topNames = new string[5];

        for (int i = 0; i < 5; i++)
        {
            topScores[i] = PlayerPrefs.GetInt("HighScoreValue_" + i, 0);
            topNames[i] = PlayerPrefs.GetString("HighScoreName_" + i, "---");
        }

        for (int i = 0; i < 5; i++)
        {
            if (nuevoScore > topScores[i])
            {
                for (int j = 4; j > i; j--)
                {
                    topScores[j] = topScores[j - 1];
                    topNames[j] = topNames[j - 1];
                }
                topScores[i] = nuevoScore;
                topNames[i] = nombre;
                break;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt("HighScoreValue_" + i, topScores[i]);
            PlayerPrefs.SetString("HighScoreName_" + i, topNames[i]);
        }
        PlayerPrefs.Save();
    }
}