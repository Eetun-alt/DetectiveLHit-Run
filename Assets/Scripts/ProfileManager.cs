using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance;

    public int selectedSlot; // 1, 2 tai 3

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Kutsutaan kun profiilin nappia painetaan
    public void SelectProfile(int slotNumber)
    {
        selectedSlot = slotNumber;

        if (PlayerPrefs.HasKey("profile_" + slotNumber))
        {
            Debug.Log("Profiili löytyy, ladataan peli...");
            SceneManager.LoadScene("kartta");
        }
        else
        {
            Debug.Log("Profiili tyhjä, kysytään luodaanko uusi.");
            UIManager.Instance.ShowCreatePopup(slotNumber);
        }
    }

    public void CreateNewProfile()
    {
        SceneManager.LoadScene("pukuhuone");
    }

    public void SaveProfileName(string profileName)
    {
        PlayerPrefs.SetString("profile_" + selectedSlot, profileName);
        PlayerPrefs.Save();
    }

    public string GetProfileName(int slot)
    {
        if (PlayerPrefs.HasKey("profile_" + slot))
            return PlayerPrefs.GetString("profile_" + slot);

        return "Tyhjä";
    }
}
