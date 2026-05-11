using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenuVolume : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioMixer audioMixer;

    void Start()
    {
        // Lataa tallennettu volume
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);

        // Aseta slider oikeaan kohtaan
        volumeSlider.value = savedVolume;

        // P‰ivit‰ ‰‰ni
        SetVolume(savedVolume);
    }

    public void OnSliderChanged()
    {
        float volume = volumeSlider.value;

        // Tallenna
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

        // P‰ivit‰ ‰‰ni heti
        SetVolume(volume);
    }

    void SetVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }
}