using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; // Vital para poder usar la tipografía TextMeshPro

public class MenuManager : MonoBehaviour
{
    private ScoreManager sistemaScoreDatos;

    [Header("UI de Pantalla de Carga")]
    [SerializeField] private GameObject panelCarga;
    [SerializeField] private TextMeshProUGUI textoPorcentaje;

    void Awake()
    {
        // Esto busca el ScoreManager sin importar donde esté en el Canvas
        // 'true' le dice a Unity: "aunque esté desactivado, encuéntralo igual"
        sistemaScoreDatos = GetComponentInChildren<ScoreManager>(true);

        // Seguridad: Nos aseguramos de que el panel de carga empiece apagado al abrir el juego
        if (panelCarga != null) panelCarga.SetActive(false);
    }

    public void PresionarScore()
    {
        if (sistemaScoreDatos != null)
        {
            sistemaScoreDatos.IniciarSecuencia();
        }
        else
        {
            Debug.LogError("¡ERROR FATAL! No se encontró el script ScoreManager. Asegúrate de que esté adjunto al Panel de Score.");
        }
    }

    // --- NUEVA SECUENCIA DE CARGA ASÍNCRONA ---
    public void PresionarIniciar()
    {
        StartCoroutine(CargarNivelAsync("Scene_Game"));
    }

    private IEnumerator CargarNivelAsync(string nombreEscena)
    {
        // 1. Encendemos el panel de carga (aparece tu imagen con la historia)
        if (panelCarga != null) panelCarga.SetActive(true);

        // 2. Mantenemos tu pausa original de 0.4 segundos. 
        // Esto permite que el jugador escuche el sonido de "Clic" del botón de Start.
        yield return new WaitForSecondsRealtime(0.4f);

        // 3. Le decimos a Unity que empiece a cargar el nivel en segundo plano
        AsyncOperation operacion = SceneManager.LoadSceneAsync(nombreEscena);

        // 4. Mientras la carga no termine, actualizamos la matemática del porcentaje
        while (!operacion.isDone)
        {
            // Unity calcula el progreso de 0 a 0.9. Lo forzamos a una escala perfecta de 0 a 100.
            float progreso = Mathf.Clamp01(operacion.progress / 0.9f);

            // Actualizamos el texto en pantalla (el "F0" quita los decimales molestos)
            if (textoPorcentaje != null)
            {
                textoPorcentaje.text = "CARGANDO... " + (progreso * 100f).ToString("F0") + "%";
            }

            // Esperamos al siguiente frame para mantener la animación fluida
            yield return null;
        }
    }

    // --- FUNCIÓN PARA EL BOTÓN DE SALIR ---
    public void PresionarSalir()
    {
        // Este mensaje aparecerá en la consola de Unity para que sepas que el botón sí funciona
        Debug.Log("¡El jugador ha cerrado el juego!");

        // Esta orden es la que cerrará el juego real una vez que lo exportes (.exe / APK)
        Application.Quit();
    }
}