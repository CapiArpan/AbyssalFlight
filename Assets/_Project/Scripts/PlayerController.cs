using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Ajustes de Vuelo")]
    [SerializeField] private float velocidadHorizontal = 7f;
    [SerializeField] private float limiteX = 6.5f; // Ajustado al nuevo ancho de pantalla
    [SerializeField] private float suavizadoAnimacion = 10f;
    [SerializeField] private float anguloDeInclinacion = 25f;

    [Header("Atributos")]
    [SerializeField] private float energia = 100f;
    [SerializeField] private int vidas = 3;
    [SerializeField] private float desgasteEnergia = 5f;

    [Header("UI")]
    [SerializeField] private Text textoScore;
    [SerializeField] private Text textoVidas;
    [SerializeField] private GameObject panelGameOver;

    private Animator anim;
    private int score = 0;
    private bool juegoTerminado = false;
    private float movimientoInput;

    void Start()
    {
        Time.timeScale = 1f;
        anim = GetComponent<Animator>();

        // Forzamos la escala a 1 para evitar que las animaciones viejas lo achiquen
        transform.localScale = Vector3.one;

        // Inicializar UI
        if (textoScore != null) textoScore.text = "SCORE: 0";
        if (textoVidas != null) textoVidas.text = "VIDAS: " + vidas;
        if (panelGameOver != null) panelGameOver.SetActive(false);

        if (anim == null) Debug.LogError("¡Falta el componente Animator en el objeto Dragon!");
    }

    void Update()
    {
        if (juegoTerminado)
        {
            // Opcional: Permitir reiniciar tocando la pantalla cuando el juego termina
            if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))
            {
                ReiniciarJuego();
            }
            return;
        }

        // 1. CAPTURAR INPUT (Híbrido: Teclado + Pantalla Táctil)
        movimientoInput = Input.GetAxisRaw("Horizontal");

        // Detectar si estamos tocando la pantalla o haciendo clic
        if (Input.GetMouseButton(0))
        {
            // Convertimos la posición de la pantalla (píxeles) a coordenadas del mundo de Unity
            Vector3 posicionToque = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Si toca la mitad izquierda de la pantalla, va a la izquierda
            if (posicionToque.x < 0)
            {
                movimientoInput = -1f;
            }
            // Si toca la mitad derecha, va a la derecha
            else
            {
                movimientoInput = 1f;
            }
        }

        // 2. MOVIMIENTO FÍSICO
        // Usamos Space.World para que no se "caiga" al rotar
        transform.Translate(Vector3.right * movimientoInput * velocidadHorizontal * Time.deltaTime, Space.World);

        // Limitar bordes
        float clampX = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(clampX, transform.position.y, transform.position.z);

        // 3. ANIMACIÓN Y BLEND TREE
        if (anim != null)
        {
            // Actualizar DireccionX para el Blend Tree
            float valorActual = anim.GetFloat("DireccionX");
            float nuevoValor = Mathf.Lerp(valorActual, movimientoInput, Time.deltaTime * suavizadoAnimacion);
            anim.SetFloat("DireccionX", nuevoValor);
        }

        // 4. ROTACIÓN VISUAL (BANKING)
        float rotacionZ = -movimientoInput * anguloDeInclinacion;
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotacionZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * suavizadoAnimacion);

        // 5. LÓGICA DE SUPERVIVENCIA
        energia -= desgasteEnergia * Time.deltaTime;
        if (energia <= 0) Morir();
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (juegoTerminado) return;

        if (colision.CompareTag("Comida"))
        {
            energia += 20f;
            score += 100;
            if (textoScore != null) textoScore.text = "SCORE: " + score;
            colision.gameObject.SetActive(false);
        }
        else if (colision.CompareTag("Obstaculo"))
        {
            vidas--;
            if (textoVidas != null) textoVidas.text = "VIDAS: " + vidas;
            colision.gameObject.SetActive(false);

            if (vidas <= 0) Morir();
        }
    }

    void Morir()
    {
        juegoTerminado = true;
        Time.timeScale = 0f;
        if (panelGameOver != null) panelGameOver.SetActive(true);
        Debug.Log("Juego Terminado. Toca la pantalla o presiona R para reiniciar.");
    }

    void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}