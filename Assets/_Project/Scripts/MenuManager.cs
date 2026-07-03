using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private ScoreManager sistemaScoreDatos;

    [Header("UI de Pantalla de Carga")]
    [SerializeField] private GameObject panelCarga;
    [SerializeField] private TextMeshProUGUI textoPorcentaje;
    [SerializeField] private GameObject botonFotoContinuar; // <- AQUÍ VA TU FOTO/BOTÓN

    private bool autorizacionParaDescender = false; // Seguro para no cambiar de escena

    void Awake()
    {
        sistemaScoreDatos = GetComponentInChildren<ScoreManager>(true);

        // Seguridad: Apagamos todo lo de la carga al iniciar el juego
        if (panelCarga != null) panelCarga.SetActive(false);
        if (botonFotoContinuar != null) botonFotoContinuar.SetActive(false);
    }

    public void PresionarScore()
    {
        if (sistemaScoreDatos != null) sistemaScoreDatos.IniciarSecuencia();
        else Debug.LogError("¡ERROR FATAL! No se encontró el script ScoreManager.");
    }

    public void PresionarIniciar()
    {
        StartCoroutine(CargarNivelAsync("Scene_Game"));
    }

    private IEnumerator CargarNivelAsync(string nombreEscena)
    {
        // 1. Encendemos el panel principal
        if (panelCarga != null) panelCarga.SetActive(true);
        autorizacionParaDescender = false;

        yield return new WaitForSecondsRealtime(0.4f);

        // 2. Iniciamos carga asíncrona y la BLOQUEAMOS
        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombreEscena);
        operacion.allowSceneActivation = false;

        // 3. Bucle de carga
        while (!operacion.isDone)
        {
            float progreso = Mathf.Clamp01(operacion.progress / 0.9f);

            if (operacion.progress < 0.9f)
            {
                if (textoPorcentaje != null)
                    textoPorcentaje.text = "Comiendo... " + (progreso * 100f).ToString("F0") + "%";
            }
            else
            {
                // YA CARGÓ EL 100%
                if (textoPorcentaje != null) textoPorcentaje.text = "Tragado, Reanuda";

                // Encendemos tu foto/botón para que el jugador lo vea
                if (botonFotoContinuar != null && !botonFotoContinuar.activeSelf)
                {
                    botonFotoContinuar.SetActive(true);
                }

                // Esperamos a que presiones el botón
                if (autorizacionParaDescender)
                {
                    operacion.allowSceneActivation = true; // ¡Liberamos la bestia!
                }
            }
            yield return null;
        }
    }

    // --- ESTA FUNCIÓN LA EJECUTARÁ TU BOTÓN-FOTO ---
    public void ConfirmarDescenso()
    {
        autorizacionParaDescender = true;
    }

    public void PresionarSalir()
    {
        Debug.Log("¡El jugador ha cerrado el juego!");
        Application.Quit();
    }
}