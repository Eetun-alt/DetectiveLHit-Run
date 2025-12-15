using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DressRoom : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Button confirmButton;

    public Image previewImage;        // Profiilikuvan preview
    public Sprite[] skinSprites;      // 3 skiniä

    private int selectedSkin = -1;

    void Start()
    {
        confirmButton.onClick.AddListener(OnConfirm);
        confirmButton.interactable = false;
    }

    public void SelectSkin(int skinIndex)
    {
        selectedSkin = skinIndex;
        previewImage.sprite = skinSprites[skinIndex];
        Validate();
    }

    void Validate()
    {
        bool nameOk = !string.IsNullOrEmpty(nameInput.text);
        bool skinOk = selectedSkin != -1;
        confirmButton.interactable = nameOk && skinOk;
    }

    void OnConfirm()
    {
        ProfileManager.Instance.CreateProfile(
            nameInput.text,
            selectedSkin
        );

        SceneManager.LoadScene("Profiilit");
    }

    public void OnNameChanged()
    {
        Validate();
    }
}
