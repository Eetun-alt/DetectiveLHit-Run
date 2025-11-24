using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        // Lataa tallennettu ‰‰nenvoimakkuus (jos ei ole tallennettua, k‰ytet‰‰n 1.0f)
        float savedVolume = PlayerPrefs.GetFloat("volume", 1.0f);
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        // Kuunnellaan slideria
        volumeSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.Save();
    }
}