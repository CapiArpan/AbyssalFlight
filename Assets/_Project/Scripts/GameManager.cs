using System.Collections.Generic;
using UnityEngine;

// Clase pequeña para organizar las texturas por pares
[System.Serializable]
public class BiomaData
{
    public string nombreBioma;
    public Texture texturaIzquierda;
    public Texture texturaDerecha;
}

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

    [Header("Muros (Configuración de Lados)")]
    [SerializeField] private ScrollingBackground muroIzquierdo;
    [SerializeField] private ScrollingBackground muroDerecho;

    [Header("Lista de Biomas (Pares de fotos)")]
    [SerializeField] private BiomaData[] listaBiomas;
    [SerializeField] private float tiempoCambioBioma = 30f;
    [SerializeField] private float duracionTransicion = 3.0f;

    private List<GameObject> poolComida = new List<GameObject>();
    private List<GameObject> poolObstaculos = new List<GameObject>();
    private int indiceBiomaActual = 0;
    private float cronometroBioma;

    void Start()
    {
        InicializarPiscina(prefabComida, poolComida, cantidadComida);
        InicializarPiscina(prefabObstaculo, poolObstaculos, cantidadObstaculos);
        InvokeRepeating(nameof(GenerarObjeto), 1f, tiempoEntreSpawns);
    }

    void Update()
    {
        cronometroBioma += Time.deltaTime;
        if (cronometroBioma >= tiempoCambioBioma)
        {
            CambiarBiomaSincronizado();
            cronometroBioma = 0;
        }
    }

    void CambiarBiomaSincronizado()
    {
        if (listaBiomas.Length == 0) return;

        indiceBiomaActual = (indiceBiomaActual + 1) % listaBiomas.Length;

        // Mandamos la textura IZQUIERDA al muro IZQUIERDO
        if (muroIzquierdo != null)
            muroIzquierdo.CambiarBiomaSuave(listaBiomas[indiceBiomaActual].texturaIzquierda, duracionTransicion);

        // Mandamos la textura DERECHA al muro DERECHO
        if (muroDerecho != null)
            muroDerecho.CambiarBiomaSuave(listaBiomas[indiceBiomaActual].texturaDerecha, duracionTransicion);

        Debug.Log("Cambiando a Bioma: " + listaBiomas[indiceBiomaActual].nombreBioma);
    }

    // --- MANTENEMOS TU LÓGICA DE POOLING ---
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
        bool tirarComida = Random.value > 0.7f;
        List<GameObject> piscinaSeleccionada = tirarComida ? poolComida : poolObstaculos;
        GameObject objetoListo = ObtenerObjetoInactivo(piscinaSeleccionada);
        if (objetoListo != null)
        {
            float xAleatorio = Random.Range(-limiteX, limiteX);
            objetoListo.transform.position = new Vector3(xAleatorio, 10f, 0f);
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