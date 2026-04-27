using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Permite reiniciar el nivel

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 7f;
    [SerializeField] private float limiteX = 2.5f;

    [Header("Atributos")]
    [SerializeField] private float energia = 100f;
    [SerializeField] private int vidas = 3;
    [SerializeField] private float desgasteEnergia = 5f;

    [Header("UI")]
    [SerializeField] private Text textoScore;
    [SerializeField] private Text textoVidas;
    [SerializeField] private GameObject panelGameOver; // Para el panel de derrota

    private int score = 0;
    private bool juegoTerminado = false;

    void Start()
    {
        Time.timeScale = 1f; // Asegura que el tiempo corra al iniciar

        // Sistemas de seguridad: Solo actualiza si los textos están conectados
        if (textoScore != null) textoScore.text = "SCORE: 0";
        if (textoVidas != null) textoVidas.text = "VIDAS: 3";
        if (panelGameOver != null) panelGameOver.SetActive(false);
    }

    void Update()
    {
        // Si ya perdimos, ignoramos el movimiento y esperamos a que reinicie
        if (juegoTerminado)
        {
            if (Input.GetKeyDown(KeyCode.R)) ReiniciarJuego();
            return;
        }

        // 1. Input fluido
        float movimientoX = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * movimientoX * velocidad * Time.deltaTime);

        // Limitar bordes
        float clampX = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(clampX, transform.position.y, transform.position.z);

        // 2. Desgaste de energía
        energia -= desgasteEnergia * Time.deltaTime;

        if (energia <= 0)
        {
            Morir(); // Llama a la muerte real, no solo a un texto
        }
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (juegoTerminado) return; // Si estamos muertos, somos fantasmas

        if (colision.CompareTag("Comida"))
        {
            energia += 20f;
            score += 100;
            if (textoScore != null) textoScore.text = "SCORE: " + score;

            colision.gameObject.SetActive(false); // Apaga la comida
        }
        else if (colision.CompareTag("Obstaculo"))
        {
            vidas--;
            if (textoVidas != null) textoVidas.text = "VIDAS: " + vidas;

            colision.gameObject.SetActive(false); // Apaga la roca

            if (vidas <= 0)
            {
                Morir();
            }
        }
    }

    // --- LÓGICA DE DERROTA ---
    void Morir()
    {
        juegoTerminado = true;
        Time.timeScale = 0f; // Congela el tiempo (ya no caen más cosas)

        if (panelGameOver != null) panelGameOver.SetActive(true); // Muestra tu pantalla

        Debug.Log("Moriste. Presiona la tecla R para reiniciar.");
    }

    void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}