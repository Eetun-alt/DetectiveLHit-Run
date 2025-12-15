using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameButton : MonoBehaviour
{
    [Header("UI Components")]
    public TMP_InputField nameInput;         // InputField, johon pelaaja kirjoittaa nimen
    public Button newGameButton;             // “Uusi peli” -nappi, hierarkiasta vedettävä
    public ProfileButton[] profileSlots;     // Kaikki slotit, hierarkiasta vedettävä

    [Header("Scene")]
    public string sceneToLoad;               // Ensimmäinen scene pelille

    void Start()
    {
        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(OnNewGameClicked);
        }
    }

    void OnNewGameClicked()
    {
        string playerName = nameInput.text;
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("Anna profiilille nimi!");
            return;
        }

        // Etsi vapaa slot
        ProfileButton freeSlot = null;
        foreach (var slot in profileSlots)
        {
            if (!slot.GetComponent<Button>().interactable)
            {
                freeSlot = slot;
                break;
            }
        }

        if (freeSlot == null)
        {
            Debug.Log("Ei vapaita slotteja!");
            return;
        }

        // Luo uusi profiili
        ProfileData newProfile = new ProfileData
        {
            profileName = playerName,
            highestMap = 1
        };

        ProfileManager.Instance.SaveProfile(freeSlot.slot, newProfile);
        freeSlot.LoadProfile(); // Päivitä UI

        // Lataa ensimmäinen scene
        SceneManager.LoadScene(sceneToLoad);
    }
}
