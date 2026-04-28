using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs Originales")]
    [SerializeField] private GameObject prefabComida;
    [SerializeField] private GameObject prefabObstaculo;

    [Header("Tamaño de la Piscina de Memoria")]
    [SerializeField] private int cantidadComida = 15;
    [SerializeField] private int cantidadObstaculos = 20;

    [Header("Configuración de Spawn")]
    [SerializeField] private float tiempoEntreSpawns = 0.8f;
    [SerializeField] private float limiteX = 6.5f;

    // Nuestras piscinas (Listas que guardarán los objetos apagados)
    private List<GameObject> poolComida = new List<GameObject>();
    private List<GameObject> poolObstaculos = new List<GameObject>();

    void Start()
    {
        // 1. Llenamos la piscina antes de que el jugador empiece a jugar
        InicializarPiscina(prefabComida, poolComida, cantidadComida);
        InicializarPiscina(prefabObstaculo, poolObstaculos, cantidadObstaculos);

        // 2. Iniciamos el ciclo del juego
        InvokeRepeating(nameof(GenerarObjeto), 1f, tiempoEntreSpawns);
    }

    // Función para pre-fabricar y apagar los objetos
    void InicializarPiscina(GameObject prefab, List<GameObject> pool, int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false); // Los apagamos
            pool.Add(obj); // Los guardamos en la lista
        }
    }

    void GenerarObjeto()
    {
        // Decidimos qué tirar (30% comida, 70% obstáculo)
        bool tirarComida = Random.value > 0.7f;
        List<GameObject> piscinaSeleccionada = tirarComida ? poolComida : poolObstaculos;

        // Buscamos un objeto que esté apagado en la piscina
        GameObject objetoListo = ObtenerObjetoInactivo(piscinaSeleccionada);

        if (objetoListo != null)
        {
            float xAleatorio = Random.Range(-limiteX, limiteX);
            // Lo teletransportamos a la parte superior de la pantalla
            objetoListo.transform.position = new Vector3(xAleatorio, 8f, 0f);
            // ¡Lo encendemos!
            objetoListo.SetActive(true);
        }
    }

    // Buscador de objetos apagados
    GameObject ObtenerObjetoInactivo(List<GameObject> pool)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy) // Si está desactivado
            {
                return obj;
            }
        }
        return null; // Si todos están cayendo al mismo tiempo (raro que pase)
    }
}