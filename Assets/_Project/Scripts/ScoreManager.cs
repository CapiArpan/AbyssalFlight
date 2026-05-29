using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Columnas de la UI (TextMeshPro)")]
    [SerializeField] private TextMeshProUGUI textoColumnaRank;
    [SerializeField] private TextMeshProUGUI textoColumnaScore;
    [SerializeField] private TextMeshProUGUI textoColumnaName;

    private int[] topScores = new int[5];
    private string[] topNames = new string[5];

    void Awake()
    {
        CargarHighScores();
    }

    public void CargarHighScores()
    {
        for (int i = 0; i < 5; i++)
        {
            topScores[i] = PlayerPrefs.GetInt("HighScoreValue_" + i, 0);
            topNames[i] = PlayerPrefs.GetString("HighScoreName_" + i, "---");
        }
    }

    public void ActualizarPanelVisual()
    {
        CargarHighScores();

        // Validamos que los textos existan antes de escribir
        if (textoColumnaRank == null || textoColumnaScore == null || textoColumnaName == null) return;

        textoColumnaRank.text = "<color=#FFFF00>RANK</color>\n";
        textoColumnaScore.text = "<color=#FFFF00>SCORE</color>\n";
        textoColumnaName.text = "<color=#FFFF00>NAME</color>\n";

        string[] coloresRank = { "#FFFFFF", "#FF0000", "#FFA500", "#FFC0CB", "#FFFF00" };
        string[] sufijos = { "1ST", "2ND", "3RD", "4TH", "5TH" };

        for (int i = 0; i < 5; i++)
        {
            string colorHex = coloresRank[i];
            textoColumnaRank.text += $"<color={colorHex}>{sufijos[i]}</color>\n";
            textoColumnaScore.text += $"<color={colorHex}>{topScores[i]}</color>\n";
            textoColumnaName.text += $"<color={colorHex}>{topNames[i]}</color>\n";
        }
    }

    public void EvaluarYGuardarNuevoPuntaje(int nuevoPuntaje, string inicialesJugador)
    {
        for (int i = 0; i < 5; i++)
        {
            if (nuevoPuntaje > topScores[i])
            {
                for (int j = 4; j > i; j--)
                {
                    topScores[j] = topScores[j - 1];
                    topNames[j] = topNames[j - 1];
                }
                topScores[i] = nuevoPuntaje;
                topNames[i] = inicialesJugador.ToUpper();
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