using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("kartta"); // Lataa kartta-skenen
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("settings"); // Lataa settings-skenen
    }
}