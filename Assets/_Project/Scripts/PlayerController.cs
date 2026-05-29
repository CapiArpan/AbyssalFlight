using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    // NUEVO: Array para guardar los 3 corazones visuales
    [SerializeField] private GameObject[] iconosCorazones;

    [Header("Sistemas de Game Over y Sonido")]
    [SerializeField] private GameOverManager managerGameOver;
    [SerializeField] private AudioSource fuenteSFX_Dragon;
    [SerializeField] private AudioClip[] sonidosDeGolpe;
    [SerializeField] private AudioClip[] sonidosDeComida;

    private Animator anim;
    private SpriteRenderer spriteDragon;
    private int score = 0;
    private bool juegoTerminado = false;
    private bool esInvulnerable = false;
    private float movimientoInput;

    void Start()
    {
        Time.timeScale = 1f;
        anim = GetComponent<Animator>();
        spriteDragon = GetComponent<SpriteRenderer>();

        transform.localScale = Vector3.one;

        if (textoScore != null) textoScore.text = "SCORE: 0";
        // Actualizamos los corazones visuales al iniciar
        ActualizarHUDCorazones();

        if (anim == null) Debug.LogError("¡Falta el componente Animator!");
        if (spriteDragon == null) Debug.LogError("¡Falta el componente SpriteRenderer!");
    }

    void Update()
    {
        if (juegoTerminado) return;

        movimientoInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetMouseButton(0))
        {
            Vector3 posicionToque = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (posicionToque.x < 0) movimientoInput = -1f;
            else movimientoInput = 1f;
        }

        transform.Translate(Vector3.right * movimientoInput * velocidadHorizontal * Time.deltaTime, Space.World);

        float clampX = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(clampX, transform.position.y, transform.position.z);

        if (anim != null)
        {
            float valorActual = anim.GetFloat("DireccionX");
            float nuevoValor = Mathf.Lerp(valorActual, movimientoInput, Time.deltaTime * suavizadoAnimacion);
            anim.SetFloat("DireccionX", nuevoValor);
        }

        float rotacionZ = -movimientoInput * anguloDeInclinacion;
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotacionZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * suavizadoAnimacion);

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

            ReproducirSonidoAleatorio(sonidosDeComida);
        }
        else if (colision.CompareTag("Obstaculo"))
        {
            if (esInvulnerable) return;

            vidas--;
            // NUEVO: Llamamos a la función que apaga un corazón
            ActualizarHUDCorazones();

            colision.gameObject.SetActive(false);
            ReproducirSonidoAleatorio(sonidosDeGolpe);

            if (vidas <= 0)
            {
                Morir();
            }
            else
            {
                StartCoroutine(RutinaInvulnerabilidad());
            }
        }
    }

    // NUEVO: Función que revisa cuántas vidas tienes y apaga/prende los corazones
    private void ActualizarHUDCorazones()
    {
        for (int i = 0; i < iconosCorazones.Length; i++)
        {
            // Si el índice del corazón es menor a las vidas, se enciende. Si no, se apaga.
            if (i < vidas)
                iconosCorazones[i].SetActive(true);
            else
                iconosCorazones[i].SetActive(false);
        }
    }

    private void ReproducirSonidoAleatorio(AudioClip[] listaDeSonidos)
    {
        if (listaDeSonidos != null && listaDeSonidos.Length > 0 && fuenteSFX_Dragon != null)
        {
            int sonidoAleatorio = Random.Range(0, listaDeSonidos.Length);
            fuenteSFX_Dragon.PlayOneShot(listaDeSonidos[sonidoAleatorio]);
        }
    }

    IEnumerator RutinaInvulnerabilidad()
    {
        esInvulnerable = true;
        for (int i = 0; i < 5; i++)
        {
            if (spriteDragon != null) spriteDragon.enabled = false;
            yield return new WaitForSeconds(0.15f);

            if (spriteDragon != null) spriteDragon.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }
        esInvulnerable = false;
    }

    void Morir()
    {
        juegoTerminado = true;
        // Apagamos todos los corazones por si acaso
        vidas = 0;
        ActualizarHUDCorazones();

        if (managerGameOver != null) managerGameOver.DispararGameOver(score);
    }
}