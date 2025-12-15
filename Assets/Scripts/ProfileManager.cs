using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance;

    public string[] profiles = new string[3];
    public int[] profileSkins = new int[3]; // 👈 UUSI
    public int selectedProfileIndex = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProfiles();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadProfiles()
    {
        for (int i = 0; i < profiles.Length; i++)
        {
            profiles[i] = PlayerPrefs.GetString("ProfileName_" + i, "Tyhjä");
            profileSkins[i] = PlayerPrefs.GetInt("ProfileSkin_" + i, -1); // -1 = ei skiniä
        }
    }

    public void SaveProfiles()
    {
        for (int i = 0; i < profiles.Length; i++)
        {
            PlayerPrefs.SetString("ProfileName_" + i, profiles[i]);
            PlayerPrefs.SetInt("ProfileSkin_" + i, profileSkins[i]);
        }
    }

    public void SelectProfile(int index)
    {
        selectedProfileIndex = index;

        if (profiles[index] == "Tyhjä")
            SceneManager.LoadScene("Pukuhuone");
        else
            SceneManager.LoadScene("MainGame");
    }

    public void CreateProfile(string name, int skinIndex)
    {
        profiles[selectedProfileIndex] = name;
        profileSkins[selectedProfileIndex] = skinIndex;
        SaveProfiles();
    }
}
