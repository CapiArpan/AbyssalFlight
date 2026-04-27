using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void EmpezarJuego()
    {
        // Carga la escena del juego. El nombre debe ser EXACTAMENTE el que le pusiste a tu escena
        SceneManager.LoadScene("Scene_Game");
    }
}