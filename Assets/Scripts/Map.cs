using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    public string ToLevel;

    private void OnMouseDown()
    {
        SceneManager.LoadScene(ToLevel);
    }
}
