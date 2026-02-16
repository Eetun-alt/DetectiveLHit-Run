using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PukuhuoneManager : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void SaveProfile()
    {
        string name = nameInput.text;

        if (name.Length > 0)
        {
            ProfileManager.Instance.SaveProfileName(name);
            SceneManager.LoadScene("profiilit");
        }
    }
}
