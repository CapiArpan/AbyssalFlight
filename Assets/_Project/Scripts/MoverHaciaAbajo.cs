using UnityEngine;

public class MoverHaciaAbajo : MonoBehaviour
{
    [SerializeField] private float velocidadCaida = 5f;

    void Update()
    {
        transform.Translate(Vector3.down * velocidadCaida * Time.deltaTime);

        // Ajustamos el límite para que el objeto se recicle apenas salga de la pantalla
        if (transform.position.y < -7f)
        {
            gameObject.SetActive(false); // Devuelve el objeto a la piscina
        }
    }
}