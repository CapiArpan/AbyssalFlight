using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Ajustes de Velocidad")]
    [SerializeField] private float velocidadBase = 1.0f;
    [SerializeField] private float multiplicadorVelocidad = 5.0f;

    private Material miMaterial;
    private Vector2 offset;
    private float blendActual = 0f;

    void Start()
    {
        miMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        float incremento = velocidadBase * multiplicadorVelocidad * Time.deltaTime;
        offset.y += incremento;

        // Aplicamos el offset a AMBAS texturas en el shader
        miMaterial.SetVector("_MainTex_ST", new Vector4(1, 1, 0, offset.y));
        // Si el shader es personalizado, usamos SetTextureOffset normalmente
        miMaterial.mainTextureOffset = offset;
    }

    public void CambiarBiomaSuave(Texture nuevaTextura, float duracion)
    {
        StartCoroutine(TransicionBioma(nuevaTextura, duracion));
    }

    IEnumerator TransicionBioma(Texture proximaTextura, float tiempo)
    {
        // 1. Ponemos la nueva imagen en el "canal de espera" del shader
        miMaterial.SetTexture("_NextTex", proximaTextura);

        float tiempoPasado = 0;
        while (tiempoPasado < tiempo)
        {
            tiempoPasado += Time.deltaTime;
            float progreso = tiempoPasado / tiempo;

            // 2. Movemos el slider de mezcla de 0 a 1
            miMaterial.SetFloat("_Blend", progreso);
            yield return null;
        }

        // 3. Al terminar, la proxima textura pasa a ser la principal
        miMaterial.SetTexture("_MainTex", proximaTextura);
        miMaterial.SetFloat("_Blend", 0); // Reseteamos el slider
    }
}