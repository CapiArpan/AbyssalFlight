using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MoverHaciaAbajo : MonoBehaviour
{
    [Header("Física de Caída Principal")]
    [SerializeField] private float velocidadCaida = 8f;

    [Header("Movimiento Orgánico (Caos en el aire)")]
    [Tooltip("Activa para que el objeto se balancee de lado a lado mientras cae")]
    [SerializeField] private bool balancearAlCaer = true;
    [Tooltip("Qué tan ancho es el balanceo de izquierda a derecha")]
    [SerializeField] private float amplitudBalanceo = 0.5f;
    [Tooltip("Qué tan rápido se balancea")]
    [SerializeField] private float velocidadBalanceo = 2f;
    [Tooltip("Le da una ligera inclinación al azar al aparecer, para que no caigan tiesos")]
    [SerializeField] private bool rotacionAleatoria = true;
    [SerializeField] private float anguloMaximoInclinacion = 15f;

    [System.Serializable]
    public struct VarianteVisual
    {
        public string nombreReferencia;
        public Sprite[] framesDeCaida;
    }

    [Header("Catálogo de Imágenes (Tus 8 carpetas)")]
    [SerializeField] private VarianteVisual[] catalogoVisual;

    [Header("Configuración de Animación")]
    [Tooltip("Metros que debe caer para cambiar a la siguiente imagen")]
    [SerializeField] private float distanciaPorFrame = 1.5f;
    [Tooltip("Si está desmarcado, se quedará congelado en la última imagen (Ideal para que mantengan la pose de ataque)")]
    [SerializeField] private bool animacionEnBucle = false;

    private SpriteRenderer renderizador;
    private Sprite[] framesActuales;
    private float yUltimaMarca;
    private int frameActual = 0;

    // Variables para calcular el balanceo
    private float xInicial;
    private float desfaseTiempo;

    void Awake()
    {
        renderizador = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // 1. Guardar la posición X donde el GameManager lo hizo aparecer
        xInicial = transform.position.x;
        // 2. Un desfase aleatorio para que, si caen 2 a la vez, no se balanceen sincronizados
        desfaseTiempo = Random.Range(0f, 100f);

        // 3. Darle una inclinación inicial al azar
        if (rotacionAleatoria)
        {
            float angulo = Random.Range(-anguloMaximoInclinacion, anguloMaximoInclinacion);
            transform.rotation = Quaternion.Euler(0, 0, angulo);
        }

        // 4. Elegir una de las 8 variantes al azar
        if (catalogoVisual != null && catalogoVisual.Length > 0)
        {
            int indiceAleatorio = Random.Range(0, catalogoVisual.Length);
            framesActuales = catalogoVisual[indiceAleatorio].framesDeCaida;

            // Preparar la primera imagen
            if (framesActuales != null && framesActuales.Length > 0)
            {
                frameActual = 0;
                renderizador.sprite = framesActuales[0];
                yUltimaMarca = transform.position.y;
            }
        }
    }

    void Update()
    {
        // 1. Calcular el balanceo en el Eje X (Movimiento de lado a lado)
        float nuevaX = transform.position.x;
        if (balancearAlCaer)
        {
            nuevaX = xInicial + Mathf.Sin((Time.time + desfaseTiempo) * velocidadBalanceo) * amplitudBalanceo;
        }

        // 2. Calcular la caída en el Eje Y
        float nuevaY = transform.position.y - (velocidadCaida * Time.deltaTime);

        // Aplicar la nueva posición
        transform.position = new Vector3(nuevaX, nuevaY, transform.position.z);

        // 3. Ejecutar la animación según la distancia recorrida
        AnimarPorCaida();

        // 4. Reciclaje cuando sale de la pantalla
        if (transform.position.y < -7f)
        {
            gameObject.SetActive(false);
        }
    }

    private void AnimarPorCaida()
    {
        if (framesActuales == null || framesActuales.Length == 0) return;

        float distanciaCaida = yUltimaMarca - transform.position.y;

        // Si ya cayó los metros necesarios, cambiamos de imagen
        if (distanciaCaida >= distanciaPorFrame)
        {
            frameActual++;

            // Control de final de animación
            if (frameActual >= framesActuales.Length)
            {
                // Si es bucle, vuelve a 0. Si no, se queda bloqueado en la última imagen (el ataque)
                frameActual = animacionEnBucle ? 0 : framesActuales.Length - 1;
            }

            renderizador.sprite = framesActuales[frameActual];
            yUltimaMarca = transform.position.y + (distanciaCaida - distanciaPorFrame);
        }
    }
}