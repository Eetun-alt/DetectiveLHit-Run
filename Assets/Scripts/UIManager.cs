using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject mainUI;
    public GameObject createPopup;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowCreatePopup(int slotNumber)
    {
        // Piilotetaan muu UI
        mainUI.SetActive(false);

        // N‰ytet‰‰n popup
        createPopup.SetActive(true);
    }

    public void OnNoPressed()
    {
        SceneManager.LoadScene("profiilit");
    }
}
