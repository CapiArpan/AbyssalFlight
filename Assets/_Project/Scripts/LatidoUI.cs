using UnityEngine;

public class LatidoUI : MonoBehaviour
{
    [Header("Configuración del Pulso")]
    public float velocidadLatido = 3f;
    public float magnitudLatido = 0.08f;

    private Vector3 escalaInicial;

    void Start()
    {
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        // Matemáticas puras para simular respiración/latido
        float escala = 1f + Mathf.Sin(Time.time * velocidadLatido) * magnitudLatido;
        transform.localScale = escalaInicial * escala;
    }
}