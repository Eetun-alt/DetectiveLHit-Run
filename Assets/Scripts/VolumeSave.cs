using UnityEngine;
using UnityEngine.UI;

public class VolumeSave : MonoBehaviour
{
    public Slider volumeSlider;

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    public void LoadVolume()
    {
        // Jos arvoa ei löydy, käytetään oletuksena 0.30f ettei korvat mee rikki vahingossa.....(jos olis äänet)
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.30f);
        volumeSlider.value = savedVolume;
    }

    public void ClearVolume()
    {
        PlayerPrefs.DeleteKey("Volume");
    }
}