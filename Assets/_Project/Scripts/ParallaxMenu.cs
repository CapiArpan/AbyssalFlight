using UnityEngine;

public class ParallaxMenu : MonoBehaviour
{
    [Header("Capas del Parallax")]
    [SerializeField] private RectTransform[] capas;

    [Header("Intensidad de Movimiento (Fondo a Frente)")]
    [SerializeField] private float[] multiplicadores = { 0.2f, 0.5f, 0.9f, 1.5f };

    [Header("Ajustes del Sistema")]
    [SerializeField] private float suavizado = 5.0f;
    [SerializeField] private float maxDesplazamiento = 60.0f;

    private Vector2 offsetInput;

    // NUEVO: La memoria fotográfica para guardar dónde dejaste cada imagen
    private Vector2[] posicionesIniciales;

    void Start()
    {
        // Al arrancar el juego, creamos la lista de memoria
        posicionesIniciales = new Vector2[capas.Length];

        // Guardamos las coordenadas exactas de cada piedra, dragón y fondo
        for (int i = 0; i < capas.Length; i++)
        {
            if (capas[i] != null)
            {
                posicionesIniciales[i] = capas[i].anchoredPosition;
            }
        }
    }

    void Update()
    {
        // Captura el touch en celular o el mouse en el editor
        Vector2 posMouse = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);

        // Normalizamos el centro de la pantalla como (0,0)
        offsetInput.x = (posMouse.x - 0.5f) * maxDesplazamiento;
        offsetInput.y = (posMouse.y - 0.5f) * maxDesplazamiento;

        // Desplazamos de forma asíncrona cada capa para generar la ilusión 3D
        for (int i = 0; i < capas.Length; i++)
        {
            if (capas[i] != null && i < multiplicadores.Length)
            {
                // EL CAMBIO MAESTRO: Tomamos la posición original guardada y le sumamos el movimiento
                Vector2 posObjetivo = posicionesIniciales[i] + (offsetInput * multiplicadores[i]);

                capas[i].anchoredPosition = Vector2.Lerp(capas[i].anchoredPosition, posObjetivo, Time.deltaTime * suavizado);
            }
        }
    }
}