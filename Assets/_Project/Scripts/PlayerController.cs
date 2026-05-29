using UnityEngine;
using UnityEngine.UI;
using System.Collections; // VITAL: Necesario para que funcione el tiempo de parpadeo (Corrutinas)

public class PlayerController : MonoBehaviour
{
    [Header("Ajustes de Vuelo")]
    [SerializeField] private float velocidadHorizontal = 7f;
    [SerializeField] private float limiteX = 6.5f;
    [SerializeField] private float suavizadoAnimacion = 10f;
    [SerializeField] private float anguloDeInclinacion = 25f;

    [Header("Atributos")]
    [SerializeField] private float energia = 100f;
    [SerializeField] private int vidas = 3;
    [SerializeField] private float desgasteEnergia = 5f;

    [Header("HUD (Interfaz en Pantalla)")]
    [SerializeField] private Text textoScore;
    [SerializeField] private Text textoVidas;

    [Header("Sistemas de Game Over y Sonido")]
    [SerializeField] private GameOverManager managerGameOver;
    [SerializeField] private AudioSource fuenteSFX_Dragon;
    [SerializeField] private AudioClip[] sonidosDeGolpe;
    [SerializeField] private AudioClip[] sonidosDeComida; // NUEVO: Array para tus 3 sonidos de comida

    private Animator anim;
    private SpriteRenderer spriteDragon; // NUEVO: Controla la visibilidad para el parpadeo
    private int score = 0;
    private bool juegoTerminado = false;
    private bool esInvulnerable = false; // NUEVO: Bloquea el daño repetido
    private float movimientoInput;

    void Start()
    {
        Time.timeScale = 1f;
        anim = GetComponent<Animator>();
        spriteDragon = GetComponent<SpriteRenderer>(); // Capturamos el gráfico del dragón

        // Forzamos la escala a 1 para evitar que las animaciones viejas lo achiquen
        transform.localScale = Vector3.one;

        // Inicializar UI del HUD
        if (textoScore != null) textoScore.text = "SCORE: 0";
        if (textoVidas != null) textoVidas.text = "VIDAS: " + vidas;

        if (anim == null) Debug.LogError("¡Falta el componente Animator en el objeto Dragon!");
        if (spriteDragon == null) Debug.LogError("¡Falta el componente SpriteRenderer en el Dragón!");
    }

    void Update()
    {
        // Si el juego terminó, congelamos el movimiento del dragón inmediatamente
        if (juegoTerminado) return;

        // 1. CAPTURAR INPUT (Híbrido: Teclado + Pantalla Táctil)
        movimientoInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButton(0))
        {
            Vector3 posicionToque = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (posicionToque.x < 0) movimientoInput = -1f;
            else movimientoInput = 1f;
        }

        // 2. MOVIMIENTO FÍSICO
        transform.Translate(Vector3.right * movimientoInput * velocidadHorizontal * Time.deltaTime, Space.World);

        float clampX = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(clampX, transform.position.y, transform.position.z);

        // 3. ANIMACIÓN Y BLEND TREE
        if (anim != null)
        {
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

            // LLAMADA AL SONIDO DE COMIDA
            ReproducirSonidoAleatorio(sonidosDeComida);
        }
        else if (colision.CompareTag("Obstaculo"))
        {
            // Si el dragón es invulnerable por un golpe reciente, cancelamos el daño
            if (esInvulnerable) return;

            vidas--;
            if (textoVidas != null) textoVidas.text = "VIDAS: " + vidas;
            colision.gameObject.SetActive(false);

            // LLAMADA AL SONIDO DE GOLPE
            ReproducirSonidoAleatorio(sonidosDeGolpe);

            if (vidas <= 0)
            {
                Morir();
            }
            else
            {
                // Si sobrevive, activamos la rutina de parpadeo e inmunidad
                StartCoroutine(RutinaInvulnerabilidad());
            }
        }
    }

    // NUEVO: Función inteligente que recibe cualquier lista de sonidos y reproduce uno al azar
    private void ReproducirSonidoAleatorio(AudioClip[] listaDeSonidos)
    {
        if (listaDeSonidos != null && listaDeSonidos.Length > 0 && fuenteSFX_Dragon != null)
        {
            int sonidoAleatorio = Random.Range(0, listaDeSonidos.Length);
            fuenteSFX_Dragon.PlayOneShot(listaDeSonidos[sonidoAleatorio]);
        }
    }

    // NUEVO: Rutina de tiempo real para el parpadeo
    IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;

        // Bucle que apaga y prende el gráfico del dragón 5 veces (efecto Arcade clásico)
        for (int i = 0; i < 5; i++)
        {
            if (spriteDragon != null) spriteDragon.enabled = false;
            yield return new WaitForSeconds(0.15f);

            if (spriteDragon != null) spriteDragon.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        esInvulnerable = false; // Al terminar el bucle, vuelve a ser vulnerable
    }

    void Morir()
    {
        juegoTerminado = true;

        // El GameOverManager ahora se encarga de pausar el tiempo, poner el video y la música
        if (managerGameOver != null)
        {
            managerGameOver.DispararGameOver(score);
        }
        else
        {
            Debug.LogError("¡ATENCIÓN! Falta asignar el GameManager en el slot del Dragón.");
        }
    }
}