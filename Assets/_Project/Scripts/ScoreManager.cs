using UnityEngine;
using TMPro;
using UnityEngine.Video;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [Header("Referencias Principales")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject telonNegro;
    [SerializeField] private CanvasGroup grupoUI;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clipClick;

    [System.Serializable]
    public struct FilaUI
    {
        [Tooltip("El objeto Fila_X completo")]
        public GameObject contenedorFila;
        public TextMeshProUGUI rank;
        public TextMeshProUGUI score;
        public TextMeshProUGUI name;
    }

    [Header("Configuración de Filas")]
    [SerializeField] private FilaUI[] filas;

    // Esta es la función que debe llamar tu botón
    public void IniciarSecuencia()
    {
        gameObject.SetActive(true);
        if (telonNegro) telonNegro.SetActive(true);

        // Escondemos la UI de golpe
        grupoUI.alpha = 0f;
        grupoUI.interactable = false;
        grupoUI.blocksRaycasts = false;

        // Apagamos todas las filas para que no se vean hasta que les toque
        foreach (var fila in filas)
        {
            if (fila.contenedorFila) fila.contenedorFila.SetActive(false);
        }

        StartCoroutine(SecuenciaCinematica());
    }

    private IEnumerator SecuenciaCinematica()
    {
        // 1. Reproducir el Video
        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared) yield return null;

            videoPlayer.Play();
            yield return new WaitUntil(() => !videoPlayer.isPlaying);
            videoPlayer.gameObject.SetActive(false);
        }

        // 2. Encendemos el CanvasGroup (ahora invisible porque las filas están apagadas)
        grupoUI.alpha = 1f;

        // 3. Revelar fila por fila con Efecto Arcade
        for (int i = 0; i < filas.Length; i++)
        {
            // Encender la fila
            if (filas[i].contenedorFila) filas[i].contenedorFila.SetActive(true);

            // Obtener datos guardados
            int puntosFinales = PlayerPrefs.GetInt("HighScoreValue_" + i, 0);
            filas[i].rank.text = (i + 1).ToString() + "ST"; // Ajusta a ND, RD, TH si prefieres
            filas[i].name.text = PlayerPrefs.GetString("HighScoreName_" + i, "---");
            filas[i].score.text = "0"; // Inicia en 0 para el contador

            // Sonido de golpe mecánico
            if (audioSource && clipClick) audioSource.PlayOneShot(clipClick);

            // Iniciar el contador rápido de puntos
            StartCoroutine(ContarPuntos(filas[i].score, puntosFinales));

            // Efecto visual: Escala de 0.8 a 1.0 suavemente
            float t = 0;
            while (t < 0.2f)
            {
                t += Time.deltaTime;
                filas[i].contenedorFila.transform.localScale = Vector3.Lerp(new Vector3(0.8f, 0.8f, 1f), Vector3.one, t / 0.2f);
                yield return null;
            }

            // Pausa antes de que caiga la siguiente fila
            yield return new WaitForSeconds(0.4f);
        }

        // 4. Liberar controles al terminar
        grupoUI.interactable = true;
        grupoUI.blocksRaycasts = true;
    }

    private IEnumerator ContarPuntos(TextMeshProUGUI texto, int finalValue)
    {
        float duracion = 0.5f;
        float elapsed = 0;
        while (elapsed < duracion)
        {
            elapsed += Time.deltaTime;
            int valorActual = (int)Mathf.Lerp(0, finalValue, elapsed / duracion);
            texto.text = valorActual.ToString();
            yield return null;
        }
        texto.text = finalValue.ToString();
    }

    public void Cerrar()
    {
        if (telonNegro) telonNegro.SetActive(false);
        gameObject.SetActive(false);
    }
}