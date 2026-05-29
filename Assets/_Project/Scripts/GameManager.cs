using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs y Pooling")]
    [SerializeField] private GameObject prefabComida;
    [SerializeField] private GameObject prefabObstaculo;
    [SerializeField] private int cantidadComida = 15;
    [SerializeField] private int cantidadObstaculos = 20;

    [Header("Configuración de Spawn")]
    [SerializeField] private float tiempoEntreSpawns = 0.8f;
    [SerializeField] private float limiteX = 6.5f;

    private List<GameObject> poolComida = new List<GameObject>();
    private List<GameObject> poolObstaculos = new List<GameObject>();

    void Start()
    {
        InicializarPiscina(prefabComida, poolComida, cantidadComida);
        InicializarPiscina(prefabObstaculo, poolObstaculos, cantidadObstaculos);

        // Comienza a generar objetos infinitamente
        InvokeRepeating(nameof(GenerarObjeto), 1f, tiempoEntreSpawns);
    }

    // Lógica de Pooling optimizada
    void InicializarPiscina(GameObject prefab, List<GameObject> pool, int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    void GenerarObjeto()
    {
        // 70% probabilidad de obstáculo, 30% de comida (puedes ajustar el 0.7f)
        bool tirarComida = Random.value > 0.7f;
        List<GameObject> piscinaSeleccionada = tirarComida ? poolComida : poolObstaculos;

        GameObject objetoListo = ObtenerObjetoInactivo(piscinaSeleccionada);
        if (objetoListo != null)
        {
            float xAleatorio = Random.Range(-limiteX, limiteX);
            objetoListo.transform.position = new Vector3(xAleatorio, 10f, 0f); // Aparecen arriba
            objetoListo.SetActive(true);
        }
    }

    GameObject ObtenerObjetoInactivo(List<GameObject> pool)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }
}