using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileButtonUI : MonoBehaviour
{
    public TextMeshProUGUI profileText;
    private int profileIndex;

    void Awake()
    {
        if (profileText == null)
        {
            Debug.LogError("ProfileText PUUTTUU: " + gameObject.name);
            return;
        }

        if (ProfileManager.Instance == null)
        {
            Debug.LogError("ProfileManager.Instance on NULL");
            return;
        }

        if (gameObject.name.StartsWith("Profile"))
        {
            string numberPart = gameObject.name.Substring("Profile".Length);
            int.TryParse(numberPart, out int num);
            profileIndex = num - 1;
        }

        profileText.text = ProfileManager.Instance.profiles[profileIndex];
        GetComponent<Button>().onClick.AddListener(OnProfileClicked);
    }

    void OnProfileClicked()
    {
        ProfileManager.Instance.SelectProfile(profileIndex);
    }

    // Päivitä nappi, esim. kun pukuhuoneesta palataan
    public void Refresh()
    {
        profileText.text = ProfileManager.Instance.profiles[profileIndex];
    }
}
