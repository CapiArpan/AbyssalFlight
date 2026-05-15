using UnityEngine;

public class ParallaxMenu : MonoBehaviour
{
    [Header("Capas del Menú (Imágenes de fondo)")]
    public RectTransform[] capas; // Arrastra tus 3 o 4 imágenes aquí
    public float[] multiplicadores; // Ejemplo: Capa fondo 0.1, Capa frente 1.5

    [SerializeField] private float suavizado = 5f;
    private Vector2 offsetInput;

    void Update()
    {
        // Captura movimiento de mouse o dedo
        Vector2 mousePos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        offsetInput = new Vector2(mousePos.x - 0.5f, mousePos.y - 0.5f) * 100f;

        for (int i = 0; i < capas.Length; i++)
        {
            if (capas[i] != null)
            {
                Vector2 targetPos = offsetInput * multiplicadores[i];
                capas[i].anchoredPosition = Vector2.Lerp(capas[i].anchoredPosition, targetPos, Time.deltaTime * suavizado);
            }
        }
    }
}