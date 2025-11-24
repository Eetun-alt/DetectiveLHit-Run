using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuClick : MonoBehaviour
{
    public string sceneToLoad;  // Tähän kirjoitetaan scenen nimi Inspectorissa

    void OnMouseDown()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}