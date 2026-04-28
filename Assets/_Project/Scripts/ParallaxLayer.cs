using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private float factorParallax; // Velocidad de la capa
    private float altoImagen; // Cambiamos ancho por alto para scroll vertical
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;

        // IMPORTANTE: Para scroll vertical usamos el eje Y (height/alto)
        altoImagen = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        // Calculamos el desplazamiento
        float movimiento = (Time.time * factorParallax);

        // Mathf.Repeat asegura que el valor siempre esté entre 0 y el alto de la imagen
        float nuevaPos = Mathf.Repeat(movimiento, altoImagen);

        // CAMBIO CLAVE: Usamos Vector3.down para que el fondo baje
        // Esto hará que el dragón parezca que sube al abismo
        transform.position = posicionInicial + Vector3.down * nuevaPos;
    }
}