using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    private ScoreManager sistemaScoreDatos;

    void Awake()
    {
        // Esto busca el ScoreManager sin importar donde esté en el Canvas
        // 'true' le dice a Unity: "aunque esté desactivado, encuéntralo igual"
        sistemaScoreDatos = GetComponentInChildren<ScoreManager>(true);
    }

    public void PresionarScore()
    {
        if (sistemaScoreDatos != null)
        {
            sistemaScoreDatos.IniciarSecuencia();
        }
        else
        {
            Debug.LogError("¡ERROR FATAL! No se encontró el script ScoreManager. Asegúrate de que está adjunto al Panel de Score.");
        }
    }

    public void PresionarIniciar() => StartCoroutine(RetrasoCarga("Scene_Game", 0.4f));

    IEnumerator RetrasoCarga(string s, float t)
    {
        yield return new WaitForSecondsRealtime(t);
        SceneManager.LoadScene(s);
    }
}