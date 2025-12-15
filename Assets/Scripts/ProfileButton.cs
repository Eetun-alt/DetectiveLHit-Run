using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ProfileButton : MonoBehaviour
{
    public int slot; // Slot numero, 1-3
    public string sceneToLoad; // Scene, johon mennään

    private ProfileData profileData;
    private Button button;
    private TextMeshProUGUI mapText;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("ProfileButton: Button-komponenttia ei löydy tästä GameObjectista!");
        }

        // Etsitään TextMeshProUGUI lapsista automaattisesti
        mapText = GetComponentInChildren<TextMeshProUGUI>();
        if (mapText == null)
        {
            Debug.LogWarning($"ProfileButton slot {slot}: TextMeshProUGUI-komponenttia ei löydy lapsista!");
        }
    }

    void Start()
    {
        LoadProfile();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    public void LoadProfile()
    {
        profileData = ProfileManager.Instance.LoadProfile(slot);

        if (mapText != null)
        {
            if (profileData != null && profileData.highestMap > 0)
            {
                mapText.text = $"Kartta: {profileData.highestMap}";
                if (button != null) button.interactable = true;
            }
            else
            {
                mapText.text = "Tyhjä";
                if (button != null) button.interactable = false;
            }
        }
        else
        {
            if (button != null) button.interactable = false;
        }
    }

    public void OnButtonClick()
    {
        if (profileData != null && profileData.highestMap > 0 && button != null && button.interactable)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}

